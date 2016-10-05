using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Collections;
using System.Drawing;

namespace FileFilter
{
    public partial class Form1 : MaterialForm
    {
        string targetPath = "C:/Windows";
        List<string> folderPaths = new List<string>();

        Dictionary<string, List<FileInfo>> Files = new Dictionary<string, List<FileInfo>>();
        Dictionary<string, List<DoublePath>> FileHashed = new Dictionary<string, List<DoublePath>>();
        float progresscount = 0;
        float currentprogress = 0;
        private Object thisLock = new Object();
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            foreach (string path in folderPaths)
            {
                Files[path] = new List<FileInfo>();
                var folderPath = new DirectoryInfo(path);
                FileInfo[] tempfiles = folderPath.GetFiles("*", System.IO.SearchOption.AllDirectories);
                foreach(FileInfo fi in tempfiles)
                {
                    try
                    {
                        string test = fi.FullName;
                        Files[path].Add(fi);
                    }
                    catch  { }
                }
            }
            foreach (string toplevelfolder in Files.Keys)
            {
                progresscount += Files[toplevelfolder].Count;
            }
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            backgroundWorker1.RunWorkerAsync();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            var backgroundWorker = sender as BackgroundWorker;
            FileHashed = new Dictionary<string, List<DoublePath>>();
            foreach (string toplevelfolder in Files.Keys)
            {
                //parallelizing might help speed if top level folders are on different drives
                Parallel.ForEach(Files[toplevelfolder], file =>
                {
                    using (var stream = File.OpenRead(file.FullName))
                    {
                        if (stream != null)
                        {
                            byte[] streamBuffer;
                            if (scanWholeFileCheckBox.Checked)
                            {
                                //If file is larger than 2GB, we will stop there.
                                int len = (int)Math.Min(stream.Length, int.MaxValue);
                                streamBuffer = new byte[len];
                                stream.Read(streamBuffer, 0, len);
                            }
                            else
                            {
                                //4kb is enough to determine uniqueness on basically any binary, and is probably overkill at that
                                streamBuffer = new byte[4096];
                                stream.Read(streamBuffer, 0, 4096);
                            }

                            string hash;
                            using (var md5 = MD5.Create())
                            {
                                hash = BitConverter.ToString(md5.ComputeHash(streamBuffer)).Replace("-", "");
                            }
                            if (!string.IsNullOrEmpty(hash))
                            {
                                lock (thisLock)
                                {
                                    if (!FileHashed.ContainsKey(hash))
                                    {
                                        FileHashed[hash] = new List<DoublePath>();
                                        FileHashed[hash].Add(new DoublePath() { FullPath = file.FullName, LocalPath = file.FullName.Replace(toplevelfolder, ""), Size = file.Length });
                                    }
                                    else
                                    {
                                        FileHashed[hash].Add(new DoublePath() { FullPath = file.FullName, LocalPath = file.FullName.Replace(toplevelfolder, ""), Size = file.Length });
                                    }
                                }
                            }
                            currentprogress++;
                            backgroundWorker.ReportProgress((int)((currentprogress / progresscount) * 100));
                        }
                    }
                });
            }
            backgroundWorker.ReportProgress(100);
        }
        private void combineButton_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(targetPath))
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                Parallel.ForEach(FileHashed.Keys, file =>
                {
                    DoublePath unique = FileHashed[file].First();
                    string newroot = targetPath;

                    string fileName = System.IO.Path.GetFileNameWithoutExtension(unique.FullPath);
                    //string fileNamewext = System.IO.Path.GetFileName(unique.FullPath);
                    string extension = System.IO.Path.GetExtension(unique.FullPath);
                    //string oldroot = System.IO.Path.GetPathRoot(unique.FullPath);
                    //string oldmiddle = unique.FullPath.Replace(oldroot, "").Replace(fileNamewext, "");

                    string destFile = System.IO.Path.Combine(newroot, string.Format("{0}{1}", fileName, extension));

                    int counter = 2;

                    while (File.Exists(destFile))
                    {
                        destFile = System.IO.Path.Combine(newroot, string.Format("{0}({1}){2}", fileName, counter, extension));
                        counter++;
                    }

                    File.Copy(unique.FullPath, destFile, true);

                    if (deleteCheckBox1.Checked)
                    {
                        foreach(DoublePath fileref in FileHashed[file])
                        {
                            FileSystem.DeleteFile(fileref.FullPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }                        
                    }
                });
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 100)
            {
                treeView1.TreeViewNodeSorter = new NodeSorter();
                treeView1.BeginUpdate();
                foreach (string key in FileHashed.Keys)
                {
                    if (FileHashed[key].Count > 1)
                    {
                        List<TreeNode> nodes = new List<TreeNode>();

                        foreach (DoublePath dblpath in FileHashed[key])
                        {
                            TreeNode node = new TreeNode(dblpath.FullPath);
                            node.Tag = "Path";
                            node.ToolTipText = "";
                            nodes.Add(node);
                        }
                        TreeNode treeNode = new TreeNode(string.Format("{1}:        {2}", key, FileHashed[key][0].Size, BytesToString(FileHashed[key][0].Size)), nodes.ToArray());
                        treeNode.Tag="Hash";
                        treeNode.ForeColor = Color.GreenYellow;
                        treeNode.ToolTipText = "";
                        treeNode.Expand();
                        treeView1.Nodes.Add(treeNode);
                    }
                }

                treeView1.EndUpdate();
            }
        }
        public class NodeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                TreeNode tx = (TreeNode)x;
                TreeNode ty = (TreeNode)y;
                if (tx.Parent != null && ty.Parent != null)
                    return 0;

                string s1 = tx.Text.Split(':').First();
                string s2 = ty.Text.Split(':').First();

                if (IsNumeric(s1) && IsNumeric(s2))
                {
                    if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return -1;
                    if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return 1;
                    if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
                }

                if (IsNumeric(s1) && !IsNumeric(s2))
                    return -1;

                if (!IsNumeric(s1) && IsNumeric(s2))
                    return 1;

                return string.Compare(s1, s2, true);
            }

            public static bool IsNumeric(object value)
            {
                try
                {
                    int i = Convert.ToInt32(value.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }
        private void addPathBox_Click(object sender, EventArgs e)
        {

            // Display the openFile dialog.
            DialogResult result = addPathDialog.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                folderPaths.Add(addPathDialog.SelectedPath);
                pathsListBox.Items.Add(addPathDialog.SelectedPath);
                Invalidate();

            }
            // Cancel button was pressed. 
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {

            // Display the openFile dialog.
            DialogResult result = addPathDialog.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                targetPath = addPathDialog.SelectedPath;
                outputPathTextField1.Text = targetPath;
                Invalidate();

            }
            // Cancel button was pressed. 
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString()=="Path" && e.Button == MouseButtons.Right)
            {
                DialogResult dialogResult = MessageBox.Show("Send this file to the recycle bin?", "Delete File?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    FileSystem.DeleteFile(e.Node.Text, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    treeView1.Nodes.Remove(e.Node);
                }
                else if (dialogResult == DialogResult.No)
                {
                }
            }
        }
        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}
