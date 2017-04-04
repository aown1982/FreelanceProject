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
    public class CredentialsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Credentials
        [Route("api/UserData/GetCredentials")]
        public IQueryable<tCredential> GettCredentials()
        {
            return db.tCredentials;
        }

        // GET: api/Credentials/5
        [Route("api/UserData/GetCredentials/{id}")]
        [ResponseType(typeof(tCredential))]
        public async Task<IHttpActionResult> GettCredential(Int64 id)
        {
            tCredential tCredential = await db.tCredentials.FindAsync(id);
            if (tCredential == null)
            {
                return NotFound();
            }

            return Ok(tCredential);
        }
        // GET: api/Credentials/5
        [Route("api/UserData/GetCredentialByUserId/{id}")]
        [ResponseType(typeof(tCredential))]
        public IHttpActionResult GetCredentialByUserId(int id)
        {
            tCredential tCredential =  db.tCredentials.FirstOrDefault(x=>x.UserID == id);
            if (tCredential == null)
            {
                return NotFound();
            }

            return Ok(tCredential);
        }

        // GET: api/Credentials/5
        [Route("api/UserData/GetCredentialBySourceUserId/{id}")]
        [ResponseType(typeof(tCredential))]
        public IHttpActionResult GetCredentialBySourceUserId(string id)
        {
            tCredential tCredential = db.tCredentials.FirstOrDefault(x => x.SourceUserID == id);
            if (tCredential == null)
            {
                return NotFound();
            }

            return Ok(tCredential);
        }

        // PUT: api/Credentials/5
        [Route("api/UserData/UpdateCredentials/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttCredential(int id, tCredential Credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Credential.ID)
            {
                return BadRequest();
            }

            db.Entry(Credential).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tCredentialExists(id))
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

        // POST: api/Credentials
        [Route("api/UserData/PostCredentials")]
        [ResponseType(typeof(tCredential))]
        public async Task<IHttpActionResult> PosttCredential(tCredential Credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //get & invalidate old credentials
            var oldCredentials = db.tCredentials.Where(x => x.UserID == Credential.UserID &&
                                                                    x.SourceID == Credential.SourceID && 
                                                                    x.SystemStatusID == 1);

            foreach(tCredential oldCred in oldCredentials)
            {
                oldCred.SystemStatusID = 4;

                //update usersourceservices with new credential
                /*var userSourceServices = db.tUserSourceServices.Where(y => y.CredentialID == oldCred.ID &&
                                                                              y.UserID == oldCred.UserID);

                foreach (tUserSourceService existingUserSourceService in userSourceServices)
                {
                    existingUserSourceService.CredentialID = Credential.ID;
                }*/
            }

            //add new credential
            db.tCredentials.Add(Credential);
            
            await db.SaveChangesAsync();

            return Ok(Credential);
            //return CreatedAtRoute("DefaultApi", new { id = Credential.ID }, Credential);
        }

        // DELETE: api/Credentials/5
        [Route("api/UserData/DeleteCredentials/{id}")]
        [ResponseType(typeof(tCredential))]
        public async Task<IHttpActionResult> DeletetCredential(int id)
        {
            tCredential tCredential = await db.tCredentials.FindAsync(id);
            if (tCredential == null)
            {
                return NotFound();
            }

            db.tCredentials.Remove(tCredential);
            await db.SaveChangesAsync();

            return Ok(tCredential);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tCredentialExists(int id)
        {
            return db.tCredentials.Count(e => e.ID == id) > 0;
        }

        // GET: api/Credentials/5
        [Route("api/UserData/GetPublicTokenByUserId/{id}")]
        [ResponseType(typeof(tCredential))]
        public async Task<IHttpActionResult> GetPublicTokenByUserId(int id)
        {
            tCredential tCredential = await db.tCredentials.FirstOrDefaultAsync(x => x.UserID == id && x.SourceID == 5 && x.SystemStatusID == 1);
            if (tCredential == null)
            {
                return NotFound();
            }

            return Ok(tCredential);
        }

    }
}