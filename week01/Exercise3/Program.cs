using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        int magicNumber = randomGenerator.Next(1, 11);

        int guess = -1;
        int guessCount = 0; // Track the number of attempts

        while (guess != magicNumber)
        {
            Console.Write("What is your guess? ");
            
            // Basic input validation to prevent crashes if the user enters non-numbers
            if (!int.TryParse(Console.ReadLine(), out guess))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }
            // Increment the counter on every valid guess
            guessCount++; 

            if (magicNumber > guess)
            {
                Console.WriteLine("Higher");
            }
            else if (magicNumber < guess)
            {
                Console.WriteLine("Lower");
            }
            else
            {
                Console.WriteLine("You guessed it!");
                Console.WriteLine($"It took you {guessCount} guesses.");
            }
        }
    }
}