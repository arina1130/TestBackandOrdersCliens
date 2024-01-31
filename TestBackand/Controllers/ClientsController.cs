using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using TestBackand.Context;
using TestBackand.Repositories;

namespace TestBackand.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        TestContext db = new TestContext();


        [HttpPost("addClient")]
        public async Task<ActionResult<Client>> AddClient(Client client)
        {
            if (client == null || client.Id > 0)
            {
                return BadRequest();
            }

            db.Clients.Add(client);
            await db.SaveChangesAsync();
            return Ok(client);
        }

        [HttpPut("UpdateClient")]
        public async Task<ActionResult<Client>> UpdateClient(Client client)
        {


            if (client == null)
            {
                return BadRequest();
            }
            if (!db.Clients.Any(x => x.Id == client.Id))
            {
                return NotFound();
            }


            db.Clients.Update(client);
            await db.SaveChangesAsync();
            return Ok(client);

        }

        [HttpDelete("deleteClient")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            Client? client = db.Clients.FirstOrDefault(u => u.Id == id);

            if (client == null) return NotFound(new { message = "Пользователь не найден" });

            db.Clients.Remove(client);
            await db.SaveChangesAsync();
            return Ok(client);
        }

        [HttpPost("getClients")]
        public async Task<ActionResult<List<Client>>> GetClients(string? searchString, DateTime? min, DateTime? max, int page = 1, int pageSize = 2)
        {
            List<Client> clients = db.Clients.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.Id.ToString().ToUpper().Contains(searchString.ToUpper())
                                            || s.Name.ToUpper().Contains(searchString.ToUpper())
                                            || s.Surname.ToUpper().Contains(searchString.ToUpper())
                                            || s.Birthday.ToString().ToUpper().Contains(searchString.ToUpper())
                                      ).ToList();

            }
            if (min != null)
            {
                clients = clients.Where(s => s.Birthday >= DateOnly.FromDateTime((DateTime)min)).ToList();
            }
            if (max != null)
            {
                clients = clients.Where(s => s.Birthday <= DateOnly.FromDateTime((DateTime)max)).ToList();
            }
            if (page > 0 && pageSize > 0) { }
            {
                clients = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            if (clients.Count == 0) return NotFound(new { message = "Пользователь не найден" });
            return clients;
        }


        
    }
}
