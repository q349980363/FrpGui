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
        public string? Value { get; set; }

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


    




        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
