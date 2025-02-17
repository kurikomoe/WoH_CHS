using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using MyControls;
using MessageFiltering;
using wyControls;

namespace WindowsFormsApplication1
{
    public partial class FormEditor : Form
    {
        // Keep track of the message filter
        private RedirectMessageFilter _messageFilter = null;

        public bool isOpenNew = true;
        public bool fileModified = false;
        public bool fileIsSJIS= false;
        public bool fileHasTwoSeps = true;
        private Color defaultBackColor;

        private string mruExportDir = "";

        public FormEditor()
        {
            InitializeComponent();
            defaultBackColor = richTextBoxMain.BackColor;
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        
        private void buttonOpen_Click(object sender, EventArgs e)
        {

            if (fileModified)
            {
                DialogResult dam = MessageBox.Show(this,
                    "Current file has not been saved.\r\nReally want to open a new one?", "",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2
                    );
                if (dam == DialogResult.No)
                {
                    return;
                }
            }
            fileIsSJIS = false;
            fileModified = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "UTF-8 Script File|*.utf|UTF-8 Text File|*.txt|Shift JIS Script files|*.sjs|Shift JIS Text files|*.txt|UTF-8 All Files|*.*|Shift JIS All Files|*.*";
            DialogResult da = ofd.ShowDialog();
            if (da == DialogResult.OK)
            {
                isOpenNew = false;

                if (ofd.FilterIndex == 3 || ofd.FilterIndex == 4 || ofd.FilterIndex == 6)
                {
                    fileIsSJIS = true;
                }

                listViewEditor.Items.Clear();

                UpdatelistViewEditor(ofd.FileName);
                UpdateTitle(toolStripStatusLabelFileName.Text);

                timerAutoSave.Enabled = true;

                try
                {
                    listViewEditor.Items[0].Selected = true;
                    listViewEditor.Items[0].Focused = true;
                    listViewEditor.Select();
                    buttonNext_Click(null, new EventArgs());
                }
                catch (Exception)
                {
                }
            }
        }

        // loading from file: open button or drag/drop
        public bool UpdatelistViewEditor(string fileName)
        {
            toolStripProgressBarPercent.Style = ProgressBarStyle.Marquee;

            int intLineNumber = 1;

            StreamReader sr;

            if (fileIsSJIS == false)
            {
                sr = new StreamReader(new FileStream(fileName, FileMode.Open), Encoding.UTF8, true);
            }
            else
            {
                sr = new StreamReader(new FileStream(fileName, FileMode.Open), Encoding.GetEncoding(932), true);
            }
            string fileContents = sr.ReadToEnd();
            sr.Close();

            if (fileContents.IndexOf(@"\=") == -1)
            {
                fileHasTwoSeps = false;
            }

            fileContents = fileContents.Replace("\r\n", "\n");
            string[] fileLines = fileContents.Split('\n');
            foreach (string stringLine in fileLines)
            {
                string stringCurrentLine;
                //Format:   <0012>_ZM00305(　努力しようと口で言うのは<容易,たやす>く、実行するのはとてもとても難しい……特に今の状況では。^)
                stringCurrentLine = ReplaceFirst(stringLine, "<", "");
                stringCurrentLine = ReplaceFirst(stringCurrentLine, ">", "■");

                if (fileHasTwoSeps)
                {
                    stringCurrentLine = ReplaceFirst(stringCurrentLine, @"\=", "■");
                }
                else
                {
                    stringCurrentLine = ReplaceFirst(stringCurrentLine, "=", "■");
                }


                string[] stringSplit = stringCurrentLine.Split('■');

                ListViewItem lvi = null;
                if (stringSplit.Length == 3)//Standard Format(You have translated this line.)
                {
                    lvi = listViewEditor.Items.Add(new ListViewItem(new string[] { stringSplit[0], (intLineNumber.ToString()), stringSplit[1], stringSplit[2] }, -1));
                }
                else if (stringSplit.Length == 2)//Standard Format(You HAVE NOT translated this line.)
                {
                    lvi = listViewEditor.Items.Add(new ListViewItem(new string[] { stringSplit[0], (intLineNumber.ToString()), stringSplit[1], "" }, -1));
                }
                else if ((stringCurrentLine.StartsWith("~")) && (stringCurrentLine.EndsWith("~")))//This line is a function.
                {
                    lvi = listViewEditor.Items.Add(new ListViewItem(new string[] { stringCurrentLine, (intLineNumber.ToString()), "", "" }, -1));
                }
                else//I dont know either...(but always a blank line)
                {
                    lvi = listViewEditor.Items.Add(new ListViewItem(new string[] { stringCurrentLine, (intLineNumber.ToString()), "", "" }, -1));
                }

                if (lvi.SubItems[3].Text != String.Empty)
                {
                    lvi.BackColor = System.Drawing.Color.GreenYellow;
                }
                else if (lvi.SubItems[2].Text == String.Empty)
                {
                    lvi.BackColor = System.Drawing.Color.FromName("InactiveCaption");
                }

                intLineNumber++;
            }

            toolStripStatusLabelFileName.Text = fileName;
            toolStripStatusLabelLine.Text = listViewEditor.Items.Count.ToString();

            toolStripProgressBarPercent.Style = ProgressBarStyle.Blocks;

            return true;
        }

        // saving to file: save button or autosave timer
        public bool SaveListViewEditor(string fileName)
        {
            return SaveListViewEditor(fileName, false);
        }

        private delegate string LocalizableViewItemToString(ListViewItem lvi);

        // saving to file: [save button or autosave timer]overload or export
        public bool SaveListViewEditor(string fileName, bool exporting)
        {
            StreamWriter sw;

            if (fileIsSJIS == false)
            {
                sw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8);
            }
            else
            {
                sw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.GetEncoding(932));
            }

