namespace Events
{
    public interface IEventRepository
    {
        public Task<Event[]> GetAllEvents();
        public Task<Event> GetOneEvent();
        public void Create();
    }
}
