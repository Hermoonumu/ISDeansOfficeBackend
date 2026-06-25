using Microsoft.EntityFrameworkCore;
using DeanInfoSystem.Domain;
public class SystemDbContext : DbContext
{
    public required DbSet<User> Users;

}