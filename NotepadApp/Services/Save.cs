using  System.IO;
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