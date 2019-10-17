using System;
using System.Windows.Forms;

namespace CloudSaves
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CloudSaves cloudSaves = new CloudSaves(Application.ExecutablePath, args);
        }
    }
}
