using FitnessTracker.Application.Interfaces;
using FitnessTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _users;
    public UsersController(IUserService users) => _users = users;

    [HttpGet]
    public Task<List<User>> GetAll() => _users.GetAllAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var u = await _users.GetByIdAsync(id);
        return u is null ? NotFound() : Ok(u);
    }

    [HttpPost]
    public Task<User> Create(User user) => _users.AddAsync(user);

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.Id) return BadRequest("Id mismatch");
        await _users.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _users.DeleteAsync(id);
        return NoContent();
    }
}
