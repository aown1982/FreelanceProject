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
    public class MedicationRoutesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/MedicationRoutes
        [Route("api/UserData/GetMedicationRoutes")]
        public IQueryable<tMedicationRoute> GettMedicationRoutes()
        {
            return db.tMedicationRoutes;
        }

        // GET: api/MedicationRoutes/5
        [Route("api/UserData/GetMedicationRoutes/{id}")]
        [ResponseType(typeof(tMedicationRoute))]
        public async Task<IHttpActionResult> GettMedicationRoute(int id)
        {
            tMedicationRoute tMedicationRoute = await db.tMedicationRoutes.FindAsync(id);
            if (tMedicationRoute == null)
            {
                return NotFound();
            }

            return Ok(tMedicationRoute);
        }

        // PUT: api/MedicationRoutes/5
        [Route("api/UserData/UpdateMedicationRoutes/{id}/MedicationRoute")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttMedicationRoute(int id, tMedicationRoute MedicationRoute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != MedicationRoute.ID)
            {
                return BadRequest();
            }

            db.Entry(MedicationRoute).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tMedicationRouteExists(id))
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

        // POST: api/MedicationRoutes
        [Route("api/UserData/PostMedicationRoutes/MedicationRoute")]
        [ResponseType(typeof(tMedicationRoute))]
        public async Task<IHttpActionResult> PosttMedicationRoute(tMedicationRoute MedicationRoute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tMedicationRoutes.Add(MedicationRoute);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = MedicationRoute.ID }, MedicationRoute);
        }

        // DELETE: api/MedicationRoutes/5
        [Route("api/UserData/DeleteMedicationRoutes/{id}")]
        [ResponseType(typeof(tMedicationRoute))]
        public async Task<IHttpActionResult> DeletetMedicationRoute(int id)
        {
            tMedicationRoute tMedicationRoute = await db.tMedicationRoutes.FindAsync(id);
            if (tMedicationRoute == null)
            {
                return NotFound();
            }

            db.tMedicationRoutes.Remove(tMedicationRoute);
            await db.SaveChangesAsync();

            return Ok(tMedicationRoute);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tMedicationRouteExists(int id)
        {
            return db.tMedicationRoutes.Count(e => e.ID == id) > 0;
        }
    }
}