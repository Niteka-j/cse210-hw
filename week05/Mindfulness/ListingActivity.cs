using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessApp
{
    public class ListingActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        public ListingActivity() : base(
            "Listing Activity",
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
        {
        }

        public override void Run()
        {
            DisplayStartingMessage();

            Random rand = new Random();
            Console.WriteLine("List as many responses you can to the following prompt:");
            string selectedPrompt = _prompts[rand.Next(_prompts.Count)];
            Console.WriteLine($"\n --- {selectedPrompt} --- \n");
            
            Console.Write("You may begin in: ");
            ShowCountDown(5);
            Console.WriteLine();

            List<string> userItems = GetListFromUser();
            Console.WriteLine($"\nYou listed {userItems.Count} items!");

            DisplayEndingMessage();
        }

        private List<string> GetListFromUser()
        {
            List<string> items = new List<string>();
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);

            while (DateTime.Now < endTime)
            {
                if (DateTime.Now >= endTime) break;

                Console.Write("> ");
                
                if (Console.KeyAvailable)
                {
                    string input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        items.Add(input);
                    }
                }
                else
                {
                    Thread.Sleep(200);
                }
            }

            return items;
        }
    }
}