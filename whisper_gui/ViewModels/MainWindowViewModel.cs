﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using whisper_gui.Enums;
using whisper_gui.Models;

namespace whisper_gui.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private Thread _taskManager;
        private object _cs = new object();

        public List<WhisperLanguages> WhisperLanguages { get; }
        public List<WhisperModels> WhisperModels { get; }
        public List<WhisperDevices> WhisperDevices { get; }
        public List<WhisperOutputFormats> WhisperOutputFormats { get; }

        private string _optionSummary;
        public string OptionSummary
        {
            get => _optionSummary;
            set => SetProperty(ref _optionSummary, value);
        }

        private WhisperLanguages _selectedLanguage;
        public WhisperLanguages SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);

                GlobalData.Options.SelectedLanguage = value;
                GlobalData.Options.SaveFile();
                OptionSummary = $"Whisper Options - {GlobalData.Options.SelectedLanguage}, {GlobalData.Options.SelectedModel}, {GlobalData.Options.SelectedDevice}, {GlobalData.Options.SelectedOutputFormat}";
            }
        }

        private WhisperDevices _selectedDevice;
        public WhisperDevices SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                SetProperty(ref _selectedDevice, value);

                GlobalData.Options.SelectedDevice = value;
                GlobalData.Options.SaveFile();
                OptionSummary = $"Whisper Options - {GlobalData.Options.SelectedLanguage}, {GlobalData.Options.SelectedModel}, {GlobalData.Options.SelectedDevice}, {GlobalData.Options.SelectedOutputFormat}";
            }
        }

        private WhisperModels _selectedModel;
        public WhisperModels SelectedModel
        {
            get => _selectedModel;
            set
            {
                SetProperty(ref _selectedModel, value);

                GlobalData.Options.SelectedModel = value;
                GlobalData.Options.SaveFile();
                OptionSummary = $"Whisper Options - {GlobalData.Options.SelectedLanguage}, {GlobalData.Options.SelectedModel}, {GlobalData.Options.SelectedDevice}, {GlobalData.Options.SelectedOutputFormat}";
            }
        }

        private WhisperOutputFormats _selectedOutputFormat;
        public WhisperOutputFormats SelectedOutputFormat
        {
            get => _selectedOutputFormat;
            set
            {
                SetProperty(ref _selectedOutputFormat, value);

                GlobalData.Options.SelectedOutputFormat = value;
                GlobalData.Options.SaveFile();
                OptionSummary = $"Whisper Options - {GlobalData.Options.SelectedLanguage}, {GlobalData.Options.SelectedModel}, {GlobalData.Options.SelectedDevice}, {GlobalData.Options.SelectedOutputFormat}";
            }
        }

        private string _pythonPath;
        public string PythonPath
        {
            get => _pythonPath;
            set
            {
                SetProperty(ref _pythonPath, value);

                GlobalData.Options.PythonPath = value;
                GlobalData.Options.SaveFile();
            }
        }

        private string _outputDirectory;
        public string OutputDirectory
        {
            get => _outputDirectory;
            set
            {
                SetProperty(ref _outputDirectory, value);

                GlobalData.Options.OutputDirectory = value;
                GlobalData.Options.SaveFile();
            }
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

        public RelayCommand ClosedCommand { get; }
        public RelayCommand BrowsePythonPathCommand { get; }
        public RelayCommand BrowseOutputDirectoryCommand { get; }
        public RelayCommand OpenFilesCommand { get; }
        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }
        public RelayCommand<WhisperTask> DeleteCommand { get; }

        public MainWindowViewModel()
        {
            WhisperLanguages = new List<WhisperLanguages>(Enum.GetValues(typeof(WhisperLanguages)).Cast<WhisperLanguages>());
            WhisperModels = new List<WhisperModels>(Enum.GetValues(typeof(WhisperModels)).Cast<WhisperModels>());
            WhisperDevices = new List<WhisperDevices>(Enum.GetValues(typeof(WhisperDevices)).Cast<WhisperDevices>());
            WhisperOutputFormats = new List<WhisperOutputFormats>(Enum.GetValues(typeof(WhisperOutputFormats)).Cast<WhisperOutputFormats>());

            OptionSummary = $"Whisper Options - {GlobalData.Options.SelectedLanguage}, {GlobalData.Options.SelectedModel}, {GlobalData.Options.SelectedDevice}, {GlobalData.Options.SelectedOutputFormat}";
            SelectedLanguage = GlobalData.Options.SelectedLanguage;
            SelectedModel = GlobalData.Options.SelectedModel;
            SelectedDevice = GlobalData.Options.SelectedDevice;
            SelectedOutputFormat = GlobalData.Options.SelectedOutputFormat;
            PythonPath = GlobalData.Options.PythonPath;
            OutputDirectory = GlobalData.Options.OutputDirectory;

            ClosedCommand = new RelayCommand(OnClosed);
            BrowsePythonPathCommand = new RelayCommand(OnBrowsePythonPath);
            BrowseOutputDirectoryCommand = new RelayCommand(OnBrowseOutputDirectory);
            OpenFilesCommand = new RelayCommand(OnOpenFiles);
            StartCommand = new RelayCommand(OnStart);
            StopCommand = new RelayCommand(OnStop);
            DeleteCommand = new RelayCommand<WhisperTask>(OnDelete);
        }

        private void OnClosed()
        {
            OnStop();
        }

        private void OnBrowsePythonPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = true;
            if (openFileDialog.ShowDialog() == true)
            {
                PythonPath = openFileDialog.FileName;
            }
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
            GlobalData.LogViewer.WriteLine("----- Started -----", LogViewerLib.StringStyleEnum.header1);

            _taskManager = new Thread(TaskManagerThreadProc);
            _taskManager.Start(this);

            Started = true;
        }

        private void OnStop()
        {
            Started = false;

            _taskManager?.Join();

            foreach (var task in WhisperTasks)
            {
                task.Stop();
                task.Status = Status.Pending;
            }
            GlobalData.LogViewer.WriteLine("----- Stopped -----", LogViewerLib.StringStyleEnum.header1);
        }

        private void OnDelete(WhisperTask task)
        {
            task.Stop();

            lock (_cs)
            {
                WhisperTasks.Remove(task);
                if (!WhisperTasks.Any())
                {
                    Started = false;
                }
            }
        }

        private static void TaskManagerThreadProc(object param)
        {
            var vm = (MainWindowViewModel)param;

            while (vm.Started)
            {
                WhisperTask task = null;
                lock (vm._cs)
                {
                    if (!vm.WhisperTasks.Any(x => x.Status.Equals(Status.Processing)))
                    {
                        task = vm.WhisperTasks.FirstOrDefault(x => x.Status.Equals(Status.Pending));
                    }
                }

                if (task != null)
                {
                    Task.Run(() =>
                    {
                        task.Start();
                    });
                }

                Thread.Sleep(100);
            }
        }
    }
}