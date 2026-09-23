using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Data;

namespace ToDoApp.Api.Tests.Helpers
{
    public class InMemoryDbContextFactory
    {
        public static AppDbContext Create(string? databaseName = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
    }
}
