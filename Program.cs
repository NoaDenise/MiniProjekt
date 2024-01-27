using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjekt.Data;
using MiniProjekt.Models;
using System.Net;

namespace MiniProjekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            //view all persons in system
            app.MapGet("/viewPersons", (ApplicationContext context) =>
            {
                var persons = context.Persons
                    .Select(person => new
                    {
                        Id = person.Id,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        PhoneNumber = person.PhoneNumber
                    }).ToList();

                return Results.Json(persons);
            });

            //view all interests connected to a person
            app.MapGet("/viewInterest/{personId}", (ApplicationContext context, int PersonId) =>
            {
                var query = from person in context.Persons
                            join interestLink in context.InterestLinks on person.Id equals interestLink.PersonId
                            join interest in context.Interests on interestLink.InterestId equals interest.Id
                            where person.Id == PersonId
                            select new
                            {
                                PersonId = person.Id,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                InterestId = interest.Id,
                                InterestTitle = interest.Title,
                                InterestDescription = interest.Description
                            };
                var result = query.ToList();
                return new JsonResult(result);
            });
 
            //view all url:s connected to a person
            app.MapGet("/viewUrl/{personId}", (ApplicationContext context, int PersonId) =>
            {
                var links = context.InterestLinks
                    .Where(link => link.PersonId == PersonId)
                    .Select(link => new
                    {
                        PersonId = link.PersonId,
                        InterestId = link.InterestId,
                        Url = link.Url
                    })
                    .ToList();
                return Results.Json(links);
            });

            //connect a person to a new interest
            app.MapPost("/connectPersonToInterest/{personId}/{interestId}", (ApplicationContext context, int personId, int interestId) =>
            {
                var person = context.Persons.FirstOrDefault(p => p.Id == personId);
                var interest = context.Interests.FirstOrDefault(i => i.Id == interestId);
                if (person != null && interest != null)
                {
                    if (person.Interests == null)
                    {
                        person.Interests = new List<Interest>();
                    }
                    person.Interests.Add(interest);
                    context.SaveChanges();
                    return Results.StatusCode((int)HttpStatusCode.Created);
                }
                else
                {
                    return Results.BadRequest("Person or interest not found");
                }
            });

            //add new url for person and interest
            app.MapPost("/addLink", async (ApplicationContext context, InterestLink interestlink) =>
            {
                var personExists = await context.Persons.AnyAsync(p => p.Id == interestlink.PersonId);
                var interestExists = await context.Interests.AnyAsync(i => i.Id == interestlink.InterestId);

                if (!personExists || !interestExists)
                {
                    return Results.BadRequest("Person or interest not found");
                }

                if (string.IsNullOrEmpty(interestlink.Url))
                {
                    return Results.BadRequest("URL can not be null or empty");
                }

                context.InterestLinks.Add(interestlink);
                await context.SaveChangesAsync();

                var response = new
                {
                    id = interestlink.Id,
                    personId = interestlink.PersonId,
                    interestId = interestlink.InterestId,
                    url = interestlink.Url
                };

                return Results.Created($"/addLink/{interestlink.Id}", response);
            });

            app.Run();
        }
    }
}
