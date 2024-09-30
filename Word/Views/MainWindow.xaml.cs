using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Word.ViewModels;
using Word.Views;
using Wpf.Ui.Controls;

namespace Word
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            FontSizeComboBox.SelectedValue = DocumentTextBox.FontSize;
        }

        private void SubscribeToViewModelEvents()
        {
            var viewModel = (MainWindowViewModel)DataContext;

            if (viewModel is null) return;

            // event subscription

        }

        #region UI

        private void TabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = true;
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object? fontSize = e.AddedItems[0];
                if (fontSize != null)
                    ApplyPropertyToSelectedText(FontSizeProperty, fontSize);
            }
            catch (Exception ex) { }

            DocumentTextBox.Focus();
        }

        private void ApplyPropertyToSelectedText(DependencyProperty dependencyProperty, object? value)
        {
            DocumentTextBox.Selection.ApplyPropertyValue(dependencyProperty, value);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FontFamily? editValue = (FontFamily?)e.AddedItems[0];
                if (editValue != null)
                    ApplyPropertyToSelectedText(FontFamilyProperty, editValue);
            }
            catch (Exception ex) { }

            DocumentTextBox.Focus();
        }

        private void ReferenceButton_Click(object sender, RoutedEventArgs e)
        {
            var referenceWindow = new ReferenceWindow();
            referenceWindow.Owner = this;
            referenceWindow.Show();
        }

        #endregion
    }
}