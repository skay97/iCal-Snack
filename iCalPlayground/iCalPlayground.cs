namespace iCalPlayground
{
    internal class iCalPlayground
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var nonCustomRecurringEvent = new NonCustomRecurringEvent();

            //Console.WriteLine(nextOccurrenceWorkflow.DailyRecurringEvent());
            //Console.WriteLine(nextOccurrenceWorkflow.WeeklyRecurringEvent());
            Console.WriteLine(nonCustomRecurringEvent.MonthlyRecurringEvent());
        }
    }
}