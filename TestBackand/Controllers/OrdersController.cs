using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using TestBackand.Context;
using TestBackand.Models;
using TestBackand.Repositories;

namespace TestBackand.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        TestContext db = new TestContext();


        [HttpPost("addOrder")]
        public async Task<ActionResult<Order>> AddOrder(Order order)
        {

            if (order == null || order.Id > 0)
            {
                return BadRequest();
            }
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return Ok(order);
        }

        [HttpPut("updateOrder")]
        public async Task<ActionResult<Order>> UpdateOrder(Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            if (!db.Orders.Any(x => x.Id == order.Id))
            {
                return NotFound();
            }


            db.Orders.Update(order);
            await db.SaveChangesAsync();
            return Ok(order);

        }

        [HttpDelete("deleteOrder")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            Order? order = db.Orders.FirstOrDefault(u => u.Id == id);

            if (order == null) return NotFound(new { message = "Заказ не найден" });

            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return Ok(order);
        }

        [HttpPost("GetOrders")]
        public async Task<ActionResult<List<Order>>> GetOrder(string? searchString, DateTime? dateMin, DateTime? dateMax, int? minSum, int? maxSum, int page = 1, int pageSize = 2)
        {
            List<Order> orders = db.Orders.ToList();
            List<Status> statuses = db.Statuses.ToList();
            List<Client> clients = db.Clients.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.Id.ToString().ToUpper().Contains(searchString.ToUpper())
                                            || s.IdClientNavigation.Name.ToUpper().Contains(searchString.ToUpper())
                                            || s.IdStatusNavigation.Type.ToUpper().Contains(searchString.ToUpper())
                                            || s.Sum.ToString().ToUpper().Contains(searchString.ToUpper())
                                            || s.DateTime.ToString().ToUpper().Contains(searchString.ToUpper())
                                      ).ToList();

            }
            if (dateMin != null)
            {
                orders = orders.Where(s => s.DateTime >= (DateTime)dateMin).ToList();
            }
            if (dateMax != null)
            {
                orders = orders.Where(s => s.DateTime <= (DateTime)dateMax).ToList();
            }
            if (minSum != null)
            {
                orders = orders.Where(s => s.Sum >= minSum).ToList();
            }
            if (maxSum != null)
            {
                orders = orders.Where(s => s.Sum <= maxSum).ToList();
            }
            if (page > 0 && pageSize > 0) { }
            {
                orders = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            if (orders.Count == 0) return NotFound(new { message = "Заказы не найдены" });
            return orders;
        }

        [HttpGet("getOrdersSum")]
        public async Task<ActionResult<List<Order>>> GetOrdersSum(int status)
        {

            string b = db.Database.GetConnectionString();
            List<SumOrders> sumList = new List<SumOrders>();
            using (var conn = new Npgsql.NpgsqlConnection(connectionString: b))
            {
                conn.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand($"select *\r\nfrom get_sum({status});", conn);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                {
                    sumList.Add(new SumOrders(rdr.GetInt32(0), rdr.GetString(1), rdr.GetDateTime(2)));
                }
            }
            return Ok(sumList);
        }

        [HttpGet("getOrdersAvg")]
        public async Task<ActionResult<List<Order>>> GetOrdersAvg(int status)
        {

            string b = db.Database.GetConnectionString();
            List<AvgOrders> avgList = new List<AvgOrders>();
            using (var conn = new Npgsql.NpgsqlConnection(connectionString: b))
            {
                conn.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand($"select *\r\nfrom get_avg({status});", conn);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                int i = 0;
                string k = "";
                while (rdr.Read())
                {
                    avgList.Add(new AvgOrders(rdr.GetTimeSpan(0), rdr.GetDouble(1)));
                }
            }
            return Ok(avgList);
        }
    }
}
