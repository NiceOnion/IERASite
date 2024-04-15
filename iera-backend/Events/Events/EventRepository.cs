namespace Events
{
    public class EventRepository : IEventRepository
    {
        private List<Event> EventList = new List<Event>();

        public EventRepository() 
        { 
            EventList.Add(new Event() { 
                Id = Guid.NewGuid(),
                Name = "DnD",
                Summary = "Bordspellen avond",
                Date = "Morgen"
            });
            EventList.Add(new Event()
            {
                Id = Guid.NewGuid(),
                Name = "Pizza vrijdag",
                Summary = "Elke week pizza!",
                Date = "Elke vrijdag"
            });
            EventList.Add(new Event()
            {
                Id = Guid.NewGuid(),
                Name = "Borrel",
                Summary = "lecker Bierchen",
                Date = "donderdag middag"
            });
        }

        public void Create(Event NewEvent)
        {
            EventList.Add((Event)NewEvent);
        }

        public Task<List<Event>> GetAllEvents()
        {
            return Task.FromResult(EventList);
        }

        public Task<Event> GetOneEvent(string? name)
        {
            throw new NotImplementedException();
        }


    }
}