            LocalizableViewItemToString fileHasTwoSepswriteDelegate;
            LocalizableViewItemToString fileHasNotTwoSepswriteDelegate;
            if (exporting)
            {
                fileHasTwoSepswriteDelegate = delegate(ListViewItem lvi)
                {
                    if (lvi.SubItems[3].Text.Equals("_r"))
                    {
                        return "";
                    }
                    return lvi.SubItems[3].Text.Replace("_n", "@n").Replace("_r", "^");  // returned line is WriteLine'd, so those are extra ones.
                };
                fileHasNotTwoSepswriteDelegate = fileHasTwoSepswriteDelegate;
            }
            else  // saving
            {
                fileHasTwoSepswriteDelegate = delegate(ListViewItem lvi)
                {
                    return "<" + lvi.SubItems[0].Text + ">" + lvi.SubItems[2].Text + @"\=" + lvi.SubItems[3].Text;
                };
                fileHasNotTwoSepswriteDelegate = delegate(ListViewItem lvi)
                {
                    return "<" + lvi.SubItems[0].Text + ">" + lvi.SubItems[2].Text + "=" + lvi.SubItems[3].Text;
                };
            }

            foreach (ListViewItem lvi in listViewEditor.Items)
            {
                if (lvi.SubItems[0].Text.StartsWith(" ~") && lvi.SubItems[0].Text.EndsWith("~ "))//This line is a function.
                {
                    sw.WriteLine(lvi.SubItems[0].Text);// + (char)0x200a);
                }
                else if (lvi.SubItems[2].Text != "")//Standard Format
                {
                    string crtLine;

                    if (fileHasTwoSeps)
                    {
                        crtLine = fileHasTwoSepswriteDelegate(lvi);
                    }
                    else
                    {
                        crtLine = fileHasNotTwoSepswriteDelegate(lvi);
                    }
                    sw.WriteLine(crtLine);
                }
                else
                {
                    sw.WriteLine(lvi.SubItems[0].Text);
                }
            }
            sw.Close();

            return true;
        }

