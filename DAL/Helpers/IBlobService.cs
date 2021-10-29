using System.Threading.Tasks;

namespace DAL.Helpers
{
    public interface IBlobService
    {
        Task<string> CreateFile(string file, string fileName = "");
        Task<bool> DeleteBlobFromServer(string fileName);
        Task<byte[]> GetBlobFromServer(string fileName);
    }
}