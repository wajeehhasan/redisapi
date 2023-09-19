﻿using AutoMapper;
using DATA.Models;
using redisapi.Models;
using LOGIC.Interface;
using LOGIC.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace redisapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly Mapper _mapper;
        private readonly ILocationInterface _location;
        private readonly ICommonInterface<LocationLOGIC,Location> _commonInterface;
        public LocationController(ILocationInterface location, ICommonInterface<LocationLOGIC, Location> commonInterface)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps("redisapi"));
            _mapper = new Mapper(configuration);
            _location = location;
            _commonInterface = commonInterface;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<GenericResultSet<Location>> Location(string ip_address)
        {
            GenericResultSet<Location> response = new();
            var returnedResponse = await _location.GetLocation(ip_address);
            response = _commonInterface.convertResultSet<Location>(returnedResponse, _mapper);
            return response;
        }
    }
}
