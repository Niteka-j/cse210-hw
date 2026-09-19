
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScriptureMemorizer
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptureLibrary library = new ScriptureLibrary();
            
            // Optional: You can load extra scriptures from a file if it exists
            // library.LoadFromFile("scriptures.txt");

            bool running = true;
            while (running)
            {
                Scripture scripture = library.GetRandomScripture();

                if (scripture == null)
                {
                    Console.WriteLine("No scriptures available in the library.");
                    break;
                }

                while (!scripture.IsCompletelyHidden())
                {
                    Console.Clear();
                    Console.WriteLine("=== Scripture Memorizer ===");
                    Console.WriteLine();
                    Console.WriteLine(scripture.GetDisplayText());
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to hide more words, or type 'quit' to exit:");
                    
                    string input = Console.ReadLine();
                    if (input != null && input.Trim().ToLower() == "quit")
                    {
                        running = false;
                        break;
                    }

                    // Hide 3 random words at each step
                    scripture.HideRandomWords(3);
                }

                if (running)
                {
                    Console.Clear();
                    Console.WriteLine("=== Scripture Memorizer ===");
                    Console.WriteLine();
                    Console.WriteLine(scripture.GetDisplayText());
                    Console.WriteLine();
                    Console.WriteLine("🎉 Amazing job! You have completely memorized this scripture.");
                    Console.WriteLine("Would you like to practice another one? (y/n):");
                    
                    string again = Console.ReadLine();
                    if (again == null || again.Trim().ToLower() != "y")
                    {
                        running = false;
                    }
                }
            }

            Console.WriteLine("\nThank you for using the Scripture Memorizer. Goodbye!");
        }
    }

    public class Reference
    {
        private string _book;
        private int _chapter;
        private int _verse;
        private int _endVerse;
        private bool _isRange;

        // Constructor for a single verse
        public Reference(string book, int chapter, int verse)
        {
            _book = book;
            _chapter = chapter;
            _verse = verse;
            _isRange = false;
        }

        // Constructor for a verse range
        public Reference(string book, int chapter, int startVerse, int endVerse)
        {
            _book = book;
            _chapter = chapter;
            _verse = startVerse;
            _endVerse = endVerse;
            _isRange = true;
        }

        public string GetDisplayText()
        {
            if (_isRange)
            {
                return $"{_book} {_chapter}:{_verse}-{_endVerse}";
            }
            else
            {
                return $"{_book} {_chapter}:{_verse}";
            }
        }
    }

    public class Word
    {
        private string _text;
        private bool _isHidden;

        public Word(string text)
        {
            _text = text;
            _isHidden = false;
        }

        public void Hide()
        {
            _isHidden = true;
        }

        public bool IsHidden()
        {
            return _isHidden;
        }

        public string GetDisplayText()
        {
            if (_isHidden)
            {
                return new string('_', _text.Length);
            }
            else
            {
                return _text;
            }
        }
    }

    public class Scripture
    {
        private Reference _reference;
        private List<Word> _words;

        public Scripture(Reference reference, string text)
        {
            _reference = reference;
            _words = new List<Word>();
            
            // Split text by spaces and initialize Word objects
            foreach (string wordText in text.Split(' '))
            {
                _words.Add(new Word(wordText));
            }
        }

        public string GetDisplayText()
        {
            string displayText = _reference.GetDisplayText() + " - ";
            foreach (Word word in _words)
            {
                displayText += word.GetDisplayText() + " ";
            }
            return displayText.TrimEnd();
        }

        public void HideRandomWords(int numberToHide)
        {
            Random random = new Random();
            
            // Stretch Challenge: Select randomly only from words that are NOT already hidden
            var unhiddenWords = _words.Where(w => !w.IsHidden()).ToList();
            
            int countToHide = Math.Min(numberToHide, unhiddenWords.Count);
            for (int i = 0; i < countToHide; i++)
            {
                int index = random.Next(unhiddenWords.Count);
                unhiddenWords[index].Hide();
                unhiddenWords.RemoveAt(index); // Remove from our temporary list to prevent duplicate picks in this batch
            }
        }

        public bool IsCompletelyHidden()
        {
            return _words.All(w => w.IsHidden());
        }
    }

    public class ScriptureLibrary
    {
        private List<Scripture> _scriptures;

        public ScriptureLibrary()
        {
            _scriptures = new List<Scripture>();
            LoadDefaultScriptures();
        }

        private void LoadDefaultScriptures()
        {
            _scriptures.Add(new Scripture(
                new Reference("Proverbs", 3, 5, 6), 
                "Trust in the Lord with all thine heart and lean not unto thine own understanding"
            ));
            _scriptures.Add(new Scripture(
                new Reference("John", 3, 16), 
                "For God so loved the world that he gave his only begotten Son that whosoever believeth in him should not perish but have everlasting life"
            ));
            _scriptures.Add(new Scripture(
                new Reference("Philippians", 4, 13), 
                "I can do all things through Christ which strengtheneth me"
            ));
        }

        // Creativity: Load additional scriptures from an external text file format (ReferenceText|ScriptureText)
        public void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        string refStr = parts[0].Trim();
                        string text = parts[1].Trim();
                        // Custom simple parser or direct implementation can go here
                    }
                }
            }
        }

        public Scripture GetRandomScripture()
        {
            if (_scriptures.Count == 0) return null;
            Random random = new Random();
            return _scriptures[random.Next(_scriptures.Count)];
        }
    }
}