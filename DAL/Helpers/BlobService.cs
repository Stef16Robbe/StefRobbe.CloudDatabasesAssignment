using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace DAL.Helpers
{
    public class BlobService : IBlobService
    {
        private IConfiguration _configuration;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateFile(string file, string fileName = "")
        {
            var containerClient = await GetContainerClient();
            if (string.IsNullOrEmpty(fileName))
                fileName = Guid.NewGuid() + ".pdf";
            try
            {
                //select the file name you want to give the file
                var cblob = containerClient.GetBlockBlobReference(fileName);
                var bytes = Decode(file);
                using (var stream = new MemoryStream(bytes))
                {
                    //saves the file
                    await cblob.UploadFromStreamAsync(stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with uploading to Blob-storage");
                throw;
            }

            Console.WriteLine("File uploaded to Blob-storage");

            return $"{fileName}";
        }

        public async Task<byte[]> GetBlobFromServer(string fileName)
        {
            var containerClient = await GetContainerClient();
            var blob = containerClient.GetBlockBlobReference($"{fileName}" + ".pdf");
            byte[] result;

            // open the cloud blob
            using (var mStream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(mStream);
                result = mStream.ToArray();
            }

            var stream = new MemoryStream();
            stream.Write(result, 0, result.Length);

            //return blob url to give to the client
            return result;
        }

        public async Task<bool> DeleteBlobFromServer(string fileName)
        {
            var containerClient = await GetContainerClient();
            var blob = containerClient.GetBlockBlobReference($"{fileName}");
            var deleted = await blob.DeleteIfExistsAsync();

            //return blob url to give to the client
            return deleted;
        }

        private async Task<CloudBlobContainer> GetContainerClient()
        {
            // Setup the connection to the storage account
            var connectionString = Environment.GetEnvironmentVariable("BlobStorage");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var serviceClient = storageAccount.CreateCloudBlobClient();
            var container =
                serviceClient.GetContainerReference(
                    $"{Environment.GetEnvironmentVariable("FileContainer")}");
            if (await container.CreateIfNotExistsAsync()) Console.WriteLine("Blob-storage Is created");

            return container;
        }

        private static byte[] Decode(string input)
        {
            var output = input;

            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding

            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    output += "==";
                    break; // Two pad chars
                case 3:
                    output += "=";
                    break; // One pad char
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(input), "Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(output); // Standard base64 decoder

            return converted;
        }
    }
}