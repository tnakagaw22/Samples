﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MyCodeCamp.Models;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCodeCamp.Controllers
{
    [Route("api/camps/{moniker}/speakers")]
    [ValidateModel]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class SpeakersController : BaseController
    {
        protected ILogger<SpeakersController> _logger;
        protected IMapper _mapper;
        protected ICampRepository _repository;
        protected UserManager<CampUser> _userManager;

        public SpeakersController(ICampRepository repository, ILogger<SpeakersController> logger, IMapper mapper, UserManager<CampUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult Get(string moniker, bool includeTalks = false)
        {
            var speakers = includeTalks ? _repository.GetSpeakersByMonikerWithTalks(moniker) : _repository.GetSpeakersByMoniker(moniker);

            return Ok(_mapper.Map<IEnumerable<SpeakerModel>>(speakers));
        }

        [HttpGet]
        [MapToApiVersion("1.1")]
        public virtual IActionResult GetWithCount(string moniker, bool includeTalks = false)
        {
            var speakers = includeTalks ? _repository.GetSpeakersByMonikerWithTalks(moniker) : _repository.GetSpeakersByMoniker(moniker);

            return Ok(new { count = speakers.Count(), results = _mapper.Map<IEnumerable<SpeakerModel>>(speakers) });
        }

        [HttpGet("{id}", Name = "SpeakerGet")]
        public IActionResult Get(string moniker, int id, bool includeTalks = false)
        {
            var speaker = includeTalks ? _repository.GetSpeakerWithTalks(id) : _repository.GetSpeaker(id);
            if (speaker == null)
                return NotFound();

            if (speaker.Camp.Moniker.ToLower() != moniker.ToLower())
                return BadRequest("Speaker not in specified Camp");

            return Ok(_mapper.Map<SpeakerModel>(speaker));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(string moniker, [FromBody]SpeakerModel model)
        {
            try
            {
                var camp = _repository.GetCampByMoniker(moniker);
                if (camp == null)
                    return BadRequest("Could not find camp");

                var speaker = _mapper.Map<Speaker>(model);
                speaker.Camp = camp;

                var campUser = await _userManager.FindByNameAsync(this.User.Identity.Name);
                if (campUser != null)
                {
                    speaker.User = campUser;

                    _repository.Add(speaker);

                    if (await _repository.SaveAllAsync())
                    {
                        var url = Url.Link("SpeakerGet", new { moniker = camp.Moniker, id = speaker.Id });
                        return Created(url, _mapper.Map<SpeakerModel>(speaker));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while adding speaker: {ex}");
            }

            return BadRequest("Could not add new speaker");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(string moniker, int id, [FromBody]SpeakerModel model)
        {
            try
            {
                var speaker = _repository.GetSpeaker(id);
                if (speaker == null)
                    return NotFound($"Could not find a speaker");

                if (speaker.Camp.Moniker != moniker)
                    return BadRequest("Speaker and Camp do not match");

                if (speaker.User.UserName != this.User.Identity.Name)
                    return Forbid();

                _mapper.Map(model, speaker);

                if (await _repository.SaveAllAsync())
                {
                    return Ok(_mapper.Map<SpeakerModel>(speaker));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while updating speaker: {ex}");
            }

            return BadRequest("Could not update speaker");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string moniker, int id)
        {
            try
            {
                var speaker = _repository.GetSpeaker(id);
                if (speaker == null)
                    return NotFound($"Could not find a speaker");

                if (speaker.Camp.Moniker != moniker)
                    return BadRequest("Speaker and Camp do not match");

                if (speaker.User.UserName != this.User.Identity.Name)
                    return Forbid();

                _repository.Delete(speaker);

                if (await _repository.SaveAllAsync())
                {
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while deleting speaker: {ex}");
            }

            return BadRequest("Could not delete speaker");
        }
    }
}
