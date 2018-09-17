using Newtonsoft.Json;
using System;

/************************
 * Name: Cristovao Galambos
 * Student ID: 459230413
 * Purpose: Network Based Arithmetic Game Challenge
 * Finished Date: 17/09/2018
 * **********************/

namespace ArithmeticChallenge
{
    [Serializable]
    class Equations
    {
        [JsonProperty("first_number")]
        public int FNumber { get; set; }
        [JsonProperty("second_number")]
        public int SNumber { get; set; }
        [JsonProperty("symbol")]
        public string Operator { get; set; }
        [JsonProperty("result")]
        public int ANumber { get; set; }
        [JsonProperty("is_correct")]
        public bool IsCorrect { get; set; }

        public Equations() { }

        public Equations(int firstNumber, int secondNumber, string symbol, int result, bool isCorrect)
        {
            FNumber = firstNumber;
            SNumber = secondNumber;
            Operator = symbol;
            ANumber = result;
            IsCorrect = isCorrect;
        }
    }
}
