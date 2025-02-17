namespace WindowsFormsApplication1
{
    partial class EncodingSelectionDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelOkButton = new System.Windows.Forms.Label();
            this.labelCancelButton = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelOkButton, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCancelButton, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.okButton, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.cancelButton, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(360, 76);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelOkButton
            // 
            this.labelOkButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelOkButton.Location = new System.Drawing.Point(87, 9);
            this.labelOkButton.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelOkButton.Name = "labelOkButton";
            this.labelOkButton.Size = new System.Drawing.Size(270, 20);
            this.labelOkButton.TabIndex = 1;
            this.labelOkButton.Text = "Interpret using the Unicode representation.";
            this.labelOkButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCancelButton
            // 
            this.labelCancelButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCancelButton.Location = new System.Drawing.Point(87, 47);
            this.labelCancelButton.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelCancelButton.Name = "labelCancelButton";
            this.labelCancelButton.Size = new System.Drawing.Size(270, 20);
            this.labelCancelButton.TabIndex = 3;
            this.labelCancelButton.Text = "Interprets using the Shift-JIS (CP932) encoding.";
            this.labelCancelButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(3, 7);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "&UTF-8";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(3, 45);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Shift-&JIS";
            // 
            // EncodingSelectionDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 94);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EncodingSelectionDialog";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Encoding";
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelOkButton;
        private System.Windows.Forms.Label labelCancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}
