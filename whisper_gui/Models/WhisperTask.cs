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
            try
            {
                if (_process?.HasExited != true)
                {
                    _process?.Kill();
                }
                _process?.Dispose();
            }
            catch { }
            _process = null;
        }

        private void RunWhisper()
        {
            if (!File.Exists(GlobalData.Options.PythonPath))
            {
                GlobalData.LogViewer.WriteLine($"Can not find python.exe", LogViewerLib.StringStyleEnum.errorText);
                Status = Status.Failed;
                return;
            }

            string arguments = $"-m whisper \"{FileName}\" " +
                $"--language {GlobalData.Options.SelectedLanguage} " +
                $"--model {GlobalData.Options.SelectedModel} " +
                $"--device {GlobalData.Options.SelectedDevice} " +
                $"--output_format {GlobalData.Options.SelectedOutputFormat} " +
                $"--output_dir \"{GlobalData.Options.OutputDirectory}\"";

            var startInfo = new ProcessStartInfo()
            {
                FileName = GlobalData.Options.PythonPath,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8
            };
            startInfo.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";
            startInfo.EnvironmentVariables["PYTHONUNBUFFERED"] = "True";

            GlobalData.LogViewer.WriteLine($"------------------------------------------", LogViewerLib.StringStyleEnum.errorText);
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

            _process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    GlobalData.LogViewer.WriteLine(e.Data, LogViewerLib.StringStyleEnum.label);
                }
            };

            _process.Exited += (s, e) =>
            {
                GlobalData.LogViewer.WriteLine($"------------------------------------------", LogViewerLib.StringStyleEnum.errorText);
                Status = Status.Completed;
            };

            if (_process.Start())
            {
                _process.BeginErrorReadLine();
                _process.BeginOutputReadLine();
            }
            else
            {
                GlobalData.LogViewer.WriteLine($"Process execution failed", LogViewerLib.StringStyleEnum.errorText);
                GlobalData.LogViewer.WriteLine($"------------------------------------------", LogViewerLib.StringStyleEnum.errorText);
                Status = Status.Failed;
            }
        }
    }
}
