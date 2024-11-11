using Microsoft.EntityFrameworkCore;

namespace SteveLauncher.Data.Database;

public class SteveDbContext: DbContext {
    DbSet<LocalServerListDatabase> LocalServerList { get; set; }
}