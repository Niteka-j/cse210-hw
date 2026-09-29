using System;
using System.Threading;

namespace MindfulnessApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Menu Options:");
                Console.WriteLine("  1. Start breathing activity");
                Console.WriteLine("  2. Start reflection activity");
                Console.WriteLine("  3. Start listing activity");
                Console.WriteLine("  4. Quit");
                Console.Write("Select a choice from the menu: ");

                string choice = Console.ReadLine();

                Activity activity = null;

                switch (choice)
                {
                    case "1":
                        activity = new BreathingActivity();
                        break;
                    case "2":
                        activity = new ReflectionActivity();
                        break;
                    case "3":
                        activity = new ListingActivity();
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        continue;
                    default:
                        Console.WriteLine("Invalid choice. Please choose between 1 and 4.");
                        Thread.Sleep(1500);
                        continue;
                }

                if (activity != null)
                {
                    activity.Run();
                }
            }
        }
    }
}