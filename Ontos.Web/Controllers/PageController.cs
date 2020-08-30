using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ontos.Storage;

namespace Scenes.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : Controller
    {
        private IGraphStorage _storage;

        public PageController(IGraphStorage storage)
        {
            _storage = storage;
        }

        ///// <summary>
        ///// Get all datasets
        ///// </summary>
        ///// <remarks>TODO: add pagination, filtering and search queries</remarks>
        ///// <response code="200">Returns all datasets</response>
        //[HttpGet]
        //[ProducesResponseType(typeof(Content[]), 200)]
        //public IActionResult GetAll()
        //{
        //    var datasets = _storage.GetAll().Select(s => new DatasetInfoDto(s)).ToArray();

        //    return Ok(datasets);
        //}

        ///// <summary>
        ///// Get an existing dataset
        ///// </summary>
        ///// <response code="200">The dataset</response>
        ///// <response code="400">Invalid GUID</response>
        ///// <response code="404">Dataset not found</response>
        //[HttpGet("{guid}")]
        //public IActionResult Get([FromRoute] Guid guid)
        //{
        //    var dataset = _storage.Get(guid);
        //    if (dataset == null)
        //        return NotFound($"Dataset not found for guid [{guid}].");

        //    var dto = new DatasetInfoDto(dataset);

        //    return Ok(dto);
        //}

        ///// <summary>
        ///// Create a new dataset
        ///// </summary>
        ///// <response code="200">The newly created dataset</response>
        ///// <response code="400">Invalid parameters</response>
        //[HttpPost()]
        //public IActionResult Create([FromBody] NewDatasetDto newDatasetDto)
        //{
        //    var validation = Validator.Validate(newDatasetDto);
        //    if (!validation.IsValid)
        //        return BadRequest(validation.ToString());

        //    var dataset = newDatasetDto.ToModel();
        //    _storage.Create(dataset);

        //    return Ok(new DatasetInfoDto(dataset));
        //}

        ///// <summary>
        ///// Update an existing dataset
        ///// </summary>
        ///// <response code="200">The newly created dataset</response>
        ///// <response code="400">Invalid parameters</response>
        //[HttpPut("{guid}")]
        //public IActionResult Update([FromRoute] Guid guid, [FromBody] UpdateDatasetDto updateDatasetDto)
        //{
        //    var validation = Validator.Validate(updateDatasetDto);
        //    if (!validation.IsValid)
        //        return BadRequest(validation.ToString());

        //    if (_storage.Update(guid, updateDatasetDto.ToModel()))
        //        return Ok();
        //    else
        //        return NotFound($"Dataset not found for guid [{guid}].");
        //}

        ///// <summary>
        ///// Delete an existing dataset
        ///// </summary>
        ///// <response code="200">The dataset</response>
        ///// <response code="400">Invalid GUID</response>
        ///// <response code="404">Dataset not found</response>
        //[HttpDelete("{guid}")]
        //public IActionResult Delete([FromRoute] Guid guid)
        //{
        //    if (_storage.Delete(guid))
        //        return Ok();
        //    else
        //        return NotFound($"Dataset not found for guid [{guid}].");
        //}

    }
}
