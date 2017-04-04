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
using DAL.WebApplication;

namespace RESTfulBAL.Controllers.WebApp
{
    public class InvitationCodesController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/InvitationCodes
        [Route("api/WebApp/GetInvitationCodes")]
        public IQueryable<tInvitationCode> GettInvitationCodes()
        {
            return db.tInvitationCodes;
        }

        // GET: api/InvitationCodes/5
        [Route("api/WebApp/GetInvitationCodes/{id}")]
        [ResponseType(typeof(tInvitationCode))]
        public async Task<IHttpActionResult> GettInvitationCode(int id)
        {
            tInvitationCode tInvitationCode = await db.tInvitationCodes.FindAsync(id);
            if (tInvitationCode == null)
            {
                return NotFound();
            }

            return Ok(tInvitationCode);
        }
        [HttpGet]
        [Route("api/WebApp/GetInvitationByCode/{code}")]
        [ResponseType(typeof(tInvitationCode))]
        public async Task<IHttpActionResult> GettInvitationCodeByCode(Guid code)
        {
            tInvitationCode tInvitationCode = await db.tInvitationCodes.FirstAsync(x => x.InvitationCode == code);
            if (tInvitationCode == null)
            {
                return NotFound();
            }

            return Ok(tInvitationCode);
        }

        // PUT: api/InvitationCodes/5
        [HttpPut]
        [Route("api/WebApp/EditInvitationCode/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttInvitationCode(int id, tInvitationCode tInvitationCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tInvitationCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tInvitationCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tInvitationCodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        //// POST: api/InvitationCodes
        //[ResponseType(typeof(tInvitationCode))]
        //public async Task<IHttpActionResult> PosttInvitationCode(tInvitationCode tInvitationCode)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tInvitationCodes.Add(tInvitationCode);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tInvitationCode.ID }, tInvitationCode);
        //}

        //// DELETE: api/InvitationCodes/5
        //[ResponseType(typeof(tInvitationCode))]
        //public async Task<IHttpActionResult> DeletetInvitationCode(int id)
        //{
        //    tInvitationCode tInvitationCode = await db.tInvitationCodes.FindAsync(id);
        //    if (tInvitationCode == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tInvitationCodes.Remove(tInvitationCode);
        //    await db.SaveChangesAsync();

        //    return Ok(tInvitationCode);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tInvitationCodeExists(int id)
        {
            return db.tInvitationCodes.Count(e => e.ID == id) > 0;
        }
    }
}