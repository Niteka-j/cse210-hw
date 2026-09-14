// Program.cs
// 1. Added a "Mood" tracking field to every journal entry to capture emotional well-being over time.
// 2. Implemented full, robust CSV serialization and parsing that correctly handles commas and 
//    escaped quotation marks, ensuring the exported file can be opened seamlessly in Excel.
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Journal theJournal = new Journal();
        PromptGenerator promptGenerator = new PromptGenerator();
        int choice = 0;

        while (choice != 5)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Quit");
            Console.Write("Enter your choice: ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out choice))
            {
                switch (choice)
                {
                    case 1:
                        string prompt = promptGenerator.GetRandomPrompt();
                        Console.WriteLine($"\nPrompt: {prompt}");
                        Console.Write("> ");
                        string response = Console.ReadLine();
                        
                        Console.Write("How would you rate your mood today (e.g., Happy, Reflective, Stressed): ");
                        string mood = Console.ReadLine();

                        string dateText = DateTime.Now.ToShortDateString();

                        Entry newEntry = new Entry
                        {
                            _date = dateText,
                            _promptText = prompt,
                            _entryText = response,
                            _mood = mood
                        };

                        theJournal.AddEntry(newEntry);
                        Console.WriteLine("Entry added successfully!\n");
                        break;

                    case 2:
                        Console.WriteLine("\n--- Journal Entries ---");
                        theJournal.DisplayJournal();
                        break;

                    case 3:
                        Console.Write("Enter filename to save (e.g., journal.csv): ");
                        string saveFile = Console.ReadLine();
                        theJournal.SaveToFile(saveFile);
                        Console.WriteLine("Journal saved successfully!\n");
                        break;

                    case 4:
                        Console.Write("Enter filename to load: ");
                        string loadFile = Console.ReadLine();
                        if (File.Exists(loadFile))
                        {
                            theJournal.LoadFromFile(loadFile);
                            Console.WriteLine("Journal loaded successfully!\n");
                        }
                        else
                        {
                            Console.WriteLine("File not found.\n");
                        }
                        break;

                    case 5:
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please choose between 1 and 5.\n");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.\n");
            }
        }
    }
}

public class Entry
{
    public string _date { get; set; }
    public string _promptText { get; set; }
    public string _entryText { get; set; }
    public string _mood { get; set; }

    public void Display()
    {
        Console.WriteLine($"Date: {_date} - Mood: {_mood}");
        Console.WriteLine($"Prompt: {_promptText}");
        Console.WriteLine($"Entry: {_entryText}\n");
    }

    public string ToCsv()
    {
        return $"{Escape(_date)},{Escape(_promptText)},{Escape(_entryText)},{Escape(_mood)}";
    }

    private string Escape(string field)
    {
        if (string.IsNullOrEmpty(field)) return "\"\"";
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

    public static Entry FromCsv(string csvLine)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        string currentField = "";

        for (int i = 0; i < csvLine.Length; i++)
        {
            char c = csvLine[i];
            if (c == '"')
            {
                if (inQuotes && i + 1 < csvLine.Length && csvLine[i + 1] == '"')
                {
                    currentField += '"';
                    i++; // Skip the escaped quote
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(currentField);
                currentField = "";
            }
            else
            {
                currentField += c;
            }
        }
        fields.Add(currentField);

        if (fields.Count >= 4)
        {
            return new Entry
            {
                _date = fields[0],
                _promptText = fields[1],
                _entryText = fields[2],
                _mood = fields[3]
            };
        }
        return null;
    }
}

public class Journal
{
    private List<Entry> _entries = new List<Entry>();

    public void AddEntry(Entry newEntry)
    {
        _entries.Add(newEntry);
    }

    public void DisplayJournal()
    {
        foreach (Entry entry in _entries)
        {
            entry.Display();
        }
    }

    public void SaveToFile(string file)
    {
        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.WriteLine("Date,Prompt,Response,Mood"); // CSV Header
            foreach (Entry entry in _entries)
            {
                writer.WriteLine(entry.ToCsv());
            }
        }
    }

    public void LoadFromFile(string file)
    {
        _entries.Clear();
        string[] lines = File.ReadAllLines(file);
        bool isHeader = true;

        foreach (string line in lines)
        {
            if (isHeader)
            {
                isHeader = false;
                continue;
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                Entry entry = Entry.FromCsv(line);
                if (entry != null)
                {
                    _entries.Add(entry);
                }
            }
        }
    }
}

public class PromptGenerator
{
    private List<string> _prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    public string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(_prompts.Count);
        return _prompts[index];
    }
}