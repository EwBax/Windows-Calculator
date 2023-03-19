using System;

namespace Calculator
{
    
    /// <summary>
    /// Class to handle mathematical calculations
    /// </summary>
    public static class CalculatorOperation
    {

        
        /// <summary>
        /// A method that takes in two numbers and a mathematical operator character, and returns the result of the operation.
        /// </summary>
        /// <param name="leftNum">The number on the left of the equation.</param>
        /// <param name="rightNum">The number on the right of the equation.</param>
        /// <param name="operation">The mathematical operator character.</param>
        /// <returns>A double representing the result of the calculation.</returns>
        /// <exception cref="ArgumentException">Thrown if the operator is not valid.</exception>
        public static double Calculate(double leftNum, double rightNum, char operation)
        {
            switch (operation)
            {
                case '+':
                    return leftNum + rightNum;
                case '−':
                    return leftNum - rightNum;
                case '×' :
                    return leftNum * rightNum;
                case '÷':
                    return leftNum / rightNum;
                default:
                    throw new ArgumentException("Invalid operator.");
            }
            
        }


    }
}