using System;
using System.Collections.Generic;

namespace YouTubeTracker
{
    // Class responsible for tracking comment details
    public class Comment
    {
        public string CommenterName { get; set; }
        public string Text { get; set; }

        public Comment(string commenterName, string text)
        {
            CommenterName = commenterName;
            Text = text;
        }
    }

    // Class responsible for tracking video details and its associated comments
    public class Video
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int LengthInSeconds { get; set; }
        private List<Comment> _comments;

        public Video(string title, string author, int lengthInSeconds)
        {
            Title = title;
            Author = author;
            LengthInSeconds = lengthInSeconds;
            _comments = new List<Comment>();
        }

        // Method to add a comment to the video's list
        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }

        // Method to return the total number of comments
        public int GetCommentCount()
        {
            return _comments.Count;
        }

        // Method to retrieve the list of comments for iteration
        public List<Comment> GetComments()
        {
            return _comments;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a list to store the video objects
            List<Video> videos = new List<Video>();

            // Video 1
            Video video1 = new Video("C# Basics for Beginners", "CodeAcademy", 720);
            video1.AddComment(new Comment("Alice", "This cleared up classes and objects so well for me!"));
            video1.AddComment(new Comment("Bob", "Great pacing, thank you for the tutorial."));
            video1.AddComment(new Comment("Charlie", "Could you do a follow-up video on inheritance?"));
            videos.Add(video1);

            // Video 2
            Video video2 = new Video("Advanced CSS Grid Layouts", "WebDev Simplified", 945);
            video2.AddComment(new Comment("David", "Grid areas make responsive design so much cleaner."));
            video2.AddComment(new Comment("Emma", "Subgrid explanation was brilliant!"));
            video2.AddComment(new Comment("Frank", "Bookmarking this for my next project."));
            videos.Add(video2);

            // Video 3
            Video video3 = new Video("Python Data Analysis Workflow", "Data Professor", 1250);
            video3.AddComment(new Comment("Grace", "Pandas grouping operations are life savers."));
            video3.AddComment(new Comment("Hannah", "Clean code and very easy to follow along."));
            video3.AddComment(new Comment("Ian", "Thanks for sharing the CSV dataset link too."));
            videos.Add(video3);

            // Video 4
            Video video4 = new Video("Building an Agribusiness Portal", "AgriTech Solutions", 610);
            video4.AddComment(new Comment("John", "This will help rural farmers track yields efficiently."));
            video4.AddComment(new Comment("Karen", "Loved the inventory tracking module breakdown."));
            video4.AddComment(new Comment("Leo", "Wonderful application of software engineering!"));
            videos.Add(video4);

            // Iterate through the list of videos and display their details and comments
            foreach (Video video in videos)
            {
                Console.WriteLine($"Title: {video.Title}");
                Console.WriteLine($"Author: {video.Author}");
                Console.WriteLine($"Length: {video.LengthInSeconds} seconds");
                Console.WriteLine($"Number of Comments: {video.GetCommentCount()}");
                Console.WriteLine("Comments:");
                
                foreach (Comment comment in video.GetComments())
                {
                    Console.WriteLine($"  - {comment.CommenterName}: \"{comment.Text}\"");
                }
                
                Console.WriteLine(new string('-', 40));
            }
        }
    }
}