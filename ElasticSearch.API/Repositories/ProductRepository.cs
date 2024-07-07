using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.DTOs.Product;
using ElasticSearch.API.Models;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "products";

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProdcut)
        {
            newProdcut.Created = DateTime.Now;

            var response = await _client.IndexAsync(newProdcut, x => x.Index(indexName));

            if (!response.IsSuccess()) return null;

            newProdcut.Id = response.Id;

            return newProdcut;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            //var result = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q=>q.MatchAll()));
            var result = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q => q.MatchAll(m => { })));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));

            if (!response.IsSuccess())
            {
                return null;
            }

            response.Source.Id = response.Id;

            return response.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(indexName, updateProduct.Id, x => x.Doc(updateProduct));

            return response.IsSuccess();
        }

        //Hata yönetimi için bu metot ele alınmıştır.
        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));

            return response;
        }
    }
}
