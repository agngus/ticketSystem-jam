﻿using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ClassLibrary;

namespace TicketSystem.DatabaseRepository
{
    public class TicketDatabase : ITicketDatabase
    {
        string connectionString = "Server=(local)\\SqlExpress; Database=TicketSystem; Trusted_connection=true";

        public IEnumerable<string> GetAllEvents()
        {           
            using (var connection = new SqlConnection(connectionString))
            {
                string queryString = "SELECT EventName FROM TicketEvents";
                connection.Open();
                return connection.Query<string>(queryString).ToList();
            }
        }
        
        public List<TicketEvent> GetEvents(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string queryString = "SELECT * FROM TicketEvents WHERE EventName like '%" + query + "%' OR EventHtmlDescription like '%" + query + "%'";
                connection.Open();
                return connection.Query<TicketEvent>(queryString).ToList();
            }
        }

        public TicketEvent EventAdd(string name, string description)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string queryString = "insert into TicketEvents(EventName, EventHtmlDescription) values(@Name, @Description)";
                connection.Open();
                connection.Query(queryString, new { Name = name, Description = description });
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<TicketEvent>("SELECT * FROM TicketEvents WHERE TicketEventID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public Venue VenueAdd(string name, string address, string city, string country)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string queryString = "insert into Venues([VenueName],[Address],[City],[Country]) values(@Name,@Address, @City, @Country)";
                connection.Open();
                connection.Query(queryString, new { Name = name, Address= address, City = city, Country = country });
                var addedVenueQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Venues') AS Current_Identity").First();
                return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueID=@Id", new { Id = addedVenueQuery }).First();
            }
        }
        public TicketEventDate EventDateAdd(int eventId, int dateId, System.DateTime date)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string queryString = "insert into TicketEventDates([TicketEventID],[VenueId],[EventStartDateTime]) values(@TicketEventID,@VenueId, @EventStartDateTime)";
                connection.Open();
                connection.Query(queryString, new { TicketEventID = eventId, VenueId = dateId, EventStartDateTime = date});
                var addedTicketEventDateQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEventDates') AS Current_Identity").First();
                return connection.Query<TicketEventDate>("SELECT * FROM TicketEventDates WHERE TicketEventDateID=@Id", new { Id = addedTicketEventDateQuery }).First();
            }
        }

        public Venue VenuesFind(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string queryString = "SELECT * FROM Venues WHERE VenueName like '%" + query + "%'";
                   connection.Open();
                return connection.Query<Venue>(queryString).First();
            }
        }

        public List<Venue> VenuesFindAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string queryString = "SELECT * FROM Venues";
                   connection.Open();
                return connection.Query<Venue>(queryString).ToList();
            }
        }
    }
}
