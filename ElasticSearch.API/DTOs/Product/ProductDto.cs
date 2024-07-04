using ElasticSearch.API.DTOs.ProductFeature;
using Nest;

namespace ElasticSearch.API.DTOs.Product
{
    public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
    {
    }
}
