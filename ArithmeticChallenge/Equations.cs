/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           14/09/2018
 *      Purpose:        Contains the serializable properties for the equation properties class
 *      Known Bugs:     nill
 */

using Newtonsoft.Json;
using System;

namespace ArithmeticChallenge
{
    [Serializable]
    class Equations
    {
        [JsonProperty("first_number")]
        public int FirstNumber { get; set; }
        [JsonProperty("second_number")]
        public int SecondNumber { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("result")]
        public int Result { get; set; }
        [JsonProperty("is_correct")]
        public bool IsCorrect { get; set; }

        public Equations() { }

        public Equations(int firstNumber, int secondNumber, string symbol, int result, bool isCorrect)
        {
            FirstNumber = firstNumber;
            SecondNumber = secondNumber;
            Symbol = symbol;
            Result = result;
            IsCorrect = isCorrect;
        }
    }
}
