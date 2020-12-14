using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NotepadApp.Services;

namespace NotepadApp
{
    public partial class MainWindow
    {
        private string _fileName = "";
        private string _statusBarInfo;
        private bool _isSaved;

        public MainWindow()
        {
            InitializeComponent();
            Title = "Notepad";
        }

        //events
        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            (_fileName, Title) = SaveService.OnOpenFile();
            
            if (!File.Exists(_fileName))
            {
                MessageBox.Show("Unable to open file", "Error", MessageBoxButton.OK);
                return;
            }

            NotepadField.Text = File.ReadAllText(_fileName);
            SaveStatusUpdate(true);
        }

        private void SaveFile_OnClick(object sender, RoutedEventArgs e)
        {
            SaveService.OnSave(ref _fileName, NotepadField.Text);
            SaveStatusUpdate(true);
        }
        
        private void SaveAsFile_OnClick(object sender, RoutedEventArgs e)
        {
            SaveService.OnSaveAs(NotepadField.Text, ref _fileName);
            SaveStatusUpdate(true);
        }
        
        private void NotepadField_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int row = NotepadField.GetLineIndexFromCharacterIndex(NotepadField.CaretIndex);
            int col = NotepadField.CaretIndex - NotepadField.GetCharacterIndexFromLineIndex(row);
            LblCursorPosition.Text = $"Line: {row + 1}, Chars: {col + 1}, {_statusBarInfo}";
        }

        private void NotepadField_OnTextChanged(object sender, TextChangedEventArgs e) => SaveStatusUpdate(false);

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
            {
                SaveService.OnSave(ref _fileName, NotepadField.Text);
                SaveStatusUpdate(true);
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_isSaved) return;
            
            var answer = MessageBox.Show(
                "Save changed?",
                "Save",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );

            switch (answer)
            {
                case MessageBoxResult.Yes:
                    SaveService.OnSave(ref _fileName, NotepadField.Text);
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
        
        //methods
        private void SaveStatusUpdate(bool status)
        {
            _isSaved = status;
            UpdateInfoOfStatusBar();
        }
        
        private void UpdateInfoOfStatusBar()
        {
            if (NotepadField.Text != " ")
            {
                int stringsCount = NotepadField.Text.Split(' ').Length;
                _statusBarInfo =
                    $"Saved: {_isSaved}, Count: {NotepadField.LineCount}, Strings count: {stringsCount}, Symbols count: {NotepadField.Text.Length}";
                LblCursorPosition.Text = _statusBarInfo;
            }
        }
    }
}