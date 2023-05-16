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
        => optionsBuilder.UseSqlServer("Server=163.178.173.130;Database=VideotecaACY;user id=basesdedatos;password=rpbases.2022;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.actor_first_name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.actor_id).ValueGeneratedOnAdd();
            entity.Property(e => e.actor_last_name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.actor_url)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.comment1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.comment_id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.duration)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.episodes_id).ValueGeneratedOnAdd();
            entity.Property(e => e.title)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.genre_id);

            entity.Property(e => e.genre_name)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MoviesAndSeries>(entity =>
        {
            entity.HasNoKey();
            entity.HasKey(e => e.id);
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
            entity.Property(e => e.episode_duration)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.id).ValueGeneratedOnAdd();
            entity.Property(e => e.movie_url)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.release_year)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.synopsis)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MoviesAndSeriesActor>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<MoviesAndSeriesGenre>(entity =>
        {
            entity.HasKey(e => new { e.movies_series_id, e.genre_id });

            entity.Property(e => e.movies_series_id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.rating_id);

            entity.Property(e => e.rating1).HasColumnName("rating");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("User");

            entity.Property(e => e.email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.user_id).ValueGeneratedOnAdd();
            entity.Property(e => e.username)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
