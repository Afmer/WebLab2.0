using Microsoft.AspNetCore.Mvc;
using Weblab.Architecture.Interfaces;

namespace Weblab.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }
    [HttpGet]
    public IActionResult Shows(string query)
    {
        if(query != null && query != "")
        {
            var result = _searchService.SearchShows(query);
            return Ok(result);
        }
        return Ok(new {});
    }
}