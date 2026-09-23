using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_List.Core;
using WPF_List.Wpf.Resources;

namespace WPF_List.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public RoomType? RoomType { get; private set; }
    public int? GuestsCount { get; private set; }

    public ObservableCollection<BookingEntry> Entries { get; private set; } = [];

    private BookingEntry? _selectedEntry;

    private static readonly Color ForegroundColor = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
    private static readonly Color DefaultForegroundColor = Color.FromArgb(0xBB, 0xAB, 0xAB, 0xAB);
    private static readonly Color ErrorColor = Color.FromArgb(0xFF, 0xD8, 0x34, 0x34);
    private static readonly Color BorderColor = Color.FromArgb(0xFF, 0xAB, 0xAD, 0xB3);

    public MainWindow()
    {
        InitializeComponent();

        StartDateLabel.Content = RInterface.StartDateLabel;
        EndDateLabel.Content = RInterface.EndDateLabel;
        RoomTypeLabel.Content = RInterface.RoomLabel;
        GuestsLabel.Content = RInterface.GuestsLabel;
        TotalLabel.Content = RInterface.TotalLabel;

        AddEntryButton.Content = RInterface.AddEntryButton;
        DeleteEntryButton.Content = RInterface.DeleteEntryButton;
        CalculateEntriesButton.Content = RInterface.CalculateButton;

        WpfUtils.InitErrorLabel(StartDateErrorLabel, RErrors.IncorrectDateFormat);
        WpfUtils.InitErrorLabel(EndDateErrorLabel, RErrors.IncorrectDateFormat);
        WpfUtils.InitErrorLabel(RoomComboErrorLabel, RErrors.NoRoomChoice);
        WpfUtils.InitErrorLabel(GuestsErrorLabel, RErrors.IncorrectGuestsCount);

        WpfUtils.SetDefault(StartDateBox, RInterface.DateFormat);
        WpfUtils.SetDefault(EndDateBox, RInterface.DateFormat);
        WpfUtils.SetDefault(GuestsBox, RInterface.GuestsExample);

        RoomTypeCombo.Items.Add(nameof(Core.RoomType.Standart));
        RoomTypeCombo.Items.Add(nameof(Core.RoomType.Deluxe));
        RoomTypeCombo.Items.Add(nameof(Core.RoomType.Suite));

        DataEntries.ItemsSource = Entries;
    }

    private void StartDateBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(StartDateBox.Text)) WpfUtils.SetDefault(StartDateBox, RInterface.DateFormat);

        if (DateOnly.TryParseExact(StartDateBox.Text,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            StartDate = date;
        }
        else
        {
            WpfUtils.SetError(StartDateBox, StartDateErrorLabel);
        }
    }

    private void EndDateBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(EndDateBox.Text)) WpfUtils.SetDefault(EndDateBox, RInterface.DateFormat);

        if (DateOnly.TryParseExact(EndDateBox.Text,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            EndDate = date;
        }
        else
        {
            WpfUtils.SetError(EndDateBox, EndDateErrorLabel);
        }
    }

    private void RoomTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (RoomTypeCombo.SelectedItem != null)
        {
            RoomType = RoomTypeCombo.SelectedItem switch
            {
                "Standart" => Core.RoomType.Standart,
                "Deluxe" => Core.RoomType.Deluxe,
                "Suite" => Core.RoomType.Suite,
                _ => null
            };
        }
    }

    private void GuestsBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(GuestsBox.Text)) WpfUtils.SetDefault(GuestsBox, RInterface.GuestsExample);

        if (int.TryParse(GuestsBox.Text,
                CultureInfo.InvariantCulture,
                out var count) && count <= 4)
        {
            GuestsCount = count;
        }
        else
        {
            WpfUtils.SetError(GuestsBox, GuestsErrorLabel);
        }
    }

    private void AddEntryButton_Click(object sender, RoutedEventArgs e)
    {
        var validations = new (bool IsInvalid, Label ErrorLabel)[]
        {
            (StartDate == null, StartDateErrorLabel),
            (EndDate == null, EndDateErrorLabel),
            (RoomType == null, RoomComboErrorLabel),
            (GuestsCount == null, GuestsErrorLabel)
        };

        foreach(var (isInvalid, errorLabel) in validations)
        {
            errorLabel.Visibility = isInvalid
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        if (validations.Any(v => v.IsInvalid)) return;
        Entries.Add(new BookingEntry
        {
            StartDate = StartDate!.Value,
            EndDate = EndDate!.Value,
            RoomType = RoomType!.Value,
            GuestsCount = GuestsCount!.Value
        });
    }

    private void DataEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedEntry = (BookingEntry?)DataEntries.SelectedItem;
    }

    private void DeleteEntryButton_Click(object sender, RoutedEventArgs e)
    {
        Entries.Remove(_selectedEntry ?? default);
    }

    private void CalculateEntriesButton_Click(object sender, RoutedEventArgs e)
    {
        if (Entries.Count <= 0) return;

        var totalSum = 0D;
        foreach (var entry in Entries)
        {
            var nights = BookingCore.CalculateNights(entry.StartDate, entry.EndDate);

            totalSum += BookingCore.CalculateTotalPrice(
                nights,
                entry.RoomType,
                BookingCore.GetSeasonType(entry.StartDate)
            );
        }

        TotalBox.Text = totalSum.ToString(CultureInfo.InvariantCulture) + " €";
    }

    private void StartDateBox_GotFocus(object sender, RoutedEventArgs e)
    {
        WpfUtils.SetActiveText(StartDateBox, StartDateErrorLabel);
    }

    private void EndDateBox_GotFocus(object sender, RoutedEventArgs e)
    {
        WpfUtils.SetActiveText(EndDateBox, EndDateErrorLabel);
    }

    private void GuestsBox_GotFocus(object sender, RoutedEventArgs e)
    {
        WpfUtils.SetActiveText(GuestsBox, GuestsErrorLabel);
    }

    private void RoomTypeCombo_DropDownOpened(object sender, EventArgs e)
    {
        if (RoomComboErrorLabel.Visibility == Visibility.Visible) RoomComboErrorLabel.Visibility = Visibility.Hidden;
    }
}