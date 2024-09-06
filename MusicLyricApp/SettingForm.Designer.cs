using MusicLyricApp.Bean;
using MusicLyricApp.Utils;
using System.ComponentModel;

namespace MusicLyricApp
{
	partial class SettingForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		private void AfterInitializeComponent()
		{
			// 歌词时间戳
			Dot_ComboBox.Items.AddRange(GlobalUtils.GetEnumDescArray<DotTypeEnum>());
			Dot_ComboBox.SelectedIndex = (int)_settingBean.Param.DotType;
			LrcTimestampFormat_TextBox.Text = _settingBean.Param.LrcTimestampFormat;
			SrtTimestampFormat_TextBox.Text = _settingBean.Param.SrtTimestampFormat;
			
			// 原文歌词
			IgnoreEmptyLyric_CheckBox.Checked = _settingBean.Param.IgnoreEmptyLyric;
			VerbatimLyric_CheckBox.Checked = _settingBean.Param.EnableVerbatimLyric;

			// 译文歌词
			TransLostRule_ComboBox.Items.AddRange(GlobalUtils.GetEnumDescArray<TransLyricLostRuleEnum>());
			TransLostRule_ComboBox.SelectedIndex = (int)_settingBean.Config.TransConfig.LostRule;
			TranslateMatchPrecisionDeviation_TextBox.Text = _settingBean.Config.TransConfig.MatchPrecisionDeviation.ToString();

			var allTransType = GlobalUtils.GetEnumList<LyricsTypeEnum>();
			foreach (var index in _settingBean.Config.DeserializationOutputLyricsTypes())
			{
				var one = (LyricsTypeEnum) index;
				allTransType.Remove(one);

				LyricShow_DataGridView.Rows.Add(true, one.ToDescription());
			}
			foreach (var one in allTransType)
			{
				LyricShow_DataGridView.Rows.Add(false, one.ToDescription());
			}

			// 输出设置
			IgnorePureMusicInSave_CheckBox.Checked = _settingBean.Config.IgnorePureMusicInSave;
			SeparateFileForIsolated_CheckBox.Checked = _settingBean.Config.SeparateFileForIsolated;
			OutputName_TextBox.Text = _settingBean.Config.OutputFileNameFormat;
			
			// 应用设置
			RememberParam_CheckBox.Checked = _settingBean.Config.RememberParam;
			AggregatedBlurSearchCheckBox.Checked = _settingBean.Config.AggregatedBlurSearch;
			AutoReadClipboard_CheckBox.Checked = _settingBean.Config.AutoReadClipboard;
			AutoCheckUpdate_CheckBox.Checked = _settingBean.Config.AutoCheckUpdate;
			QQMusic_Cookie_TextBox.Text = _settingBean.Config.QQMusicCookie;
			NetEase_Cookie_TextBox.Text = _settingBean.Config.NetEaseCookie;
			
