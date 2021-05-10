﻿using AutoMapper;
using GroceryStore.API.Models;
using GroceryStore.BLL.DTOs;
using GroceryStore.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomersService _customersService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomersService customersService, IMapper mapper)
        {
            _customersService = customersService;
            _mapper = mapper;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<CustomerDTO>), 200)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var customers = await _customersService.GetAll(cancellationToken);
            return Json(customers);
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        public async Task<IActionResult> Get(int customerId, CancellationToken cancellationToken = default)
        {
            var customer = await _customersService.GetById(customerId, cancellationToken);
            return Json(customer);
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(CustomerDTO), 201)]
        public async Task<IActionResult> Post(AddCustomerRequest addCustomerRequest, CancellationToken cancellationToken = default)
        {
            var customerDto = _mapper.Map<CustomerDTO>(addCustomerRequest);
            var customer = await _customersService.Add(customerDto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { customerId = customer.Id }, customer);
        }

        [HttpPut("{customerId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int customerId, CustomerDTO customerDto, CancellationToken cancellationToken = default)
        {
            if(customerId != customerDto.Id)
            {
                return BadRequest("Id does not match");
            }
            await _customersService.Update(customerDto, cancellationToken);
            return NoContent();
        }
    }
}
