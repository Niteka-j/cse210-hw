using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();
        int userNumber = -1;

        Console.WriteLine("Enter a list of numbers, type 0 when finished.");

        while (userNumber != 0)
        {
            Console.Write("Enter number: ");
            string userResponse = Console.ReadLine();
            
            // Safe parsing prevents the program from crashing if the user enters letters
            if (!int.TryParse(userResponse, out userNumber))
            {
                Console.WriteLine("Invalid input. Please enter a whole number.");
                continue; 
            }

            if (userNumber != 0)
            {
                numbers.Add(userNumber);
            }
        }

        // Prevents an index out-of-bounds crash if the user enters '0' immediately
        if (numbers.Count == 0)
        {
            Console.WriteLine("No numbers were entered.");
            return;
        }

        int sum = 0;
        foreach (int number in numbers)
        {
            sum += number;
        }

        float average = ((float)sum) / numbers.Count;

        int max = numbers[0];
        int min = numbers[0]; 

        foreach (int number in numbers)
        {
            if (number > max)
            {
                max = number;
            }
            // Find the smallest number alongside the maximum
            if (number < min)
            {
                min = number;
            }
        }

        Console.WriteLine($"The sum is: {sum}");
        Console.WriteLine($"The average is: {average}");
        Console.WriteLine($"The largest number is: {max}");
        Console.WriteLine($"The smallest number is: {min}");

        // Automatically sorts the list in ascending order
        numbers.Sort();
        Console.WriteLine("The sorted list is:");
        foreach (int number in numbers)
        {
            Console.WriteLine(number);
        }
    }
}