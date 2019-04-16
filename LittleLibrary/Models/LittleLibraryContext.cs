using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LittleLibrary.Models
{
    public partial class LittleLibraryContext : DbContext
    {
        public LittleLibraryContext()
        {
        }

        public LittleLibraryContext(DbContextOptions<LittleLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersBooks> UsersBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.HasKey(e => e.AuthorId);

                entity.Property(e => e.AuthorId).HasColumnName("authorID");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postalCode")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StreetAddress)
                    .HasColumnName("streetAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.authorProfile)
                   .HasColumnName("authorProfile")
                   .HasColumnType("text");
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.Property(e => e.BookId).HasColumnName("bookID");

                entity.Property(e => e.AuthorId).HasColumnName("authorID");

                entity.Property(e => e.BookContent).HasColumnName("bookContent");

                entity.Property(e => e.BookCover).HasColumnName("bookCover");

                entity.Property(e => e.DatePublished)
                    .HasColumnName("datePublished")
                    .HasColumnType("date");

                entity.Property(e => e.Genre)
                    .HasColumnName("genre")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.Summary)
                    .HasColumnName("summary")
                    .HasColumnType("text");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__Books__authorID__6E01572D");
            });          

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("userID");               

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);
               
            });

            modelBuilder.Entity<UsersBooks>(entity =>
            {
                entity.HasKey(e => e.UserBookId);

                entity.Property(e => e.UserBookId).HasColumnName("userBookID");

                entity.Property(e => e.Firstname)
                   .HasColumnName("firstname")
                   .HasMaxLength(30)
                   .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PaypalEmail)
                    .HasColumnName("paypalEmail")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BookId).HasColumnName("bookID");

                entity.Property(e => e.IsPurchased).HasColumnName("isPurchased");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Review)
                    .HasColumnName("review")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewDate)
                    .HasColumnName("reviewDate")
                    .HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.UsersBooks)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__UsersBook__bookI__75A278F5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UsersBook__userI__74AE54BC");
            });
        }
    }
}