			// 提示区
			SettingTips_TextBox.Text = Constants.HelpTips.Prefix;
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(SettingForm));
			Save_Btn = new System.Windows.Forms.Button();
			RememberParam_CheckBox = new System.Windows.Forms.CheckBox();
			AutoReadClipboard_CheckBox = new System.Windows.Forms.CheckBox();
			AutoCheckUpdate_CheckBox = new System.Windows.Forms.CheckBox();
			label6 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			LrcTimestampFormat_TextBox = new System.Windows.Forms.TextBox();
			SrtTimestampFormat_TextBox = new System.Windows.Forms.TextBox();
			label8 = new System.Windows.Forms.Label();
			Dot_ComboBox = new System.Windows.Forms.ComboBox();
			label1 = new System.Windows.Forms.Label();
			TransLostRule_ComboBox = new System.Windows.Forms.ComboBox();
			label9 = new System.Windows.Forms.Label();
			TranslateMatchPrecisionDeviation_TextBox = new System.Windows.Forms.TextBox();
			label5 = new System.Windows.Forms.Label();
			IgnoreEmptyLyric_CheckBox = new System.Windows.Forms.CheckBox();
			VerbatimLyric_CheckBox = new System.Windows.Forms.CheckBox();
			NetEase_Cookie_TextBox = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			QQMusic_Cookie_TextBox = new System.Windows.Forms.TextBox();
			Timestamp_GroupBox = new System.Windows.Forms.GroupBox();
			TimestampHelp_Btn = new System.Windows.Forms.Button();
			SettingTips_TextBox = new System.Windows.Forms.TextBox();
			OutputHelp_Btn = new System.Windows.Forms.Button();
			AppConfig_GroupBox = new System.Windows.Forms.GroupBox();
			AggregatedBlurSearchCheckBox = new System.Windows.Forms.CheckBox();
			OriginLyric_GroupBox = new System.Windows.Forms.GroupBox();
			TransLyric_GroupBox = new System.Windows.Forms.GroupBox();
			LyricShow_DataGridView = new System.Windows.Forms.DataGridView();
			Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Output_GroupBox = new System.Windows.Forms.GroupBox();
			SeparateFileForIsolated_CheckBox = new System.Windows.Forms.CheckBox();
			OutputName_TextBox = new System.Windows.Forms.TextBox();
			label10 = new System.Windows.Forms.Label();
			IgnorePureMusicInSave_CheckBox = new System.Windows.Forms.CheckBox();
			Reset_Btn = new System.Windows.Forms.Button();
			Timestamp_GroupBox.SuspendLayout();
			AppConfig_GroupBox.SuspendLayout();
			OriginLyric_GroupBox.SuspendLayout();
			TransLyric_GroupBox.SuspendLayout();
			((ISupportInitialize)LyricShow_DataGridView).BeginInit();
			Output_GroupBox.SuspendLayout();
			SuspendLayout();
			// 
			// Save_Btn
			// 
			Save_Btn.BackColor = System.Drawing.Color.Honeydew;
			Save_Btn.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
			Save_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			Save_Btn.Location = new System.Drawing.Point(715, 642);
			Save_Btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Save_Btn.Name = "Save_Btn";
			Save_Btn.Size = new System.Drawing.Size(113, 71);
			Save_Btn.TabIndex = 0;
			Save_Btn.Text = "保存";
			Save_Btn.UseVisualStyleBackColor = false;
			Save_Btn.Click += Close_Btn_Click;
			// 
			// RememberParam_CheckBox
			// 
			RememberParam_CheckBox.Location = new System.Drawing.Point(7, 52);
			RememberParam_CheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			RememberParam_CheckBox.Name = "RememberParam_CheckBox";
			RememberParam_CheckBox.Size = new System.Drawing.Size(91, 24);
			RememberParam_CheckBox.TabIndex = 1;
			RememberParam_CheckBox.Text = "参数记忆";
			RememberParam_CheckBox.UseVisualStyleBackColor = true;
			// 
			// AutoReadClipboard_CheckBox
			// 
			AutoReadClipboard_CheckBox.Location = new System.Drawing.Point(241, 52);
			AutoReadClipboard_CheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			AutoReadClipboard_CheckBox.Name = "AutoReadClipboard_CheckBox";
			AutoReadClipboard_CheckBox.Size = new System.Drawing.Size(131, 24);
			AutoReadClipboard_CheckBox.TabIndex = 2;
			AutoReadClipboard_CheckBox.Text = "自动读取剪贴板";
			AutoReadClipboard_CheckBox.UseVisualStyleBackColor = true;
			// 
			// AutoCheckUpdate_CheckBox
			// 
			AutoCheckUpdate_CheckBox.Location = new System.Drawing.Point(392, 52);
			AutoCheckUpdate_CheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			AutoCheckUpdate_CheckBox.Name = "AutoCheckUpdate_CheckBox";
			AutoCheckUpdate_CheckBox.Size = new System.Drawing.Size(114, 24);
			AutoCheckUpdate_CheckBox.TabIndex = 3;
			AutoCheckUpdate_CheckBox.Text = "自动检查更新";
			AutoCheckUpdate_CheckBox.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(9, 51);
			label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(70, 17);
			label6.TabIndex = 13;
			label6.Text = "LRC 时间戳";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(9, 106);
			label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(70, 17);
			label7.TabIndex = 14;
			label7.Text = "SRT 时间戳";
			// 
			// LrcTimestampFormat_TextBox
			// 
			LrcTimestampFormat_TextBox.Location = new System.Drawing.Point(120, 47);
			LrcTimestampFormat_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			LrcTimestampFormat_TextBox.Name = "LrcTimestampFormat_TextBox";
			LrcTimestampFormat_TextBox.Size = new System.Drawing.Size(116, 23);
			LrcTimestampFormat_TextBox.TabIndex = 15;
			// 
			// SrtTimestampFormat_TextBox
			// 
			SrtTimestampFormat_TextBox.Location = new System.Drawing.Point(120, 102);
			SrtTimestampFormat_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			SrtTimestampFormat_TextBox.Name = "SrtTimestampFormat_TextBox";
			SrtTimestampFormat_TextBox.Size = new System.Drawing.Size(116, 23);
			SrtTimestampFormat_TextBox.TabIndex = 16;
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(9, 163);
			label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(80, 17);
			label8.TabIndex = 17;
			label8.Text = "毫秒截位规则";
			// 
			// Dot_ComboBox
			// 
			Dot_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			Dot_ComboBox.FormattingEnabled = true;
			Dot_ComboBox.Location = new System.Drawing.Point(120, 159);
			Dot_ComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Dot_ComboBox.Name = "Dot_ComboBox";
			Dot_ComboBox.Size = new System.Drawing.Size(116, 25);
			Dot_ComboBox.TabIndex = 18;
			// 
			// label1
			// 
			label1.Location = new System.Drawing.Point(18, 51);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(96, 17);
			label1.TabIndex = 19;
			label1.Text = "译文缺省规则";
			// 
			// TransLostRule_ComboBox
			// 
			TransLostRule_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			TransLostRule_ComboBox.FormattingEnabled = true;
			TransLostRule_ComboBox.Location = new System.Drawing.Point(134, 47);
			TransLostRule_ComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			TransLostRule_ComboBox.Name = "TransLostRule_ComboBox";
			TransLostRule_ComboBox.Size = new System.Drawing.Size(114, 25);
			TransLostRule_ComboBox.TabIndex = 20;
			// 
			// label9
			// 
			label9.Location = new System.Drawing.Point(18, 105);
			label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(96, 17);
			label9.TabIndex = 21;
			label9.Text = "译文匹配精度";
			// 
			// TranslateMatchPrecisionDeviation_TextBox
			// 
			TranslateMatchPrecisionDeviation_TextBox.Location = new System.Drawing.Point(134, 101);
			TranslateMatchPrecisionDeviation_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			TranslateMatchPrecisionDeviation_TextBox.Name = "TranslateMatchPrecisionDeviation_TextBox";
			TranslateMatchPrecisionDeviation_TextBox.Size = new System.Drawing.Size(84, 23);
			TranslateMatchPrecisionDeviation_TextBox.TabIndex = 22;
			TranslateMatchPrecisionDeviation_TextBox.KeyPress += LrcMatchDigit_TextBox_KeyPress;
			// 
			// label5
			// 
			label5.Font = new System.Drawing.Font("宋体", 10F);
			label5.Location = new System.Drawing.Point(226, 105);
			label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(28, 17);
			label5.TabIndex = 23;
			label5.Text = "MS";
			// 
			// IgnoreEmptyLyric_CheckBox
			// 
			IgnoreEmptyLyric_CheckBox.Location = new System.Drawing.Point(7, 50);
			IgnoreEmptyLyric_CheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			IgnoreEmptyLyric_CheckBox.Name = "IgnoreEmptyLyric_CheckBox";
			IgnoreEmptyLyric_CheckBox.Size = new System.Drawing.Size(128, 24);
			IgnoreEmptyLyric_CheckBox.TabIndex = 24;
			IgnoreEmptyLyric_CheckBox.Text = "跳过空白歌词行";
			IgnoreEmptyLyric_CheckBox.UseVisualStyleBackColor = true;
			// 
			// VerbatimLyric_CheckBox
			// 
			VerbatimLyric_CheckBox.Location = new System.Drawing.Point(7, 103);
			VerbatimLyric_CheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			VerbatimLyric_CheckBox.Name = "VerbatimLyric_CheckBox";
			VerbatimLyric_CheckBox.Size = new System.Drawing.Size(128, 24);
			VerbatimLyric_CheckBox.TabIndex = 25;
			VerbatimLyric_CheckBox.Text = "QQ音乐逐字歌词";
			VerbatimLyric_CheckBox.UseVisualStyleBackColor = true;
			VerbatimLyric_CheckBox.CheckedChanged += VerbatimLyric_CheckBox_CheckedChanged;
			// 
			// NetEase_Cookie_TextBox
			// 
			NetEase_Cookie_TextBox.Location = new System.Drawing.Point(122, 106);
			NetEase_Cookie_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			NetEase_Cookie_TextBox.Name = "NetEase_Cookie_TextBox";
			NetEase_Cookie_TextBox.Size = new System.Drawing.Size(376, 23);
			NetEase_Cookie_TextBox.TabIndex = 26;
			// 
			// label2
			// 
			label2.Location = new System.Drawing.Point(7, 110);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(93, 17);
			label2.TabIndex = 27;
			label2.Text = "网易云Cookie";
			// 
			// label4
			// 
			label4.Location = new System.Drawing.Point(7, 173);
			label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(93, 17);
			label4.TabIndex = 28;
			label4.Text = "QQ音乐Cookie";
			// 
			// QQMusic_Cookie_TextBox
			// 
			QQMusic_Cookie_TextBox.Location = new System.Drawing.Point(122, 173);
			QQMusic_Cookie_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			QQMusic_Cookie_TextBox.Name = "QQMusic_Cookie_TextBox";
			QQMusic_Cookie_TextBox.Size = new System.Drawing.Size(376, 23);
			QQMusic_Cookie_TextBox.TabIndex = 29;
			// 
			// Timestamp_GroupBox
			// 
			Timestamp_GroupBox.Controls.Add(Dot_ComboBox);
			Timestamp_GroupBox.Controls.Add(LrcTimestampFormat_TextBox);
			Timestamp_GroupBox.Controls.Add(label8);
			Timestamp_GroupBox.Controls.Add(label6);
			Timestamp_GroupBox.Controls.Add(SrtTimestampFormat_TextBox);
			Timestamp_GroupBox.Controls.Add(label7);
			Timestamp_GroupBox.Location = new System.Drawing.Point(14, 17);
			Timestamp_GroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Timestamp_GroupBox.Name = "Timestamp_GroupBox";
			Timestamp_GroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Timestamp_GroupBox.Size = new System.Drawing.Size(265, 220);
			Timestamp_GroupBox.TabIndex = 30;
			Timestamp_GroupBox.TabStop = false;
			Timestamp_GroupBox.Text = "歌词时间戳";
			// 
			// TimestampHelp_Btn
			// 
			TimestampHelp_Btn.ForeColor = System.Drawing.Color.Red;
			TimestampHelp_Btn.Location = new System.Drawing.Point(247, 17);
			TimestampHelp_Btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			TimestampHelp_Btn.Name = "TimestampHelp_Btn";
			TimestampHelp_Btn.Size = new System.Drawing.Size(24, 30);
			TimestampHelp_Btn.TabIndex = 19;
			TimestampHelp_Btn.Text = "?";
			TimestampHelp_Btn.UseVisualStyleBackColor = true;
			TimestampHelp_Btn.Click += Help_Btn_Click;
			// 
			// SettingTips_TextBox
			// 
			SettingTips_TextBox.BackColor = System.Drawing.SystemColors.Info;
			SettingTips_TextBox.Location = new System.Drawing.Point(564, 259);
			SettingTips_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			SettingTips_TextBox.Multiline = true;
			SettingTips_TextBox.Name = "SettingTips_TextBox";
			SettingTips_TextBox.ReadOnly = true;
			SettingTips_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			SettingTips_TextBox.Size = new System.Drawing.Size(264, 352);
			SettingTips_TextBox.TabIndex = 31;
			// 
			// OutputHelp_Btn
			// 
			OutputHelp_Btn.ForeColor = System.Drawing.Color.Red;
			OutputHelp_Btn.Location = new System.Drawing.Point(496, 0);
			OutputHelp_Btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			OutputHelp_Btn.Name = "OutputHelp_Btn";
			OutputHelp_Btn.Size = new System.Drawing.Size(24, 30);
			OutputHelp_Btn.TabIndex = 20;
			OutputHelp_Btn.Text = "?";
			OutputHelp_Btn.UseVisualStyleBackColor = true;
			OutputHelp_Btn.Click += Help_Btn_Click;
			// 
			// AppConfig_GroupBox
			// 
			AppConfig_GroupBox.Controls.Add(AggregatedBlurSearchCheckBox);
			AppConfig_GroupBox.Controls.Add(AutoCheckUpdate_CheckBox);
			AppConfig_GroupBox.Controls.Add(AutoReadClipboard_CheckBox);
			AppConfig_GroupBox.Controls.Add(RememberParam_CheckBox);
			AppConfig_GroupBox.Controls.Add(NetEase_Cookie_TextBox);
			AppConfig_GroupBox.Controls.Add(label2);
			AppConfig_GroupBox.Controls.Add(QQMusic_Cookie_TextBox);
			AppConfig_GroupBox.Controls.Add(label4);
			AppConfig_GroupBox.Location = new System.Drawing.Point(14, 259);
			AppConfig_GroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			AppConfig_GroupBox.Name = "AppConfig_GroupBox";
			AppConfig_GroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			AppConfig_GroupBox.Size = new System.Drawing.Size(527, 232);
			AppConfig_GroupBox.TabIndex = 32;
			AppConfig_GroupBox.TabStop = false;
			AppConfig_GroupBox.Text = "应用设置";
			// 
			// AggregatedBlurSearchCheckBox
			// 
			AggregatedBlurSearchCheckBox.Location = new System.Drawing.Point(120, 52);
			AggregatedBlurSearchCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			AggregatedBlurSearchCheckBox.Name = "AggregatedBlurSearchCheckBox";
			AggregatedBlurSearchCheckBox.Size = new System.Drawing.Size(114, 24);
			AggregatedBlurSearchCheckBox.TabIndex = 30;
			AggregatedBlurSearchCheckBox.Text = "聚合模糊搜索";
			AggregatedBlurSearchCheckBox.UseVisualStyleBackColor = true;
			// 
			// OriginLyric_GroupBox
			// 
			OriginLyric_GroupBox.Controls.Add(IgnoreEmptyLyric_CheckBox);
			OriginLyric_GroupBox.Controls.Add(VerbatimLyric_CheckBox);
			OriginLyric_GroupBox.Location = new System.Drawing.Point(296, 17);
			OriginLyric_GroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			OriginLyric_GroupBox.Name = "OriginLyric_GroupBox";
			OriginLyric_GroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			OriginLyric_GroupBox.Size = new System.Drawing.Size(245, 220);
			OriginLyric_GroupBox.TabIndex = 33;
			OriginLyric_GroupBox.TabStop = false;
			OriginLyric_GroupBox.Text = "原文歌词";
			// 
			// TransLyric_GroupBox
			// 
			TransLyric_GroupBox.Controls.Add(TransLostRule_ComboBox);
			TransLyric_GroupBox.Controls.Add(label1);
			TransLyric_GroupBox.Controls.Add(TranslateMatchPrecisionDeviation_TextBox);
			TransLyric_GroupBox.Controls.Add(label9);
			TransLyric_GroupBox.Controls.Add(label5);
			TransLyric_GroupBox.Location = new System.Drawing.Point(564, 17);
			TransLyric_GroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			TransLyric_GroupBox.Name = "TransLyric_GroupBox";
			TransLyric_GroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			TransLyric_GroupBox.Size = new System.Drawing.Size(265, 220);
			TransLyric_GroupBox.TabIndex = 34;
			TransLyric_GroupBox.TabStop = false;
			TransLyric_GroupBox.Text = "译文歌词";
			// 
			// LyricShow_DataGridView
			// 
			LyricShow_DataGridView.AllowDrop = true;
			LyricShow_DataGridView.AllowUserToAddRows = false;
			LyricShow_DataGridView.AllowUserToDeleteRows = false;
			LyricShow_DataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
			LyricShow_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			LyricShow_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column1, Column2 });
			LyricShow_DataGridView.Location = new System.Drawing.Point(14, 33);
			LyricShow_DataGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			LyricShow_DataGridView.Name = "LyricShow_DataGridView";
			LyricShow_DataGridView.RowTemplate.Height = 23;
			LyricShow_DataGridView.Size = new System.Drawing.Size(237, 174);
			LyricShow_DataGridView.TabIndex = 37;
			LyricShow_DataGridView.CellContentClick += TransType_DataGridView_CellContentClick;
			LyricShow_DataGridView.CellMouseMove += TransList_DataGridView_CellMouseMove;
			LyricShow_DataGridView.RowsAdded += TransList_DataGridView_RowsAdded;
			LyricShow_DataGridView.DragDrop += TransList_DataGridView_DragDrop;
			LyricShow_DataGridView.DragEnter += TransList_DataGridView_DragEnter;
			// 
			// Column1
			// 
			Column1.HeaderText = "是否启用";
			Column1.Name = "Column1";
			Column1.Width = 60;
			// 
			// Column2
			// 
			Column2.HeaderText = "歌词类型";
			Column2.Name = "Column2";
			// 
			// Output_GroupBox
			// 
			Output_GroupBox.Controls.Add(SeparateFileForIsolated_CheckBox);
			Output_GroupBox.Controls.Add(OutputName_TextBox);
			Output_GroupBox.Controls.Add(label10);
			Output_GroupBox.Controls.Add(IgnorePureMusicInSave_CheckBox);
			Output_GroupBox.Controls.Add(OutputHelp_Btn);
			Output_GroupBox.Controls.Add(LyricShow_DataGridView);
			Output_GroupBox.Location = new System.Drawing.Point(14, 513);
			Output_GroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Output_GroupBox.Name = "Output_GroupBox";
			Output_GroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Output_GroupBox.Size = new System.Drawing.Size(527, 220);
			Output_GroupBox.TabIndex = 35;
			Output_GroupBox.TabStop = false;
			Output_GroupBox.Text = "输出设置";
			// 
			// SeparateFileForIsolated_CheckBox
			// 
			SeparateFileForIsolated_CheckBox.Location = new System.Drawing.Point(264, 86);
			SeparateFileForIsolated_CheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			SeparateFileForIsolated_CheckBox.Name = "SeparateFileForIsolated_CheckBox";
			SeparateFileForIsolated_CheckBox.Size = new System.Drawing.Size(236, 24);
			SeparateFileForIsolated_CheckBox.TabIndex = 39;
			SeparateFileForIsolated_CheckBox.Text = "“独立”歌词格式分文件保存";
			SeparateFileForIsolated_CheckBox.UseVisualStyleBackColor = true;
			// 
			// OutputName_TextBox
			// 
			OutputName_TextBox.Location = new System.Drawing.Point(259, 177);
			OutputName_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			OutputName_TextBox.Name = "OutputName_TextBox";
			OutputName_TextBox.Size = new System.Drawing.Size(242, 23);
			OutputName_TextBox.TabIndex = 38;
			// 
			// label10
			// 
			label10.Location = new System.Drawing.Point(259, 139);
			label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(76, 17);
			label10.TabIndex = 37;
			label10.Text = "保存文件名";
			// 
			// IgnorePureMusicInSave_CheckBox
			// 
			IgnorePureMusicInSave_CheckBox.Location = new System.Drawing.Point(264, 33);
			IgnorePureMusicInSave_CheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			IgnorePureMusicInSave_CheckBox.Name = "IgnorePureMusicInSave_CheckBox";
			IgnorePureMusicInSave_CheckBox.Size = new System.Drawing.Size(108, 24);
			IgnorePureMusicInSave_CheckBox.TabIndex = 36;
			IgnorePureMusicInSave_CheckBox.Text = "跳过纯音乐";
			IgnorePureMusicInSave_CheckBox.UseVisualStyleBackColor = true;
			// 
			// Reset_Btn
			// 
			Reset_Btn.BackColor = System.Drawing.Color.OldLace;
			Reset_Btn.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
			Reset_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			Reset_Btn.Location = new System.Drawing.Point(564, 642);
			Reset_Btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Reset_Btn.Name = "Reset_Btn";
			Reset_Btn.Size = new System.Drawing.Size(113, 71);
			Reset_Btn.TabIndex = 36;
			Reset_Btn.Text = "重置";
			Reset_Btn.UseVisualStyleBackColor = false;
			Reset_Btn.Click += Close_Btn_Click;
			// 
			// SettingForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(845, 746);
			Controls.Add(Reset_Btn);
			Controls.Add(Output_GroupBox);
			Controls.Add(TransLyric_GroupBox);
			Controls.Add(OriginLyric_GroupBox);
			Controls.Add(AppConfig_GroupBox);
			Controls.Add(TimestampHelp_Btn);
			Controls.Add(SettingTips_TextBox);
			Controls.Add(Save_Btn);
			Controls.Add(Timestamp_GroupBox);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			MaximizeBox = false;
			Name = "SettingForm";
			Text = "设置";
			Timestamp_GroupBox.ResumeLayout(false);
			Timestamp_GroupBox.PerformLayout();
			AppConfig_GroupBox.ResumeLayout(false);
			AppConfig_GroupBox.PerformLayout();
			OriginLyric_GroupBox.ResumeLayout(false);
			TransLyric_GroupBox.ResumeLayout(false);
			TransLyric_GroupBox.PerformLayout();
			((ISupportInitialize)LyricShow_DataGridView).EndInit();
			Output_GroupBox.ResumeLayout(false);
			Output_GroupBox.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		private System.Windows.Forms.CheckBox SeparateFileForIsolated_CheckBox;

		private System.Windows.Forms.CheckBox AggregatedBlurSearchCheckBox;

		private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		private System.Windows.Forms.DataGridView LyricShow_DataGridView;

		private System.Windows.Forms.Button Reset_Btn;

		private System.Windows.Forms.TextBox OutputName_TextBox;

		private System.Windows.Forms.Label label10;

		private System.Windows.Forms.GroupBox Output_GroupBox;
		private System.Windows.Forms.CheckBox IgnorePureMusicInSave_CheckBox;

		private System.Windows.Forms.GroupBox TransLyric_GroupBox;

		private System.Windows.Forms.GroupBox OriginLyric_GroupBox;

		private System.Windows.Forms.GroupBox AppConfig_GroupBox;

		private System.Windows.Forms.Button OutputHelp_Btn;

		private System.Windows.Forms.Button TimestampHelp_Btn;

		private System.Windows.Forms.GroupBox Timestamp_GroupBox;
		private System.Windows.Forms.TextBox SettingTips_TextBox;

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox QQMusic_Cookie_TextBox;

		private System.Windows.Forms.TextBox NetEase_Cookie_TextBox;
		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.CheckBox VerbatimLyric_CheckBox;

		private System.Windows.Forms.CheckBox IgnoreEmptyLyric_CheckBox;

		private System.Windows.Forms.Label label5;

		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox TranslateMatchPrecisionDeviation_TextBox;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox TransLostRule_ComboBox;

		private System.Windows.Forms.CheckBox AutoCheckUpdate_CheckBox;

		private System.Windows.Forms.CheckBox AutoReadClipboard_CheckBox;

		private System.Windows.Forms.CheckBox RememberParam_CheckBox;

		private System.Windows.Forms.Button Save_Btn;

		#endregion

		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox LrcTimestampFormat_TextBox;
		private System.Windows.Forms.TextBox SrtTimestampFormat_TextBox;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox Dot_ComboBox;
	}
}
