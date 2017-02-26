﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MyCodeCamp.Models;
using MyCodeCamp.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCodeCamp.Controllers
{
    [Authorize]
    [EnableCors("AnyGET")]
    [Route("api/[controller]")]
    [ValidateModel]
    public class CampsController : BaseController
    {
        private ICampRepository _repo;
        private ILogger<CampsController> _logger;
        private IMapper _mapper;

        public CampsController(ICampRepository repo, ILogger<CampsController> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/{controller}
        [HttpGet("")]
        public IActionResult Get()
        {
            var camps = _repo.GetAllCamps();

            return Ok(_mapper.Map<IEnumerable<CampModel>>(camps));
        }

        // GET api/{controller}/{id}
        [HttpGet("{moniker}", Name = "CampGet")]
        public IActionResult Get(string moniker, bool includeSpeakers = false)
        {
            try
            {
                Camp camp = null;

                if (includeSpeakers)
                    camp = _repo.GetCampByMonikerWithSpeakers(moniker);
                else
                    camp = _repo.GetCampByMoniker(moniker);

                if (camp == null)
                    return NotFound($"Camp {moniker} was not found");

                return Ok(_mapper.Map<CampModel>(camp));
            }
            catch (Exception)
            {

            }

            return BadRequest();
        }

        // POST api/values
        [EnableCors("Wildermuth")]
        [Authorize(Policy = "SuperUsers")]
        [HttpPost]
        public async Task  <IActionResult> Post([FromBody]CampModel model)
        {
            try
            {
                _logger.LogInformation("Creating a new Code Camp");

                var camp = _mapper.Map<Camp>(model);

                _repo.Add(camp);
                if (await _repo.SaveAllAsync())
                {
                    var newUri = Url.Link("CampGet", new { moniker = camp.Moniker });
                    return Created(newUri, _mapper.Map<CampModel>(camp));
                }
                else
                {
                    _logger.LogWarning("Could not save Camp to the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while saving Camp : {ex}");
            }

            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{moniker}")]
        public async Task<IActionResult> Put(string moniker, [FromBody]CampModel model)
        {
            try
            {
                var oldCamp = _repo.GetCampByMoniker(moniker);
                if (oldCamp == null)
                    return NotFound($"Could not find a camp with an moniker of {moniker}");

                _mapper.Map(model, oldCamp);

                if (await _repo.SaveAllAsync())
                {
                    return Ok(_mapper.Map<CampModel>(oldCamp));
                }
                else
                {
                    _logger.LogWarning("Could not save Camp to the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while saving Camp : {ex}");
            }

            return BadRequest("Couldn't update Camp");
        }

        // DELETE api/values/5
        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var oldCamp = _repo.GetCampByMoniker(moniker);
                if (oldCamp == null)
                    return NotFound($"Could not find a camp with an moniker of {moniker}");

                _repo.Delete(oldCamp);

                if (await _repo.SaveAllAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while deleting Camp : {ex}");
            }

            return BadRequest("Couldn't delete Camp");
        }
    }
}
