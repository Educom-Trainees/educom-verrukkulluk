using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk;
using Verrukkulluk.Data;
using Verrukkulluk.Models.DTOModels;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ICrud _crud;
        private IMapper _mapper;

        public EventsController(ICrud crud, IMapper mapper)
        {
            _crud = crud;
            _mapper = mapper;
        }

        // GET: api/Events
        [HttpGet]
        public IEnumerable<EventDTO> GetEvents()
        {
            IEnumerable<Event> events = _crud.ReadAllEvents();
            IEnumerable<EventDTO> eventDTOs = _mapper.Map<IEnumerable<EventDTO>>(events);

            if (eventDTOs == null)
            {
                return Enumerable.Empty<EventDTO>();
            }

            return eventDTOs;
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EventDTO> GetEvent(int id)
        {
            EventDTO eventDTO = _mapper.Map<EventDTO>(_crud.ReadEventById(id));

            if (eventDTO == null)
            {
                return NotFound();
            }

            return eventDTO;
        }


        //// PUT: api/Events/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEvent(int id, Event @event)
        //{
        //    if (id != @event.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(@event).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EventExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //[Consumes("application/json")]
        //[Produces("application/json")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public ActionResult<EventDTO> PostEvent(EventDTO eventDTO)
        //{
        //    Event event = _mapper.Map<Event>(eventDTO);

        //    Event createdEvent = _crud.CreateEvent(event);

        //    if (createdEvent != null)
        //    {
        //        // Map the created event back to a DTO
        //        EventDTO createdEventDTO = _mapper.Map<EventDTO>(createdEvent);
        //        // Return the DTO of the created event with the appropriate status code
        //        return CreatedAtAction("GetEvent", new { id = createdEvent.Id }, createdEventDTO);
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to create event.");
        //    }
        //}

        //// DELETE: api/Events/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEvent(int id)
        //{
        //    var @event = await _context.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Events.Remove(@event);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EventExists(int id)
        //{
        //    return _context.Events.Any(e => e.Id == id);
        //}
    }
}
