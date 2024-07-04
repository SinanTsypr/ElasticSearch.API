using ElasticSearch.API.DTOs.ProductFeature;

namespace ElasticSearch.API.DTOs.Product
{
    public record ProductUpdateDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto Feature)
    {
    }
}
