﻿using AutoMapper;
using CompanyAPI.Entities;
using CompanyAPI.Models;
using CompanyAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.Controllers
{
	[ApiController]
	[Route("api/employee")]
	[Produces("application/json")]
	public class EmployeeController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IEmployeeInfoRepository _employeeInfoRepository;
		public EmployeeController(IMapper mapper, IEmployeeInfoRepository employeeInfoRepository)
		{
			_mapper = mapper;
			_employeeInfoRepository = employeeInfoRepository;
		}

		[HttpGet("{id}", Name = "GetEmployee")]
		public async Task<IActionResult> GetEmployee(int id, bool includeTasks)
		{
			var employee = await _employeeInfoRepository.GetEmployeeAsync(id, includeTasks);
			if (employee == null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<GetEmployeeResponseDto>(employee));
		}
		[HttpPost]
		public async Task<ActionResult> AddEmployeeAsync(PostEmployeeRequestDto newEmployee)
		{
			var createdEmployee = _mapper.Map<Entities.Employee>(newEmployee);
			await _employeeInfoRepository.AddEmployeeAsync(createdEmployee);
			await _employeeInfoRepository.SaveChangesAsync();
			var createdEmployeeToReturn = _mapper.Map<Models.GetEmployeeResponseDto>(createdEmployee);
			return CreatedAtRoute("GetEmployee",
				new
				{
					id = createdEmployeeToReturn.Id
				},
				createdEmployeeToReturn);
		}
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateEmployee(int id, bool task, PutEmployeeRequestDto employee)
		{
			var employeeForUpdate = await _employeeInfoRepository.GetEmployeeAsync(id, task);
			if(employeeForUpdate == null)
			{
				return NotFound();
			}
			_mapper.Map(employee, employeeForUpdate);

			await _employeeInfoRepository.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteEmployee(int id, bool task)
		{
			var employeeForDelete = await _employeeInfoRepository.GetEmployeeAsync(id, task);
			if (employeeForDelete == null)
			{
				return NotFound();
			}
			_employeeInfoRepository.DeleteEmployee(employeeForDelete);
			await _employeeInfoRepository.SaveChangesAsync();
			return NoContent();
		}
	}
}
