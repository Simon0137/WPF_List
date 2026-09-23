using WPF_List.Core;

namespace WPF_List.Wpf;

public record struct BookingEntry
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public RoomType RoomType { get; set; }
    public int GuestsCount { get; set; }
}