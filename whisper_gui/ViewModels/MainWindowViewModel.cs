using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
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

        private string _outputDirectory;
        public string OutputDirectory
        {
            get => _outputDirectory;
            set => SetProperty(ref _outputDirectory, value);
        }

        private ObservableCollection<WhisperTask> _whisperTasks = new ObservableCollection<WhisperTask>();
        public ObservableCollection<WhisperTask> WhisperTasks
        {
            get => _whisperTasks;
            set => SetProperty(ref _whisperTasks, value);
        }

        private bool _started = false;
        public bool Started
        {
            get => _started;
            set
            {
                SetProperty(ref _started, value);
                OnPropertyChanged(nameof(IsOptionEnabled));
                OnPropertyChanged(nameof(IsStartButtonEnabled));
                OnPropertyChanged(nameof(IsStopButtonEnabled));
            }
        }

        public bool IsOptionEnabled => !Started;
        public bool IsStartButtonEnabled => !Started;
        public bool IsStopButtonEnabled => Started;

        public RelayCommand BrowseOutputDirectoryCommand { get; }
        public RelayCommand OpenFilesCommand { get; }
        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }

        public MainWindowViewModel()
        {
            WhisperLanguages = new List<WhisperLanguages>(Enum.GetValues(typeof(WhisperLanguages)).Cast<WhisperLanguages>());
            WhisperModels = new List<WhisperModels>(Enum.GetValues(typeof(WhisperModels)).Cast<WhisperModels>());
            OutputDirectory = System.IO.Directory.GetCurrentDirectory();

            BrowseOutputDirectoryCommand = new RelayCommand(OnBrowseOutputDirectory);
            OpenFilesCommand = new RelayCommand(OnOpenFiles);
            StartCommand = new RelayCommand(OnStart);
            StopCommand = new RelayCommand(OnStop);
        }

        private void OnBrowseOutputDirectory()
        {
            VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog();
            folderBrowserDialog.Multiselect = false;
            if (folderBrowserDialog.ShowDialog() == true)
            {
                OutputDirectory = folderBrowserDialog.SelectedPath;
            }
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
                    if (!WhisperTasks.Any(x => x.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        var task = new WhisperTask();
                        task.FileName = fileName;
                        task.Status = Status.Pending;

                        WhisperTasks.Add(task);
                    }
                }
            }
        }

        private void OnStart()
        {
            Started = true;
        }

        private void OnStop()
        {
            Started = false;
        }
    }
}
