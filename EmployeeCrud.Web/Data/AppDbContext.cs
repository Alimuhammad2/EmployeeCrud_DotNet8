using EmployeeCrud.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrud.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
}
