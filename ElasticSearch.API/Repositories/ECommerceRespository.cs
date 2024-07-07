using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models.EcommerceModel;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
    public class ECommerceRespository
    {
        private const string indexName = "kibana_sample_data_ecommerce";
        private readonly ElasticsearchClient _client;

        public ECommerceRespository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<ImmutableList<ECommerce>> TermQueryAsync(string customerFirstName)
        {
            //1. Yol
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));
            //foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            //return result.Documents.ToImmutableList();

            //2.Yol
            //var result = await _client.SearchAsync<ECommerce>(s => s
            //    .Index(indexName)
            //    .Query(q => q
            //    .Term(t => t.Field(f => f.CustomerFirstName.Suffix("keyword")).Value(customerFirstName))));
            //foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            //return result.Documents.ToImmutableList();

            //3.Yol
            var termQuery = new TermQuery("customer_first_name.keyword") { Value = customerFirstName, CaseInsensitive = true };
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termQuery));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFirstNameList.ForEach(x =>
            {
                terms.Add(x);
            });

            //1.Yol
            //var termsQuery = new TermsQuery()
            //{
            //    Field = "customer_first_name.keyword",
            //    Term = new TermsQueryField(terms.AsReadOnly())
            //};
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termsQuery));

            //2.Yol
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                .Terms(t => t
                .Field(f => f.CustomerFirstName
                .Suffix("keyword"))
                .Term(new TermsQueryField(terms.AsReadOnly())))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(10)
                .Query(q => q
                .Prefix(p => p
                .Field(f => f.CustomerFullName.Suffix("keyword"))
                .Value(customerFullName))));

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double ToPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Query(q => q
                .Range(r => r
                .NumberRange(nr => nr
                .Field(f => f.TaxfulTotalPrice)
                .Gte(fromPrice)
                .Lte(ToPrice)))));

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllAsync()
        {
            //var result = await _client.SearchAsync<ECommerce>(s => s
            //    .Index(indexName)
            //    .Size(100)
            //    .Query(q => q
            //    .MatchAll(m => { })));

            var result = await _client.SearchAsync<ECommerce>(s => s
            .Index(indexName)
            .Size(100)
            .Query(q => q
            .Match(m => m
            .Field(f => f.CustomerFullName)
            .Query("shaw"))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PaginationAsync(int page, int pageSize)
        {
            var pageFrom = (page - 1) * pageSize;

            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(pageSize)
                .From(pageFrom)
                .Query(q => q.MatchAll(m => { })));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> WildCardQueryAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Query(q => q
                .Wildcard(w => w
                .Field(f => f.CustomerFullName
                .Suffix("keyword"))
                .Wildcard(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string customerFirstName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Query(q => q
                .Fuzzy(f => f
                .Field(fi => fi.CustomerFirstName
                .Suffix("keyword"))
                .Value(customerFirstName)
                .Fuzziness(new Fuzziness(2))))
                .Sort(sort => sort
                .Field(field => field.TaxfulTotalPrice, new FieldSort() { Order = SortOrder.Desc })));


            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                .Match(m => m
                .Field(f => f.Category)
                .Query(categoryName)
                .Operator(Operator.And))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MultiMatchQueryFullTextAsync(string name)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                .MultiMatch(mm => mm
                .Fields(new Field("customer_first_name")
                .And(new Field("customer_last_name"))
                .And(new Field("customer_full_name")))
                .Query(name))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                .MatchBoolPrefix(m => m
                .Field(f => f.CustomerFullName)
                .Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchPhraseFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                .MatchPhrase(m => m
                .Field(f => f.CustomerFullName)
                .Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleOneAsync(string cityName, double taxfulTotalPrice, string categoryName, string manufacturer)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                .Bool(b => b
                .Must(m => m
                .Term(t => t
                .Field("geoip.city_name")
                .Value(cityName)))
                .MustNot(mn => mn
                .Range(r => r
                .NumberRange(nr => nr
                .Field(f => f.TaxfulTotalPrice)
                .Lte(taxfulTotalPrice))))
                .Should(s => s
                .Term(t => t
                .Field(f => f.Category
                .Suffix("keyword"))
                .Value(categoryName)))
                .Filter(f => f
                .Term(t => t
                .Field("manufacturer.keyword")
                .Value(manufacturer))))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleTwoAsync(string customerFullName)
        {
            //var result = await _client.SearchAsync<ECommerce>(s => s
            //    .Index(indexName)
            //    .Size(100)
            //    .Query(q => q
            //    .MatchPhrasePrefix(m => m
            //    .Field(f => f.CustomerFullName)
            //    .Query(customerFullName))));

            var result = await _client.SearchAsync<ECommerce>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                .Bool(b => b
                .Must(m => m
                .Match(m => m
                .Field(f => f.CustomerFullName)
                .Query(customerFullName))
                .Prefix(p => p
                .Field(f => f.CustomerFullName)
                .Value(customerFullName))))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }
    }
}
