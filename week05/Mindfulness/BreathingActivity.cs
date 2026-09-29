using System;
using System.Threading;

namespace MindfulnessApp
{
    public class BreathingActivity : Activity
    {
        public BreathingActivity() : base(
            "Breathing Activity",
            "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
        {
        }

        public override void Run()
        {
            DisplayStartingMessage();

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);

            while (DateTime.Now < endTime)
            {
                Console.Write("\nBreathe in...");
                ShowGrowAnimation(4);
                Console.WriteLine();

                if (DateTime.Now >= endTime) break;

                Console.Write("Now breathe out...");
                ShowShrinkAnimation(6);
                Console.WriteLine();
            }

            DisplayEndingMessage();
        }

        private void ShowGrowAnimation(int seconds)
        {
            for (int i = 1; i <= seconds; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
        }

        private void ShowShrinkAnimation(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
        }
    }
}