using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using Word.Commands;
using Word.Models;
using Word.Services;

namespace Word.ViewModels
{
    public class DocumentViewModel : ViewModelBase
    {
        private DocumentRepository _documentRepository;
        private DocumentModel _documentModel;
        private PrintService? _printService;

        // Dependencies [Path, IsSaved]
        public string Title
        {
            get
            {
                string result = "DeepEditor";
                if (Path is not null)
                {
                    result += $": {System.IO.Path.GetFileName(Path)}";
                }
                if (!IsSaved) result += "*";
                return result;
            }
        }

        public string? Path
        {
            get => _documentModel.Path;
            set
            {
                _documentModel.Path = value;
                OnPropertyChanged(nameof(Path));
                OnPropertyChanged(nameof(Title));
            }
        }

        private bool _isSaved;
        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                _isSaved = value;
                OnPropertyChanged(nameof(IsSaved));
                OnPropertyChanged(nameof(Title));
            }
        }

        public bool IsEnabled
        {
            get => _documentModel is not null;
        }

        public string Content
        {
            get => _documentModel.Content ?? "";
            set
            {
                _documentModel.Content = value;
                IsSaved = false;
                OnPropertyChanged(nameof(Content));
            }
        }

        public DocumentViewModel()
        {
            _documentRepository = new DocumentRepository();
            _printService = new PrintService();
            _documentModel = new DocumentModel();

            // Initialize commands
            SaveAsCommand = new RelayCommand((param) => SaveAs());
            SaveCommand = new RelayCommand((param) => Save());
            OpenDocumentCommand = new RelayCommand((param) => OpenDocument());
            CreateNewCommand = new RelayCommand((param) => CreateNew());
            PrintCommand = new RelayCommand((param) => _printService.Print(_documentModel));
        }

        public void UpdateModel(DocumentModel model)
        {
            _documentModel = model;
            NotifyModelUpdate();
        }

        public void NotifyModelUpdate()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Path));
            OnPropertyChanged(nameof(Content));
            OnPropertyChanged(nameof(IsEnabled));
            //OnPropertyChanged(nameof(SaveCommand));
            //OnPropertyChanged(nameof(SaveAsCommand));
        }

        #region Commands
        public ICommand SaveAsCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand OpenDocumentCommand { get; set; }
        public ICommand CreateNewCommand { get; set; }
        public ICommand PrintCommand { get; set; }

        public void CreateNew()
        {
            if (_documentModel is not null && !IsSaved)
            {
                // ask if we want to save our file.
                var result = MessageBox.Show("Want to save your changes?", "DeepEditor",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            UpdateModel(new DocumentModel());
            IsSaved = true;
        }

        public void Save()
        {
            if (_documentModel is null) return;

            if (IsSaved) return;

            if (_documentModel.Path is null)
            {
                SaveAs();
                return;
            }

            _documentRepository.SaveDocument(_documentModel);
            IsSaved = true;
        }

        public void SaveAs()
        {
            if (_documentModel is null) return;

            var dialog = new SaveFileDialog();
            // set a default file name
            dialog.FileName = "document.rtf";
            // set filters - this can be done in properties as well

            dialog.Filter = "Document files (*.rtf)|*.rtf";
            dialog.ShowDialog();

            var documentModelResult = new DocumentModel()
            {
                Path = dialog.FileName,
                Content = _documentModel.Content
            };

            _documentRepository.SaveDocument(documentModelResult);
            UpdateModel(documentModelResult);
            IsSaved = true;
        }

        public void OpenDocument()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Document files (*.rtf)|*.rtf";
            var result = dialog.ShowDialog();

            if (result is null || result.Value == false)
            {
                // user decided to close dialog
                return;
            }

            var documentModel = _documentRepository.OpenDocument(dialog.FileName);
            if (documentModel is not null)
                UpdateModel(documentModel);
            IsSaved = true;
        }
        #endregion

    }
}
