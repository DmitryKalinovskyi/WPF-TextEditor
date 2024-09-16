using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Word.Services;

namespace Word.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        private readonly MainWindow _view;

        public double[] FontSizes => _fontSizes;

        public bool IsSaved { get; set; }

        private DocumentSevice documentSevice = new DocumentSevice();

        public MainWindowViewModel() : this(Application.Current.MainWindow as MainWindow) { }

        public MainWindowViewModel(MainWindow view)
        {
            _view = view;

            SubscribeToViewEvents();

            // Initialize commands
            SaveAsCommand = new Commands.RelayCommand((param) => SaveAs());
            OpenDocumentCommand = new Commands.RelayCommand((param) => OpenDocument());
        }

        private void SubscribeToViewEvents()
        { 
        }

        #region Commands
        public ICommand SaveAsCommand { get; set; }
        public ICommand OpenDocumentCommand { get; set; }

        public void SaveAs()
        {
            var document = _view.DocumentTextBox.Document;
            documentSevice.SaveDocument(new TextRange(document.ContentStart, document.ContentEnd));
        }

        public void OpenDocument()
        {
            var document = _view.DocumentTextBox.Document;
            documentSevice.OpenDocument(new TextRange(document.ContentStart, document.ContentEnd));
        }
        #endregion

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private double[] _fontSizes = [3.0, 4.0, 5.0, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0,
9.5, 10.0, 10.5, 11.0, 11.5, 12.0, 12.5, 13.0, 13.5, 14.0, 15.0, 16.0, 17.0, 18.0, 19.0,
20.0, 22.0, 24.0, 26.0, 28.0, 30.0, 32.0, 34.0, 36.0, 38.0, 40.0, 44.0, 48.0, 52.0, 56.0,
60.0, 64.0, 68.0, 72.0, 76.0, 80.0, 88.0, 96.0, 104.0, 112.0, 120.0, 128.0, 136.0, 144.0
];
    }
}
