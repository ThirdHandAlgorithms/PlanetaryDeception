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
        /// Start
        /// </summary>
        public override void Start()
        {
            base.Start();

            InitQuestions();
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