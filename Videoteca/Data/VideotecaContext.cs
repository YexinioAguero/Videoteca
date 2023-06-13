using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Videoteca.Models;

namespace Videoteca.Data;

public partial class VideotecaContext : DbContext
{
    public VideotecaContext()
    {
    }

    public VideotecaContext(DbContextOptions<VideotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<MoviesAndSeries> MoviesAndSeries { get; set; }

    public virtual DbSet<MoviesAndSeriesActor> MoviesAndSeriesActors { get; set; }

    public virtual DbSet<MoviesAndSeriesGenre> MoviesAndSeriesGenres { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=163.178.173.130;Database=VideotecaACY;TrustServerCertificate=True; User Id=basesdedatos; Password=rpbases.2022");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.actor_id).HasName("pk_actor");

            entity.Property(e => e.actor_first_name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.actor_last_name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.actor_url).IsUnicode(false);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.comment_id, e.movies_series_id, e.userName }).HasName("pk_c_m_u");

            entity.Property(e => e.comment_id).ValueGeneratedOnAdd();
            entity.Property(e => e.userName).HasMaxLength(256);
            entity.Property(e => e.comment1)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.dateC).HasColumnType("date");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => new { e.episodes_id, e.movies_series_id }).HasName("pk_ep_mov");

            entity.Property(e => e.episodes_id).ValueGeneratedOnAdd();
            entity.Property(e => e.duration)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.title)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.genre_id).HasName("pk_genre");

            entity.Property(e => e.genre_name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MoviesAndSeries>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_idMvS");

            entity.Property(e => e.classification)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.date_added).HasColumnType("date");
            entity.Property(e => e.director)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.duration)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.movie_url).IsUnicode(false);
            entity.Property(e => e.release_year)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.synopsis).IsUnicode(false);
            entity.Property(e => e.title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MoviesAndSeriesActor>(entity =>
        {
            entity.HasKey(e => new { e.movies_series_id, e.actor_id }).HasName("pk_m_s_a");
        });

        modelBuilder.Entity<MoviesAndSeriesGenre>(entity =>
        {
            entity.HasKey(e => new { e.movies_series_id, e.genre_id });
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.rating_id);

            entity.Property(e => e.rating1).HasColumnName("rating");
            entity.Property(e => e.userName).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("User");

            entity.Property(e => e.email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.user_id)
                .HasMaxLength(450)
                .IsUnicode(false);
            entity.Property(e => e.username)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
