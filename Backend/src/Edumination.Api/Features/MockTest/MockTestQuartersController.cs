using Edumination.Api.Domain.Entities;
using Edumination.Api.Domain.Enums;
using Edumination.Api.Features.MockTest.Dtos;
using Edumination.Api.Features.MockTest.Services;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using Edumination.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Edumination.Api.MockTest;
    [Route("api/[controller]")]
    [ApiController]
    public class MockTestQuartersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MockTestQuartersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MockTestQuarters/1
        [HttpGet("{id}")]
        public async Task<ActionResult<MockTestQuarter>> GetMockTestQuarter(long id)
        {
            var mockTestQuarter = await _context.MockTestQuarters.FindAsync(id);

            if (mockTestQuarter == null)
            {
                return NotFound(new { message = "MockTestQuarter not found" });
            }

            return Ok(mockTestQuarter);
        }
    }