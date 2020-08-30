using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ontos.Contracts;
using Ontos.Storage;
using Ontos.Web.Contracts;
using Ontos.Web.Validation;
using Scenes.Web.Contracts;

namespace Scenes.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : Controller
    {
        private IGraphStorage _storage;

        public PagesController(IGraphStorage storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Get paginated pages
        /// </summary>
        /// <response code="200">Paginated pages</response>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<PageDto[]>), 200)]
        public async Task<IActionResult> GetPages(int page, int pageSize, string sortColumn, string sortDirection)
        {
            var paginationParams = GetPagePagination(page, pageSize, sortColumn, sortDirection);

            var pages = await _storage.GetPages(paginationParams.ToModel());
            var dtos = pages.Map(s => new PageDto(s));

            return Ok(pages);
        }

        /// <summary>
        /// Get an existing page
        /// </summary>
        /// <response code="200">The page</response>
        /// <response code="404">Page not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PageDto[]), 200)]
        public async Task<IActionResult> GetPage([FromRoute] long id)
        {
            var page = await _storage.GetPage(id);
            if (page == null)
                return NotFound($"Page not found for id [{id}].");

            var dto = new PageDto(page);

            return Ok(dto);
        }

        /// <summary>
        /// Create a new page
        /// </summary>
        /// <response code="200">The newly created page</response>
        /// <response code="400">Invalid parameters</response>
        [HttpPost()]
        [ProducesResponseType(typeof(PageDto[]), 200)]
        public async Task<IActionResult> Create([FromBody] NewPageDto newPageDto)
        {
            var validation = Validator.Validate(newPageDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToString());

            var page = await _storage.CreatePage(newPageDto.ToModel());

            return Ok(new PageDto(page));
        }

        /// <summary>
        /// Update an existing page
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid parameters</response>
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] UpdatePageDto updatePageDto)
        {
            var validation = Validator.Validate(updatePageDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToString());

            var page = await _storage.UpdatePage(updatePageDto.ToModel());
            if (page != null)
                return Ok();
            else
                return NotFound($"Page not found for id [{updatePageDto.Id}].");
        }

        /// <summary>
        /// Delete an existing page
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Dataset not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (await _storage.DeletePage(id))
                return Ok();
            else
                return NotFound($"Dataset not found for id [{id}].");
        }

        
        private static PaginationParamsDto<PageSortKey> GetPagePagination(int page, int pageSize, string sortColumn, string sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortDirection))
            {
                sortColumn = "updatedAt";
                sortDirection = "desc";
            }

            return new PaginationParamsDto<PageSortKey>(page, pageSize, sortColumn, sortDirection);
        }

    }
}
