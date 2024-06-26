﻿using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Formats.Asn1;
using Verrukkulluk.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Verrukkulluk.Data;
using Verrukkulluk.ViewModels;
using Verrukkulluk.Models.DbModels;
using Microsoft.Extensions.Logging;



namespace Verrukkulluk.Controllers
{

    public class EventController : Controller
    {

        private IVerModel VerModel;
        private IHomeModel HomeModel;
        private IEventModel EventModel;
        private IServicer Servicer;
        private ISessionManager SessionManager;

        public EventController(IVerModel verModel, IHomeModel homeModel, IEventModel eventModel, IServicer servicer, ISessionManager sessionManager)
        {
            VerModel = verModel;
            HomeModel = homeModel;
            EventModel = eventModel;
            Servicer = servicer;
            SessionManager = sessionManager;
        }

        public IActionResult Event(int id)
        {
            EventModel.Event = Servicer.GetEventById(id);
            return View(EventModel);
        }

        public async Task<IActionResult> JoinEvent(int id)
        {

            User loggedInUser = await Servicer.GetCurrentUser();

            EventModel.Event = Servicer.GetEventById(id);

            ViewBag.User = loggedInUser;

            return View("EventParticipation", EventModel);
        }




        [HttpPost]
        public IActionResult EventSignUp(string name, string email, int EventId)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                if (Servicer.AddParticipantToEvent(name, email, EventId)) {
                    EventModel.Event = Servicer.GetEventById(EventId);
                    return View("ThankYou", EventModel);
                }
                return View("Error", "Event is full");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please provide valid name and email.");
                return View("EventParticipation");
            }
        }



    }
}
