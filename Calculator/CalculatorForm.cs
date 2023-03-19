using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace Calculator
{
    
    /// <summary>
    /// GUI Form representing a basic calculator.
    /// </summary>
    public partial class CalculatorForm : Form
    {
        // Class constants
        private const int MAX_DIGITS = 14;
        private const int MAX_STR_LENGTH = 16;

        // Data for use in the calculation
        private double _leftNum;
        private double _rightNum;
        private char _operator;
        
        public CalculatorForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        /// <summary>
        /// Event handler for numerical button presses.
        /// </summary>
        /// <param name="sender">The object that called this event handler.</param>
        /// <param name="e"></param>
        private void numBtn_Click(object sender, EventArgs e)
        {
            
            // Checking if expressionTB needs to be cleared
            checkTextBoxes();
            
            // Casting the sender to a button
            var btnClicked = (Button)sender;

            // If only 0 is displayed, erase it before writing the number pressed
            if (displayTB.Text.Equals("0"))
            {
                displayTB.Text = "";
            }

            // Removing the negative and decimal from the number to get the number of digits
            var justNumbersStr = displayTB.Text.Replace("-", "");
            justNumbersStr = displayTB.Text.Replace(".", "");

            // Checking number of digits, then appending if allowed, or displaying error message if too long
            if (justNumbersStr.Length < MAX_DIGITS )
            {
                displayTB.AppendText(btnClicked.Text);
                
            }
            else
            {
                MessageBox.Show("Maximum number length reached.", "Max Length");
            }
        }

        /// <summary>
        /// Clears display text box, and expression test box if expression already evaluated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ceBtn_Click(object sender, EventArgs e)
        {
            // Checking if expressionTB needs to be cleared
            checkTextBoxes();

            displayTB.Text = "0";
        }

        
        /// <summary>
        /// Resets and clears both text boxes on calculator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cBtn_Click(object sender, EventArgs e)
        {
            expressionTB.Clear();
            displayTB.Text = "0";
        }

        /// <summary>
        /// Removes the last entry to the display text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backspaceBtn_Click(object sender, EventArgs e)
        {
            // Checking if expressionTB needs to be cleared
            checkTextBoxes();

            // Removing the last character
            displayTB.Text = displayTB.Text.Remove(displayTB.Text.Length - 1);

            if (displayTB.Text.Length == 0)
            {
                displayTB.Text = "0";
            }
        }

        /// <summary>
        /// Applies the selected mathematical operator to the expression text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void operatorBtn_Click(object sender, EventArgs e)
        {
            // Casting sender to button to get text
            var btnPressed = (Button)sender;
            
            // Checking if expressionTB already contains an operator
            if (Regex.IsMatch(expressionTB.Text, "^-?\\d+\\.?\\d*(E[\\+\\-]\\d+)?[+\\−×\\÷]$"))
            {
                // Removing the operator that was already pressed
                expressionTB.Text = expressionTB.Text.Remove(expressionTB.Text.Length - 1);
            }
            else
            {
                // If an operator has not been pressed yet, store left number and display in expressionTB
                _leftNum = double.Parse(displayTB.Text);
                displayTB.Text = "0";

                expressionTB.Text = _leftNum.ToString();
            }
            
            // Appending the operator pressed and storing it.
            expressionTB.AppendText(btnPressed.Text);
            _operator = char.Parse(btnPressed.Text);


        }

        /// <summary>
        /// Negates the number currently displayed in displayTB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negateBtn_Click(object sender, EventArgs e)
        {
            // Checking if expressionTB needs to be cleared
            checkTextBoxes();

            var num = double.Parse(displayTB.Text);
            displayTB.Text = (num * -1).ToString();
        }

        /// <summary>
        /// Handles decimal button. Only adds decimal if not already in expressionTB,
        /// and there are still enough digits left for one to be entered after the decimal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decimalBtn_Click(object sender, EventArgs e)
        {
            // Checking if it already contains a decimal
            if (displayTB.Text.Contains(".")) {return;}
            
            // Checking if expressionTB needs to be cleared
            checkTextBoxes();

            // Removing the negative and decimal from the number to get the number of digits
            var justNumbersStr = displayTB.Text.Replace("-", "");
            justNumbersStr = displayTB.Text.Replace(".", "");
            
            // Checking number of digits, only adding decimal if there is enough space left for another digit after the decimal
            if (justNumbersStr.Length < MAX_DIGITS)
            {
                displayTB.AppendText(".");
            }
            
        }

        /// <summary>
        /// Evaluates the expression in expressionTB if valid, and updates displayTB with result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void equalsBtn_Click(object sender, EventArgs e)
        {
            // Checking that a number and operator have been entered using regex
            if (!Regex.IsMatch(expressionTB.Text, "^-?\\d+\\.?\\d*(E[\\+\\-]\\d+)?[+\\−×\\÷]$")) { return; }

            // Storing right number
            _rightNum = double.Parse(displayTB.Text);

            // checking how much longer result string is than MAX_STR_LENGTH and trimming until correct length
            string result = CalculatorOperation.Calculate(_leftNum, _rightNum, _operator).ToString();
            if (result.Length > MAX_STR_LENGTH)
            {
                while (result.Length - MAX_STR_LENGTH > 0)
                {
                    result = Regex.Replace(result, "\\dE", "E");
                }
            }

            // Updating displayTB to show result
            displayTB.Text = result;

            // Updating expressionTB with right num and =
            expressionTB.AppendText(_rightNum + "=");

        }

        /// <summary>
        /// Checks if expression was recently evaluated and clears expressionTB if so.
        /// Also checks if displayTB contains scientific notation and resets if so.
        /// </summary>
        private void checkTextBoxes()
        {
            // If the expression was evaluated with equals we want to clear the expressionTB when the displayTB is updated
            if (expressionTB.Text.Contains("="))
            {
                expressionTB.Clear();
            }

            // We do not want people modifying numbers with scientific notation so if E is in displayTB, reset it to zero.
            if (displayTB.Text.Contains("E"))
            {
                displayTB.Text="0";
            }
        }
        
    }
}