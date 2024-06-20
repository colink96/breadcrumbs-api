using BreadcrumbsApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreadcrumbsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CrumbsController : ControllerBase
    {
        private readonly BreadcrumbsContext _breadcrumbsContext;
        private readonly ILogger<CrumbsController> _logger;

        public CrumbsController(BreadcrumbsContext breadcrumbsContext, ILogger<CrumbsController> logger)
        {
            _breadcrumbsContext = breadcrumbsContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Crumb>> GetAllCrumbsAsync()
        {
            return await Task.FromResult(_breadcrumbsContext.Crumbs);
        }
    }
}
