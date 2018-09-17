using Newtonsoft.Json;
using System;

/************************
 * Name: Cristovao Galambos
 * Student ID: 459230413
 * Purpose: Network Based Arithmetic Game Challenge
 * Finished Date: 17/09/2018
 * **********************/

namespace ArithmeticChallengeStudent
{
    [Serializable]
    class Equations
    {
        [JsonProperty("first_number")]
        public ushort FirstNumber { get; set; }
        [JsonProperty("second_number")]
        public ushort SecondNumber { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("result")]
        public ushort Result { get; set; }
        [JsonProperty("is_correct")]
        public bool IsCorrect { get; set; }

        public Equations() { }

        public Equations(ushort firstNumber, ushort secondNumber, string symbol, ushort result, bool isCorrect)
        {
            FirstNumber = firstNumber;
            SecondNumber = secondNumber;
            Symbol = symbol;
            Result = result;
            IsCorrect = isCorrect;
        }
    }
}
