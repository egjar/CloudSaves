using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace CloudSaves
{
    public partial class Form1 : Form
    {
        private RegistryKey mykey;
        public Form1(string[] args)
        {
            InitializeComponent();
            SavePath.Text = args[0]; 
            mykey = Registry.CurrentUser.OpenSubKey("Software",true);
            if (mykey.GetSubKeyNames().Contains("CloudSaves"))
            {
                mykey = mykey.OpenSubKey("CloudSaves", true);
                if (mykey.GetValue("CloudPath") != null)
                {
                    CloudPath.Text = mykey.GetValue("CloudPath").ToString();
                    Create();
                    Close();
                }
                else
                {
                    Application.Run(this);
                }
            }
            else
            {
                Application.Run(this);
            }
        }

        private void Button_browse(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK) {
                CloudPath.Text=dlg.SelectedPath;     
            }
        }

        private void Button_cancel(object sender, EventArgs e)
        {
            Close();
        }

        private void Button_create(object sender, EventArgs e)
        {
            if (Directory.Exists(CloudPath.Text))
            {
                Create();
                var mykey = Registry.CurrentUser.OpenSubKey("Software", true);
                if (!mykey.GetSubKeyNames().Contains("CloudSaves"))
                {
                    mykey.CreateSubKey("CloudSaves");
                }
                mykey = mykey.OpenSubKey("CloudSaves", true);
                mykey.SetValue("CloudPath", CloudPath.Text,RegistryValueKind.String);
                Close();
            }
        }

        private void Create() {
            DirectoryInfo source = new DirectoryInfo(SavePath.Text);
            DirectoryInfo target = new DirectoryInfo(Path.Combine(CloudPath.Text,source.Name));
            MoveAll(source,target);
            JunctionPoint.Create(SavePath.Text, target.FullName, false);
        }

        public static void MoveAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.

            if (Directory.Exists(target.FullName)==false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                fi.Delete();
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    new DirectoryInfo(Path.Combine(target.FullName, diSourceSubDir.Name));
                MoveAll(diSourceSubDir, nextTargetSubDir);
            }
            source.Delete();
        }

    }
}
