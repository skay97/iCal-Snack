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

            // Add monthly test cases

            // Add case for dst
        }
    }
}