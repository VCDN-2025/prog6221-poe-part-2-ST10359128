using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace CybersecurityAwarenessBot
{
    class Program
    {
        // Memory for user details
        static string userName = "Friend";
        static string userInterest = "";

        // Predefined phishing responses (random)
        static Dictionary<string, List<string>> phishingTips = new Dictionary<string, List<string>>()
        {
            { "phishing", new List<string> {
                "Never click on suspicious links or attachments.",
                "Scammers often impersonate trusted institutions. Always verify!",
                "Watch for urgent language or threats in emails — it’s a red flag.",
                "Use a spam filter and keep your antivirus software up to date."
            }}
        };

        // Predefined keyword responses
        static Dictionary<string, string> keywordResponses = new Dictionary<string, string>()
        {
            { "password", "Use strong, unique passwords. Avoid using names, birthdays or simple sequences like '1234'." },
            { "scam", "Online scams often look real. Never share sensitive info via unsolicited messages or calls." },
            { "privacy", "Check your social media privacy settings and be cautious about what you share publicly." }
        };

        // Sentiment detection
        static Dictionary<string, string> sentiments = new Dictionary<string, string>()
        {
            { "worried", "It's okay to feel worried. Cyber threats are real, but you're taking a great step by learning more." },
            { "curious", "Curiosity is key to learning! Ask away and I’ll do my best to explain." },
            { "frustrated", "Sorry you’re feeling frustrated. Let's break it down together!" }
        };

        static void Main(string[] args)
        {
            PlayVoiceGreeting();
            DisplayAsciiArt();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================================================");
            Console.WriteLine("         WELCOME TO THE CYBERSECURITY AWARENESS BOT");
            Console.WriteLine("==================================================");
            Console.ResetColor();

            Console.Write("Please enter your name: ");
            userName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userName))
                userName = "Friend";

            TypeWrite($"\nHello, {userName}!  I'm your Cybersecurity Assistant.\nYou can ask me about topics like password safety, phishing, scams, and online privacy.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nAsk a question (or type 'exit' to quit): ");
                Console.ResetColor();

                string input = Console.ReadLine()?.ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(" I didn’t catch that. Please type something.");
                    continue;
                }

                if (input == "exit")
                {
                    Console.WriteLine($"\n Goodbye, {userName}! Stay safe online!");
                    break;
                }

                ProcessInput(input);
            }
        }

        static void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("Resources/greeting.wav");
                player.PlaySync();
            }
            catch
            {
                Console.WriteLine("[!] Voice greeting could not be played. Check file path.");
            }
        }

        static void DisplayAsciiArt()
        {
            try
            {
                string art = File.ReadAllText("Resources/ascii.txt");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(art);
                Console.ResetColor();
            }
            catch
            {
                Console.WriteLine("[!] ASCII art could not be displayed. Check file path.");
            }
        }

        static void TypeWrite(string message, int delay = 30)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static void ProcessInput(string input)
        {
            // Sentiment detection
            foreach (var sentiment in sentiments)
            {
                if (input.Contains(sentiment.Key))
                {
                    Console.WriteLine(sentiment.Value);
                    return;
                }
            }

            // Recognise and respond to specific cybersecurity keywords
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    userInterest = keyword;
                    Console.WriteLine(keywordResponses[keyword]);
                    return;
                }
            }

            // Handle follow-up on remembered topic
            if (input.Contains("more") && !string.IsNullOrEmpty(userInterest))
            {
                Console.WriteLine($"Since you're interested in {userInterest}, remember this: {keywordResponses[userInterest]}");
                return;
            }

            // Random response for phishing topic
            if (input.Contains("phishing"))
            {
                var tips = phishingTips["phishing"];
                Random rand = new Random();
                int index = rand.Next(tips.Count);
                Console.WriteLine(tips[index]);
                return;
            }

            // Friendly fallback for unknown input
            Console.WriteLine(" I'm not sure I understand. Could you ask about something like passwords, scams, or privacy?");
        }
    }
}


