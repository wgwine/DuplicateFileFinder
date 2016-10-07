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
using System.Diagnostics;

namespace FileFilter
{
    public partial class Form1 : MaterialForm
    {
        string targetPath = "C:/merged";
        List<string> folderPaths = new List<string>();

        Dictionary<string, List<FileInfo>> Files = new Dictionary<string, List<FileInfo>>();
        Dictionary<string, List<DoublePath>> FileHashed = new Dictionary<string, List<DoublePath>>();
        float progresscount = 0;
        float currentprogress = 0;
        private Object thisLock = new Object();
        enum ScanMethod { First4KB, Whole, Byte8Per4KB, Bytes8PerKB, BytePer128th };
        ScanMethod selectedMethod;
        Stopwatch time = new Stopwatch();
        private class Item
        {
            public string Name;
            public ScanMethod Value;
            public Item(string name, ScanMethod value)
            {
                Name = name; Value = value;
            }
            public override string ToString()
            {
                return Name;
            }
        }
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            comboBox1.Items.Add(new Item("First 4KB", ScanMethod.First4KB));
            comboBox1.Items.Add(new Item("Whole File(slow)",ScanMethod.Whole));
            comboBox1.Items.Add(new Item("8 bytes per 4KB", ScanMethod.Byte8Per4KB));
            comboBox1.Items.Add(new Item("8 bytes per kilobyte", ScanMethod.Bytes8PerKB));
            comboBox1.Items.Add(new Item("Byte per 128th of file", ScanMethod.BytePer128th));
            comboBox1.SelectedIndex = 0;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            foreach (string path in folderPaths)
            {
                Files[path] = new List<FileInfo>();
                var folderPath = new DirectoryInfo(path);
                try
                {
                    FileInfo[] tempfiles = folderPath.GetFiles("*", System.IO.SearchOption.AllDirectories);
                    foreach (FileInfo fi in tempfiles)
                    {
                        try
                        {
                            string test = fi.FullName;
                            Files[path].Add(fi);
                        }
                        catch { }
                    }
                }
                catch (UnauthorizedAccessException)
                {
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
        public void ProcessFile(FileInfo file, string toplevelfolder)
        {
            try
            {
                using (var stream = File.OpenRead(file.FullName))
                {
                    if (stream != null)
                    {
                        byte[] streamBuffer = new byte[0];
                        //If file is larger than 2GB, we have to stop here.
                        int maxBytes = (int)Math.Min(stream.Length, int.MaxValue);
                        int cycles;
                        int bufSize;
                        switch (selectedMethod)
                        {
                            case ScanMethod.Whole:
                                streamBuffer = new byte[maxBytes];
                                stream.Read(streamBuffer, 0, maxBytes);
                                break;
                            case ScanMethod.Byte8Per4KB:
                                cycles = Math.Max(maxBytes / 4096, 1);
                                bufSize = Math.Max(8 * cycles, 8);
                                streamBuffer = new byte[bufSize];
                                for (int i = 0; i < cycles; i++)
                                {
                                    stream.Read(streamBuffer, 8 * i, 8);
                                    stream.Seek(i * 4096, SeekOrigin.Begin);
                                }
                                break;
                            case ScanMethod.Bytes8PerKB:
                                cycles = Math.Max(maxBytes / 1024, 1);
                                bufSize = Math.Max(8 * cycles, 8);
                                streamBuffer = new byte[bufSize];
                                for (int i = 0; i < cycles; i++)
                                {
                                    stream.Read(streamBuffer, 8 * i, 8);
                                    stream.Seek(i * 1024, SeekOrigin.Begin);
                                }
                                break;
                            case ScanMethod.BytePer128th:
                                int bytesPerPortion = Math.Max(maxBytes / 128, 1);
                                streamBuffer = new byte[128 ];
                                for (int i = 0; i < 128; i++)
                                {
                                    stream.Read(streamBuffer, i, 1);
                                    stream.Seek(128, SeekOrigin.Current);
                                }
                                break;
                            default:
                                streamBuffer = new byte[4096];
                                stream.Read(streamBuffer, 0, 4096);
                                break;
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

                    }
                }
            }
            catch (Exception ee)
            {

            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            time.Reset();
            time.Start();
            var backgroundWorker = sender as BackgroundWorker;
            FileHashed = new Dictionary<string, List<DoublePath>>();
            bool isParallel = false;
            if (isParallel)
            {
                Parallel.ForEach(Files.Keys, toplevelfolder =>
                {
                    Parallel.ForEach(Files[toplevelfolder], file =>
                    {
                        ProcessFile(file, toplevelfolder);
                        currentprogress++;
                        backgroundWorker.ReportProgress((int)((currentprogress / progresscount) * 100));
                    });
                });
            }
            else
            {
                foreach(string toplevelfolder in Files.Keys)
                {
                    foreach(var file in Files[toplevelfolder])
                    {
                        ProcessFile(file, toplevelfolder);
                        currentprogress++;
                        backgroundWorker.ReportProgress((int)((currentprogress / progresscount) * 100));
                    }
                }
            }
            time.Stop();
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
                        foreach (DoublePath fileref in FileHashed[file])
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
                        TreeNode treeNode = new TreeNode(string.Format("{1}:        {2}         {0}", key, FileHashed[key][0].Size, BytesToString(FileHashed[key][0].Size)), nodes.ToArray());
                        treeNode.Tag = "Hash";
                        treeNode.ForeColor = Color.GreenYellow;
                        treeNode.ToolTipText = "";
                        treeNode.Expand();
                        treeView1.Nodes.Add(treeNode);

                    }
                }
                TreeNode timenode = new TreeNode(string.Format("{0}ms", time.ElapsedMilliseconds));
                timenode.Tag = "time";
                treeView1.Nodes.Add(timenode);
                treeView1.EndUpdate();
            }
        }
        public class NodeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                TreeNode tx = (TreeNode)x;
                TreeNode ty = (TreeNode)y;
                if (tx.Tag.ToString() == "time")
                    return -1;
                if (ty.Tag.ToString() == "time")
                    return 1;

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
                scanButton1.Enabled = true;
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
            if (e.Node.Tag.ToString() == "Path" && e.Button == MouseButtons.Right)
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

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString() == "Path" && e.Button == MouseButtons.Left)
            {
                System.Diagnostics.Process.Start(e.Node.Text);
            }
        }


        private void removePathButton4_Click(object sender, EventArgs e)
        {
            pathsListBox.Items.Remove(pathsListBox.SelectedItem);
            if (pathsListBox.Items.Count == 0)
            {
                scanButton1.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item itm = (Item)comboBox1.SelectedItem;
            selectedMethod = itm.Value;
        }
    }
}
