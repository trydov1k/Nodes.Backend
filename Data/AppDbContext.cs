using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Node> Nodes { get; set; }
    public DbSet<NodeGroup> NodeGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NodeGroup>()
            .HasKey(group => group.GroupId);
        modelBuilder.Entity<NodeGroup>()
            .Property(group => group.Name)
            .IsRequired();

        modelBuilder.Entity<Node>()
            .HasKey(node => node.NodeId);
        modelBuilder.Entity<Node>()
            .Property(node => node.Header)
            .IsRequired();
        modelBuilder.Entity<Node>()
            .Property(node => node.Text)
            .IsRequired();
        modelBuilder.Entity<Node>()
            .HasOne(node => node.Group)
            .WithMany(group => group.Nodes)
            .HasForeignKey(node => node.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}
