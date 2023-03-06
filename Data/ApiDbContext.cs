using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnet7_webapi_jwt.Data;

public partial class ApiDbContext : IdentityDbContext<IdentityUser>
{
    public ApiDbContext()
    {
    }

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql("User ID =docker;Password=docker;Server=localhost;Port=5432;Database=pg_testdb; Integrated Security=true;Pooling=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<TodoList>? TodoList { get; set; }
}
