using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.Documents;
using System.Configuration;
using System.Collections.Generic;

namespace document_db.Helpers
{
    public class ConnectionHelpers
    {
        private static readonly string databaseId = "SampleDatabase";
        private static readonly string collectionId = "SampleCollection";
        private static readonly string endpointUrl = "https://localhost:8081";
        private static readonly string authorizationKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly ConnectionPolicy connectionPolicy = new ConnectionPolicy { UserAgentSuffix = " samples-net/2" };
        private DocumentClient _client;
        DocumentCollection collection;
        Database database;


        public ConnectionHelpers()
        {
            //Initialize Document Client.
            _client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

            //Initialize Create of Select Database.
            database = _client.CreateDatabaseQuery().Where(db => db.Id == databaseId).AsEnumerable().FirstOrDefault();
            if (database == null)
            {
                database = _client.CreateDatabaseAsync(new Database { Id = databaseId }).Result;
            }

            //Initialize Create of Select Collection.
            collection = _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId)).Result;
            if (collection == null)
            {
                collection = _client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = collectionId }).Result;
            }
        }

        public async Task<Document> CreateDocument(Models.UserModel documentObject)
        {
            var result = await _client.CreateDocumentAsync(collection.SelfLink, documentObject);
            var document = result.Resource;
            return result;
        }

        public List<Models.UserUpdateModel> ReadDocument()
        {
            return _client.CreateDocumentQuery<Models.UserUpdateModel>(collection.DocumentsLink).AsEnumerable().ToList();
        }

        public Models.UserUpdateModel ReadDocumentWithWhere(string id)
        {
            var usermodel = _client.CreateDocumentQuery<Models.UserUpdateModel>(collection.SelfLink).Where(so => so.Firstname == id.ToString()).AsEnumerable().SingleOrDefault();
            return usermodel;
        }

        public async Task UpdateDocument(Models.UserUpdateModel request)
        {
            Document doc = _client.CreateDocumentQuery<Document>(collection.SelfLink).Where(r => r.Id == request.Id.ToString()).AsEnumerable().SingleOrDefault();
            doc.SetPropertyValue("ModifiedAt", request.ModifiedAt);
            doc.SetPropertyValue("Email", request.Email);
            doc.SetPropertyValue("IsActive", request.IsActive);
            Document updated = await _client.ReplaceDocumentAsync(doc);
        }

        public async Task<bool> DeleteDocument(Guid id)
        {
            var usermodel = _client.CreateDocumentQuery<Document>(collection.SelfLink).Where(so => so.Id == id.ToString()).AsEnumerable().SingleOrDefault();
            await _client.DeleteDocumentAsync(usermodel.SelfLink);
            return true;
        }
    }
}