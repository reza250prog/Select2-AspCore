using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Select2.WebApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Select2.WebApplication.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CustomerController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet(nameof(Search))]
        public async Task<IActionResult> Search([FromQuery] int page, [FromQuery] string term = "")
        {
            try
            {
                page = page <= 0 ? 1 : page;

                int pageSize = 10;

                var customers = context.Customers
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(term))
                {
                    customers = customers
                        .Where(a => a.FirstName.Contains(term)
                    || a.LastName.Contains(term)
                    || a.City.Contains(term));
                }

                int skip = (page - 1) * pageSize;

                var countAll = await customers.CountAsync();

                var data = await customers
                    .OrderBy(x => x.LastName)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                ResultList<ModelDto> results = new ResultList<ModelDto>
                {
                    items = data.Select(x => new ModelDto
                    {
                        id = x.Id.ToString(),
                        text = $"{x.FirstName} {x.LastName} page{page}"
                    }).ToList(),
                    total_count = countAll,
                    more = (page * pageSize) < countAll
                };

                return Ok(results);
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
    }

    public class ModelDto
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class ResultList<T>
    {
        public List<T> items { get; set; }
        public bool more { get; set; }
        public int total_count { get; set; }
    }
}