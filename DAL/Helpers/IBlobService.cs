using System.Threading.Tasks;
using Domain;

namespace DAL.Helpers
{
    public interface IBlobService
    {
        Task<string> CreateFile(FileStorage file, string fileName = "");
        Task<bool> DeleteBlobFromServer(string fileName);
        Task<byte[]> GetBlobFromServer(string fileName);
    }
}