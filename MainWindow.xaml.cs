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
                TextRange textRange = new TextRange(notesContentRichTextBox.Document.ContentStart, notesContentRichTextBox.Document.ContentEnd);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    textRange.Save(memoryStream, DataFormats.Xaml);
                    string xamlContent = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());

                    RichTextBoxData data = new RichTextBoxData { Content = xamlContent };
                    XmlSerializer serializer = new XmlSerializer(typeof(RichTextBoxData));
                    using (FileStream fileStream = new FileStream(_fileNameXML,FileMode.Create))
                    {
                        serializer.Serialize(fileStream, data);
                    }
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
            //If the file exists
            if (File.Exists(_fileNameXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RichTextBoxData));
                RichTextBoxData data;

                using (FileStream fileStream = new FileStream(_fileNameXML, FileMode.Open))
                {
                    data = (RichTextBoxData)serializer.Deserialize(fileStream);
                }

                TextRange textRange = new TextRange(notesContentRichTextBox.Document.ContentStart, notesContentRichTextBox.Document.ContentEnd);
                using (MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data.Content)))
                {
                    textRange.Load(memoryStream, DataFormats.Xaml);
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

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleTextStyle(TextElement.FontWeightProperty, FontWeights.Bold, FontWeights.Normal);
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleTextStyle(TextElement.FontStyleProperty, FontStyles.Italic, FontStyles.Normal);
        }

        private void ToggleTextStyle(DependencyProperty formattingProperty, object value, object defaultValue)
        {
            TextSelection selectedText = notesContentRichTextBox.Selection;

            if (!selectedText.IsEmpty)
            {
                object currentValue = selectedText.GetPropertyValue(formattingProperty);
                if (currentValue == DependencyProperty.UnsetValue || !currentValue.Equals(value))
                {
                    selectedText.ApplyPropertyValue(formattingProperty, value);
                }
                else
                {
                    selectedText.ApplyPropertyValue(formattingProperty, defaultValue);
                }
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            TextSelection selectedText = notesContentRichTextBox.Selection;

            if (!selectedText.IsEmpty)
            {
                
                var currentDecorations = selectedText.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection;

                if (currentDecorations != null && currentDecorations.Contains(TextDecorations.Underline.FirstOrDefault()))
                {
                    selectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
                else
                {
                    selectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
            }
        }
    }
}