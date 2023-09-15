using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace iCalPlayground
{
    public abstract class BaseRecurringEvent
    {
        protected Calendar ICalCalendar = new Calendar();
        protected DateTime CurrentDate = DateTime.Now;
        public static CalendarEvent CreateGenericRecurringEvent(DateTime startTime, DateTime endTime, RecurrencePattern recurrenceRule)
        {
            return new CalendarEvent
            {
                Start = new CalDateTime(startTime),
                End = new CalDateTime(endTime),
                RecurrenceRules = new List<RecurrencePattern> { recurrenceRule }
            };
        }
    }
}
