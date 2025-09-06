using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

//using backend.Data.Entities;

namespace backend.Data.Context;

public class DataContext : IdentityDbContext<User>
{
    public DbSet<InviteCode> InviteCodesDb { get; set; }
    public DbSet<ChatMessage> ChatMessagesDb { get; set; }
    public DbSet<Room> RoomsDb { get; set; }
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ChatMessage>()
            .Property(b => b.Timestamp)
            .HasDefaultValueSql("current_timestamp");
    }
}
