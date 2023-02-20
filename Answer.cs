using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagineRITGame
{
    /// <summary>
    /// Class <c>Answer</c> represents a possible answer for a question
    /// </summary>
    class Answer
    {
        // Answer primitive state
        private bool correct;

        // Answer reference state
        private string text;

        /// <summary>
        /// Method <c>Answer</c> constructs a new instance of the Answer class
        /// </summary>
        /// <param name="correct">Boolean indicating correctness, true if correct, false otherwise</param>
        /// <param name="text">The text of the answer to be displayed</param>
        public Answer(bool correct, string text)
        {
            this.correct = correct;
            this.text = text;
        }

        /// <summary>
        /// Method <c>Text</c> fetches the text of the answer
        /// </summary>
        /// <returns>the string text of the answer</returns>
        public string Text()
        {
            return this.text;
        }

        /// <summary>
        /// Method <c>Correct</c> fetches the correctness of the answer
        /// </summary>
        /// <returns>true if the answer is correct, false otherwise</returns>
        public bool Correct()
        {
            return this.correct;
        }
    }
}
