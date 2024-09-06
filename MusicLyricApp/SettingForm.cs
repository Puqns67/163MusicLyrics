﻿using MusicLyricApp.Bean;
using MusicLyricApp.Exception;
using MusicLyricApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MusicLyricApp
{
	public partial class SettingForm : MusicLyricForm
	{
		private readonly SettingBean _settingBean;

		public SettingForm(SettingBean settingBean)
		{
			_settingBean = settingBean;

			InitializeComponent();

			AfterInitializeComponent();

			VerbatimLyricChangeListener(VerbatimLyric_CheckBox.Checked);
		}

		/// <summary>
		/// 保存 & 重置按钮，点击事件
		/// </summary>
		private void Close_Btn_Click(object sender, EventArgs e)
		{
			if (sender is not Button input)
			{
				throw new MusicLyricException(ErrorMsg.SYSTEM_ERROR);
			}

			if (input == Save_Btn)
			{
				// 歌词时间戳
				_settingBean.Param.DotType = (DotTypeEnum)Dot_ComboBox.SelectedIndex;
				_settingBean.Param.LrcTimestampFormat = LrcTimestampFormat_TextBox.Text;
				_settingBean.Param.SrtTimestampFormat = SrtTimestampFormat_TextBox.Text;

				// 原文歌词
				_settingBean.Param.IgnoreEmptyLyric = IgnoreEmptyLyric_CheckBox.Checked;
				_settingBean.Param.EnableVerbatimLyric = VerbatimLyric_CheckBox.Checked;

				// 译文歌词
				_settingBean.Config.TransConfig.LostRule = (TransLyricLostRuleEnum)TransLostRule_ComboBox.SelectedIndex;
				_settingBean.Config.TransConfig.MatchPrecisionDeviation = int.Parse(TranslateMatchPrecisionDeviation_TextBox.Text);

				var selectTransType = new List<int>();
				var transTypeDict = GlobalUtils.GetEnumDict<LyricsTypeEnum>();
				foreach (DataGridViewRow row in LyricShow_DataGridView.Rows)
				{
					if ((bool)row.Cells[0].Value)
					{
						var transType = transTypeDict[row.Cells[1].Value.ToString()];
						selectTransType.Add(Convert.ToInt32(transType));
					}
				}
				_settingBean.Config.OutputLyricTypes = string.Join(",", selectTransType);

				// 输出设置
				_settingBean.Config.IgnorePureMusicInSave = IgnorePureMusicInSave_CheckBox.Checked;
				_settingBean.Config.SeparateFileForIsolated = SeparateFileForIsolated_CheckBox.Checked;
				_settingBean.Config.OutputFileNameFormat = OutputName_TextBox.Text;

				// 应用设置
				_settingBean.Config.RememberParam = RememberParam_CheckBox.Checked;
				_settingBean.Config.AggregatedBlurSearch = AggregatedBlurSearchCheckBox.Checked;
				_settingBean.Config.AutoReadClipboard = AutoReadClipboard_CheckBox.Checked;
				_settingBean.Config.AutoCheckUpdate = AutoCheckUpdate_CheckBox.Checked;
				_settingBean.Config.QQMusicCookie = QQMusic_Cookie_TextBox.Text;
				_settingBean.Config.NetEaseCookie = NetEase_Cookie_TextBox.Text;
			}
			else if (input == Reset_Btn)
			{
				_settingBean.Param = new PersistParamBean();
				_settingBean.Config = new ConfigBean();
			}

			Close();
		}

		/// <summary>
		/// 帮助按钮，点击事件
		/// </summary>
		private void Help_Btn_Click(object sender, EventArgs e)
		{
			if (sender is not Button input)
			{
				throw new MusicLyricException(ErrorMsg.SYSTEM_ERROR);
			}

			var typeEnum = Constants.HelpTips.TypeEnum.DEFAULT;
			if (input == TimestampHelp_Btn)
			{
				typeEnum = Constants.HelpTips.TypeEnum.TIME_STAMP_SETTING;
			}
			else if (input == OutputHelp_Btn)
			{
				typeEnum = Constants.HelpTips.TypeEnum.OUTPUT_SETTING;
			}

			SettingTips_TextBox.Text = Constants.HelpTips.GetContent(typeEnum);
		}

		/// <summary>
		/// 译文匹配精度输入内容有效性校验
		/// </summary>
		private void LrcMatchDigit_TextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\b' && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		/// <summary>
		/// 逐字歌词依赖检查
		/// </summary>
		private void VerbatimLyric_CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			VerbatimLyricChangeListener(VerbatimLyric_CheckBox.Checked);
		}

		private void VerbatimLyricChangeListener(bool isEnable)
		{
			// 检查依赖
			if (isEnable && Constants.VerbatimLyricDependency.Any(e => !File.Exists(e)))
			{
				MessageBox.Show(string.Format(ErrorMsg.DEPENDENCY_LOSS, "Verbatim"), CaptionMsg.LOST_DENPENDCY);
				VerbatimLyric_CheckBox.Checked = false;
			}
		}

		/// <summary>
		/// 译文列表，控制移动时鼠标的图形
		/// </summary>
		private void TransList_DataGridView_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		/// <summary>
		/// 译文列表，控制拖动的条件
		/// </summary>
		private void TransList_DataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Clicks < 2 && e.Button == MouseButtons.Left)

			{
				if (e.ColumnIndex == -1 && e.RowIndex > -1)
					LyricShow_DataGridView.DoDragDrop(LyricShow_DataGridView.Rows[e.RowIndex], DragDropEffects.Move);
			}
		}

		private int _transListSelectionIdx;

		/// <summary>
		/// 译文列表，拖动后实现行的删除和添加，实现行交换位置的错觉
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TransList_DataGridView_DragDrop(object sender, DragEventArgs e)
		{
			var idx = GetRowFromPoint(e.X, e.Y);

			if (idx < 0) return;

			if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
			{
				var row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

				LyricShow_DataGridView.Rows.Remove(row);

				_transListSelectionIdx = idx;

				LyricShow_DataGridView.Rows.Insert(idx, row);
			}
		}

		private int GetRowFromPoint(int x, int y)
		{
			for (var i = 0; i < LyricShow_DataGridView.RowCount; i++)
			{
				var rec = LyricShow_DataGridView.GetRowDisplayRectangle(i, false);
				if (LyricShow_DataGridView.RectangleToScreen(rec).Contains(x, y))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// 控制被移动的行始终是选中行
		/// </summary>
		private void TransList_DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if (_transListSelectionIdx > -1)
			{
				LyricShow_DataGridView.Rows[_transListSelectionIdx].Selected = true;
				LyricShow_DataGridView.CurrentCell = LyricShow_DataGridView.Rows[_transListSelectionIdx].Cells[0];
			}
		}

		private void TransType_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			LyricShow_DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}
	}
}