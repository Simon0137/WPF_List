using WPF_List.Core.Models;

namespace WPF_List.Core;

public static class BookingCore
{
    public static List<Room> RoomList { get; } =
    [
        new() { Type = RoomType.Standart, Price = 30.00 },
        new() { Type = RoomType.Deluxe, Price = 55.00 },
        new() { Type = RoomType.Suite, Price = 90.00 }
    ];

    public static List<Season> SeasonList { get; } =
    [
        new() { Type = SeasonType.Winter, PriceMultiplier = 0.75 },
        new() { Type = SeasonType.Spring, PriceMultiplier = 1.25 },
        new() { Type = SeasonType.Summer, PriceMultiplier = 2.00 },
        new() { Type = SeasonType.Autumn, PriceMultiplier = 1.00 }
    ];

    public static int CalculateNights(DateOnly startDate, DateOnly endDate)
    {
        return endDate.DayNumber - startDate.DayNumber;
    }

    public static double CalculateTotalPrice(int nightsCount, RoomType roomType, SeasonType seasonType)
    {
        var roomPrice = RoomList.First(r => r.Type == roomType).Price;
        var multiplier = SeasonList.First(s => s.Type == seasonType).PriceMultiplier;

        return nightsCount * roomPrice * multiplier;
    }

    public static SeasonType GetSeasonType(DateOnly date)
    {
        var month = date.Month;
        return month switch
        {
            < 3 or 12 => SeasonType.Winter,
            < 6 => SeasonType.Spring,
            < 9 => SeasonType.Summer,
            _ => SeasonType.Autumn
        };
    }
}