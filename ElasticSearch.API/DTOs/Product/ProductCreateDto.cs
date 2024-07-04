using ElasticSearch.API.DTOs.ProductFeature;
using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs.Product
{
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto Feature)
    {
        public Models.Product CreateProduct()
        {
            return new Models.Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new Models.ProductFeature()
                {
                    Width = Feature.Width,
                    Height = Feature.Height,
                    Color = (EColor)int.Parse(Feature.Color),
                }
            };
        }
    }
}
