using MusicLyricApp.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicLyricApp.Utils
{
	/// <summary>
	/// 歌词处理基类
	/// </summary>
	public abstract partial class LyricUtils
	{
		[GeneratedRegex(@"\(\d+,\d+\)")]
		private static partial Regex VerbatimRegex();

		/// <summary>
		/// 获取输出结果
		/// </summary>
		/// <param name="lyricVo"></param>
		/// <param name="searchInfo"></param>
		/// <returns></returns>
		public static List<string> GetOutputContent(LyricVo lyricVo, SearchInfo searchInfo)
		{
			var param = searchInfo.SettingBean.Param;

			var dotType = param.DotType;
			var timestampFormat = param.OutputFileFormat == OutputFormatEnum.SRT ? param.SrtTimestampFormat : param.LrcTimestampFormat;

			var voListList = FormatLyric(lyricVo.Lyric, lyricVo.TranslateLyric, searchInfo);

			if (lyricVo.SearchSource == SearchSourceEnum.QQ_MUSIC && searchInfo.SettingBean.Param.EnableVerbatimLyric)
			{
				for (var i = 0; i < voListList.Count; i++)
				{
					voListList[i] = FormatSubLineLyric(voListList[i], timestampFormat, dotType);
				}
			}

			var res = new List<string>();

			foreach (var voList in voListList)
			{
				if (param.OutputFileFormat == OutputFormatEnum.SRT)
				{
					res.Add(SrtUtils.LrcToSrt(voList, timestampFormat, dotType, lyricVo.Duration));
				}
				else
				{
					res.Add(string.Join(Environment.NewLine, from o in voList select o.Print(timestampFormat, dotType)));
				}
			}

			return res;
		}

		/// <summary>
		/// 处理逐字歌词格式为常规格式
		/// </summary>
		public static string DealVerbatimLyric(string originLrc, SearchSourceEnum searchSource)
		{
			var defaultParam = new PersistParamBean();
			var sb = new StringBuilder();

			foreach (var line in SplitLrc(originLrc))
			{
				// skip illegal verbatim line, eg: https://y.qq.com/n/ryqq/songDetail/000sNzbP2nHGs2
				if (!line.EndsWith(')'))
				{
					continue;
				}

				var matches = VerbatimRegex().Matches(line);
				if (matches.Count > 0)
				{
					int contentStartIndex = 0, i = 0;

					do
					{
						var curMatch = matches[i];
						var group = curMatch.Groups[0];
						int leftParenthesesIndex = group.Index, parenthesesLength = group.Length;

						// (404,202)
						var timeStr = line.Substring(leftParenthesesIndex, parenthesesLength);
						// 404
						var timestamp = long.Parse(timeStr.Split(',')[0].Trim()[1..]);
						var lyricTimestamp = new LyricTimestamp(timestamp);

						var content = line[contentStartIndex..leftParenthesesIndex];
						// 首次执行，去除全行时间戳
						if (i == 0)
						{
							content = new LyricLineVo(content).Content;
						}

						contentStartIndex = leftParenthesesIndex + parenthesesLength;

						sb.Append(lyricTimestamp.PrintTimestamp(defaultParam.LrcTimestampFormat, defaultParam.DotType)).Append(content);

						// 最后一次执行，增加行结束时间戳
						if (i == matches.Count - 1)
						{
							// 202
							var timeCostStr = timeStr.Split(',')[1].Trim();
							var timeCost = long.Parse(timeCostStr[..^1]);

							sb.Append(lyricTimestamp.Add(timeCost)
								.PrintTimestamp(defaultParam.LrcTimestampFormat, defaultParam.DotType));
						}
					} while (++i < matches.Count);
				}
				else
				{
					sb.Append(line);
				}

				sb.Append(Environment.NewLine);
			}

			return sb.ToString();
		}

		/// <summary>
		/// need try split sub lyricLineVO, resolve verbatim lyric mode
		/// </summary>
		/// <returns></returns>
		private static List<LyricLineVo> FormatSubLineLyric(List<LyricLineVo> vos, string timestampFormat, DotTypeEnum dotType)
		{
			var res = new List<LyricLineVo>();
			foreach (var vo in vos)
			{
				var sb = new StringBuilder();
				foreach (var subVo in LyricLineVo.Split(vo))
				{
					sb.Append(subVo.Timestamp.PrintTimestamp(timestampFormat, dotType) + subVo.Content);
				}

				res.Add(new LyricLineVo(sb.ToString()));
			}
			return res;
		}

		/// <summary>
		/// 歌词格式化
		/// </summary>
		/// <param name="originLrc">原始的歌词内容</param>
		/// <param name="translateLrc">原始的译文内容</param>
		/// <param name="searchInfo">处理参数</param>
		/// <returns></returns>
		private static List<List<LyricLineVo>> FormatLyric(string originLrc, string translateLrc, SearchInfo searchInfo)
		{
			var outputLyricsTypes = searchInfo.SettingBean.Config.DeserializationOutputLyricsTypes();
			var showLrcType = searchInfo.SettingBean.Param.ShowLrcType;
			var searchSource = searchInfo.SettingBean.Param.SearchSource;
			var ignoreEmptyLyric = searchInfo.SettingBean.Param.IgnoreEmptyLyric;

			var res = new List<List<LyricLineVo>>();

			// 未配置任何输出
			if (outputLyricsTypes.Count == 0)
			{
				return res;
			}

			var originLyrics = SplitLrc(originLrc, searchSource, ignoreEmptyLyric);

			var originLyricsOutputSortInConfig = outputLyricsTypes.IndexOf(LyricsTypeEnum.ORIGIN);

			// 仅输出原文            
			if (outputLyricsTypes.Count == 1 && originLyricsOutputSortInConfig != -1)
			{
				res.Add(originLyrics);
				return res;
			}

			// 原始译文歌词的空行没有意义，指定 true 不走配置
			var basicTransLyrics = SplitLrc(translateLrc, searchSource, true);

			var lyricsComplexList = DealTranslateLyric(originLyrics, basicTransLyrics, searchInfo.SettingBean.Config.TransConfig, outputLyricsTypes);

			// 原文歌词插入到结果集的指定位置
			if (originLyricsOutputSortInConfig != -1)
			{
				lyricsComplexList.Insert(originLyricsOutputSortInConfig, originLyrics);
			}

			var single = new List<LyricLineVo>();
			switch (showLrcType)
			{
				case ShowLrcTypeEnum.STAGGER:
					foreach (var each in lyricsComplexList)
					{
						single = SortLrc(single, each, true);
					}
					break;
				case ShowLrcTypeEnum.ISOLATED:
					if (searchInfo.SettingBean.Config.SeparateFileForIsolated)
					{
						res.AddRange(lyricsComplexList);
					}
					else
					{
						foreach (var each in lyricsComplexList)
						{
							single.AddRange(each);
						}
					}
					break;
				case ShowLrcTypeEnum.MERGE:
					foreach (var each in lyricsComplexList)
					{
						single = MergeLrc(single, each, searchInfo.SettingBean.Param.LrcMergeSeparator, true);
					}
					break;
				default:
					throw new NotSupportedException("not support showLrcType: " + showLrcType);
			}

			if (single.Count > 0)
			{
				res.Add(single);
			}

			return res;
		}

		private static string[] SplitLrc(string lrc)
		{
			// 换行符统一
			return (lrc ?? "")
				.Replace("\r\n", "\n")
				.Replace("\r", "")
				.Split('\n');
		}

		/**
         * 切割歌词
         */
		private static List<LyricLineVo> SplitLrc(string lrc, SearchSourceEnum searchSource, bool ignoreEmptyLine)
		{

			var temp = SplitLrc(lrc);

			var resultList = new List<LyricLineVo>();

			foreach (var line in temp)
			{
				// QQ 音乐歌词正式开始标识符
				if (searchSource == SearchSourceEnum.QQ_MUSIC && "[offset:0]".Equals(line))
				{
					resultList.Clear();
					continue;
				}

				var lyricLineVo = new LyricLineVo(line);

				// 无效内容处理
				if (lyricLineVo.IsIllegalContent())
				{
					if (ignoreEmptyLine)
					{
						continue;
					}

					// 重置空行内容
					lyricLineVo.Content = string.Empty;
				}

				resultList.Add(lyricLineVo);
			}

			return resultList;
		}


		/// <summary>
		/// 歌词排序
		/// </summary>
		private static List<LyricLineVo> SortLrc(List<LyricLineVo> listA, List<LyricLineVo> listB, bool aFirst)
		{
			int lenA = listA.Count, lenB = listB.Count;
			var c = new List<LyricLineVo>();

			int i = 0, j = 0;

			while (i < lenA && j < lenB)
			{
				var compare = Compare(listA[i], listB[j], aFirst);

				if (compare > 0)
				{
					c.Add(listB[j++]);
				}
				else if (compare < 0)
				{
					c.Add(listA[i++]);
				}
				else
				{
					c.Add(aFirst ? listA[i++] : listB[j++]);
				}
			}

			while (i < lenA)
				c.Add(listA[i++]);
			while (j < lenB)
				c.Add(listB[j++]);
			return c;
		}

		/// <summary>
		/// 歌词合并
		/// </summary>
		private static List<LyricLineVo> MergeLrc(List<LyricLineVo> listA, List<LyricLineVo> listB, string splitStr, bool aFirst)
		{
			var c = SortLrc(listA, listB, aFirst);

			if (c.Count == 0)
			{
				return c;
			}

			var list = new List<LyricLineVo>
			{
				c[0]
			};

			for (var i = 1; i < c.Count; i++)
			{
				if (c[i - 1].Timestamp.TimeOffset == c[i].Timestamp.TimeOffset)
				{
					var index = list.Count - 1;

					list[index].Content = list[index].Content + splitStr + c[i].Content;
				}
				else
				{
					list.Add(c[i]);
				}
			}

			return list;
		}

		/// <summary>
		/// 译文逻辑处理: 译文精度误差, 译文缺省规则, 译文类型填充
		/// </summary>
		/// <param name="originList">原文歌词</param>
		/// <param name="baseTransList">初始的译文歌词</param>
		/// <param name="transConfig">译文配置</param>
		/// <param name="outputLyricsTypes">输出歌词类型列表</param>
		/// <returns></returns>
		public static List<List<LyricLineVo>> DealTranslateLyric(List<LyricLineVo> originList,
			List<LyricLineVo> baseTransList, TransConfigBean transConfig, List<LyricsTypeEnum> outputLyricsTypes)
		{
			// 不存在原文歌词或不要求输出翻译
			if (originList == null || originList.Count == 0 || !outputLyricsTypes.Contains(LyricsTypeEnum.ORIGIN_TRANS))
			{
				return [];
			}

			// 处理译文精度误差, 译文缺省规则
			var transList = ResolveTransLyricDigitDeviationAndLost(originList, baseTransList, transConfig.MatchPrecisionDeviation, transConfig.LostRule);

			return [transList];
		}

		/// <summary>
		/// 解决译文歌词的精度误差和丢失问题
		/// </summary>
		/// <param name="originList">原文歌词</param>
		/// <param name="baseTransList">初始译文歌词</param>
		/// <param name="precisionDigitDeviation">译文匹配精度误差</param>
		/// <param name="lostRule">译文缺失规则</param>
		/// <returns></returns>
		private static List<LyricLineVo> ResolveTransLyricDigitDeviationAndLost(List<LyricLineVo> originList, List<LyricLineVo> baseTransList,
			int precisionDigitDeviation, TransLyricLostRuleEnum lostRule)
		{
			var originTimeOffsetDict = new Dictionary<long, LyricLineVo>();
			foreach (var one in originList)
			{
				originTimeOffsetDict[one.Timestamp.TimeOffset] = one;
			}

			var notMatchTranslateDict = new Dictionary<int, LyricLineVo>();

			// 误差 == 0
			for (var i = 0; i < baseTransList.Count; i++)
			{
				var translate = baseTransList[i];
				var timestamp = translate.Timestamp.TimeOffset;

				if (!originTimeOffsetDict.Remove(timestamp))
				{
					notMatchTranslateDict.Add(i, translate);
				}
			}

			if (precisionDigitDeviation != 0)
			{
				foreach (var pair in notMatchTranslateDict)
				{
					var index = pair.Key;
					var translate = pair.Value;
					var timestamp = translate.Timestamp.TimeOffset;

					var tsStart = Math.Max(index == 0 ? 0 : baseTransList[index - 1].Timestamp.TimeOffset + 1, timestamp - precisionDigitDeviation);

					long tsEnd;
					if (index == baseTransList.Count - 1)
					{
						tsEnd = Math.Max(timestamp, originList[^1].Timestamp.TimeOffset);
					}
					else
					{
						tsEnd = baseTransList[index + 1].Timestamp.TimeOffset - 1;
					}
					tsEnd = Math.Min(tsEnd, timestamp + precisionDigitDeviation);

					for (var ts = tsStart; ts <= tsEnd; ts++)
					{
						if (originTimeOffsetDict.Remove(ts))
						{
							// 将译文时间调整为误差后的译文
							var newTranslate = new LyricLineVo(translate.Content, new LyricTimestamp(ts));

							baseTransList[pair.Key] = newTranslate;
						}
					}
				}
			}

			// 处理译文缺失规则
			if (lostRule != TransLyricLostRuleEnum.IGNORE)
			{
				foreach (var pair in originTimeOffsetDict)
				{
					var content = lostRule == TransLyricLostRuleEnum.FILL_ORIGIN ? pair.Value.Content : "";

					baseTransList.Add(new LyricLineVo(content, pair.Value.Timestamp));
				}
			}

			var transList = new List<LyricLineVo>(baseTransList);
			transList.Sort();

			return transList;
		}

		/**
         * 歌词排序函数
         */
		private static int Compare(LyricLineVo originLrc, LyricLineVo translateLrc, bool hasOriginLrcPrior)
		{
			var compareTo = originLrc.CompareTo(translateLrc);

			if (compareTo == 0)
			{
				return hasOriginLrcPrior ? -1 : 1;
			}

			return compareTo;
		}
	}
}
