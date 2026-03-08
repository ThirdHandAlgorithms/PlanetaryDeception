namespace PlanetaryDeception
{
    using System;

    public interface IDialogConsole
    {
        void NewQuestion(string description);
        void AddPossibleAnswer(string answer, Action onAnswer);
        void Clear();
    }
}
