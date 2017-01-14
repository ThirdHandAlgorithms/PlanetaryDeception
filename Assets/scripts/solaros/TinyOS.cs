namespace PlanetaryDeception
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Tiny OS for QnA's
    /// </summary>
    public class TinyOS : SolarOS
    {
        /// <summary>
        /// Currently active question
        /// </summary>
        private string currentQuestion = string.Empty;

        /// <summary>
        /// Return OS header
        /// </summary>
        /// <param name="optionalAppName">string</param>
        /// <returns>string</returns>
        public override string OSTxt(string optionalAppName = "")
        {
            return string.Empty;
        }

        /// <summary>
        /// Start a new question
        /// </summary>
        /// <param name="description"></param>
        public void NewQuestion(string description)
        {
            LoadMainMenu();

            currentQuestion = description;
        }

        /// <summary>
        /// Add a possible answer to the current question
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="onAnswer"></param>
        public void AddPossibleAnswer(string answer, RunApplication onAnswer)
        {
            MenuItems.Add(new SolarOSMenuItem(answer, onAnswer, RefreshDisplay));
        }

        /// <summary>
        /// Main menu
        /// </summary>
        protected override void LoadMainMenu()
        {
            MenuItems = new List<SolarOSMenuItem>();
        }

        /// <summary>
        /// Return menu header
        /// </summary>
        /// <returns>string</returns>
        protected override string MenuTxt()
        {
            return currentQuestion + "\n\n";
        }
    }
}
