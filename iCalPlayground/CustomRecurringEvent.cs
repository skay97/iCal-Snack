using Ical.Net;
using Ical.Net.DataTypes;

namespace iCalPlayground
{
    public class CustomRecurringEvent : BaseRecurringEvent
    {
        public Calendar CustomDayRecurrence(int interval)
        {
            var recurrenceRule = new RecurrencePattern
            {
                Frequency = FrequencyType.Daily,
                Interval = interval,
                Until = DateTime.MaxValue
            };

            var recurringCalEvent = CreateGenericRecurringEvent(DateTime.Parse("2023-09-15T07:00"),
                DateTime.Parse("2023-09-15T07:00").AddHours(1),
                recurrenceRule);

            ICalCalendar.Events.Add(recurringCalEvent);

            return ICalCalendar;

            //return ICalCalendar
            //    .GetOccurrences(CurrentDate, CurrentDate.AddMonths(3));
        }
    }
}
