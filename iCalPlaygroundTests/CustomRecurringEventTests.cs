using Ical.Net.DataTypes;
using NodaTime;
using Xunit;

namespace iCalPlayground.Tests
{
    public class CustomRecurringEventTests
    {
        public class CustomDayRecurrence_Should
        {
            [Fact]
            public void Return_Expected_Day_Occurrence_If_Current_Time_Is_Later_Than_Event_End_Time()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                // Act
                var dailyRecurrences = calWithRecurrence
                    // If current time is 11:30am, the eventStartTime is 9:30am,
                    // and the eventEndTime is 10:30am
                    .CustomDayRecurrence(interval: 1, eventStartTime: DateTime.Parse("2023-09-19T08:30"), eventEndTime: DateTime.Parse("2023-09-19T09:30"))
                    // Since the event's end time is prior to the current time, the
                    // total number of occurrences from right now to 5 days from now
                    // should be 5 - we will skip the event that should've taken
                    // place today.
                    .GetOccurrences(DateTime.Parse("2023-09-19T10:30"), DateTime.Parse("2023-09-24T10:30"));

                // Assert
                // If today is the 19th of September, we expect the last occurrence
                // to take place on the 24th. This means, the next occurrence
                // takes place on the 20th, since current time is greater than
                // end time of the event. Similarly, the final occurrence takes
                // place 5 days from now on the 24th
                Assert.Equal(DateTime.Parse("2023-09-24T06:00").Date, dailyRecurrences.Last().Period.StartTime.Date);
               
                Assert.Equal(5, dailyRecurrences.Count);
                
                //Despite us getting occurrences from the 19th, the first occurrence
                //in the list does not include the 19th because the event end time
                //is BEFORE the start time of GetOccurrences method.
                Assert.False(dailyRecurrences.First().Period.StartTime.Date == DateTime.Parse("2023-09-19T10:30").Date);
            }

            [Fact]
            public void Return_Expected_Day_Occurrences_If_Current_Time_Is_Before_Event_End_Time()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                // Act
                var dailyRecurrences = calWithRecurrence
                    .CustomDayRecurrence(interval: 1,
                        eventStartTime: DateTime.Parse("2023-09-19T08:30"),
                        eventEndTime: DateTime.Parse("2023-09-19T09:30"))
                    .GetOccurrences(DateTime.Parse("2023-09-19T06:00"),
                        DateTime.Parse("2023-09-24T06:00"));

                // Assert
                Assert.Equal(5, dailyRecurrences.Count);
               
                // If today is the 19th of September, we expect the last occurrence
                // to take place on the 23rd. This means, the next occurrence
                // takes place today and the final occurrence takes place 4 days from now.
                Assert.Equal(DateTime.Parse("2023-09-23T06:00").Date, dailyRecurrences.Last().Period.StartTime.Date);

                // since the start time of the GetOccurrences method is before the
                // end time of the event, we expect the first occurrence to take
                // place on the 19th.
                Assert.True(dailyRecurrences.First().Period.StartTime.Date == DateTime.Parse("2023-09-19T06:00").Date);
            }

            [Fact]
            public void Return_Expected_Month_Occurrences()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                // Act
                var monthlyRecurrence = calWithRecurrence
                    .CustomMonthRecurrence(ordinalDayOfMonth: 10,
                        eventStartTime: DateTime.Parse("2023-09-19T08:30"),
                        eventEndTime: DateTime.Parse("2023-09-19T09:30"))
                    .GetOccurrences(DateTime.Parse("2023-09-19T06:00"),
                        DateTime.Parse("2023-12-19T06:00"));

