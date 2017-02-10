using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MyCodeCamp.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCodeCamp.Controllers
{
    [Route("api/camps/{moniker}/speakers")]
    public class SpeakersController : BaseController
    {
        private ILogger<SpeakersController> _logger;
        private IMapper _mapper;
        private ICampRepository _repository;

        public SpeakersController(ICampRepository repository, ILogger<SpeakersController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(string moniker)
        {
            var speakers = _repository.GetSpeakersByMoniker(moniker);

            return Ok(_mapper.Map<IEnumerable<SpeakerModel>>(speakers));
        }

        [HttpGet("{id}", Name = "SpeakerGet")]
        public IActionResult Get(string moniker, int id)
        {
            var speaker = _repository.GetSpeaker(id);
            if (speaker == null)
                return NotFound();

            if (speaker.Camp.Moniker.ToLower() != moniker.ToLower())
                return BadRequest("Speaker not in specified Camp");

            return Ok(_mapper.Map<SpeakerModel>(speaker));
        }
    }
}
