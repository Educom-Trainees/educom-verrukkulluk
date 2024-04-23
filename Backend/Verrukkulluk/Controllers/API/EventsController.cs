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
        private readonly IMapper _mapper;
        private readonly ILogger<EventsController> _logger;

        public EventsController(ICrud crud, IMapper mapper, ILogger<EventsController> logger)
        {
            _crud = crud;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// Get all events and participants
        /// </summary>
        /// <returns>All events</returns>
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

        /// <summary>
        /// Get the event with this id
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>the event including the participants or 404</returns>
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


        /// <summary>
        /// Update an event
        /// </summary>
        /// <param name="id">The id of the event</param>
        /// <param name="event">The event</param>
        /// <remarks>
        ///   * Title must be unique on that day, may have the same title as an event on another day.
        ///   * participants is either absent or contains the current list of participants with id and email for existing participants or name and email for new participants, all emails must be unique
        ///   
        /// Sample request
        ///  
        ///     PUT /Events/2
        ///     {
        ///          "id": 2,
        ///          "title": "Cooking class",
        ///          "description": "The cooking class",
        ///          "date": "2024-05-25",
        ///          "startTime": "09:30:00",
        ///          "endTime": "12:30:00",
        ///          "place": "The store",
        ///          "price": 15.99,
        ///          "maxParticipants": 20,
        ///          "participants": [
        ///               { 
        ///                   id: 3,
        ///                   email: "bla@mail.com"
        ///               },
        ///               { 
        ///                   id: 14,
        ///                   email: "someone@example.com"
        ///               },
        ///          ]
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult PutEvent(int id, EventDTO @event)
        {
            ValidateEvent(@event, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var theEvent = _mapper.Map<Event>(@event);

                _crud.UpdateEvent(theEvent);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException && _crud.ReadEventById(id) == null)
                {
                    return NotFound();
                }
                _logger.LogError(e, "Update event {@event.Title} failed", @event.Title);
                return UnprocessableEntity();
            }
        }
        /// <summary>
        /// Add an participant to an event
        /// </summary>
        /// <param name="id">The event id</param>
        /// <param name="participant">The participant</param>
        /// <remarks>
        /// Sample request
        ///  
        ///     POST /Events/3/Participants
        ///     {
        ///       "name": "Firstname",
        ///       "email": "firstname@example.com"
        ///     }
        /// </remarks>
        [HttpPost("{id}/Participants")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AddParticipant(int id, ParticipantDTO participant)
        {
            if (participant.Name == null || participant.Email == null)
            {
                return BadRequest("Name and Email are mandatory");
            }
            if (_crud.AddParticipantToEvent(participant.Name, participant.Email, id))
            {
                return NoContent();
            }
            return NotFound($"Event with id {id} not found or full");
        }

        /// <summary>
        /// Remove an participant from an event
        /// </summary>
        /// <param name="id">The event id</param>
        /// <param name="email">(body) The participant's email address in quotes</param>
        [HttpDelete("{id}/Participants")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveParticipant(int id, [FromBody]string? email)
        {
            if (email == null)
            {
                return BadRequest("email in the body is mandatory");
            }
            if (_crud.RemoveParticipantFromEvent(email, id))
            {
                return NoContent();
            }
            return NotFound("Event with id {id} not found");
        }

        /// <summary>
        /// Create an event
        /// </summary>
        /// <param name="theEvent">The new event</param>
        /// <remarks>
        ///  * Title must be unique on that day, may have the same title as an event on another day.
        /// 
        /// Sample request
        ///  
        ///     POST /Events
        ///     {
        ///          "title": "Cooking class",
        ///          "description": "The cooking class",
        ///          "date": "2024-05-25",
        ///          "startTime": "09:30:00",
        ///          "endTime": "12:30:00",
        ///          "place": "The store",
        ///          "price": 15.99,
        ///          "maxParticipants": 20
        ///     }      
        /// </remarks>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EventDTO> PostEvent(EventDTO theEvent)
        {
            ValidateEvent(theEvent);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Event @event = _mapper.Map<Event>(theEvent);

                _crud.CreateEvent(@event);

                // Map the created event back to a DTO
                EventDTO createdEventDTO = _mapper.Map<EventDTO>(@event);
                // Return the DTO of the created event with the appropriate status code
                return CreatedAtAction("GetEvent", new { id = @event.Id }, createdEventDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to create event");
                return Problem(statusCode: 500);
            }
        }

        /// <summary>
        /// Remove an (old) event, and it's participants 
        /// </summary>
        /// <param name="id">The id of the event</param>
        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEvent(int id)
        {
            var @event = _crud.ReadEventById(id);
            if (@event == null)
            {
                return NotFound();
            }

            _crud.DeleteEvent(@event);

            return NoContent();
        }

        private void ValidateEvent(EventDTO @event, int id = 0)
        {
            if (@event.Id != id)
            {
                ModelState.AddModelError(nameof(Event.Id), $"Id must be identical to {id}");
            }
            if (_crud.DoesEventTitleAlreadyExistThatDay(@event.Title, @event.Date, @event.Id))
            {
                ModelState.AddModelError(nameof(Event.Title), "There is another event with this title on this day");
            }
            if (@event.StartTime > @event.EndTime)
            {
                ModelState.AddModelError(nameof(Event.EndTime), "End time must be greater than start time");
            }
            if (@event.Participants != null)
            {
                int count = @event.Participants.Count;

                if (count > @event.MaxParticipants)
                {
                    ModelState.AddModelError(nameof(Event.Participants), "Number of participants exceeds max number of participants");
                }
                if (@event.Participants.DistinctBy(p => p.Email).Count() != count)
                {
                    ModelState.AddModelError(nameof(Event.Participants), "All email addresses must be unique");
                }
                for (int i=0; i< count; i++)
                {
                    var participant = @event.Participants[i];
                    if (participant.Email == null)
                    {
                        ModelState.AddModelError(nameof(Event.Participants) + $"[{i}].Email", "email must be set");
                    }
                    if (participant.Id == 0 && participant.Name == null)
                    {
                        ModelState.AddModelError(nameof(Event.Participants) + $"[{i}].Name", "either name must be set for new Participants or id for existing participants");
                    }
                }
            }
        }
    }
}
