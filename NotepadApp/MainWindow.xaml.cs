using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using NotepadApp.Services;

namespace NotepadApp
{
    public partial class MainWindow
    {
        private string _fileName = "";
        private string _statusBarInfo;

        public MainWindow()
        {
            InitializeComponent();
            Title = "Notepad";
        }
        
        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Документы (*.docx)|*.docx|Все файлы (*.*)|*.*",
                FilterIndex = 1
            };
            
            dialog.ShowDialog();
            
            _fileName = dialog.FileName;
            
            Title = dialog.SafeFileName;
            
            if (!File.Exists(_fileName))
            {
                MessageBox.Show("Unable to open file", "Error", MessageBoxButton.OK);
                return;
            }
            
            StreamReader reader = new StreamReader(dialog.FileName);
            NotepadField.Text = reader.ReadToEnd();
            reader.Close();
            
            UpdateInfoOfStatusBar();
        }

        private void UpdateInfoOfStatusBar()
        {
            int stringsCount = NotepadField.Text.Split(' ').Length;
            _statusBarInfo = $"Count: {NotepadField.LineCount}, Strings count: {stringsCount}, Symbols count: {NotepadField.Text.Length}";
            LblCursorPosition.Text = _statusBarInfo;
        }
        
        private void NotepadField_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int row = NotepadField.GetLineIndexFromCharacterIndex(NotepadField.CaretIndex) + 1;
            int col = NotepadField.CaretIndex - NotepadField.GetCharacterIndexFromLineIndex(row) + 1;
            LblCursorPosition.Text = $"Line: {row}, Chars: {col}, {_statusBarInfo}";
        }

        private void SaveFile_OnClick(object sender, RoutedEventArgs e) =>
            SaveService.OnSave(_fileName, NotepadField.Text);
        
        private void SaveAsFile_OnClick(object sender, RoutedEventArgs e) => 
            SaveService.OnSaveAs(NotepadField.Text);
        
     
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
          
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
            {
                if (NotepadField.FontSize < 72)
                    NotepadField.FontSize++;
            }
                
            if (e.Delta < 0)
            {
                if (NotepadField.FontSize > 6)
                    NotepadField.FontSize--;
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
                SaveService.OnSave(_fileName, NotepadField.Text);
        }

        private void NotepadField_OnTextChanged(object sender, TextChangedEventArgs e) => UpdateInfoOfStatusBar();
    }
}