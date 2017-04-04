using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.UserData;

namespace RESTfulBAL.Controllers.UserData
{
    public class UserOrdersController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserOrders
        [Route("api/UserData/GetUserOrders")]
        public IQueryable<tUserOrder> GettUserOrders()
        {
            return db.tUserOrders;
        }

        // GET: api/UserOrders/5
        [Route("api/UserData/GetUserOrders/{id}")]
        [ResponseType(typeof(tUserOrder))]
        public async Task<IHttpActionResult> GettUserOrder(int id)
        {
            tUserOrder tUserOrder = await db.tUserOrders.FindAsync(id);
            if (tUserOrder == null)
            {
                return NotFound();
            }

            return Ok(tUserOrder);
        }

        // PUT: api/UserOrders/5
        [Route("api/UserData/UpdateUserOrders/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserOrder(int id, tUserOrder tUserOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserOrder.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserOrder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserOrders
        [Route("api/UserData/PostUserOrders")]
        [ResponseType(typeof(tUserOrder))]
        public async Task<IHttpActionResult> PosttUserOrder(tUserOrder tUserOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserOrders.Add(tUserOrder);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserOrder.ID }, tUserOrder);
        }

        // DELETE: api/UserOrders/5
        [Route("api/UserData/DeleteUserOrders/{id}")]
        [ResponseType(typeof(tUserOrder))]
        public async Task<IHttpActionResult> DeletetUserOrder(int id)
        {
            tUserOrder tUserOrder = await db.tUserOrders.FindAsync(id);
            if (tUserOrder == null)
            {
                return NotFound();
            }

            db.tUserOrders.Remove(tUserOrder);
            await db.SaveChangesAsync();

            return Ok(tUserOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserOrderExists(int id)
        {
            return db.tUserOrders.Count(e => e.ID == id) > 0;
        }
    }
}