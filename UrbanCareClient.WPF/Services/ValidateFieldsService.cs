using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;

namespace UrbanCareClient.WPF.Services
{
    public static class ValidateFieldsService
    {
        public static bool ValidateTextBoxFilled(TextBox textBox, string textBoxName, Color borderColor)
        {
            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                IndicateControlError(textBox, $"Поле {textBoxName} обязательно для заполнения");
                return false;
            }

            IndicateControlCorrect(textBox, borderColor);
            return true;
        }

        public static bool ValidateComboBoxFilled(ComboBox comboBox, string textBoxName, Color borderColor)
        {
            if (string.IsNullOrEmpty(comboBox.Text.Trim()))
            {
                IndicateControlError(comboBox, $"Поле {textBoxName} обязательно для заполнения");
                return false;
            }

            IndicateControlCorrect(comboBox, borderColor);
            return true;
        }

        public static bool ValidateTextBoxRegex(TextBox textBox, string pattern, string error, Color borderColor)
        {
            if (!Regex.IsMatch(textBox.Text.Trim(), pattern))
            {
                IndicateControlError(textBox, error);
                return false;
            }

            IndicateControlCorrect(textBox, borderColor);
            return true;
        }

        public static bool ValidatePasswordBoxFilled(PasswordBox passwordBox, string textBoxName, Color borderColor)
        {
            if (string.IsNullOrEmpty(passwordBox.Password.Trim()))
            {
                IndicateControlError(passwordBox, $"Поле {textBoxName} обязательно для заполнения");
                return false;
            }

            IndicateControlCorrect(passwordBox, borderColor);
            return true;
        }

        public static bool ValidatePasswordBoxMin(PasswordBox passwordBox, int min, Color borderColor)
        {
            if (passwordBox.Password.Length < min)
            {
                IndicateControlError(passwordBox, $"Пароль должен содержать минимум {min} символов");
                return false;
            }

            IndicateControlCorrect(passwordBox, borderColor);
            return true;
        }

        public static bool ValidatePasswordBoxConfirm(PasswordBox passwordBox, PasswordBox confirmPasswordBox, Color borderColor)
        {
            if (passwordBox.Password != confirmPasswordBox.Password)
            {
                IndicateControlError(passwordBox, $"Пароли не совпадают");
                return false;
            }

            IndicateControlCorrect(passwordBox, borderColor);
            return true;
        }

        public static bool ValidatePasswordBoxRegex(PasswordBox passwordBox, string pattern, string error, Color borderColor)
        {
            if (!Regex.IsMatch(passwordBox.Password, pattern))
            {
                IndicateControlError(passwordBox, error);
                return false;
            }

            IndicateControlCorrect(passwordBox, borderColor);
            return true;
        }

        public static bool ValidateDatePickerFilled(DatePicker datePicker, string datePickerName, Color borderColor)
        {
            if (datePicker.SelectedDate == null)
            {
                IndicateControlError(datePicker, $"Поле {datePickerName} обязательно для заполнения");
                return false;
            }

            IndicateControlCorrect(datePicker, borderColor);
            return true;
        }

        public static bool ValidateDatePickerRange(DatePicker datePicker, string datePickerName,
            DateTime minDate, DateTime maxDate, Color borderColor)
        {
            if (datePicker.SelectedDate == null) return false;

            if (datePicker.SelectedDate < minDate || datePicker.SelectedDate > maxDate)
            {
                IndicateControlError(datePicker, $"Поле {datePickerName} должно быть в диапазоне от " +
                    $"{minDate.ToString("dd.MM.yyyy")} до {maxDate.ToString("dd.MM.yyyy")}");
                return false;
            }

            IndicateControlCorrect(datePicker, borderColor);
            return true;
        }

        private static void IndicateControlError(Control control, string error)
        {
            if (control.ToolTip != null)
            {
                var toolTip = control.ToolTip.ToString();
                if (toolTip != null && toolTip.Contains(error))
                    return;
            }
            control.ToolTip += error + '\n';
            control.Background = new SolidColorBrush(Colors.LightPink);
            control.BorderBrush = new SolidColorBrush(Colors.Red);
            control.BorderThickness = new System.Windows.Thickness(2);
        }

        private static void IndicateControlCorrect(Control control, Color borderColor)
        {
            control.ToolTip = null;
            control.Background = new SolidColorBrush(Colors.White);
            control.BorderBrush = new SolidColorBrush(borderColor);
            control.BorderThickness = new System.Windows.Thickness(1);
        }

    }
}
