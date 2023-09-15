namespace iCalPlayground
{
    internal class iCalPlayground
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var nextOccurrenceWorkflow = new NextOccurrenceWorkflow();

            //Console.WriteLine(nextOccurrenceWorkflow.DailyRecurringEvent());
            Console.WriteLine(nextOccurrenceWorkflow.WeeklyRecurringEvent());
        }
    }
}