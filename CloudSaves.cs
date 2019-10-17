using Microsoft.Win32;
using System.Linq;


namespace CloudSaves
{
    class CloudSaves
    {
        private const string PROGRAMKEYNAME = "cloudsaves";
        private const string COMMANDKEYNAME = "command";
        private Form1 form1;
        private RegistryKey mykey;
        public CloudSaves(string executablePath, string[] args)
        {

            if (args.Length == 0)
            {
                mykey = Registry.ClassesRoot.OpenSubKey("Directory", true).OpenSubKey("shell", true);
                if (mykey.GetSubKeyNames().Contains("cloudsaves") == false) {
                    mykey = mykey.CreateSubKey(PROGRAMKEYNAME);
                    mykey.SetValue("Icon", "\"C:\\WINDOWS\\system32\\imageres.dll\",177", RegistryValueKind.String);
                    mykey.SetValue("", "CloudSaves");
                }
                else {
                    mykey = mykey.OpenSubKey(PROGRAMKEYNAME, true);
                }

                if (mykey.GetSubKeyNames().Contains(COMMANDKEYNAME) ==false) {
                    mykey = mykey.CreateSubKey(COMMANDKEYNAME);
                }
                else {
                    mykey = mykey.OpenSubKey(COMMANDKEYNAME, true);
                }
                string commandString = executablePath + " \"%1\"";
                if (mykey.GetValue("").ToString().Equals(commandString) == false)
                {
                    mykey.SetValue("", commandString, RegistryValueKind.ExpandString);
                }
            }
            else
            {
                form1 = new Form1(args);
            }
        }

    }
}
