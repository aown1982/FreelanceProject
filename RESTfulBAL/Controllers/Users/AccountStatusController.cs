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
using DAL.Users;

namespace RESTfulBAL.Controllers.Users
{
    public class AccountStatusController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/AccountStatus
        [Route("api/WebApp/GetAccountStatus")]
        public IQueryable<tAccountStatu> GettAccountStatus()
        {
            return db.tAccountStatus;
        }

        // GET: api/AccountStatus/5
        [Route("api/WebApp/GetAccountStatus/{id}")]
        [ResponseType(typeof(tAccountStatu))]
        public async Task<IHttpActionResult> GettAccountStatu(int id)
        {
            tAccountStatu tAccountStatu = await db.tAccountStatus.FindAsync(id);
            if (tAccountStatu == null)
            {
                return NotFound();
            }

            return Ok(tAccountStatu);
        }

        //// PUT: api/AccountStatus/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttAccountStatu(int id, tAccountStatu tAccountStatu)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tAccountStatu.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tAccountStatu).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tAccountStatuExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/AccountStatus
        //[ResponseType(typeof(tAccountStatu))]
        //public async Task<IHttpActionResult> PosttAccountStatu(tAccountStatu tAccountStatu)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tAccountStatus.Add(tAccountStatu);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tAccountStatu.Id }, tAccountStatu);
        //}

        //// DELETE: api/AccountStatus/5
        //[ResponseType(typeof(tAccountStatu))]
        //public async Task<IHttpActionResult> DeletetAccountStatu(int id)
        //{
        //    tAccountStatu tAccountStatu = await db.tAccountStatus.FindAsync(id);
        //    if (tAccountStatu == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tAccountStatus.Remove(tAccountStatu);
        //    await db.SaveChangesAsync();

        //    return Ok(tAccountStatu);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tAccountStatuExists(int id)
        {
            return db.tAccountStatus.Count(e => e.Id == id) > 0;
        }
    }
}