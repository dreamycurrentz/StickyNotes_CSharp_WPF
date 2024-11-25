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
            // Load the RichTextBox contents from the locally saved file at the start of the application.
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
                // Extract text from RichTextBox
                TextRange textRange = new TextRange(notesContentRichTextBox.Document.ContentStart, notesContentRichTextBox.Document.ContentEnd);
                string richText = textRange.Text;

                // Create an instance of the class and set the text
                RichTextBoxData data = new RichTextBoxData { Text = richText };

                // Serialize the class instance to XMLSerializer
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
            // If the file exists
            if (File.Exists(_fileNameXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RichTextBoxData));
                using (FileStream fs = new FileStream(_fileNameXML, FileMode.Open))
                {
                    RichTextBoxData data = (RichTextBoxData)serializer.Deserialize(fs);
                    // Clear the RichTextBox content
                    notesContentRichTextBox.Document.Blocks.Clear();
                    // Load the text into the RichTextBox
                    notesContentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(data.Text)));
                }
            }
            // If the file doesn't exist, enter this text to the RichTextBox
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