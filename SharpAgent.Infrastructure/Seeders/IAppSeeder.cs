
namespace SharpAgent.Infrastructure.Seeders
{
    // Made publicso it can be called from Program.cs in Presentation layer
    public interface IAppSeeder
    {
        Task Seed();
    }
}