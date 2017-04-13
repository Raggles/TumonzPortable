using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace TumonzPortable
{
    static class Program
    {
        [Flags]
        public enum TumonzModules
        {
            T3 =    0x01,
            T3Pro = 0x02,
            T33D =  0x04,
            T4 =    0x08,
            T6Pro = 0x10,
            VLS =   0x20,
            T7    = 0x40,
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string p = Application.StartupPath;
            bool showExceptions = false;
            TumonzModules mods = 0;

            foreach (string arg in args)
            {
                switch (arg.ToLower())
                {
                    case "-t3":
                        mods |= TumonzModules.T3;
                        break;
                    case "-t3pro":
                        mods |= TumonzModules.T3Pro;
                        break;
                    case "-t4":
                        mods |= TumonzModules.T4;
                        break;
                    case "-t6pro":
                        mods |= TumonzModules.T6Pro;
                        break;
                    case "-t7":
                        mods |= TumonzModules.T7;
                        break;
                    case "-vls":
                        mods |= TumonzModules.VLS;
                        break;
                    case "-sud":
                        Registry.CurrentUser.CreateSubKey("Software\\Vision Software\\Tumonz");
                        Registry.CurrentUser.OpenSubKey("Software\\Vision Software\\Tumonz",true).SetValue("User Path",p,RegistryValueKind.String);
                        break;
                    case "-v":
                        showExceptions = true;
                        break;
                }
            }

            Patch(p, "T3", "\\T3.exe", "3.41", new Dictionary<long, long>() { { 3236864, 0x00115b60 } }, showExceptions);
            Patch(p, "T33D", "\\T33D.exe", "3.2", new Dictionary<long, long>() { { 3137536, 0x00111b34 } }, showExceptions);
            Patch(p, "T3Pro", "\\T3Pro.exe", "3.44", new Dictionary<long, long>() { { 4016640, 0x00125bc8 } }, showExceptions);
            Patch(p, "T4", "\\T4.exe", "4.21", new Dictionary<long, long>() { { 3895296, 0x2f02a4 } }, showExceptions);
            Patch(p, "T6Pro", "\\T6Pro.exe", "6.1", new Dictionary<long, long>() { { 5961216, 0x4483d0 } }, showExceptions);
            Patch(p, "T6Pro", "\\T7.exe", "7.01", new Dictionary<long, long>() { { 6063104, 0x45D118 } }, showExceptions); 

            if ((mods & TumonzModules.VLS) == TumonzModules.VLS)
                if (Process.GetProcessesByName("VLicenseServer").Length == 0)
                    Process.Start(p + "\\VLicenseServer.exe");
            if ((mods & TumonzModules.T3) == TumonzModules.T3)
                Process.Start(p + "\\T3.exe");
            if ((mods & TumonzModules.T3Pro) == TumonzModules.T3Pro)
                Process.Start(p + "\\T3Pro.exe");
            if ((mods & TumonzModules.T4) == TumonzModules.T4)
                Process.Start(p + "\\T4.exe");
            if ((mods & TumonzModules.T6Pro) == TumonzModules.T6Pro)
                Process.Start(p + "\\T6Pro.exe");
            if ((mods & TumonzModules.T7) == TumonzModules.T7)
                Process.Start(p + "\\T7.exe");

        }


        public static void Patch(string path, string friendlyName, string exeName, string versions, Dictionary<long, long> sizeOffsets, bool showExceptions)
        {
            try
            {
                string errorPrefix = friendlyName + " Error::";
                if (path.Length > 67)
                {
                    throw new Exception(errorPrefix + "The startup path is too long.  It must be less than 68 characters long.");
                }

                FileStream file = new FileStream(path + "\\" + exeName, FileMode.Open);
                long offset;
                try
                {
                    offset = sizeOffsets[file.Length];
                }
                catch
                {
                    throw new Exception(errorPrefix + "File version is not recognised.  Compatable only with version(s) " + versions);
                }

                file.Seek(offset, SeekOrigin.Begin);
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] writeBytes = new byte[path.Length + 1];
                encoding.GetBytes(path.ToCharArray()).CopyTo(writeBytes, 0);
                writeBytes[path.Length] = 0;
                file.Write(writeBytes, 0, writeBytes.Length);
                file.Close();
            }
            catch (Exception ex)
            {
                if (showExceptions)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
