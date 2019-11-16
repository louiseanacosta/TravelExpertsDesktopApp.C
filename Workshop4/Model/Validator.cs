using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Workshop4
{
    /*
     * Author: DongMing Hu
     * Date: Feb. 22, 2019
     * Purpose: A class contains various validation methods to validate a textbox.
     * 
     */

    public static class Validator
    {
        /// <summary>
        /// Test if a TextBox have a non-negative integer value, return bool.
        /// </summary>
        /// <param name="tb">TextBox</param>
        /// <param name="txtBoxName">Name for the TextBox</param>
        /// <returns>Bool: if TextBox pass validation</returns>
        public static bool TBHasNonNegativeInt(TextBox tb, string txtBoxName)
        {
            if (TBIsEmpty(tb, txtBoxName))  // check if empty
                return false;
            else  // not empty, check if has integer
            {
                if (!TBHasInt(tb, txtBoxName))
                    return false;
                else  // is integer, check if negative  
                {
                    return !TBHasNegativeValue(tb, txtBoxName);
                }
            }
        }

        // Check if a textbox has non-negative double
        public static bool TBHasNonNegativeDouble(TextBox tb, string txtBoxName)
        {
            if (TBIsEmpty(tb, txtBoxName))  // check if empty
                return false;
            else  // not empty, check if has double
            {
                if (!TBHasDouble(tb, txtBoxName))
                    return false;
                else  // is double, check if negative  
                {
                    return !TBHasNegativeValue(tb, txtBoxName);
                }
            }
        }

        //--------------------- Breakdown Methods ---------------------------//

        // check if a textbox is empty, if yes, show messagebox
        // always do this FIRST!
        public static bool TBIsEmpty(TextBox tb, string txtBoxName)
        {
            if (tb.Text == "")
            {
                MessageBox.Show(txtBoxName + " need to be provided.", "Input Error");
                tb.Focus();
                return true;
            }
            else
                return false;
        }

        // check if a textbox has integer value, if no, show messagebox
        // always do this after isEmpty validation
        public static bool TBHasInt(TextBox tb, string txtBoxName)
        {
            if (!Int32.TryParse(tb.Text, out int myInt))
            {
                MessageBox.Show(txtBoxName + " requires a whole number.", "Input Error");
                tb.SelectAll();  // highlight text for easy replacement
                tb.Focus();
                return false;
            }
            else
                return true;
        }

        // check if a textbox has a double value, if no, show messagebox
        // always do this after isEmpty validation
        public static bool TBHasDouble(TextBox tb, string txtBoxName)
        {
            if (!Double.TryParse(tb.Text, out double myDouble))
            {
                MessageBox.Show(txtBoxName + " requires a valid number.", "Input Error");
                tb.SelectAll();  // highlight text for easy replacement
                tb.Focus();
                return false;
            }
            else
                return true;
        }

        // check if a textbox has negative value, if yes, show messagebox
        // always do this after isEmpty validation
        public static bool TBHasNegativeValue(TextBox tb, string txtBoxName)
        {
            if (Convert.ToDouble(tb.Text) < 0)
            {
                MessageBox.Show(txtBoxName + " requires a non-negative value.", "Input Error");
                tb.SelectAll();  // highlight text for easy replacement
                tb.Focus();
                return true;
            }
            else
                return false;
        }

        private static string title = "Entry Error";



        // --------------------- LOUISE ACOSTA------------------
        /// <summary>
        /// The title that will appear in dialog boxes.
        /// </summary>
        public static string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        /// <summary>
        /// Checks whether the user entered data into a text box.
        /// </summary>
        /// <param name="textBox">The text box control to be validated.</param>
        /// <returns>True if the user has entered data.</returns>
        public static bool IsPresent(Control control)
        {
            if (control.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                TextBox textBox = (TextBox)control;
                if (textBox.Text == "")
                {
                    MessageBox.Show(textBox.Tag + " is a required field.", Title);
                    textBox.Focus();
                    return false;
                }
            }
            else if (control.GetType().ToString() == "System.Windows.Forms.ComboBox")
            {
                ComboBox comboBox = (ComboBox)control;
                if (comboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(comboBox.Tag + " is a required field.", "Entry Error");
                    comboBox.Focus();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether the user entered a decimal value into a text box.
        /// </summary>
        /// <param name="textBox">The text box control to be validated.</param>
        /// <returns>True if the user has entered a decimal value.</returns>
        public static bool IsDecimal(TextBox textBox)
        {
            try
            {
                Convert.ToDecimal(textBox.Text);
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show(textBox.Tag + " must be a decimal number.", Title);
                textBox.Focus();
                return false;
            }
        }

        /// <summary>
        /// Checks whether the user entered an int value into a text box.
        /// </summary>
        /// <param name="textBox">The text box control to be validated.</param>
        /// <returns>True if the user has entered an int value.</returns>
        public static bool IsInt32(TextBox textBox)
        {
            try
            {
                Convert.ToInt32(textBox.Text);
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show(textBox.Tag + " must be an integer.", Title);
                textBox.Focus();
                return false;
            }
        }

        public static bool IsNonNegativeDouble(TextBox tb)
        {
            bool result = true;
            double val;
            if (!Double.TryParse(tb.Text, out val)) // not a double
            {
                result = false;
                MessageBox.Show(tb.Tag + " should be a number", Title);
                tb.SelectAll(); // highlight content of the box for replacing
                tb.Focus();
            }
            else if (val < 0) // negative
            {
                result = false;
                MessageBox.Show(tb.Tag + " should be 0 or greater.", Title);
                tb.SelectAll(); // highlight content of the box for replacing
                tb.Focus();
            }
            return result;
        }


    }
}
