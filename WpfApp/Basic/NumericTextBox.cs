using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp.Basic
{
    public class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            PreviewTextInput += NumericTextBox_PreviewTextInput;
            AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(NumericTextBox_PreviewExecuted), true);
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumericInput(e.Text))
                e.Handled = true;
        }

        private void NumericTextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!IsNumericInput(Clipboard.GetText()))
                    e.Handled = true;
            }
        }

        private bool IsNumericInput(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
