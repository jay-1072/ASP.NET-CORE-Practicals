using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Practical_20.Interfaces;
using Practical_20.Models;

namespace Practical_20.Data
{
	public class DatabaseContext : DbContext
	{
		private string _username;

		public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			var claimsPrincipal = httpContextAccessor.HttpContext?.User;

			_username = claimsPrincipal?.Claims?.SingleOrDefault(c => c.Type == "username")?.Value ?? "Unauthenticated user";
		}

		public DbSet<Student> Students { get; set; }

		public DbSet<User> Users { get; set; }
		public DbSet<AuditEntry> AuditEntries { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AuditEntry>().Property(ae => ae.Changes).HasConversion(
				value => JsonConvert.SerializeObject(value),
				serializedValue => JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedValue));
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			var auditEntries = OnBeforeSaveChanges();

			var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

			await OnAfterSaveChangesAsync(auditEntries);
			return result;
		}

		private List<AuditEntry> OnBeforeSaveChanges()
		{
			ChangeTracker.DetectChanges();
			var entries = new List<AuditEntry>();

			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || !(entry.Entity is IAuditable))
					continue;

				var auditEntry = new AuditEntry
				{
					ActionType = entry.State == EntityState.Added ? "INSERT" : entry.State == EntityState.Deleted ? "DELETE" : "UPDATE",
					EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue.ToString(),
					EntityName = entry.Metadata.ClrType.Name,
					Username = _username,
					TimeStamp = DateTime.UtcNow,
					Changes = entry.Properties.Select(p => new { p.Metadata.Name, p.CurrentValue }).ToDictionary(i => i.Name, i => i.CurrentValue),

					TempProperties = entry.Properties.Where(p => p.IsTemporary).ToList(),
				};

				entries.Add(auditEntry);
			}

			return entries;
		}

		private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
		{
			if (auditEntries == null || auditEntries.Count == 0)
				return Task.CompletedTask;

			foreach (var entry in auditEntries)
			{
				foreach (var prop in entry.TempProperties)
				{
					if (prop.Metadata.IsPrimaryKey())
					{
						entry.EntityId = prop.CurrentValue.ToString();
						entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
					}
					else
					{
						entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
					}
				}
			}

			AuditEntries.AddRange(auditEntries);
			return SaveChangesAsync();
		}
	}
}
