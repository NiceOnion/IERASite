namespace Events
{
    public interface IEventRepository
    {
        public Task<List<Event>> GetAllEvents();
        public Task<Event> GetOneEvent(string? name);
        public void Create(Event NewEvent);
    }
}
