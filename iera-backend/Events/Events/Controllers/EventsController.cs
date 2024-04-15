using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventrepository;

        public EventsController(IEventRepository eventrepos)
        {
            _eventrepository = eventrepos;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetAll(){
            return await _eventrepository.GetAllEvents();
        }

        /*[HttpPost]
        [Route("GetOne")]
        public async Task<Event> GetOne(string? name)
        {
            return await _eventrepository.GetOneEvent(name);
        }*/

        [HttpPost]
        [Route("Create")]
        public void Create(Event NewEvent)
        {
            _eventrepository.Create(NewEvent);
        }
    }
}