using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Ical.Net;
using Calendar = Ical.Net.Calendar;

namespace iCalPlayground
{
    public class NextOccurrenceWorkflow
    {
        public static string Calendar()
        {
            var calendar = new Calendar();

            var icalEvent = new CalendarEvent
            {
                Summary = "S&R - Reception",
                Description = "Salman and Rooshna's Reception",
                // 30th of December at 7 o'clock.
                Start = new CalDateTime(2023, 12, 30, 19, 0, 0),
                // Ends 4 hours later.
                End = new CalDateTime(2021, 3, 15, 23, 0, 0)
            };

            calendar.Events.Add(icalEvent);

            var iCalSerializer = new CalendarSerializer();
            string result = iCalSerializer.SerializeToString(calendar);

            return result;
        }

        public static Occurrence RecurringEventThanksgiving()
        {
            var calendar = new Calendar();

            var recurrenceRule = new RecurrencePattern
            {
                Frequency = FrequencyType.Yearly,
                Interval = 1,
                ByMonth = new List<int> { 11 },
                ByDay = new List<WeekDay> { new WeekDay { DayOfWeek = DayOfWeek.Thursday, Offset = 4 } },
                Until = DateTime.MaxValue
            };

            var recurringCalEvent = new CalendarEvent
            {
                Summary = "US Thanksgiving",
                Start = new CalDateTime(DateTime.Parse("2000-11-23T07:00")),
                End = new CalDateTime(DateTime.Parse("2000-11-23T19:00")),
                IsAllDay = true,
                RecurrenceRules = new List<RecurrencePattern>() { recurrenceRule }
            };

            calendar.Events.Add(recurringCalEvent);

            var searchStart = DateTime.Parse("2000-01-01");

            var nextOccurrences = calendar.GetOccurrences(searchStart, DateTime.Now);

            return nextOccurrences.First();
        }
    }
}
