namespace PlanetaryDeception
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Level 2 interrogation part
    /// </summary>
    public class Level_2_interrogation : LevelBase
    {
        /// <summary>
        /// Console output
        /// </summary>
        public Text ConsoleOutput;

        /// <summary>
        /// Put the link to the TinyOS here
        /// </summary>
        public TinyOS OS;

        /// <summary>
        /// are the questions set
        /// </summary>
        private bool questionsInitialized = false;

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            // Order of executing Start() on TinyOS seems to be After the Level is Started for some reason...
            if (OS.IsStarted && !questionsInitialized)
            {
                InitQuestions();
                questionsInitialized = true;
            }
        }

        /// <summary>
        /// Init Questions
        /// </summary>
        private void InitQuestions()
        {
            OS.NewQuestion("Hi");
            OS.AddPossibleAnswer(
                "Ok",
                () =>
                {
                    OS.NewQuestion("Are you sure?");
                    OS.AddPossibleAnswer("Yes", () => { });
                    OS.AddPossibleAnswer("No", () => { });
                });

            OS.AddPossibleAnswer(
                "What?",
                () =>
                {
                });

            // at the end of interrogation add KnownItemsInventory.Instance().TransferItem(KnownItem.VenrefInterrogated, PlayerInventory.Instance());
        }
    }
}