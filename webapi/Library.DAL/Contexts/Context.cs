using Library.Domain.Const;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Contexts
{
	public class Context : DbContext
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<BookText> BookTexts { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }

		public Context(DbContextOptions<Context> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Book>()
				.HasOne(item => item.BookText)
				.WithOne(item => item.Book)
				.HasForeignKey<BookText>(item => item.Id);

			modelBuilder.Entity<Book>().ToTable("Books");
			modelBuilder.Entity<BookText>().ToTable("Books");

			modelBuilder.Entity<User>()
				.HasIndex(item => item.Login)
				.IsUnique();

			modelBuilder.Entity<User>()
				.HasMany(item => item.Roles)
				.WithMany(item => item.Users)
				.UsingEntity<UserRole>();

			modelBuilder.Entity<User>()
				.HasMany(item => item.Books)
				.WithOne(item => item.User)
				.HasForeignKey(item => item.UserId);

			SetData(modelBuilder);
		}

		private static void SetData(ModelBuilder modelBuilder)
		{
			Role[] roles = {
				new Role()
				{
					Id = Guid.Parse("e5e7f84a-685f-429b-ae38-9b44e067d76a"),
					Name = LibraryRoles.Author
				},
				new Role()
				{
					Id = Guid.Parse("2949d456-901a-4f7b-a7a3-7fd1e8c2f65d"),
					Name = LibraryRoles.Admin
				}
			};

			User admin = new()
			{
				Id = Guid.Parse("630313d4-d313-4fa8-bb0e-bfce2dcb26b9"),
				Login = "admin",
				PasswordHash = Convert.FromBase64String(
					"PY5glS3zNRz1wqN8Fcw+tznBe9FEBfCVN38KxUAx2k9WDpHnxvOErceYolyjnWy5zM+659hK9wOnan5EYIBvDA=="),
				PasswordSalt = Convert.FromBase64String(
					"ghH/dvpMPe2CTS3d/lVfmudCAspmmEc0RJn40a6iPkez+CdYdAs3wwUVuL6l4ClUJbehuNrCDaHXhP8smLmpZfXGvpczTt" +
					"+j4BET+O2cWYfSquE6auNdT3chbSeJj7HAwaDftuN6HfCEcEaopEdxTu/7mWPfAfIbRE6OT09huDk=")
			};

			UserRole[] adminRole = {
				new UserRole()
				{
					UserId = Guid.Parse("630313d4-d313-4fa8-bb0e-bfce2dcb26b9"),
					RoleId = Guid.Parse("2949d456-901a-4f7b-a7a3-7fd1e8c2f65d")
				}
			};

			modelBuilder.Entity<User>().HasData(admin);
			modelBuilder.Entity<Role>().HasData(roles);
			modelBuilder.Entity<UserRole>().HasData(adminRole);
		}
	}
}
