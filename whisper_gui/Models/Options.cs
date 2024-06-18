using System.IO;
using System.Text.Json;
using whisper_gui.Enums;

namespace whisper_gui.Models
{
    public class Options
    {
        private static string optionFileName = "whisper_gui_option.json";

        public WhisperLanguages SelectedLanguage { get; set; }
        public WhisperModels SelectedModel { get; set; }
        public string PythonPath { get; set; }
        public string OutputDirectory { get; set; }

        public Options()
        {
            SelectedLanguage = WhisperLanguages.Japanese;
            SelectedModel = WhisperModels.medium;

            OutputDirectory = Directory.GetCurrentDirectory();

            var fileInfo = new FileInfo("python.exe");
            PythonPath = fileInfo.FullName;
        }

        public void SaveFile()
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.WriteIndented = true;
            jsonSerializerOptions.AllowTrailingCommas = true;

            File.WriteAllText(optionFileName, JsonSerializer.Serialize(this, jsonSerializerOptions));
        }

        public void LoadFile()
        {
            try
            {
                if (File.Exists(optionFileName))
                {
                    string jsonString = File.ReadAllText(optionFileName);
                    var options = JsonSerializer.Deserialize<Options>(jsonString);
                    SelectedLanguage = options.SelectedLanguage;
                    SelectedModel = options.SelectedModel;
                    PythonPath = options.PythonPath;
                    OutputDirectory = options.OutputDirectory;
                }
            }
            catch { }
        }
    }
}