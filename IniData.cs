using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrpGui {
    public class IniData : INotifyPropertyChanged {
        public IniFile File { get; set; }
        public string Section { get; set; }
        public string? Key { get; set; }
        public string? KeyWpf => Key?.Replace("_", "__");
        public string? Value { get; set; }

        public IniData() {
        }
        public IniData(string FilePath) {
            this.File = new IniFile(FilePath);
        }
        public IniData(IniFile File) {
            this.File = File;
        }

        public IniData(IniFile File, string Section) {
            this.File = File;
            this.Section = Section;
        }

        public IniData(IniData data, string? Key) {
            this.File = data.File;
            this.Section = data.Section;
            this.Key = Key;
            Value = File.Read(Key, Section);
        }

        public void SetValue(string value) {
            File.Write(Key, value, Section);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
