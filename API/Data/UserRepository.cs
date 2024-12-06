using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username) =>
        await context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<MemberDto>> GetMembersAsync() =>
        await context.Users
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<AppUser?> GetUserByIdAsync(int id) =>
        await context.Users
            //    .Include(x => x.Photos)       // doesn't work with FindAsync
            .FindAsync(id);

    public async Task<AppUser?> GetUserByUsernameAsync(string username) =>
        await context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);

    public async Task<IEnumerable<AppUser>> GetUsersAsync() =>
        await context.Users
            .Include(x => x.Photos)
            .ToListAsync();

    public async Task<bool> SaveAllAsync() =>
        await context.SaveChangesAsync() > 0;

    public void Update(AppUser user) =>
        context.Entry(user).State = EntityState.Modified;
}
