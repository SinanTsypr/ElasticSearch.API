using ElasticSearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {
        private readonly ECommerceRespository _repository;

        public ECommerceController(ECommerceRespository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            return Ok(await _repository.TermQueryAsync(customerFirstName));
        }

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {
            return Ok(await _repository.TermsQueryAsync(customerFirstNameList));
        }

        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            return Ok(await _repository.PrefixQueryAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            return Ok(await _repository.RangeQueryAsync(fromPrice, toPrice));
        }

        [HttpGet]
        public async Task<IActionResult> MatchAll()
        {
            return Ok(await _repository.MatchAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> PaginationQuery(int page=1, int pageSize=3)
        {
            return Ok(await _repository.PaginationAsync(page, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> WildCardQuery(string customerFullName)
        {
            return Ok(await _repository.WildCardQueryAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> FuzzyQuery(string customerFirstName)
        {
            return Ok(await _repository.FuzzyQueryAsync(customerFirstName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText(string categoryName)
        {
            return Ok(await _repository.MatchQueryFullTextAsync(categoryName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchBoolPrefixFullText(string customerFullName)
        {
            return Ok(await _repository.MatchBoolPrefixFullTextAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchPhraseFullText(string customerFullName)
        {
            return Ok(await _repository.MatchPhraseFullTextAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleOne(string cityName, double taxfulTotalPrice, string categoryName, string manufacturer)
        {
            return Ok(await _repository.CompoundQueryExampleOneAsync(cityName, taxfulTotalPrice, categoryName, manufacturer));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleTwo(string customerFullName)
        {
            return Ok(await _repository.CompoundQueryExampleTwoAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> MultiMatchQueryFullText(string name)
        {
            return Ok(await _repository.MultiMatchQueryFullTextAsync(name));
        }
    }
}
