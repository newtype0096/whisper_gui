using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whisper_gui.Enums;

namespace whisper_gui.Models
{
    public class WhisperTask : ObservableObject
    {
        private Process _process;

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

        public void Start()
        {
            Status = Status.Processing;
            RunWhisper();
        }

        public void Stop()
        {
            if (_process?.HasExited == true)
            {
                _process?.Kill();
            }
            _process?.Dispose();
            _process = null;
        }

        private void RunWhisper()
        {
            //string arguments = $"-m whisper {FileName} --language {GlobalData.Options.SelectedLanguage} --model {GlobalData.Options.SelectedModel}";
            string arguments = $"-m whisper";

            var startInfo = new ProcessStartInfo()
            {
                FileName = GlobalData.Options.PythonPath,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true
            };
            GlobalData.LogViewer.WriteLine($"{startInfo.FileName} {startInfo.Arguments}");

            _process = new Process();
            _process.StartInfo = startInfo;
            _process.EnableRaisingEvents = true;

            _process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    GlobalData.LogViewer.WriteLine(e.Data);
                }
            };

            _process.Exited += (s, e) =>
            {
                Status = Status.Completed;
            };

            _process.Start();
            _process.BeginErrorReadLine();
            _process.WaitForExit();
        }
    }
}
