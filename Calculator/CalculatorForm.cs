using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    
    // TODO add calls to method checkExpression every time a number button, negate, or decimal is pressed
    
    // TODO modify regex to account for scientific notation (E[+-]\d+)
    
    public partial class CalculatorForm : Form
    {
        
        private const int MAX_DIGITS = 14;
        private const int MAX_STR_LENGTH = 16;

        private double _leftNum;
        private double _rightNum;
        private char _operator;
        
        public CalculatorForm()
        {
            
            InitializeComponent();
            CenterToScreen();
        }

        private void numBtn_Click(object sender, EventArgs e)
        {
            
            // Checking if expressionTB needs to be cleared
            checkExpression();
            
            var btnClicked = (Button)sender;

            if (displayTB.Text.Equals("0"))
            {
                displayTB.Text = "";
            }

            // Removing the negative and decimal from the number to get the number of digits
            var justNumbersStr = displayTB.Text.Replace("-", "");
            justNumbersStr = displayTB.Text.Replace(".", "");

            
            if (justNumbersStr.Length < MAX_DIGITS )
            {
                displayTB.AppendText(btnClicked.Text);
            }
            else
            {
                MessageBox.Show("Maximum number length reached.", "Max Length");
            }
        }

        private void ceBtn_Click(object sender, EventArgs e)
        {
            // Checking if expressionTB needs to be cleared
            checkExpression();

            displayTB.Text = "0";
        }

        private void cBtn_Click(object sender, EventArgs e)
        {
            // Checking if expressionTB needs to be cleared
            checkExpression();

            expressionTB.Clear();
            displayTB.Text = "0";
        }

        private void backspaceBtn_Click(object sender, EventArgs e)
        {
            // Checking if expressionTB needs to be cleared
            checkExpression();

            // Removing the last character
            displayTB.Text = displayTB.Text.Remove(displayTB.Text.Length - 1);

            if (displayTB.Text.Length == 0)
            {
                displayTB.Text = "0";
            }
        }

        private void operatorBtn_Click(object sender, EventArgs e)
        {
            var btnPressed = (Button)sender;
            
            if (Regex.IsMatch(expressionTB.Text, "^-?\\d+\\.?\\d*(E[\\+\\-]\\d+)?[+\\−×\\÷]$"))
            {
                // Removing the last character
                expressionTB.Text = expressionTB.Text.Remove(expressionTB.Text.Length - 1);
            }
            else
            {
                _leftNum = double.Parse(displayTB.Text);
                displayTB.Text = "0";

                expressionTB.Text = _leftNum.ToString();
            }
            
            expressionTB.AppendText(btnPressed.Text);
            _operator = char.Parse(btnPressed.Text);


        }

        private void negateBtn_Click(object sender, EventArgs e)
        {
            // Checking if expressionTB needs to be cleared
            checkExpression();

            var num = double.Parse(displayTB.Text);
            displayTB.Text = (num * -1).ToString();
        }

        private void decimalBtn_Click(object sender, EventArgs e)
        {
            // Checking if it already contains a decimal
            if (displayTB.Text.Contains(".")) {return;}
            
            // Checking if expressionTB needs to be cleared
            checkExpression();

            // Removing the negative and decimal from the number to get the number of digits
            var justNumbersStr = displayTB.Text.Replace("-", "");
            justNumbersStr = displayTB.Text.Replace(".", "");
            
            // Checking number of digits
            if (justNumbersStr.Length < MAX_DIGITS)
            {
                displayTB.AppendText(".");
            }
            
        }

        private void equalsBtn_Click(object sender, EventArgs e)
        {
            // Checking that a number and operator have been entered using regex
            if (!Regex.IsMatch(expressionTB.Text, "^-?\\d+\\.?\\d*(E[\\+\\-]\\d+)?[+\\−×\\÷]$")) { return; }

            _rightNum = double.Parse(displayTB.Text);

            string result = CalculatorOperation.Calculate(_leftNum, _rightNum, _operator).ToString();
            if (result.Length > MAX_STR_LENGTH)
            {
                while (result.Length - MAX_STR_LENGTH > 0)
                {
                    result = Regex.Replace(result, "\\dE", "E");
                }
            }

            displayTB.Text = result;

            expressionTB.AppendText(_rightNum + "=");

        }

        private void checkExpression()
        {
            if (expressionTB.Text.Contains("="))
            {
                expressionTB.Clear();
            }

            if (displayTB.Text.Contains("E"))
            {
                displayTB.Text="0";
            }
        }
        
    }
}