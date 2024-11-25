using StickyNotes.Models;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace StickyNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _fileName = "C:\\Users\\akash\\source\\repos\\StickyNotes\\StickyNotesSavedFile.txt";
        private readonly string _fileNameXML = "C:\\Users\\akash\\source\\repos\\StickyNotes\\RichTextBoxData.xml";
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
                TextRange textRange = new TextRange(notesContentRichTextBox.Document.ContentStart, notesContentRichTextBox.Document.ContentEnd);
                string richText = textRange.Text;

                RichTextBoxData data = new RichTextBoxData { Text = richText };

                XmlSerializer serializer = new XmlSerializer(typeof(RichTextBoxData));
                using (FileStream fs = new FileStream(_fileNameXML, FileMode.Create))
                {
                    serializer.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            notesContentRichTextBox.Document.Blocks.Clear();
        }

        private void LoadNotesContent()
        {
            if (File.Exists(_fileNameXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RichTextBoxData));
                using (FileStream fs = new FileStream(_fileNameXML, FileMode.Open))
                {
                    RichTextBoxData data = (RichTextBoxData)serializer.Deserialize(fs);
                    notesContentRichTextBox.Document.Blocks.Clear();
                    notesContentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(data.Text)));
                }
            }
            else
            {
                notesContentRichTextBox.AppendText("Enter here...");
            }
        }

        private void notesContentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveToTextFile();
        }
    }
}