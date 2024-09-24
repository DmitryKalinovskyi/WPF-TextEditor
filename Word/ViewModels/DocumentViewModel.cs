using Microsoft.Win32;
using System.Windows.Documents;
using System.Windows.Input;
using Word.Commands;
using Word.Models;
using Word.Services;

namespace Word.ViewModels
{
    public class DocumentViewModel : ViewModelBase
    {
        private DocumentRepository _documentRepository;
        private DocumentModel? _documentModel;

        // Dependencies [Path, IsSaved]
        public string Title
        {
            get
            {
                string result = "DeepEditor";
                if(Path is not null)
                {
                    result += $": {Path}";
                    if (!IsSaved) result += "*";
                }
                return result;
            }
        }

        public string? Path
        {
            get => _documentModel?.Path;
            set
            {
                if(_documentModel is not null)
                {
                    _documentModel.Path = value;
                    OnPropertyChanged(nameof(Path));
                    OnPropertyChanged(nameof(Title));
                }
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
            get => _documentModel?.Content ?? "";
            set
            {
                if(_documentModel is not null)
                {
                    _documentModel.Content = value;
                    IsSaved = false;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        public DocumentViewModel()
        {
            _documentRepository = new DocumentRepository();

            // Initialize commands
            SaveAsCommand = new RelayCommand((param) => SaveAs());
            SaveCommand = new RelayCommand((param) => Save());
            OpenDocumentCommand = new RelayCommand((param) => OpenDocument());
            CreateNewCommand = new RelayCommand((param) => CreateNew());
        }

        public void UpdateModel(DocumentModel? model)
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

        public void CreateNew()
        {
            if(_documentModel is not null)
            {
                // ask if we want to save our file.

            }

            UpdateModel(new DocumentModel());
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
            IsSaved = true;
            UpdateModel(documentModelResult);
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
            if(documentModel is not null) 
            UpdateModel(documentModel);
        }
#endregion

    }
}