                // Assert
                // Returns events from September to December.
                Assert.Equal(4, monthlyRecurrence.Count);
                Assert.True(monthlyRecurrence.First().Period.StartTime.Month == 9);
            }

            [Fact]
            public void Return_Expected_Month_Occurrences_For_Day_That_Does_Not_Fall_In_Every_Month()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                // Act
                var monthlyRecurrence = calWithRecurrence
                    .CustomMonthRecurrence(ordinalDayOfMonth: 31,
                        eventStartTime: DateTime.Parse("2023-08-31T08:30"),
                        eventEndTime: DateTime.Parse("2023-08-31T09:30"))
                    .GetOccurrences(DateTime.Parse("2023-09-19T06:00"),
                        DateTime.Parse("2024-01-01T06:00"));

                // Assert
                // Returns events from September 19th to January 1st. Since October
                // and December are only only months in the timeline that have
                // 31 days, only 2 events will be returned.
                Assert.Equal(2, monthlyRecurrence.Count);

                // verifies first occurrence takes place in October.
                Assert.True(monthlyRecurrence.First().Period.StartTime.Month == 10);
            }

            [Fact]
            public void Return_Expected_Month_Occurrences_For_Interval_Based_Event()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                // Act
                var monthlyRecurrence = calWithRecurrence
                    .CustomMonthRecurrence(ordinalDayOfMonth: 10,
                        eventStartTime: DateTime.Parse("2023-09-19T08:30"),
                        eventEndTime: DateTime.Parse("2023-09-19T09:30"),
                        interval: 2)
                    .GetOccurrences(DateTime.Parse("2023-09-19T06:00"),
                        DateTime.Parse("2024-06-19T06:00"));

                // Assert
                // Returns events from September '23 to June '24.
                Assert.Equal(5, monthlyRecurrence.Count);
                Assert.True(monthlyRecurrence.First().Period.StartTime.Month == 9);

                // Ensures the months are what we expect: September, Nov, Jan,
                // March, May
                Assert.Collection(monthlyRecurrence, (occurrence) => Assert.Equal(9, occurrence.Period.StartTime.Month),
                    (occurrence) => Assert.Equal(11, occurrence.Period.StartTime.Month),
                    (occurrence) => Assert.Equal(1, occurrence.Period.StartTime.Month),
                    (occurrence) => Assert.Equal(3, occurrence  .Period.StartTime.Month),
                    (occurrence) => Assert.Equal(5, occurrence.Period.StartTime.Month));
                Assert.True(monthlyRecurrence.Last().Period.StartTime.Month == 5);
            }

            [Fact]
            public void Return_Expected_Weekly_Occurrences()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                var daysOfWeekList = new List<WeekDay> 
                {
                    new WeekDay(DayOfWeek.Monday),
                    new WeekDay(DayOfWeek.Tuesday), 
                    new WeekDay(DayOfWeek.Wednesday)
                }; 

                // Act
                var weeklyRecurrence = calWithRecurrence
                    .CustomWeekRecurrence(daysOfWeekList: daysOfWeekList,
                        eventStartTime: DateTime.Parse("2023-09-18T08:30"),
                        eventEndTime: DateTime.Parse("2023-09-18T09:30"))
                    .GetOccurrences(DateTime.Parse("2023-09-18T06:00"),
                        DateTime.Parse("2023-09-30T06:00"));

                // Assert
                Assert.Equal(6, weeklyRecurrence.Count);
                Assert.Collection(weeklyRecurrence, 
                    (occurrence) => Assert.Equal(18, occurrence.Period.StartTime.Day), // Monday
                    (occurrence) => Assert.Equal(19, occurrence.Period.StartTime.Day), // Tuesday
                    (occurrence) => Assert.Equal(20, occurrence.Period.StartTime.Day), // Wednesday
                    (occurrence) => Assert.Equal(25, occurrence.Period.StartTime.Day), // Monday
                    (occurrence) => Assert.Equal(26, occurrence.Period.StartTime.Day), // Tuesday
                    (occurrence) => Assert.Equal(27, occurrence.Period.StartTime.Day)); // Wednesday
            }

            [Fact]
            public void Return_Expected_Weekly_Occurrences_Without_Date_that_does_not_fall_in_timeline()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                var daysOfWeekList = new List<WeekDay>
                {
                    new WeekDay(DayOfWeek.Monday),
                    new WeekDay(DayOfWeek.Tuesday),
                    new WeekDay(DayOfWeek.Wednesday)
                };

                // Act
                var weeklyRecurrence = calWithRecurrence
                    .CustomWeekRecurrence(daysOfWeekList: daysOfWeekList,
                        eventStartTime: DateTime.Parse("2023-09-19T08:30"),
                        eventEndTime: DateTime.Parse("2023-09-19T09:30"))
                    .GetOccurrences(DateTime.Parse("2023-09-18T06:00"),
                        DateTime.Parse("2023-09-30T06:00"));

                // Assert
                Assert.Equal(5, weeklyRecurrence.Count);

                // Despite the 18th being a Monday, it is not part of the schedule
                // because the event starts from the 19th.
                Assert.DoesNotContain(weeklyRecurrence, (x) => x.Period.StartTime.Day == 18);

                // Since the schedule started on the 19th, it includes the event
                // that takes place on Monday the 25th.
                Assert.Contains(weeklyRecurrence, (x) => x.Period.StartTime.Day == 25);
            }

            [Fact]
            public void Return_Expected_Day_Occurrences_For_DST_Spring()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                // Act
                var dailyRecurrencesCalendar = calWithRecurrence
                    .CustomDayRecurrence(interval: 1,
                        eventStartTime: DateTime.Parse("2023-03-10T02:30"),
                        eventEndTime: DateTime.Parse("2023-03-10T03:30"));

                foreach (var e in dailyRecurrencesCalendar.Events)
                {
                    e.Start = e.Start.ToTimeZone("Central Standard Time");
                    e.End = e.End.ToTimeZone("Central Standard Time");
                }

                var dailyRecurrences = dailyRecurrencesCalendar
                    .GetOccurrences(DateTime.Parse("2023-03-10T01:00"),
                        DateTime.Parse("2023-03-15T01:00"));

                var dailyRecurrencesInLocalAndUtcTime = dailyRecurrences
                    .Select(o =>
                        new
                        {
                            Local = o.Period.StartTime.AsUtc.ToLocalTime(),
                            Utc = o.Period.StartTime.AsUtc
                        })
                    .OrderBy(o => o.Local)
                    .ToList();

                // Assert
                Assert.Equal(5, dailyRecurrencesInLocalAndUtcTime.Count);

                // Ensure UTC hour is an hour BEHIND after DST goes into effect
                // and we DO NOT SKIP an instance.
                Assert.Collection(dailyRecurrencesInLocalAndUtcTime,
                    (occurrences) =>
                    {
                        Assert.Equal(2, occurrences.Local.Hour);
                        Assert.Equal(8, occurrences.Utc.Hour);
                        Assert.Equal(10, occurrences.Utc.Day);
                    },
                    (occurrences) =>
                    {
                        Assert.Equal(2, occurrences.Local.Hour);
                        Assert.Equal(8, occurrences.Utc.Hour);
                        Assert.Equal(11, occurrences.Utc.Day);

                    },
                    (occurrences) =>
                    {
                        // Since the 2 AM hour does not exist when DST goes into
                        // effect the occurrence takes place at the 3 AM hour.
                        Assert.Equal(3, occurrences.Local.Hour);

                        Assert.Equal(8, occurrences.Utc.Hour);

                        // DST begins on the 12th in 2023. Since we can assert
                        // that the occurrence includes the 12, we know the
                        // instance was not skipped.
                        Assert.Equal(12, occurrences.Utc.Day);
                    },
                    (occurrences) =>
                    {
                        Assert.Equal(2, occurrences.Local.Hour);
                        Assert.Equal(7, occurrences.Utc.Hour);
                        Assert.Equal(13, occurrences.Utc.Day);

                    },
                    (occurrences) =>
                    {
                        Assert.Equal(2, occurrences.Local.Hour);
                        Assert.Equal(7, occurrences.Utc.Hour);
                        Assert.Equal(14, occurrences.Utc.Day);

                    });
            }

            [Fact]
            public void Return_Expected_Day_Occurrences_For_DST_Fall()
            {
                // Arrange
                var calWithRecurrence = new CustomRecurringEvent();

                // Act
                var dailyRecurrencesCalendar = calWithRecurrence
                    .CustomDayRecurrence(interval: 1,
                        eventStartTime: DateTime.Parse("2023-11-03T01:00"),
                        eventEndTime: DateTime.Parse("2023-11-03T02:00"));

                foreach (var e in dailyRecurrencesCalendar.Events)
                {
                    e.Start = e.Start.ToTimeZone("Central Standard Time");
                    e.End = e.End.ToTimeZone("Central Standard Time");
                }

                var dailyRecurrences = dailyRecurrencesCalendar
                    .GetOccurrences(DateTime.Parse("2023-11-03T12:00"),
                        DateTime.Parse("2023-11-08T12:00"));

                var dailyRecurrencesInLocalAndUtcTime = dailyRecurrences
                    .Select(o => 
                        new 
                        { 
                            Local = o.Period.StartTime.AsUtc.ToLocalTime(),
                            Utc = o.Period.StartTime.AsUtc 
                        })
                    .OrderBy(o => o.Local)
                    .ToList();

                // Assert
                Assert.Equal(5, dailyRecurrencesInLocalAndUtcTime.Count);

                // Ensure UTC hour is an hour AHEAD when DST ENDS
                // and we DO NOT REPEAT instances.
                Assert.Collection(dailyRecurrencesInLocalAndUtcTime,
                    (occurrences) =>
                    {
                        Assert.Equal(6, occurrences.Utc.Hour);
                        Assert.Equal(4, occurrences.Utc.Day);
                    },
                    (occurrences) =>
                    {
                        Assert.Equal(6, occurrences.Utc.Hour);
                        Assert.Equal(5, occurrences.Utc.Day);
                    },
                    (occurrences) =>
                    {
                        Assert.Equal(7, occurrences.Utc.Hour);
                        // since day is not the 5th again, we can successfully
                        // determine that the schedule instance did not repeat.
                        Assert.Equal(6, occurrences.Utc.Day);
                    },
                    (occurrences) =>
                    {
                        Assert.Equal(7, occurrences.Utc.Hour);
                        Assert.Equal(7, occurrences.Utc.Day);
                    },
                    (occurrences) =>
                    {
                        Assert.Equal(7, occurrences.Utc.Hour);
                        Assert.Equal(8, occurrences.Utc.Day);
                    });
            }
        }
    }
}