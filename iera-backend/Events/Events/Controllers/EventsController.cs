using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [ApiController]
    public class EventsController : ControllerBase
    {
        private Event[] Eventlist = new[]
        {
            new Event("Borrel", "Dit is een borrel", "Morgen"),
            new Event("DnD", "Dit is een bordspel avond", "Gister"),
            new Event("Pizavrijdag", "Tijd voor pizza", "Elke vrijdag")
        };

        private readonly ILogger<EventsController> _logger;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAll")]
        public Event[] GetAll()
        {
            return Eventlist;
        }

        [HttpPost]
        [Route("GetOne")]
        public Event GetOne(string? name)
        {
            foreach (Event Event in Eventlist) 
            { 
                if (Event.Name == name) 
                { 
                return Event;
                }
            }
            return (null);
        }

        [HttpPost]
        [Route("Create")]
        public void Create(Event NewEvent)
        {
            Eventlist.Append(NewEvent);
        }
    }
}