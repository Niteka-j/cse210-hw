using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Create a list to hold different activities
        List<Activity> activities = new List<Activity>();

        // Instantiate activities
        Running runningActivity = new Running("03 Nov 2022", 30, 3.0);
        Cycling cyclingActivity = new Cycling("04 Nov 2022", 45, 12.0);
        Swimming swimmingActivity = new Swimming("05 Nov 2022", 25, 20);

        // Add them to the list
        activities.Add(runningActivity);
        activities.Add(cyclingActivity);
        activities.Add(swimmingActivity);

        // Iterate through the list and display summaries
        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
// ==========================================
//              ACTIVITY CLASS 
// ==========================================
public abstract class Activity
{
    private string _date;
    private int _minutes;

    public Activity(string date, int minutes)
    {
        _date = date;
        _minutes = minutes;
    }

    // Encapsulated getters
    public string GetDate()
    {
        return _date;
    }

    public int GetMinutes()
    {
        return _minutes;
    }

    // Abstract methods to be overridden in derived classes
    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    // Virtual or common summary method using polymorphism
    public virtual string GetSummary()
    {
        return $"{GetDate()} {GetType().Name} ({GetMinutes()} min)- Distance {GetDistance():F1} miles, Speed {GetSpeed():F1} mph, Pace: {GetPace():F1} min per mile";
    }
}
// ==========================================
//          RUNNING CLASS
// ==========================================
public class Running : Activity
{
    private double _distance; // in miles

    public Running(string date, int minutes, double distance) : base(date, minutes)
    {
        _distance = distance;
    }

    public override double GetDistance()
    {
        return _distance;
    }

    public override double GetSpeed()
    {
        return (GetDistance() / GetMinutes()) * 60;
    }

    public override double GetPace()
    {
        return GetMinutes() / GetDistance();
    }
}
// ==========================================
//                  CYCLING CLASS
// ==========================================
public class Cycling : Activity
{
    private double _speed; // in mph

    public Cycling(string date, int minutes, double speed) : base(date, minutes)
    {
        _speed = speed;
    }

    public override double GetDistance()
    {
        return (_speed * GetMinutes()) / 60;
    }

    public override double GetSpeed()
    {
        return _speed;
    }

    public override double GetPace()
    {
        return 60 / _speed;
    }
}
// ==========================================
//                 SWIMMING CLASS
// ==========================================
public class Swimming : Activity
{
    private int _laps;

    public Swimming(string date, int minutes, int laps) : base(date, minutes)
    {
        _laps = laps;
    }

    public override double GetDistance()
    {
        // Distance (miles) = swimming laps * 50 / 1000 * 0.62
        return _laps * 50.0 / 1000.0 * 0.62;
    }

    public override double GetSpeed()
    {
        return (GetDistance() / GetMinutes()) * 60;
    }

    public override double GetPace()
    {
        return GetMinutes() / GetDistance();
    }
}