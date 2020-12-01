using System.Threading.Tasks;

namespace CarvedRockSoftware.Seeder
{
    public interface ISeeder
    {
        Task RunAsync();
    }
}
