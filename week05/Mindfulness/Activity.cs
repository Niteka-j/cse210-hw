using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessApp
{
    public abstract class Activity
    {
        private string _name;
        private string _description;
        protected int _duration;

        public Activity(string name, string description)
        {
            _name = name;
            _description = description;
            _duration = 0;
        }

        public virtual void DisplayStartingMessage()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the {_name}.\n");
            Console.WriteLine($"{_description}\n");
            
            Console.Write("Enter the duration of the activity in seconds: ");
            if (int.TryParse(Console.ReadLine(), out int duration))
            {
                _duration = duration;
            }
            else
            {
                _duration = 30;
                Console.WriteLine("Invalid input. Defaulting to 30 seconds.");
            }

            Console.Clear();
            Console.WriteLine("Get ready...");
            ShowSpinner(3);
        }

        public virtual void DisplayEndingMessage()
        {
            Console.WriteLine("\nWell done!!");
            ShowSpinner(3);

            Console.WriteLine($"\nYou have completed another {_duration} seconds of the {_name}.");
            ShowSpinner(5);
        }

        protected void ShowSpinner(int seconds)
        {
            List<string> animations = new List<string> { "|", "/", "-", "\\" };
            int i = 0;
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(seconds);

            while (DateTime.Now < endTime)
            {
                string s = animations[i];
                Console.Write(s);
                Thread.Sleep(250);
                Console.Write("\b \b");
                i++;
                if (i >= animations.Count)
                {
                    i = 0;
                }
            }
        }

        protected void ShowCountDown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write(i);
                Thread.Sleep(1000);
                Console.Write("\b \b");
            }
        }

        public abstract void Run();
    }
}