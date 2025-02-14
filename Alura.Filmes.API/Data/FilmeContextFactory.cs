using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Alura.Filmes.API.Data
{
    public class FilmeContextFactory : IDesignTimeDbContextFactory<FilmeContext>
    {
        public FilmeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FilmeContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Database=filme;User=root;Password=@Mysql031996;",
                new MySqlServerVersion(new Version(8, 0, 32))
            );

            return new FilmeContext(optionsBuilder.Options);
        }
    }
}
