using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_List.Wpf;

internal static class WpfUtils
{
    private static readonly Color ForegroundColor = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
    private static readonly Color DefaultForegroundColor = Color.FromArgb(0xBB, 0xAB, 0xAB, 0xAB);
    private static readonly Color ErrorColor = Color.FromArgb(0xFF, 0xD8, 0x34, 0x34);
    private static readonly Color BorderColor = Color.FromArgb(0xFF, 0xAB, 0xAD, 0xB3);

    public static void SetDefault(TextBox textBox, string defaultText)
    {
        if (textBox.BorderBrush != new SolidColorBrush(BorderColor))
            textBox.BorderBrush = new SolidColorBrush(BorderColor);

        textBox.Foreground = new SolidColorBrush(DefaultForegroundColor);
        textBox.Text = defaultText;
    }

    public static void SetActiveText(TextBox textBox, Label? errorLabel = null)
    {
        textBox.BorderBrush = new SolidColorBrush(BorderColor);
        errorLabel?.Visibility = Visibility.Hidden;

        if (textBox.Foreground is SolidColorBrush tc
            && tc.Color != new SolidColorBrush(DefaultForegroundColor).Color) return;

        textBox.Text = string.Empty;
        textBox.Foreground = new SolidColorBrush(ForegroundColor);
    }

    public static void InitErrorLabel(Label errorLabel, string? errorText = null)
    {
        errorLabel.Visibility = Visibility.Hidden;
        errorLabel.Foreground = new SolidColorBrush(ErrorColor);
        if (errorText != null) errorLabel.Content = errorText;
    }

    public static void SetError(TextBox textBox, Label? errorLabel = null, string? errorText = null)
    {
        textBox.BorderBrush = new SolidColorBrush(ErrorColor);
        errorLabel?.Visibility = Visibility.Visible;
        if (errorText != null) errorLabel?.Content = errorText;
    }
}