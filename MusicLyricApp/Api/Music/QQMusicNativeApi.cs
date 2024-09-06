using MusicLyricApp.Bean;
using MusicLyricApp.Exception;
using MusicLyricApp.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace MusicLyricApp.Api.Music
{
	public partial class QQMusicNativeApi(Func<string> cookieFunc) : BaseNativeApi(cookieFunc)
	{
		private static readonly DateTime _dtFrom = new(1970, 1, 1, 8, 0, 0, 0, DateTimeKind.Local);

		[LibraryImport("QQMusicVerbatim.dll", EntryPoint = "?Ddes@qqmusic@@YAHPAE0H@Z", StringMarshalling = StringMarshalling.Utf16)]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
		private static partial void DDES(sbyte[] a, string b, int c);

		[LibraryImport("QQMusicVerbatim.dll", EntryPoint = "?des@qqmusic@@YAHPAE0H@Z", StringMarshalling = StringMarshalling.Utf16)]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
		private static partial void DES(sbyte[] a, string b, int c);

		private static readonly Dictionary<string, string> VerbatimXmlMappingDict = new()
		{
			{ "content", "orig" }, // 原文
			{ "contentts", "ts" }, // 译文
			{ "contentroma", "roma" }, // 罗马音
			{ "Lyric_1", "lyric" }, // 解压后的内容
		};

		protected override string HttpRefer()
		{
			return "https://c.y.qq.com/";
		}

		public QQMusicBean.MusicFcgApiResult Search(string keyword, SearchTypeEnum searchType)
		{
			// 0单曲 2专辑 1歌手 3歌单 7歌词 12mv
			var type = searchType switch
			{
				SearchTypeEnum.SONG_ID => 0,
				SearchTypeEnum.ALBUM_ID => 2,
				SearchTypeEnum.PLAYLIST_ID => 3,
				_ => throw new MusicLyricException(ErrorMsg.SYSTEM_ERROR),
			};
			var data = new Dictionary<string, object>
			{
				{
					"req_1", new Dictionary<string, object>
					{
						{ "method", "DoSearchForQQMusicDesktop" },
						{ "module", "music.search.SearchCgiService" },
						{
							"param", new Dictionary<string, object>
							{
								{ "num_per_page", "20" },
								{ "page_num", "1" },
								{ "query", keyword },
								{ "search_type", type },
							}
						}
					}
				}
			};

			var resp = SendJsonPost("https://u.y.qq.com/cgi-bin/musicu.fcg", data);

			return resp.ToEntity<QQMusicBean.MusicFcgApiResult>();
		}

		public QQMusicBean.AlbumResult GetAlbum(string albumMid)
		{
			var data = new Dictionary<string, string>
			{
				{ "albummid", albumMid }
			};

			var resp = SendPost("https://c.y.qq.com/v8/fcg-bin/fcg_v8_album_info_cp.fcg", data);

			return resp.ToEntity<QQMusicBean.AlbumResult>();
		}

		public QQMusicBean.PlaylistResult GetPlaylist(string playlistId)
		{
			var data = new Dictionary<string, string>
			{
				{ "disstid", playlistId },
				{ "format", "json" },
				{ "outCharset", "utf8" },
				{ "type", "1" },
				{ "json", "1" },
				{ "utf8", "1" },
				{ "onlysong", "0" }, // 返回歌曲明细
				{ "new_format", "1" },
			};
			var resp = SendPost("https://c.y.qq.com/qzone/fcg-bin/fcg_ucc_getcdinfo_byids_cp.fcg", data);

			return resp.ToEntity<QQMusicBean.PlaylistResult>();
		}

		/// <summary>
		/// query music song
		/// </summary>
		/// <param name="id">query song by id, support songId and midId, eg: 001RaE0n4RrGX9 or 204422870</param>
		/// <returns>music song</returns>
		public QQMusicBean.SongResult GetSong(string id)
		{
			const string callBack = "getOneSongInfoCallback";

			var data = new Dictionary<string, string>
			{
				{ GlobalUtils.CheckNum(id) ? "songid" : "songmid", id },
				{ "tpl", "yqq_song_detail" },
				{ "format", "jsonp" },
				{ "callback", callBack },
				{ "g_tk", "5381" },
				{ "jsonpCallback", callBack },
				{ "loginUin", "0" },
				{ "hostUin", "0" },
				{ "outCharset", "utf8" },
				{ "notice", "0" },
				{ "platform", "yqq" },
				{ "needNewCode", "0" },
			};

			var resp = SendPost("https://c.y.qq.com/v8/fcg-bin/fcg_play_single_song.fcg", data);

			return ResolveRespJson(callBack, resp).ToEntity<QQMusicBean.SongResult>();
		}

		public QQMusicBean.LyricResult GetLyric(string songMid)
		{
			var currentMillis = (DateTime.Now.ToLocalTime().Ticks - _dtFrom.Ticks) / 10000;

			const string callBack = "MusicJsonCallback_lrc";

			var data = new Dictionary<string, string>
			{
				{ "callback", "MusicJsonCallback_lrc" },
				{ "pcachetime", currentMillis.ToString() },
				{ "songmid", songMid },
				{ "g_tk", "5381" },
				{ "jsonpCallback", callBack },
				{ "loginUin", "0" },
				{ "hostUin", "0" },
				{ "format", "jsonp" },
				{ "inCharset", "utf8" },
				{ "outCharset", "utf8" },
				{ "notice", "0" },
				{ "platform", "yqq" },
				{ "needNewCode", "0" },
			};

			var resp = SendPost("https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg", data);

			return ResolveRespJson(callBack, resp).ToEntity<QQMusicBean.LyricResult>();
		}

		public QQMusicBean.LyricResult GetVerbatimLyric(string songId)
		{
			var resp = SendPost("https://c.y.qq.com/qqmusic/fcgi-bin/lyric_download.fcg", new Dictionary<string, string>
			{
				{ "version", "15" },
				{ "miniversion", "82" },
				{ "lrctype", "4" },
				{ "musicid", songId },
			});
			// qq music 返回的内容需要去除注释
			resp = resp.Replace("<!--", "").Replace("-->", "");

			var dict = new Dictionary<string, XmlNode>();

			XmlUtils.RecursionFindElement(XmlUtils.Create(resp), VerbatimXmlMappingDict, dict);

			var result = new QQMusicBean.LyricResult
			{
				Code = 0,
				Lyric = "",
				Trans = ""
			};

			foreach (var pair in dict)
			{
				var text = pair.Value.InnerText;

				if (string.IsNullOrWhiteSpace(text))
				{
					continue;
				}

				var sz = MathUtils.ConvertStringToHexSbytes(text, out var sbytes);

				DDES(sbytes, "!@#)(NHLiuy*$%^&", sz);
				DES(sbytes, "123ZXC!@#)(*$%^&", sz);
				DDES(sbytes, "!@#)(*$%^&abcDEF", sz);

				var decompressBytes = MathUtils.SharpZipLibDecompress(MathUtils.SbytesToBytes(sbytes, sz));
				var decompressText = Encoding.UTF8.GetString(decompressBytes);

				var s = "";
				if (decompressText.Contains("<?xml"))
				{
					// 移除字符串头部的 BOM 标识 (如果有)
					string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
					if (decompressText[0] == byteOrderMarkUtf8[0])
					{
						decompressText = decompressText.Remove(0, byteOrderMarkUtf8.Length);
					}

					var doc = XmlUtils.Create(decompressText);

					var subDict = new Dictionary<string, XmlNode>();

					XmlUtils.RecursionFindElement(doc, VerbatimXmlMappingDict, subDict);

					if (subDict.TryGetValue("lyric", out var d))
					{
						s = d.Attributes["LyricContent"].InnerText;
					}
				}
				else
				{
					s = decompressText;
				}

				if (!string.IsNullOrWhiteSpace(s))
				{
					switch (pair.Key)
					{
						case "orig":
							result.Lyric = LyricUtils.DealVerbatimLyric(s, SearchSourceEnum.QQ_MUSIC);
							break;
						case "ts":
							result.Trans = LyricUtils.DealVerbatimLyric(s, SearchSourceEnum.QQ_MUSIC);
							break;
					}
				}
			}

			return result;
		}

		public ResultVo<string> GetSongLink(string songMid)
		{
			var guid = GetGuid();

			var data = new Dictionary<string, object>
			{
				{
					"req", new Dictionary<string, object>
					{
						{ "method", "GetCdnDispatch" },
						{ "module", "CDN.SrfCdnDispatchServer" },
						{
							"param", new Dictionary<string, object>
							{
								{ "guid", guid },
								{ "calltype", "0" },
								{ "userip", "" },
							}
						}
					}
				},
				{
					"req_0", new Dictionary<string, object>
					{
						{ "method", "CgiGetVkey" },
						{ "module", "vkey.GetVkeyServer" },
						{
							"param", new Dictionary<string, object>
							{
								{ "guid", "8348972662" },
								{ "songmid", new[] { songMid } },
								{ "songtype", new[] { 1 } },
								{ "uin", "0" },
								{ "loginflag", 1 },
								{ "platform", "20" },
							}
						}
					}
				},
				{
					"comm", new Dictionary<string, object>
					{
						{ "uin", 0 },
						{ "format", "json" },
						{ "ct", 24 },
						{ "cv", 0 },
					}
				}
			};

			var resp = SendJsonPost("https://u.y.qq.com/cgi-bin/musicu.fcg", data);

			var res = resp.ToEntity<QQMusicBean.MusicFcgApiResult>();

			var link = "";
			if (res.Code == 0 && res.Req.Code == 0 && res.Req_0.Code == 0)
			{
				link = res.Req.Data.Sip[0] + res.Req_0.Data.Midurlinfo[0].Purl;
			}

			return new ResultVo<string>(link);
		}

		private static string ResolveRespJson(string callBackSign, string val)
		{
			if (!val.StartsWith(callBackSign))
			{
				return string.Empty;
			}

			var jsonStr = val.Replace(callBackSign + "(", string.Empty);
			return jsonStr.Remove(jsonStr.Length - 1);
		}

		protected virtual string GetGuid()
		{
			var guid = new StringBuilder(10);
			var r = new Random();
			for (var i = 0; i < 10; i++)
			{
				guid.Append(Convert.ToString(r.Next(10)));
			}

			return guid.ToString();
		}
	}
}
