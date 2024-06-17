using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whisper_gui.Enums;
using whisper_gui.Models;

namespace whisper_gui.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public List<WhisperLanguages> WhisperLanguages { get; }
        public List<WhisperModels> WhisperModels { get; }

        private WhisperLanguages _selectedLanguage = Enums.WhisperLanguages.Japanese;
        public WhisperLanguages SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }
        
        private WhisperModels _selectedModel = Enums.WhisperModels.medium;
        public WhisperModels SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        private ObservableCollection<WhisperTask> _whisperTasks = new ObservableCollection<WhisperTask>();
        public ObservableCollection<WhisperTask> WhisperTasks
        {
            get => _whisperTasks;
            set => SetProperty(ref _whisperTasks, value);
        }

        public RelayCommand OpenFilesCommand { get; }

        public MainWindowViewModel()
        {
            WhisperLanguages = new List<WhisperLanguages>(Enum.GetValues(typeof(WhisperLanguages)).Cast<WhisperLanguages>());
            WhisperModels = new List<WhisperModels>(Enum.GetValues(typeof(WhisperModels)).Cast<WhisperModels>());

            OpenFilesCommand = new RelayCommand(OnOpenFiles);
        }

        private void OnOpenFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files (*.wav, *.mp3, *.mp4, *.avi, *.mkv)|*.wav;*.mp3;*.mp4;*.avi;*.mkv|All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = true;
            if (openFileDialog.ShowDialog() == true)
            {
                var fileNames = openFileDialog.FileNames.ToArray();
                foreach (var fileName in fileNames)
                {
                    var task = new WhisperTask();
                    task.FileName = fileName;
                    task.Status = Status.Pending;

                    WhisperTasks.Add(task);
                }
            }
        }
    }
}
