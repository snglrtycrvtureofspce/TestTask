using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUser()
    {
        var user = await _context.Users
            .Where(u => u.Orders
                .Any(o => o.CreatedAt.Year == 2003))
            .OrderByDescending(u => u.Orders
                .Where(o => o.CreatedAt.Year == 2003)
                .Sum(o => o.Price * o.Quantity))
            .FirstOrDefaultAsync() ?? null;

        return user;
    }

    public async Task<List<User>> GetUsers()
    {
        var users = await _context.Users
            .Where(u => u.Orders
                .Any(o => o.CreatedAt.Year == 2010 && o.Status == OrderStatus.Paid))
            .ToListAsync();

        return users;
    }
}