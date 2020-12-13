using System.IO;
using Microsoft.Win32;

namespace NotepadApp.Services
{
    public static class SaveService
    {
        /// <summary>
        /// Saves text in the selected file by file name
        /// </summary>
        /// <param name="fileName">The name of the file to be saved along with the extension</param>
        /// <param name="text">Text to be saves</param>
        public static void OnSave(string fileName, string text)
        {
            if (fileName == "")
            {
                OnSaveAs(text);
                return;
            }
            
            File.WriteAllText(fileName, text);
        }
        
        /// <summary>
        /// Methods opened selected file
        /// </summary>
        /// <returns>
        /// fileName with extensions, fileName but no extensions
        /// </returns>
        public static (string, string) OnOpenFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Документы (*.docx)|*.docx|Все файлы (*.*)|*.*",
                FilterIndex = 1
            };

            dialog.ShowDialog();

            return (dialog.FileName, dialog.SafeFileName);
        }
        
        /// <summary>
        /// Opens a dialog window "Save As" for saving a file  
        /// </summary>
        /// <param name="text">Text to be saves in new file</param>
        public static void OnSaveAs(string text)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Text file|*.txt",
                FileName = "note"
            };
            
            if (dialog.ShowDialog() == true)
                File.WriteAllText(dialog.FileName, text);
        }
    }
}