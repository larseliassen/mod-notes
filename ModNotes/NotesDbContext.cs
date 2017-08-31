using Microsoft.EntityFrameworkCore;
using ModNotes.DbModels;


namespace ModNotes
{
	public class NotesDbContext : DbContext
	{
		public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
		{
		}

		public DbSet<Note> Notes { get; set; }      
	}
}