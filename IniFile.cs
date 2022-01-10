using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FrpGui {
    // revision 11
    public class IniFile {
        string Path;

        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        static extern int GetPrivateProfileString_Buffer(string Section, string Key, string Default, byte[] RetVal, int Size, string FilePath);




        public IniFile(string IniPath) {
            Path = new FileInfo(IniPath).FullName;
        }


        public string Read(string Key, string Section) {
            var RetVal = new StringBuilder(255);
            RetVal.Length = 255;
            var bytes = new byte[255];
            var length = GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);

            var str = Encoding.ASCII.GetString(bytes);

            return RetVal.ToString(0, length);
        }

        public List<string> ReadList(string Key, string Section) {
            var buffer = new byte[255];
            var length = GetPrivateProfileString_Buffer(Section, Key, "", buffer, 255, Path);
            var list = Encoding.ASCII.GetString(buffer.Take(length).ToArray()).Split('\0').ToList();
            list.Sort();
            list = list.Select(x => x.Trim()).ToList();
            list = list.Where(v => !string.IsNullOrEmpty(v)).Where(v => v[0] != '#').ToList();
            return list;
        }

        public void Write(string Key, string Value, string Section) {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section) {
            Write(Key, null, Section);
        }

        public void DeleteSection(string Section) {
            Write(null, null, Section);
        }

        public bool KeyExists(string Key, string Section) {
            return Read(Key, Section).Length > 0;
        }
    }
}
