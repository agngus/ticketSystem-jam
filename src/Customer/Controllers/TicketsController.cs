﻿using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using TicketSystem.RestApiClient;


namespace Customer.Controllers
{
    public class TicketsController : Controller
    {
        public static Value value;
        public static TicketApi ticketApi;
        public SeatsAtEventDate SeatsAtEventDate;
        public TicketEvent TicketEvent;
        public static int infoNumber;
        public static string infoMessage;


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tickets()
        {
            if (value == null)
            {
                value = new Value();
            }
            if (ticketApi == null)
            {
                ticketApi = new TicketApi();
            }
           
            value.EventSummaries = ticketApi.GetAllSummary();
            ViewBag.Message = "";
            return View(value);
        }

        public IActionResult ShowInfo(int id)
        {
            if(id == infoNumber) {
                infoMessage = "";
                infoNumber = 0;
                return View("Tickets", value);
            }

            infoNumber = id;
            infoMessage = "";

            foreach (EventSummary x in value.EventSummaries)
            {
                if(x.TicketEventDateID == id)
                {
                    infoMessage = x.EventHtmlDescription;
                    ViewBag.InfoMessage = infoMessage;
                }
            }
            return View("Tickets", value);
        }
        public IActionResult Search(string searchText)
        {
            if (searchText != null)
            {
                value.EventSummaries = ticketApi.GetSearchSummary(searchText);
                return View("Tickets", value);
            }
            else
            {
                return View("Tickets", value);
            }
        }

        public IActionResult GetAllEventDates()
        {
            ticketApi.GetAllEventDates();
            return View(value);
        }

    }
}
