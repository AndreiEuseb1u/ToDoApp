using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Data;

namespace ToDoApp.Api.Tests.Helpers
{
    public class InMemoryDbContextFactory
    {
        public static AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
    }
}
