using System.Net;
using Domain.Shared;

namespace Domain.Errors;

public class ServiceErrors
{
    public static readonly Error NotFound= new ("Service.NotFound",
        "Service was not found.",HttpStatusCode.NotFound);

    public static readonly Error NameEmpty = new("Service.NameEmpty",
        "Service name cannot be empty.");

    public static readonly Error InvalidSchedule = new("Service.InvalidSchedule",
        "Opening time must be earlier than closing time.");

    public static readonly Error ScheduleOutOfRange = new("Service.ScheduleOutOfRange",
        "Opening and closing times must fall within a single day.");

    public static readonly Error WorkDaysEmpty = new("Service.WorkDaysEmpty",
        "A service must operate on at least one day of the week.");

    public static readonly Error NegativePrice = new("Service.NegativePrice",
        "Service price cannot be negative.");
}
