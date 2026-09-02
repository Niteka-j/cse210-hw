using System;

class Program
{
    static void Main(string[] args)
    {
        string playAgain = "yes";

        while (playAgain.ToLower() == "yes")
        {
            // prompt the user for their grade percentage
            
            Console.Write("What is your grade percentage? ");
            string answer = Console.ReadLine();
            int percent = int.Parse(answer);

            string letter = "";

            if (percent >= 95)
            {
                letter = "A";
            }
            else if (percent >= 85)
            {
                letter = "B";
            }
            else if (percent >= 70)
            {
                letter = "C";
            }
            else if (percent >= 65)
            {
                letter = "D";
            }
            else
            {
                letter = "F";
            }

            Console.WriteLine($"Your grade is: {letter}");
            
            if (percent >= 70)
            {
                Console.WriteLine("You passed!");
            }
            else
            {
                Console.WriteLine("Next time will be your time!");
            }

            // Prompt user to try again
            Console.Write("\nWould you like to check another grade? (yes/no): ");
            playAgain = Console.ReadLine();
            Console.WriteLine();
        }
    }
}