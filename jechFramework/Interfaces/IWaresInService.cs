namespace jechFramework.Interfaces
{
    public interface IWaresInService
    {
        void ScheduleWaresIn(WaresIn waresIn);
        void ScheduleRecurringOrder(RecurringOrder recurringOrder);
        void UpdateWaresIn(WaresIn waresIn);
        WaresIn GetWaresIn(int orderId);
        void DeleteWaresIn(int orderId);
        IEnumerable<WaresIn> GetAllScheduledWaresIn();
    }
}
