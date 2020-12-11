using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace WpfApp2
{
    public partial class MainWindow
    {
        private string _fileName;
        private string _data;

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
            
            var reader = new StreamReader(dialog.FileName);
            NotepadField.Text = reader.ReadToEnd();

            int stringsCount = NotepadField.Text.Split(' ').Length;
            _data = $"Count: {NotepadField.LineCount}, Strings count: {stringsCount}, Symbols count: {NotepadField.Text.Length}";
            lblCursorPosition.Text = _data;
            reader.Close();
        }

        //TODO проверка есть ли такой файл, и вызывать save as
        private void SaveFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (_fileName == "")
            {
                MessageBox.Show("BAN");
                return;
            }
            
            File.WriteAllText(_fileName, NotepadField.Text);
        }
        

        private void SaveAsFile_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Text file|*.txt"
            };
            
            if (dialog.ShowDialog() == true)
                File.WriteAllText(dialog.FileName, NotepadField.Text);
        }

        private void NotepadField_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int row = NotepadField.GetLineIndexFromCharacterIndex(NotepadField.CaretIndex);
            int col = NotepadField.CaretIndex - NotepadField.GetCharacterIndexFromLineIndex(row);
            lblCursorPosition.Text = "Line " + (row + 1) + ", Char " + (col + 1) + " " + _data;
        }

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
    }
}