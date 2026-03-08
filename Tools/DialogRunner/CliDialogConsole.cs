using System;
using System.Collections.Generic;
using PlanetaryDeception;

namespace DialogRunner
{
    public class CliDialogConsole : IDialogConsole
    {
        private string currentQuestion = string.Empty;
        private readonly List<(string Text, Action OnSelect)> options = new List<(string, Action)>();
        private bool ended = false;

        public bool HasEnded => ended;

        public void NewQuestion(string description)
        {
            options.Clear();
            currentQuestion = description;
        }

        public void AddPossibleAnswer(string answer, Action onAnswer)
        {
            options.Add((answer, onAnswer));
        }

        public void Clear()
        {
            options.Clear();
            currentQuestion = string.Empty;
        }

        public void End()
        {
            ended = true;
        }

        public void Run()
        {
            while (!ended)
            {
                Console.WriteLine();
                Console.WriteLine(currentQuestion);
                Console.WriteLine();

                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"  [{i + 1}] {options[i].Text}");
                }

                Console.WriteLine();
                Console.Write("> ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Count)
                {
                    options[choice - 1].OnSelect();
                }
                else
                {
                    Console.WriteLine("Invalid choice, try again.");
                }
            }
        }
    }
}
