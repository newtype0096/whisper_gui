using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whisper_gui.Enums;

namespace whisper_gui.Models
{
    public class WhisperTask : ObservableObject
    {
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private Status _status;
        public Status Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
    }
}
