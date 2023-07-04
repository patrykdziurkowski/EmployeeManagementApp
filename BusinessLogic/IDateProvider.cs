namespace BusinessLogic
{
    public interface IDateProvider
    {
        public DateOnly GetNow();
    }
}