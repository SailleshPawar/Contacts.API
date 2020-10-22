using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Contacts_.API
{
    public class Repository<T> where T : class
    {
        private readonly string DatabaseId = "contactslist";
        private readonly string CollectionId = "contacts_development";
        private readonly DocumentClient client;


        public Repository()
        {
            client = new DocumentClient(new Uri("https://dev-contact-manager-store.documents.azure.com:443/"), "");
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }


        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        private RequestOptions GetOptions(string partitionKey)
        {
            return new RequestOptions
            {
                PartitionKey = string.IsNullOrWhiteSpace(partitionKey) ? new PartitionKey(Undefined.Value) : new PartitionKey(partitionKey)
            };
        }



        public async Task<T> GetItemsAsync(string partitionKey)
        {

            try
            {

                var options = GetOptions(partitionKey);
                var collectionUrl = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, partitionKey);
                Document document = await client.ReadDocumentAsync(collectionUrl, options);
                return (T)(dynamic)document;
            }
            catch (DocumentClientException ex)
            {
                throw ex;
            }
        }

        private string GetValueAsString(object obj)
        {
            if (obj is null) return null;
            return obj.ToString();
        }

        public async Task<Document> CreateItemAsync(T item, string partitionKeyName)
        {
            var obj = JObject.FromObject(item);
            var partitionKey = obj != default(JObject) ? GetValueAsString(obj.GetValue(partitionKeyName, StringComparison.InvariantCulture)) :
                GetValueAsString(item.GetType().GetProperty(partitionKeyName)?.GetValue(item, null));
            var options = GetOptions(partitionKey);
            return await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item, options);
        }
    }
}
