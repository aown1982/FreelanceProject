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
    public class OrderTypesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/OrderTypes
        [Route("api/UserData/GetOrderTypes")]
        public IQueryable<tOrderType> GettOrderTypes()
        {
            return db.tOrderTypes;
        }

        // GET: api/OrderTypes/5
        [Route("api/UserData/GetOrderTypes/{id}")]
        [ResponseType(typeof(tOrderType))]
        public async Task<IHttpActionResult> GettOrderType(int id)
        {
            tOrderType tOrderType = await db.tOrderTypes.FindAsync(id);
            if (tOrderType == null)
            {
                return NotFound();
            }

            return Ok(tOrderType);
        }

        // PUT: api/OrderTypes/5
        [Route("api/UserData/UpdateOrderTypes/{id}/OrderType")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttOrderType(int id, tOrderType OrderType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != OrderType.ID)
            {
                return BadRequest();
            }

            db.Entry(OrderType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tOrderTypeExists(id))
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

        // POST: api/OrderTypes
        [Route("api/UserData/PostOrderTypes/OrderType")]
        [ResponseType(typeof(tOrderType))]
        public async Task<IHttpActionResult> PosttOrderType(tOrderType OrderType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tOrderTypes.Add(OrderType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = OrderType.ID }, OrderType);
        }

        // DELETE: api/OrderTypes/5
        [Route("api/UserData/DeleteOrderTypes/{id}")]
        [ResponseType(typeof(tOrderType))]
        public async Task<IHttpActionResult> DeletetOrderType(int id)
        {
            tOrderType tOrderType = await db.tOrderTypes.FindAsync(id);
            if (tOrderType == null)
            {
                return NotFound();
            }

            db.tOrderTypes.Remove(tOrderType);
            await db.SaveChangesAsync();

            return Ok(tOrderType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tOrderTypeExists(int id)
        {
            return db.tOrderTypes.Count(e => e.ID == id) > 0;
        }
    }
}