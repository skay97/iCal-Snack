﻿using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Ical.Net;
using Calendar = Ical.Net.Calendar;

namespace iCalPlayground
{
    public class NextOccurrenceWorkflow
    {
        private Calendar ICalCalendar = new Calendar();
        private DateTime CurrentDate = DateTime.Now;
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

        public Occurrence RecurringEventThanksgiving()
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

            ICalCalendar.Events.Add(recurringCalEvent);

            var searchStart = DateTime.Parse("2000-01-01");

            var nextOccurrences = ICalCalendar.GetOccurrences(searchStart, CurrentDate);

            return nextOccurrences.First();
        }

        public Occurrence DailyRecurringEvent()
        {
            var calendar = new Calendar();

            var recurrenceRule = new RecurrencePattern
            {
                Frequency = FrequencyType.Daily,
                Interval = 1,
                Until = DateTime.MaxValue
            };

            var recurringCalEvent = new CalendarEvent
            {
                Start = new CalDateTime(DateTime.Parse("2023-09-13T07:00")),
                End = new CalDateTime(DateTime.Parse("2023-09-13T07:00").AddHours(1)),
                RecurrenceRules = new List<RecurrencePattern>() { recurrenceRule }
            };

            ICalCalendar.Events.Add(recurringCalEvent);

            var nextOccurrences = ICalCalendar.GetOccurrences(CurrentDate, CurrentDate.AddDays(5));

            // TODO: Add tests to verify you do not get the event for today if
            // the event end time is before current time.

            // Returns tomorrow if current time is after recurringCalEvent.End. Otherwise, returns today.
            return nextOccurrences.First();
        }
    }
}
