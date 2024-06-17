using CommunityToolkit.Mvvm.ComponentModel;
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

        private WhisperLanguages _selectedLanguage = Enums.WhisperLanguages.Japanese;
        public WhisperLanguages SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public List<WhisperModels> WhisperModels { get; }

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

        public MainWindowViewModel()
        {
            WhisperLanguages = new List<WhisperLanguages>(Enum.GetValues(typeof(WhisperLanguages)).Cast<WhisperLanguages>());
            WhisperModels = new List<WhisperModels>(Enum.GetValues(typeof(WhisperModels)).Cast<WhisperModels>());
        }
    }
}
