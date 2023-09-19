using Ical.Net;
using Ical.Net.DataTypes;

namespace iCalPlayground
{
    public class CustomRecurringEvent : BaseRecurringEvent
    {
        public Calendar CustomDayRecurrence(int interval, DateTime eventStartTime, DateTime eventEndTime)
        {
            var recurrenceRule = new RecurrencePattern
            {
                Frequency = FrequencyType.Daily,
                Interval = interval,
                Until = DateTime.MaxValue
            };

            var recurringCalEvent = CreateGenericRecurringEvent(eventStartTime,
                eventEndTime,
                recurrenceRule);

            ICalCalendar.Events.Add(recurringCalEvent);

            return ICalCalendar;
        }

        public Calendar CustomMonthRecurrence(int ordinalDayOfMonth, DateTime eventStartTime, DateTime eventEndTime, int interval = 1)
        {
            var recurrenceRule = new RecurrencePattern
            {
                Frequency = FrequencyType.Monthly,
                Interval = interval,
                ByMonthDay = new List<int> { ordinalDayOfMonth },
                Until = DateTime.MaxValue
            };

            var recurringCalEvent = CreateGenericRecurringEvent(eventStartTime,
                eventEndTime,
                recurrenceRule);

            ICalCalendar.Events.Add(recurringCalEvent);

            return ICalCalendar;
        }
    }
}
