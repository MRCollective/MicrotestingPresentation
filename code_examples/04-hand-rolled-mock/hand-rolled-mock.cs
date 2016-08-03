// Interface
public interface IDateTimeProvider
{
    DateTimeOffset Now();
}
 
// Production code
public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now()
    {
        return DateTimeOffset.UtcNow;
    }
}













// Basic hand-rolled mock
public class StaticDateTimeProvider : IDateTimeProvider
{
    public StaticDateTimeProvider()
    {
        _now = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset Now()
    {
        return _now;
    }

    private DateTimeOffset _now;
}
 








// Behaviourally codified hand-rolled mock
public class StaticDateTimeProvider : IDateTimeProvider
{
    private DateTimeOffset _now;
 
    public StaticDateTimeProvider()
    {
        _now = DateTimeOffset.UtcNow;
    }
 
    public StaticDateTimeProvider(DateTimeOffset now)
    {
        _now = now;
    }
 
    // This one is good for data-driven tests that take a string representation of the date
    public StaticDateTimeProvider(string now)
    {
        _now = DateTimeOffset.Parse(now);
    }
 
    public DateTimeOffset Now()
    {
        return _now;
    }
 
    public StaticDateTimeProvider SetNow(string now)
    {
        _now = DateTimeOffset.Parse(now);
        return this;
    }
 
    public StaticDateTimeProvider MoveTimeForward(TimeSpan amount)
    {
        _now = _now.Add(amount);
        return this;
    }
}


// e.g.
//
// Provide(new StaticDateTimeProvider(Resolve<IDateTimeProvider>().Now().Add(TimeSpan.FromHours(1))));
//
// vs
//
// Resolve<StaticDateTimeProvider>().MoveTimeForward(TimeSpan.FromHours(1));









// What about this?
//
// using (Resolve<StaticDateTimeProvider>().TemporarilyChangeBy(TimeSpan.FromHours(4)))
// {
//      // Do stuff...
// }
//
// Now is back what is was again...
