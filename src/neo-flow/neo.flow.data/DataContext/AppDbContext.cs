using Microsoft.EntityFrameworkCore;
using neo.flow.data.Models;

namespace neo.flow.data.DataContext;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<BusinessStepExecutionInstance> BusinessStepExecutionInstances { get; set; }

    public DbSet<ProcessExecutionInstance> ProcessExecutionInstances { get; set; }

    public AppDbContext()
    {
        _connectionString = "Server=localhost\\SQLEXPRESS;Database=EmployeeDb;Trusted_Connection=True;TrustServerCertificate=True";
    }
}
