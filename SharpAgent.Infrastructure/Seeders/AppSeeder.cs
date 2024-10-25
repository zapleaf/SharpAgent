using SharpAgent.Infrastructure.Data;

namespace SharpAgent.Infrastructure.Seeders;

internal class AppSeeder(AppDbContext dbContext) : IAppSeeder
{
    public async Task Seed()
    {
        // Can you connect to the database that was passed in
        if (await dbContext.Database.CanConnectAsync())
        {
            // Should check if table is empty before inserting seed data
            //if (!dbContext.YourEntity.Any())
            //{
            //    var entities = GetYourEntity();
            //    dbContext.YourEntity.AddRange(entities);
            //    await dbContext.SaveChangesAsync();
            //}
        }
    }

    //private IEnumerable<YourEntity> GetYourEntity()
    //{
    //   // Create a list of seed entities to add to table
    //   List<YourEntity> entities = [
    //      new()...,
    //      new()...
    //   ];
    //}
}
