
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicLyricApp.Bean
{
	public class SettingBean
	{
		public ConfigBean Config = new();

		public PersistParamBean Param = new();
	}

	public class ConfigBean
	{
		/// <summary>
		/// 参数记忆
		/// </summary>
		public bool RememberParam = false;

		/// <summary>
		/// 聚合模糊搜索
		/// </summary>
		public bool AggregatedBlurSearch = false;

		/// <summary>
		/// 自读读取剪切板
		/// </summary>
		public bool AutoReadClipboard = false;

		/// <summary>
		/// 自动检查更新
		/// </summary>
		public bool AutoCheckUpdate = true;

		/// <summary>
		/// 保存时跳过纯音乐
		/// </summary>
		public bool IgnorePureMusicInSave = true;

		/// <summary>
		/// 对于 "独立" 歌词格式，保存在不同的文件中
		/// </summary>
		public bool SeparateFileForIsolated = false;

		/// <summary>
		/// 输出文件名格式
		/// </summary>
		public string OutputFileNameFormat = "${name} - ${singer}";

		/// <summary>
		/// 输出的歌词类型列表
		/// </summary>
		public string OutputLyricTypes = string.Join(",", new[] { (int)LyricsTypeEnum.ORIGIN, (int)LyricsTypeEnum.ORIGIN_TRANS });

		/// <summary>
		/// QQ 音乐 Cookie
		/// </summary>
		public string QQMusicCookie = "";

		/// <summary>
		/// 网易云音乐 Cookie
		/// </summary>
		public string NetEaseCookie = "";

		public TransConfigBean TransConfig = new();

		public List<LyricsTypeEnum> DeserializationOutputLyricsTypes()
		{
			return string.IsNullOrWhiteSpace(OutputLyricTypes) ? [] :
				OutputLyricTypes.Split(',').Select(e => (LyricsTypeEnum)Convert.ToInt32(e)).ToList();
		}
	}

	public class TransConfigBean
	{
		/// <summary>
		/// 译文缺省规则
		/// </summary>
		public TransLyricLostRuleEnum LostRule = TransLyricLostRuleEnum.IGNORE;

		/// <summary>
		/// 译文歌词匹配精度
		/// </summary>
		public int MatchPrecisionDeviation = 0;
	}

	public class PersistParamBean
	{
		/// <summary>
		/// 搜索来源
		/// </summary>
		public SearchSourceEnum SearchSource = SearchSourceEnum.NET_EASE_MUSIC;

		/// <summary>
		/// 搜索类型
		/// </summary>
		public SearchTypeEnum SearchType = SearchTypeEnum.SONG_ID;

		/// <summary>
		/// 歌词展示格式
		/// </summary>
		public ShowLrcTypeEnum ShowLrcType = ShowLrcTypeEnum.STAGGER;

		/// <summary>
		/// 指定歌词合并的分隔符
		/// </summary>
		public string LrcMergeSeparator = string.Empty;

		/// <summary>
		/// LRC 歌词时间戳格式
		/// </summary>
		public string LrcTimestampFormat = "[mm:ss.SSS]";

		/// <summary>
		/// SRT 歌词时间戳格式
		/// </summary>
		public string SrtTimestampFormat = "HH:mm:ss,SSS";

		/// <summary>
		/// 启用逐字歌词模式
		/// </summary>
		public bool EnableVerbatimLyric = false;

		/// <summary>
		/// 忽略空的歌词行
		/// </summary>
		public bool IgnoreEmptyLyric = true;

		/// <summary>
		/// 小数位处理策略
		/// </summary>
		public DotTypeEnum DotType = DotTypeEnum.DOWN;

		/// <summary>
		/// 输出文件格式
		/// </summary>
		public OutputFormatEnum OutputFileFormat = OutputFormatEnum.LRC;

		/// <summary>
		/// 输出文件编码
		/// </summary>
		public OutputEncodingEnum Encoding = OutputEncodingEnum.UTF_8;
	}
}
