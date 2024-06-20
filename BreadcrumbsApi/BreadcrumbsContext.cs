using BreadcrumbsApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BreadcrumbsApi;

public partial class BreadcrumbsContext : DbContext
{
    public BreadcrumbsContext()
    {
    }

    public BreadcrumbsContext(DbContextOptions<BreadcrumbsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Crumb> Crumbs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=breadcrumbs;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Crumb>(entity =>
        {
            entity.HasKey(e => e.CrumbId).HasName("crumbs_pkey");

            entity.ToTable("crumbs", "breadcrumbs");

            entity.Property(e => e.CrumbId)
                .ValueGeneratedNever()
                .HasColumnName("crumbId");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Radius)
                .HasDefaultValueSql("5")
                .HasColumnName("radius");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");

            entity.HasOne(d => d.User).WithMany(p => p.Crumbs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("crumbs_userId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users", "breadcrumbs");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
            entity.Property(e => e.Password).HasColumnName("password").ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
