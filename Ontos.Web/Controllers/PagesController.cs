﻿using System;
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
        /// Search pages by their names
        /// </summary>
        /// <response code="200">Pages</response>
        [HttpPost("search")]
        [ProducesResponseType(typeof(PageDto[]), 200)]
        public async Task<IActionResult> SearchPages([FromBody] SearchPageDto searchPageDto)
        {
            var validation = Validator.Validate(searchPageDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToString());

            var q = new SearchQuery(searchPageDto.Text);
            var autocomplete = q.Autocomplete();

            var results = await _storage.SearchPages(searchPageDto.Language, autocomplete);
            var dtos = results.Select(r => new PageSearchResultDto(r));

            return Ok(results);
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
            var deleted = await _storage.DeletePages(id);
            if (deleted.Length == 1)
                return Ok();
            else
                return NotFound($"Dataset not found for id [{id}].");
        }

        /// <summary>
        /// Get all references to the given page
        /// </summary>
        /// <response code="200">References to page</response>
        [HttpGet("{id}/references")]
        [ProducesResponseType(typeof(ReferenceDto[]), 200)]
        public async Task<IActionResult> GetPageReferences([FromRoute] long id)
        {
            var references = await _storage.GetPageReferences(id);
            var dtos = references.Select(r => new ReferenceDto(r)).ToArray();

            return Ok(dtos);
        }

        /// <summary>
        /// Create a new reference to given page
        /// </summary>
        /// <response code="200">Created reference</response>
        [HttpPost("/api/references")]
        [ProducesResponseType(typeof(ReferenceDto[]), 200)]
        public async Task<IActionResult> CreateReference([FromBody] NewReferenceDto newReferenceDto)
        {
            var validation = Validator.Validate(newReferenceDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToString());

            var reference = await _storage.CreateReference(newReferenceDto.ToModel());
            var dto = new ReferenceDto(reference);

            return Ok(dto);
        }

        /// <summary>
        /// Delete the given reference
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Reference not found</response>
        [HttpDelete("/api/references/{id}")]
        public async Task<IActionResult> DeleteReference([FromRoute] long id)
        {
            var deleted = await _storage.DeleteReferences(id);
            if (deleted.Length == 1)
                return Ok();
            else
                return NotFound($"Reference not found for id [{id}].");
        }

        /// <summary>
        /// Get all relations on the given page
        /// </summary>
        /// <response code="200">Relations on page</response>
        [HttpGet("{id}/relations")]
        [ProducesResponseType(typeof(RelatedPageDto[]), 200)]
        public async Task<IActionResult> GetPageRelations([FromRoute] long id)
        {
            var relations = await _storage.GetAllRelatedPages(id);
            var dtos = relations.Select(r => new RelatedPageDto(r)).ToArray();

            return Ok(dtos);
        }

        /// <summary>
        /// Create a related page
        /// </summary>
        /// <response code="200">Created related page</response>
        [HttpPost("{id}/relations")]
        [ProducesResponseType(typeof(RelationDto), 200)]
        public async Task<IActionResult> CreateRelatedPage([FromRoute] long id, [FromBody] NewRelatedPageDto newRelatedPageDto)
        {
            //var validation = Validator.Validate(newRelatedPageDto);
            //if (!validation.IsValid)
            //    return BadRequest(validation.ToString());

            var targetPage = await _storage.CreatePage(newRelatedPageDto.GetNewPage());
            var relation = await _storage.CreateRelation(newRelatedPageDto.GetNewRelation(id, targetPage.Id));

            var dto = new RelationDto(relation);

            return Ok(dto);
        }

        /// <summary>
        /// Create a new relation between two pages
        /// </summary>
        /// <response code="200">Created relation</response>
        [HttpPost("/api/relations")]
        [ProducesResponseType(typeof(RelationDto), 200)]
        public async Task<IActionResult> CreateRelation([FromBody] NewRelationDto newRelationDto)
        {
            var validation = Validator.Validate(newRelationDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToString());

            var relation = await _storage.CreateRelation(newRelationDto.ToModel());
            if (relation == null)
                return BadRequest("Invalid relation");

            var dto = new RelationDto(relation);

            return Ok(dto);
        }

        /// <summary>
        /// Delete the given relation
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Relation not found</response>
        [HttpDelete("/api/relations/{id}")]
        public async Task<IActionResult> DeleteRelation([FromRoute] long id)
        {
            var deleted = await _storage.DeleteRelations(id);
            if (deleted.Length == 1)
                return Ok();
            else
                return NotFound($"Relation not found for id [{id}].");
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
