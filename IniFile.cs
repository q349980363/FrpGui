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
    public class _IniFile {
        static _IniFile() {
        }


        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        static extern int GetPrivateProfileString_Buffer(string Section, string Key, string Default, byte[] RetVal, int Size, string FilePath);


        static readonly Encoding GB2132 = Encoding.GetEncoding("gbk");



        static public string Read(string Key, string Section, string Path) {
            var RetVal = new StringBuilder(255);
            RetVal.Length = 255;
            var bytes = new byte[255];
            var length = GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            var str = GB2132.GetString(bytes);
            return RetVal.ToString(0, length);
        }

        static public List<string> ReadList(string Key, string Section, string Path) {
            var buffer = new byte[255];
            var length = GetPrivateProfileString_Buffer(Section, Key, "", buffer, 255, Path);
            var list = GB2132.GetString(buffer.Take(length).ToArray()).Split('\0').ToList();
            //list.Sort();
            list = list.Select(x => x.Trim()).ToList();
            list = list.Where(v => !string.IsNullOrEmpty(v)).Where(v => v[0] != '#').ToList();
            return list;
        }

        static public void Write(string Key, string Value, string Section, string Path) {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        static public void DeleteKey(string Key, string Section, string Path) {
            Write(Key, null, Section, Path);
        }

        static public void DeleteSection(string Section, string Path) {
            Write(null, null, Section, Path);
        }

        static public bool KeyExists(string Key, string Section, string Path) {
            return Read(Key, Section, Path).Length > 0;
        }
    }
}
