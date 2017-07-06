using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFiller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string sourceFolder = "";
        string[] sourceFiles = new string[] { };
        string targetFolder = "";
        string[] targetFiles = new string[] { };
        List<string> sfnames = new List<string> { };
        List<string> tfnames = new List<string> { };

        //Create txt file on desktop and update fileLoc below with its path to save already existing files that aren't copied
        public string fileLoc = @"C:\Users\Jack\Desktop\ExistingFiles.txt";

        public void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                sourceFolder = dlg.SelectedPath;

            }

            sourceFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
            foreach (string f in sourceFiles)
            {
                string sfn;
                sfn = Path.GetFileName(f);
                sfnames.Add(sfn);
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fillHere = new FolderBrowserDialog();

            if (fillHere.ShowDialog() == DialogResult.OK)
            {
                targetFolder = fillHere.SelectedPath;
            }
            targetFiles = Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories);
            foreach (string t in targetFiles)
            {
                string tfn;
                tfn = Path.GetFileName(t);
                tfnames.Add(tfn);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //File Location to write already existed files
            //Update FileLoc to your desired output location
            
            FileStream fs = null;
            if (!File.Exists(fileLoc))
            {
                using (fs = File.Create(fileLoc)) { };
            }

            DirectoryCopy(sourceFolder, targetFolder, true);
            MessageBox.Show("Copying Complete");
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                if (!File.Exists(tempPath))
                {
                    file.CopyTo(tempPath, false);
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(fileLoc, true))
                    {
                        sw.WriteLine(tempPath);
                    }
                }
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
             {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
        