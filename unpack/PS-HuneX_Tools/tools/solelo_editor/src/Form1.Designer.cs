using wyControls;

namespace WindowsFormsApplication1
{
    partial class FormEditor
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listViewEditor = new System.Windows.Forms.ListView();
            this.columnHeaderOffset = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNumber = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOrg = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOK = new System.Windows.Forms.ColumnHeader();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelFileName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCurrent = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarPercent = new System.Windows.Forms.ToolStripProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSaveCurrent = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.splitBtnSaveMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitBtnSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitBtnExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxLookup = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timerAutoSave = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxAutoCopy = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxPreFillNew = new System.Windows.Forms.CheckBox();
            this.richTextBoxMain = new MyControls.PaddedRichTextBox();
            this.buttonSave = new wyControls.SplitButton();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitBtnSaveMenuStrip.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewEditor
            // 
            this.listViewEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewEditor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderOffset,
            this.columnHeaderNumber,
            this.columnHeaderOrg,
            this.columnHeaderOK});
            this.listViewEditor.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listViewEditor.FullRowSelect = true;
            this.listViewEditor.GridLines = true;
            this.listViewEditor.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewEditor.HideSelection = false;
            this.listViewEditor.LabelWrap = false;
            this.listViewEditor.Location = new System.Drawing.Point(0, 0);
            this.listViewEditor.Margin = new System.Windows.Forms.Padding(4);
            this.listViewEditor.MultiSelect = false;
            this.listViewEditor.Name = "listViewEditor";
            this.listViewEditor.Size = new System.Drawing.Size(1142, 459);
            this.listViewEditor.TabIndex = 2;
            this.listViewEditor.UseCompatibleStateImageBehavior = false;
            this.listViewEditor.View = System.Windows.Forms.View.Details;
            this.listViewEditor.SelectedIndexChanged += new System.EventHandler(this.listViewEditor_SelectedIndexChanged);
            this.listViewEditor.Enter += new System.EventHandler(this.listViewEditor_Enter);
            this.listViewEditor.MouseEnter += new System.EventHandler(this.listViewEditor_MouseEnter);
            this.listViewEditor.MouseLeave += new System.EventHandler(this.listViewEditor_MouseLeave);
            // 
            // columnHeaderOffset
            // 
            this.columnHeaderOffset.Text = "Offset";
            this.columnHeaderOffset.Width = 80;
            // 
            // columnHeaderNumber
            // 
            this.columnHeaderNumber.Text = "Number";
            this.columnHeaderNumber.Width = 49;
            // 
            // columnHeaderOrg
            // 
            this.columnHeaderOrg.Text = "Original Text";
            this.columnHeaderOrg.Width = 419;
            // 
            // columnHeaderOK
            // 
            this.columnHeaderOK.Text = "Translated text";
            this.columnHeaderOK.Width = 388;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelFileName,
            this.toolStripStatusLabelCurrent,
            this.toolStripStatusLabelLine,
            this.toolStripProgressBarPercent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 767);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1143, 27);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip2";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // toolStripStatusLabelFileName
            // 
            this.toolStripStatusLabelFileName.AutoSize = false;
            this.toolStripStatusLabelFileName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelFileName.Name = "toolStripStatusLabelFileName";
            this.toolStripStatusLabelFileName.Size = new System.Drawing.Size(828, 22);
            this.toolStripStatusLabelFileName.Spring = true;
            this.toolStripStatusLabelFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelCurrent
            // 
            this.toolStripStatusLabelCurrent.AutoSize = false;
            this.toolStripStatusLabelCurrent.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelCurrent.Name = "toolStripStatusLabelCurrent";
            this.toolStripStatusLabelCurrent.Size = new System.Drawing.Size(80, 22);
            this.toolStripStatusLabelCurrent.TextChanged += new System.EventHandler(this.toolStripStatusLabelCurrent_TextChanged);
            // 
            // toolStripStatusLabelLine
            // 
            this.toolStripStatusLabelLine.AutoSize = false;
            this.toolStripStatusLabelLine.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelLine.Name = "toolStripStatusLabelLine";
            this.toolStripStatusLabelLine.Size = new System.Drawing.Size(80, 22);
            // 
            // toolStripProgressBarPercent
            // 
            this.toolStripProgressBarPercent.AutoSize = false;
            this.toolStripProgressBarPercent.MarqueeAnimationSpeed = 10;
            this.toolStripProgressBarPercent.Name = "toolStripProgressBarPercent";
            this.toolStripProgressBarPercent.Size = new System.Drawing.Size(133, 21);
            this.toolStripProgressBarPercent.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonSaveCurrent);
            this.panel1.Controls.Add(this.buttonQuit);
            this.panel1.Controls.Add(this.buttonNext);
            this.panel1.Controls.Add(this.buttonPrev);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonOpen);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 721);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1143, 46);
            this.panel1.TabIndex = 0;
            // 
            // buttonSaveCurrent
            // 
            this.buttonSaveCurrent.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonSaveCurrent.AutoSize = true;
            this.buttonSaveCurrent.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonSaveCurrent.Image = global::UTF_Editor.Properties.Resources.yes;
            this.buttonSaveCurrent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSaveCurrent.Location = new System.Drawing.Point(395, 0);
            this.buttonSaveCurrent.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveCurrent.Name = "buttonSaveCurrent";
            this.buttonSaveCurrent.Size = new System.Drawing.Size(188, 46);
            this.buttonSaveCurrent.TabIndex = 2;
            this.buttonSaveCurrent.Text = "Save Current Line";
            this.buttonSaveCurrent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSaveCurrent.UseVisualStyleBackColor = false;
            this.buttonSaveCurrent.Click += new System.EventHandler(this.buttonSaveCurrent_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuit.Location = new System.Drawing.Point(953, 0);
            this.buttonQuit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(173, 46);
            this.buttonQuit.TabIndex = 5;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonNext.Image = global::UTF_Editor.Properties.Resources.down;
            this.buttonNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonNext.Location = new System.Drawing.Point(772, 0);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(173, 46);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.Text = "Next Line";
            this.buttonNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonNext, "Ctrl+Down: Go to Next Line");
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPrev.Image = global::UTF_Editor.Properties.Resources.up;
            this.buttonPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPrev.Location = new System.Drawing.Point(591, 0);
            this.buttonPrev.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(173, 46);
            this.buttonPrev.TabIndex = 3;
            this.buttonPrev.Text = "Previous Line";
            this.buttonPrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonPrev, "Ctrl+Up: Go to Previous Line");
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // splitBtnSaveMenuStrip
            // 
            this.splitBtnSaveMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.splitBtnSaveMenuItem,
            this.splitBtnExportMenuItem});
            this.splitBtnSaveMenuStrip.Name = "splitBtnSaveMenuStrip";
            this.splitBtnSaveMenuStrip.Size = new System.Drawing.Size(258, 60);
            // 
            // splitBtnSaveMenuItem
            // 
            this.splitBtnSaveMenuItem.Image = global::UTF_Editor.Properties.Resources.download;
            this.splitBtnSaveMenuItem.Name = "splitBtnSaveMenuItem";
            this.splitBtnSaveMenuItem.Size = new System.Drawing.Size(257, 28);
            this.splitBtnSaveMenuItem.Text = "&Save File";
            this.splitBtnSaveMenuItem.Click += new System.EventHandler(this.splitBtnSaveMenuItem_Click);
            // 
            // splitBtnExportMenuItem
            // 
            this.splitBtnExportMenuItem.Image = global::UTF_Editor.Properties.Resources.text;
            this.splitBtnExportMenuItem.Name = "splitBtnExportMenuItem";
            this.splitBtnExportMenuItem.Size = new System.Drawing.Size(257, 28);
            this.splitBtnExportMenuItem.Text = "&Export Localization to...";
            this.splitBtnExportMenuItem.Click += new System.EventHandler(this.splitBtnExportMenuItem_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Image = global::UTF_Editor.Properties.Resources.upload;
            this.buttonOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOpen.Location = new System.Drawing.Point(16, 0);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(173, 46);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "Open File";
            this.buttonOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem1,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selectAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 212);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(149, 28);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(149, 28);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(149, 28);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(149, 28);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(149, 28);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(149, 28);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(146, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(149, 28);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // textBoxLookup
            // 
            this.textBoxLookup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLookup.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxLookup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLookup.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxLookup.HideSelection = false;
            this.textBoxLookup.Location = new System.Drawing.Point(0, 466);
            this.textBoxLookup.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLookup.Multiline = true;
            this.textBoxLookup.Name = "textBoxLookup";
            this.textBoxLookup.ReadOnly = true;
            this.textBoxLookup.Size = new System.Drawing.Size(1142, 40);
            this.textBoxLookup.TabIndex = 3;
            this.textBoxLookup.TabStop = false;
            this.textBoxLookup.TextChanged += new System.EventHandler(this.textBoxLookup_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 694);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1141, 22);
            this.label1.TabIndex = 5;
            this.label1.Text = "[Ctrl + Up]Previous Line   [Ctrl + Down]Next Line   [Ctrl + Enter]Save Current Li" +
                "ne";
            // 
            // timerAutoSave
            // 
            this.timerAutoSave.Interval = 18000;
            this.timerAutoSave.Tick += new System.EventHandler(this.timerAutoSave_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxMain, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 508);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1143, 182);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // checkBoxAutoCopy
            // 
            this.checkBoxAutoCopy.AccessibleDescription = "Whether to copy to clipboard on Line selection";
            this.checkBoxAutoCopy.AccessibleName = "AutoCopy";
            this.checkBoxAutoCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoCopy.AutoSize = true;
            this.checkBoxAutoCopy.Location = new System.Drawing.Point(960, 692);
            this.checkBoxAutoCopy.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAutoCopy.Name = "checkBoxAutoCopy";
            this.checkBoxAutoCopy.Size = new System.Drawing.Size(175, 21);
            this.checkBoxAutoCopy.TabIndex = 6;
            this.checkBoxAutoCopy.Text = "Auto Copy to Clipboard";
            this.toolTip1.SetToolTip(this.checkBoxAutoCopy, "Copies to clipboard on line selection");
            this.checkBoxAutoCopy.UseMnemonic = false;
            this.checkBoxAutoCopy.UseVisualStyleBackColor = true;
            // 
            // checkBoxPreFillNew
            // 
            this.checkBoxPreFillNew.AccessibleDescription = "Prefill";
            this.checkBoxPreFillNew.AccessibleName = "Whether to automatically fill textbox for untranslated lines";
            this.checkBoxPreFillNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxPreFillNew.AutoSize = true;
            this.checkBoxPreFillNew.Location = new System.Drawing.Point(839, 692);
            this.checkBoxPreFillNew.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxPreFillNew.Name = "checkBoxPreFillNew";
            this.checkBoxPreFillNew.Size = new System.Drawing.Size(86, 21);
            this.checkBoxPreFillNew.TabIndex = 6;
            this.checkBoxPreFillNew.Text = "Prefill TL";
            this.toolTip1.SetToolTip(this.checkBoxPreFillNew, "Automatically fills textbox for untranslated lines");
            this.checkBoxPreFillNew.UseMnemonic = false;
            this.checkBoxPreFillNew.UseVisualStyleBackColor = true;
            // 
            // richTextBoxMain
            // 
            this.richTextBoxMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxMain.BackColor = System.Drawing.Color.LemonChiffon;
            this.richTextBoxMain.BorderColor = System.Drawing.Color.LemonChiffon;
            this.richTextBoxMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxMain.BorderWidth = 10;
            this.richTextBoxMain.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBoxMain.DetectUrls = false;
            this.richTextBoxMain.FixedSingleLineColor = System.Drawing.Color.Moccasin;
            this.richTextBoxMain.FixedSingleLineWidth = 1;
            this.richTextBoxMain.Font = new System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.richTextBoxMain.Location = new System.Drawing.Point(24, 4);
            this.richTextBoxMain.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxMain.Name = "richTextBoxMain";
            this.richTextBoxMain.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBoxMain.Size = new System.Drawing.Size(1095, 174);
            this.richTextBoxMain.TabIndex = 0;
            this.richTextBoxMain.Text = "";
            this.richTextBoxMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBoxMain_KeyDown);
            this.richTextBoxMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.richTextBoxMain_KeyUp);
            this.richTextBoxMain.TextChanged += new System.EventHandler(this.richTextBoxMain_TextChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.AutoSize = true;
            this.buttonSave.ContextMenuStrip = this.splitBtnSaveMenuStrip;
            this.buttonSave.Image = global::UTF_Editor.Properties.Resources.download;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSave.Location = new System.Drawing.Point(197, 0);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.buttonSave.Size = new System.Drawing.Size(173, 46);
            this.buttonSave.SplitMenuStrip = this.splitBtnSaveMenuStrip;
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save File";
            this.buttonSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 794);
            this.Controls.Add(this.checkBoxAutoCopy);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.checkBoxPreFillNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBoxLookup);
            this.Controls.Add(this.listViewEditor);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormEditor";
            this.Text = "UTF Editor";
            this.SizeChanged += new System.EventHandler(this.FormEditor_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitBtnSaveMenuStrip.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewEditor;
        private System.Windows.Forms.ColumnHeader columnHeaderOffset;
        private System.Windows.Forms.ColumnHeader columnHeaderNumber;
        private System.Windows.Forms.ColumnHeader columnHeaderOrg;
        private System.Windows.Forms.ColumnHeader columnHeaderOK;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFileName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLine;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarPercent;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonSaveCurrent;
        private wyControls.SplitButton buttonSave;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.TextBox textBoxLookup;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurrent;
        private MyControls.PaddedRichTextBox richTextBoxMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerAutoSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBoxAutoCopy;
        private System.Windows.Forms.ContextMenuStrip splitBtnSaveMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem splitBtnSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem splitBtnExportMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxPreFillNew;

    }
}

