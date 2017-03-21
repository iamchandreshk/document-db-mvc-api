using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace document_db.Controllers
{
    public class RegisterController : ApiController
    {
        private Helpers.ConnectionHelpers connection = new Helpers.ConnectionHelpers();

        [HttpPost]
        [Route("create_document")]
        public async Task<IHttpActionResult> create_document(Models.UserModel user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.ModifiedAt = DateTime.UtcNow;

            var result = await connection.CreateDocument(user);
            return Ok();
        }

        [HttpGet]
        [Route("read_document")]
        public IHttpActionResult read_document()
        {
            var result = connection.ReadDocument();
            return Ok(result);
        }

        [HttpGet]
        [Route("read_document")]
        public IHttpActionResult read_document(string id)
        {
            var result = connection.ReadDocumentWithWhere(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("update_document")]
        public async Task<IHttpActionResult> update_document(Models.UserUpdateModel request)
        {
            await connection.UpdateDocument(request);
            return Ok();
        }

        [HttpGet]
        [Route("delete_document")]
        public async Task<IHttpActionResult> delete_document(Guid id)
        {
            await connection.DeleteDocument(id);
            return Ok();
        }
    }
}
