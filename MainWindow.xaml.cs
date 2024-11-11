using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace StickyNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _fileName = "C:\\Users\\akash\\source\\repos\\StickyNotes\\StickyNotesSavedFile.txt";
        private TextRange range;
        private FileStream fStream;

        public MainWindow()
        {
            InitializeComponent();
            LoadNotesContent();
            
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveToTextFile();

        }

        private void SaveToTextFile()
        {
            try
            {
                range = new TextRange(notesContentRichTextBox.Document.ContentStart, notesContentRichTextBox.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.Create);
                range.Save(fStream, DataFormats.Text);
                fStream.Close();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Exception: {ex}" );
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            notesContentRichTextBox.Document.Blocks.Clear();
        }

        private void LoadNotesContent()
        {
            if (File.Exists(_fileName))
            {
                TextRange range = new TextRange(notesContentRichTextBox.Document.ContentStart, notesContentRichTextBox.Document.ContentEnd);
                FileStream fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Text);
                fStream.Close();
            }
            else
            {
                MessageBox.Show("File not exists!");
            }
        }

        private void notesContentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveToTextFile();
        }
    }
}