        private void listViewEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEditor.SelectedItems.Count > 0)
            {
                toolStripStatusLabelCurrent.Text = listViewEditor.SelectedItems[0].SubItems[1].Text;

                try
                {
                    textBoxLookup.Text = listViewEditor.SelectedItems[0].SubItems[2].Text;
                }
                catch (ArgumentOutOfRangeException)//Original Line is Empty
                {
                    textBoxLookup.Text = "";
                }

                try
                {
                    // richTextBoxMain is a transformed view of actual data. It will be converted back on commit.
                    richTextBoxMain.Text = listViewEditor.SelectedItems[0].SubItems[3].Text.Replace("_r", "\n"); //.Replace("_n", "@n");
                    if (checkBoxPreFillNew.Checked && textBoxLookup.Text.Length > 0 && richTextBoxMain.Text.Length == 0)
                    {
                        richTextBoxMain.Text = textBoxLookup.Text.Replace("_r", "\n"); //.Replace("_n", "@n");
                        richTextBoxMain.BackColor = Color.Aquamarine;
                    }
                }
                catch (ArgumentOutOfRangeException)//Current Line has not been translateed yet
                {
                    richTextBoxMain.Text = "";
                }
            }
        }

        // bottom toolbar's Save current for displayed line
        private void buttonSaveCurrent_Click(object sender, EventArgs e)
        {
            if (isOpenNew)
            {
                return;
            }

            fileModified = true;

            ListViewItem lvi = listViewEditor.SelectedItems[0];
            try
            {
                // convert textbox view to actual data
                lvi.SubItems[3].Text = richTextBoxMain.Text/*.Replace("@n", "_n")*/.Replace("\n", "_r");
            }
            catch (Exception)
            {
            }

            if (lvi.SubItems[3].Text == String.Empty)
            {
                lvi.BackColor = System.Drawing.Color.Empty;
            }
            else
            {
                lvi.BackColor = System.Drawing.Color.GreenYellow;
                richTextBoxMain.BackColor = Color.LightGreen;
            }

            EventArgs ea = new EventArgs();
            buttonNext_Click(sender, ea);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (isOpenNew || listViewEditor.Items.Count < 2)
            {
                return;
            }

            int orgIndex = listViewEditor.FocusedItem.Index;

            for (int index = orgIndex + 1;
                 index != orgIndex;
                 ++index)
            {
                if (index >= listViewEditor.Items.Count)
                {
                    index = 0;
                }

                ListViewItem lvi = listViewEditor.Items[index];
                if (lvi.SubItems[2].Text != String.Empty)
                {
                    try
                    {
                        lvi.Selected = true;
                        lvi.Focused = true;
                        lvi.EnsureVisible();
                        listViewEditor.Select();
                        
                    }
                    catch (Exception)
                    {
                    }
                    return;
                }
            }
        }

        private void listViewEditor_Enter(object sender, EventArgs e)
        {
            richTextBoxMain.Focus();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (isOpenNew)
            {
                return;
            }

            try
            {
                listViewEditor.Items[listViewEditor.FocusedItem.Index - 1].Selected = true;
                listViewEditor.Items[listViewEditor.FocusedItem.Index - 1].Focused = true;
                listViewEditor.Items[listViewEditor.FocusedItem.Index].EnsureVisible();
                listViewEditor.Select();
            }
            catch (Exception)
            {
            }
        }


        private void richTextBoxMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                if (e.KeyCode == Keys.Z)//Undo
                {
                    richTextBoxMain.Undo();
                }
                else if (e.KeyCode == Keys.Y)//Redo
                {
                    richTextBoxMain.Redo();
                }
                else if (e.KeyCode == Keys.X)//Cut
                {
                    richTextBoxMain.Cut();
                }
                else if (e.KeyCode == Keys.C)//Copy
                {
                    richTextBoxMain.Copy();
                }
                else if (e.KeyCode == Keys.V)//Paste
                {
                    richTextBoxMain.Paste();
                }
                else if (e.KeyCode == Keys.D)//Clear
                {
                    richTextBoxMain.Clear();
                }
                else if (e.KeyCode == Keys.A)//SelectAll
                {
                    richTextBoxMain.SelectAll();
                }
                else
                {
                    return;
                }
                e.Handled = true;
            }
            else
            {
                // Redirect those keypresses to the right control
                switch(e.KeyCode)
                {
                    case Keys.PageDown:
                        RedirectMessageFilter.PostMessage(listViewEditor.Handle, (int)WindowsMessages.WM_KEYDOWN, new IntPtr((int)VirtualKeys.VK_NEXT), IntPtr.Zero);
                        break;
                    case Keys.PageUp:
                        RedirectMessageFilter.PostMessage(listViewEditor.Handle, (int)WindowsMessages.WM_KEYDOWN, new IntPtr((int)VirtualKeys.VK_PRIOR), IntPtr.Zero);
                        break;
//                    case Keys.Home:
//                        RedirectMessageFilter.PostMessage(listViewEditor.Handle, (int)WindowsMessages.WM_KEYDOWN, new IntPtr((int)VirtualKeys.VK_HOME), IntPtr.Zero);
//                        break;
//                    case Keys.End:
//                        RedirectMessageFilter.PostMessage(listViewEditor.Handle, (int)WindowsMessages.WM_KEYDOWN, new IntPtr((int)VirtualKeys.VK_END), IntPtr.Zero);
//                        break;
                    default:
                        return;
                }
                e.Handled = true;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Paste();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Clear();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxMain.SelectAll();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Redo();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // bottom toolbar's Save (splitbutton) is clicked
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (isOpenNew)
            {
                return;
            }
            try
            {
                File.Copy(toolStripStatusLabelFileName.Text, toolStripStatusLabelFileName.Text + ".bak");
                File.Delete(toolStripStatusLabelFileName.Text + ".autosave");
            }
            catch (Exception)
            {
            }
            try
            {
                SaveListViewEditor(toolStripStatusLabelFileName.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, String.Format("Could not save to file : {0}", exc.Message), "File Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            fileModified = false;
            UpdateTitle(toolStripStatusLabelFileName.Text);
            MessageBox.Show(this, "File successfully saved.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripStatusLabelCurrent_TextChanged(object sender, EventArgs e)
        {
            RenewProgressBar();
        }

        public void RenewProgressBar()
        {
            int intTotalLine = 0;
            int intTranslatedLine = 0;

            foreach (ListViewItem lvi in listViewEditor.Items)
            {
                if (lvi.SubItems[2].Text != "")
                {
                    intTotalLine++;

                    if (lvi.SubItems[3].Text != "")
                    {
                        intTranslatedLine++;
                    }
                }

            }
            if (intTotalLine > 0)
            {
                toolStripProgressBarPercent.Value = (int)(((float)intTranslatedLine / (float)intTotalLine) * 100);
            }
        }

        private void richTextBoxMain_TextChanged(object sender, EventArgs e)
        {
            richTextBoxMain.BackColor = defaultBackColor;  // revert when editing
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!isOpenNew)
            {
                if (fileModified)
                {
                    DialogResult dam = MessageBox.Show(this,
                        "Current file has not been saved.\r\nReally want to quit?", "",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2
                        );
                    if (dam == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void FormEditor_SizeChanged(object sender, EventArgs e)
        {
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void richTextBoxMain_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBoxLookup_TextChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoCopy.Checked && textBoxLookup.Text.Length > 0)
            {
                Clipboard.SetText(textBoxLookup.Text);
            }
        }

        // Triggered by Auto Save timer
        private void timerAutoSave_Tick(object sender, EventArgs e)
        {
            SaveListViewEditor(toolStripStatusLabelFileName.Text + ".autosave");
        }

        // Remove any active message filter
        private void RemoveMessageFilter()
        {
            if (_messageFilter != null)
            {
                Application.RemoveMessageFilter(_messageFilter);
                _messageFilter = null;
            }
        }

        // Add filter to redirect to control with handle hWnd
        private void AddMessageFilter(IntPtr hWnd)
        {
            _messageFilter = new RedirectMessageFilter(WindowsMessages.WM_MOUSEWHEEL, hWnd);
            Application.AddMessageFilter(_messageFilter);
        }

        // Mouse hovers listView, install mouse scroll events capture
        private void listViewEditor_MouseEnter(object sender, EventArgs e)
        {
            RemoveMessageFilter();
            System.Windows.Forms.ListView listView = (System.Windows.Forms.ListView)sender; // get what listbox it was sent from
            AddMessageFilter(listView.Handle);
        }

        private void listViewEditor_MouseLeave(object sender, EventArgs e)
        {
            RemoveMessageFilter();
        }

        private void UpdateTitle(string title)
        {
            if (isOpenNew)
            {
                return;
            }
            string prefix = (fileModified ? "* " : "");
            Text = prefix + title;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                if ( isOpenNew )
                {
                    EncodingSelectionDialog enc = new EncodingSelectionDialog();
                    enc.ShowDialog(this);
                    fileIsSJIS = (enc.DialogResult == DialogResult.Cancel);
                    isOpenNew = false;
                }

                listViewEditor.Items.Clear();

                UpdatelistViewEditor(fileNames[0]);
                UpdateTitle(toolStripStatusLabelFileName.Text);

                timerAutoSave.Enabled = true;

                try
                {
                    listViewEditor.Items[0].Selected = true;
                    listViewEditor.Items[0].Focused = true;
                    listViewEditor.Select();
                    buttonNext_Click(null, new EventArgs());
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                if (e.KeyCode == Keys.O)
                {
                    buttonOpen_Click(sender, new EventArgs());
                }
                else if (e.KeyCode == Keys.S)
                {
                    buttonSave_Click(sender, new EventArgs());
                }
                else if (e.KeyCode == Keys.E)
                {
                    splitBtnExportMenuItem_Click(sender, new EventArgs());
                }
                else if (e.KeyCode == Keys.Enter)//Save current and Next
                {
                    buttonSaveCurrent_Click(sender, new EventArgs());
                }
                else if (e.KeyCode == Keys.Up)//Prev line
                {
                    buttonPrev_Click(sender, new EventArgs());
                }
                else if (e.KeyCode == Keys.Down)//Next line
                {
                    buttonNext_Click(sender, new EventArgs());
                }
                else
                {
                    return;
                }
                e.Handled = true;
            }
        }

        private void splitBtnSaveMenuItem_Click(object sender, EventArgs e)
        {
            buttonSave_Click(sender, e);
        }

        // bottom toolbar's Save (splitbutton): Export is clicked
        private void splitBtnExportMenuItem_Click(object sender, EventArgs e)
        {
            if (isOpenNew)
            {
                return;
            }
            if (fileModified)
            {
                MessageBox.Show(this, "Please commit changes to file prior to exporting.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            string ext = Path.GetExtension(toolStripStatusLabelFileName.Text);
            if (ext.Length == 0)
            {
                ext = ".*";
            }
            else
            {
                sfd.DefaultExt = ext.Remove(0, 1);
            }
            string mask = "*" + ext;
            sfd.Filter = "Export File|" + mask + "|All Files|*.*";
            sfd.InitialDirectory = mruExportDir.Length > 0 ? mruExportDir : Path.GetDirectoryName(toolStripStatusLabelFileName.Text);
            sfd.FileName = Path.GetFileName(toolStripStatusLabelFileName.Text);

            
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                mruExportDir = Path.GetDirectoryName(sfd.FileName);
                try
                {
                    SaveListViewEditor(sfd.FileName, true);
                    MessageBox.Show(this, "File successfully exported.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(this, String.Format("Could not export to file : {0}", exc.Message), "File Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

    }
}
