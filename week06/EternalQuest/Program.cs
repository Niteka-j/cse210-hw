using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EternalQuest
{
    // ==========================================
    // BASE CLASS (Abstraction & Inheritance)
    // ==========================================
    public abstract class Goal
    {
        // Encapsulated protected member variables
        protected string _name;
        protected string _description;
        protected int _points;

        // Public properties (Getters) for abstraction
        public string Name => _name;
        public string Description => _description;
        public int Points => _points;

        protected Goal(string name, string description, int points)
        {
            _name = name;
            _description = description;
            _points = points;
        }

        // Abstract methods to be overridden (Polymorphism)
        public abstract int RecordEvent();
        public abstract string GetStatus();
        public abstract Dictionary<string, object> ToDictionary();
    }

    // ==========================================
    // DERIVED GOAL CLASSES (Polymorphism)
    // ==========================================
    public class SimpleGoal : Goal
    {
        private bool _completed;

        public SimpleGoal(string name, string description, int points, bool completed = false) 
            : base(name, description, points)
        {
            _completed = completed;
        }

        public override int RecordEvent()
        {
            if (!_completed)
            {
                _completed = true;
                Console.WriteLine($"Congratulations! You completed '{_name}' and earned {_points} points!");
                return _points;
            }
            else
            {
                Console.WriteLine($"Goal '{_name}' is already completed.");
                return 0;
            }
        }

        public override string GetStatus() => _completed ? "[X]" : "[ ]";

        public override Dictionary<string, object> ToDictionary() => new()
        {
            { "Type", "SimpleGoal" },
            { "Name", _name },
            { "Description", _description },
            { "Points", _points },
            { "Completed", _completed }
        };
    }

    public class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points) 
            : base(name, description, points) { }

        public override int RecordEvent()
        {
            Console.WriteLine($"Recorded '{_name}'! You earned {_points} points.");
            return _points;
        }

        public override string GetStatus() => "[∞]";

        public override Dictionary<string, object> ToDictionary() => new()
        {
            { "Type", "EternalGoal" },
            { "Name", _name },
            { "Description", _description },
            { "Points", _points }
        };
    }

    public class ChecklistGoal : Goal
    {
        private int _target;
        private int _bonus;
        private int _currentCount;

        public ChecklistGoal(string name, string description, int points, int target, int bonus, int currentCount = 0) 
            : base(name, description, points)
        {
            _target = target;
            _bonus = bonus;
            _currentCount = currentCount;
        }

        public override int RecordEvent()
        {
            if (_currentCount < _target)
            {
                _currentCount++;
                int earned = _points;
                Console.WriteLine($"Progress recorded for '{_name}'! Earned {_points} points.");

                if (_currentCount == _target)
                {
                    earned += _bonus;
                    Console.WriteLine($"Target reached! You earned a bonus of {_bonus} points for completing '{_name}'!");
                }
                return earned;
            }
            else
            {
                Console.WriteLine($"Goal '{_name}' is already fully completed!");
                return 0;
            }
        }

        public override string GetStatus()
        {
            string mark = _currentCount >= _target ? "X" : " ";
            return $"[{mark}] Completed {_currentCount}/{_target} times";
        }

        public override Dictionary<string, object> ToDictionary() => new()
        {
            { "Type", "ChecklistGoal" },
            { "Name", _name },
            { "Description", _description },
            { "Points", _points },
            { "Target", _target },
            { "Bonus", _bonus },
            { "CurrentCount", _currentCount }
        };
    }

    // ==========================================
    // CREATIVE GOAL EXTENSIONS
    // ==========================================
    public class ProgressGoal : Goal
    {
        private double _totalUnits;
        private double _currentUnits;

        public ProgressGoal(string name, string description, double totalUnits, int pointsPerUnit, double currentUnits = 0) 
            : base(name, description, pointsPerUnit)
        {
            _totalUnits = totalUnits;
            _currentUnits = currentUnits;
        }

        public override int RecordEvent()
        {
            if (_currentUnits >= _totalUnits)
            {
                Console.WriteLine($"Goal '{_name}' is already fully achieved!");
                return 0;
            }

            Console.Write($"Enter amount of progress made (Total required: {_totalUnits}): ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                _currentUnits = Math.Min(_totalUnits, _currentUnits + amount);
                int earned = (int)(amount * _points);
                Console.WriteLine($"Added {amount} units to '{_name}'. Earned {earned} points!");

                if (_currentUnits >= _totalUnits)
                {
                    Console.WriteLine($"🏆 Milestone Reached! You completed your grand goal: '{_name}'!");
                }
                return earned;
            }
            else
            {
                Console.WriteLine("Invalid progress amount.");
                return 0;
            }
        }

        public override string GetStatus()
        {
            string mark = _currentUnits >= _totalUnits ? "X" : " ";
            return $"[{mark}] Progress: {_currentUnits}/{_totalUnits} units";
        }

        public override Dictionary<string, object> ToDictionary() => new()
        {
            { "Type", "ProgressGoal" },
            { "Name", _name },
            { "Description", _description },
            { "Points", _points },
            { "TotalUnits", _totalUnits },
            { "CurrentUnits", _currentUnits }
        };
    }

    public class NegativeGoal : Goal
    {
        public NegativeGoal(string name, string description, int penaltyPoints) 
            : base(name, description, penaltyPoints) { }

        public override int RecordEvent()
        {
            Console.WriteLine($" Habit recorded for '{_name}'. You lost {_points} points.");
            return -_points; // Deducts points from the score
        }

        public override string GetStatus() => "[!] (Penalty Habit)";

        public override Dictionary<string, object> ToDictionary() => new()
        {
            { "Type", "NegativeGoal" },
            { "Name", _name },
            { "Description", _description },
            { "Points", _points }
        };
    }

    // ==========================================
    // GAME MANAGER ENGINE
    // ==========================================
    public class EternalQuestGame
    {
        private List<Goal> _goals = new();
        private int _score = 0;

        public void Run()
        {
            while (true)
            {
                Console.WriteLine($"\n You currently have {_score} points.");
                Console.WriteLine("\nMenu Options:");
                Console.WriteLine("  1. Create New Goal");
                Console.WriteLine("  2. List Goals");
                Console.WriteLine("  3. Record Event");
                Console.WriteLine("  4. Save Goals");
                Console.WriteLine("  5. Load Goals");
                Console.WriteLine("  6. Quit");
                Console.Write("Select a choice from the menu: ");

                string choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CreateGoal();
                        break;
                    case "2":
                        ListGoalDetails();
                        break;
                    case "3":
                        RecordEvent();
                        break;
                    case "4":
                        SaveGoals();
                        break;
                    case "5":
                        LoadGoals();
                        break;
                    case "6":
                        Console.WriteLine("Goodbye! Keep up your eternal quest.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please choose between 1 and 6.");
                        break;
                }
            }
        }

        private void CreateGoal()
        {
            Console.WriteLine("--- Choose Goal Type ---");
            Console.WriteLine("1. Simple Goal (One-time completion)");
            Console.WriteLine("2. Eternal Goal (Repeatable indefinitely)");
            Console.WriteLine("3. Checklist Goal (Accomplish X times with bonus)");
            Console.WriteLine("4. Progress Goal (Incremental milestone tracking)");
            Console.WriteLine("5. Negative Goal (Bad habit penalty)");
            Console.Write("Select a goal type (1-5): ");

            string choice = Console.ReadLine()?.Trim();

            Console.Write("Enter goal short name: ");
            string name = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter a short description: ");
            string desc = Console.ReadLine()?.Trim() ?? "";

            try
            {
                if (choice == "1")
                {
                    Console.Write("Enter points for completing this goal: ");
                    int points = int.Parse(Console.ReadLine() ?? "0");
                    _goals.Add(new SimpleGoal(name, desc, points));
                }
                else if (choice == "2")
                {
                    Console.Write("Enter points earned each time recorded: ");
                    int points = int.Parse(Console.ReadLine() ?? "0");
                    _goals.Add(new EternalGoal(name, desc, points));
                }
                else if (choice == "3")
                {
                    Console.Write("Enter points per occurrence: ");
                    int points = int.Parse(Console.ReadLine() ?? "0");
                    Console.Write("Enter target number of completions: ");
                    int target = int.Parse(Console.ReadLine() ?? "0");
                    Console.Write("Enter bonus points upon final completion: ");
                    int bonus = int.Parse(Console.ReadLine() ?? "0");
                    _goals.Add(new ChecklistGoal(name, desc, points, target, bonus));
                }
                else if (choice == "4")
                {
                    Console.Write("Enter total units to reach (e.g., 26.2 for marathon): ");
                    double total = double.Parse(Console.ReadLine() ?? "0");
                    Console.Write("Enter points earned per unit: ");
                    int ptsPerUnit = int.Parse(Console.ReadLine() ?? "0");
                    _goals.Add(new ProgressGoal(name, desc, total, ptsPerUnit));
                }
                else if (choice == "5")
                {
                    Console.Write("Enter penalty points lost when triggered: ");
                    int penalty = int.Parse(Console.ReadLine() ?? "0");
                    _goals.Add(new NegativeGoal(name, desc, penalty));
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                    return;
                }
                Console.WriteLine($"Goal '{name}' successfully created!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid numeric input. Goal creation canceled.");
            }
        }

        private void ListGoalDetails()
        {
            Console.WriteLine("--- The Goals in Your Quest ---");
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals created yet.");
                return;
            }

            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetStatus()} {_goals[i].Name} ({_goals[i].Description})");
            }
        }

        private void RecordEvent()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals available to record.");
                return;
            }

            ListGoalDetails();
            Console.Write("Which goal did you accomplish/record? (Enter number): ");

            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= _goals.Count)
            {
                Goal selectedGoal = _goals[index - 1];
                int pointsEarned = selectedGoal.RecordEvent();
                _score += pointsEarned;
                Console.WriteLine($"Current Score: {_score} points.");
            }
            else
            {
                Console.WriteLine("Invalid goal selection number.");
            }
        }

        private void SaveGoals()
        {
            Console.Write("Enter filename to save goals (e.g., quest.json): ");
            string filename = Console.ReadLine()?.Trim() ?? "quest.json";

            try
            {
                var saveData = new Dictionary<string, object>
                {
                    { "Score", _score },
                    { "Goals", _goals.ConvertAll(g => g.ToDictionary()) }
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(saveData, options);
                File.WriteAllText(filename, jsonString);

                Console.WriteLine($"Quest successfully saved to {filename}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        private void LoadGoals()
        {
            Console.Write("Enter filename to load goals from: ");
            string filename = Console.ReadLine()?.Trim() ?? "quest.json";

            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(filename);
                using JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;

                _score = root.GetProperty("Score").GetInt32();
                _goals.Clear();

                JsonElement goalsElement = root.GetProperty("Goals");
                foreach (JsonElement item in goalsElement.EnumerateArray())
                {
                    string type = item.GetProperty("Type").GetString() ?? "";
                    string name = item.GetProperty("Name").GetString() ?? "";
                    string desc = item.GetProperty("Description").GetString() ?? "";
                    int points = item.GetProperty("Points").GetInt32();

                    if (type == "SimpleGoal")
                    {
                        bool completed = item.GetProperty("Completed").GetBoolean();
                        _goals.Add(new SimpleGoal(name, desc, points, completed));
                    }
                    else if (type == "EternalGoal")
                    {
                        _goals.Add(new EternalGoal(name, desc, points));
                    }
                    else if (type == "ChecklistGoal")
                    {
                        int target = item.GetProperty("Target").GetInt32();
                        int bonus = item.GetProperty("Bonus").GetInt32();
                        int currentCount = item.GetProperty("CurrentCount").GetInt32();
                        _goals.Add(new ChecklistGoal(name, desc, points, target, bonus, currentCount));
                    }
                    else if (type == "ProgressGoal")
                    {
                        double totalUnits = item.GetProperty("TotalUnits").GetDouble();
                        double currentUnits = item.GetProperty("CurrentUnits").GetDouble();
                        _goals.Add(new ProgressGoal(name, desc, totalUnits, points, currentUnits));
                    }
                    else if (type == "NegativeGoal")
                    {
                        _goals.Add(new NegativeGoal(name, desc, points));
                    }
                }

                Console.WriteLine($"Quest successfully loaded from {filename}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
    }

    // ==========================================
    // PROGRAM ENTRY POINT
    // ==========================================
    public class Program
    {
        public static void Main(string[] args)
        {
            EternalQuestGame game = new EternalQuestGame();
            game.Run();
        }
    }
}