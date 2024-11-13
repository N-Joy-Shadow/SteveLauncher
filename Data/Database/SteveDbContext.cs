using Microsoft.EntityFrameworkCore;

namespace SteveLauncher.Data.Database;

public class SteveDbContext: DbContext {
    #region constructor
    public SteveDbContext(DbContextOptions<SteveDbContext> options)
        : base(options) {
        SQLitePCL.Batteries_V2.Init();
        this.Database.EnsureCreated();
    }
    public SteveDbContext() {
        SQLitePCL.Batteries_V2.Init();
        this.Database.EnsureCreated();
    }
    #endregion
    
    public DbSet<LocalServerListDatabase> LocalServerList { get; set; }
    
    
    #region overriding method
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite($"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "steveLauncher.db")}");
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
#if DEBUG
        modelBuilder.Entity<LocalServerListDatabase>().HasData(
            new LocalServerListDatabase() { Id = 1, HostName = "mc.hypixel.net", Port = 25565, SRVHostName = "mc.hypixel.net",SRVPort = 25565}
            );
#endif
    }

    #endregion

}