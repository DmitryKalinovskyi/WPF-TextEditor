using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Word
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            _fontSize.SelectedValue = _richTextBox.FontSize;
        }

        private void TabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = true;
        }

        private void _fontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object? fontSize = e.AddedItems[0];
                if(fontSize != null)
                ApplyPropertyToSelectedText(FontSizeProperty, fontSize);
            }
            catch (Exception ex) { }

            _richTextBox.Focus();
        }

        private void ApplyPropertyToSelectedText(DependencyProperty dependencyProperty, object? value)
        {
            _richTextBox.Selection.ApplyPropertyValue(dependencyProperty, value);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FontFamily? editValue = (FontFamily?)e.AddedItems[0];
                if(editValue != null) 
                    ApplyPropertyToSelectedText(FontFamilyProperty, editValue);
            }
            catch (Exception ex) { }

            _richTextBox.Focus();
        }
    }
}