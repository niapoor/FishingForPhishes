using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagineRITGame
{
    /// <summary>
    /// Class <c>Question</c> represents a question to be asked and asnwers
    /// </summary>
    class Question
    {

        // Question state
        private int numAnswers;

        private string questionText;
        private List<Answer> answerList;

        /// <summary>
        /// Method <c>Question</c> Constructs a Question instance from an Array of input strings
        /// </summary>
        /// <param name="questionData"></param>A String array of length 5. Index 0 represents question text, index 1 represents the correct answer, indexes 2-4 indicate incorrect answers(with an empty string indicating an unused answer)
        /// <param name="shuffle"></param>Boolean to specify if the answers should be randomly ordered. Defaults to true
        public Question(String[] questionData, bool shuffle = true)
        {
            // Determines the Question Text
            this.questionText = questionData[0];

            this.answerList = new List<Answer>();

            // Create the create correct asnwer
            this.answerList.Add(new Answer(true, questionData[1]));

            // Fill the incorrect answers
            int counter = 2;
            while (counter < questionData.Length && questionData[counter] != "")
            {
                this.answerList.Add(new Answer(false, questionData[counter]));
                counter++;
            }

            // Shuffle if necessary
            if (shuffle)
            {
                this.ShuffleList();
            }

            // If there is an 'all of the above' answer, put it at the bottom
            for (int i = 0; i < this.answerList.Count; i++)
            {
                if (this.answerList[i].Text().ToLower().Contains("all of the above"))
                {
                    Answer temp = this.answerList[i];
                    this.answerList[i] = this.answerList[this.answerList.Count - 1];
                    this.answerList[this.answerList.Count - 1] = temp;
                }
            }

            // Store the number of answers for ease of access
            this.numAnswers = this.answerList.Count;
        }

        /// <summary>
        /// Method <c>QuestionText</c> returns the question text of the question
        /// </summary>
        /// <returns>The string representation of the Question Text</returns>
        public String QuestionText()
        {
            return this.questionText;
        }

        /// <summary>
        /// Method <c>CheckAnswer</c> Determines if the selected answer is correct or incorrect
        /// </summary>
        /// <param name="index">The index of the answer object in the Question's AnswerList</param>
        /// <returns>true if the answer is correct, false otherwise</returns>
        public bool CheckAnswer(int index)
        {
            return this.answerList[index].Correct();
        }

        /// <summary>
        /// Method <c>NumAnswers</c> fetches the number of answers for the question
        /// </summary>
        /// <returns>The integer representation of the total number of answers for the question</returns>
        public int NumAnswers()
        {
            return this.numAnswers;
        }

        /// <summary>
        /// Method <c>ShuffleList</c> shuffles the Question's AnswerList
        /// </summary>
        private void ShuffleList()
        {
            int n = this.answerList.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                Answer answer = this.answerList[k];
                this.answerList[k] = this.answerList[n];
                this.answerList[n] = answer;
            }
        }
    }
}
