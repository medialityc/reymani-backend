using Microsoft.EntityFrameworkCore;


namespace reymani_web_api.Data;

public class DBContext : DbContext
{
  public DBContext(DbContextOptions<DBContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
  }

}
