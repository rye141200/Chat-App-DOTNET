using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<AppUser?> GetUserByIdAsync(int id) =>
        await context.Users.FindAsync(id);


    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    => await context.Users.SingleOrDefaultAsync(user => user.Username == username);

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    => await context.Users.ToListAsync();

    public async Task<bool> SaveAllAsync()
    => (await context.SaveChangesAsync()) > 0;

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
