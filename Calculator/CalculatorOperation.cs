using System;

namespace Calculator
{
    public class CalculatorOperation
    {

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