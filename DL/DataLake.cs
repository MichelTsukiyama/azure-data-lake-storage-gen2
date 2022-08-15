using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;

namespace DL
{
    public class DataLake
    {
        public static DataLakeServiceClient GetDataLakeServiceClient(string connectionString)
        {
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(connectionString);
            return serviceClient;
        }

        public async Task<DataLakeFileSystemClient> CreateFileSystem(DataLakeServiceClient serviceClient, string containerName)
        {
            try
            {
                return await serviceClient.CreateFileSystemAsync(containerName.ToLower());
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e);
            }
            return null;
        }

        public async Task<DataLakeDirectoryClient> CreateDirectoryAsync(DataLakeServiceClient serviceClient, string containerName)
        {
            var data = DateTime.Now;

            DataLakeFileSystemClient fileSystemClient = serviceClient.GetFileSystemClient(containerName);

            return await fileSystemClient.CreateDirectoryAsync($"odonto/{data.Year}/{data.Month}");
        }

        public async Task UploadFile(DataLakeFileSystemClient fileSystemClient, string fileName,string filePath)
        {
            var data = DateTime.Now;

            DataLakeDirectoryClient directoryClient =
                fileSystemClient.GetDirectoryClient($"odonto/{data.Year}/{data.Month}");

            DataLakeFileClient fileClient = await directoryClient.CreateFileAsync($"{data.Day}-{fileName}");

            FileStream fileStream =
                File.OpenRead(filePath);
            
            long fileSize = fileStream.Length;

            await fileClient.AppendAsync(fileStream, offset: 0);

            await fileClient.FlushAsync(position: fileSize);
        }

        public async Task ListFilesInDirectory(DataLakeFileSystemClient fileSystemClient, string path)
        {
            IAsyncEnumerator<PathItem> enumerator =
                fileSystemClient.GetPathsAsync(path).GetAsyncEnumerator();

            await enumerator.MoveNextAsync();

            PathItem item = enumerator.Current;

            while (item != null)
            {
                Console.WriteLine($"{item.Name} - created on: {item.CreatedOn} - size: {item.ContentLength}");

                if (!await enumerator.MoveNextAsync())
                {
                    break;
                }
                item = enumerator.Current;
            }
        }

        public async Task UploadFileFromUrl(DataLakeFileSystemClient fileSystemClient, string fileName, string url)
        {
            var data = DateTime.Now;

            DataLakeDirectoryClient directoryClient =
                fileSystemClient.GetDirectoryClient($"odonto/{data.Year}/{data.Month}");

            DataLakeFileClient fileClient = await directoryClient.CreateFileAsync($"{data.Day}-{fileName}");
            
            //não esquecer de alterar a extensão do arquivo no Program.cs-> fileName
            Stream fileStream = await GetStreamFromUrlAsync(url);
            fileStream.Position = 0;

            long fileSize = fileStream.Length;

            await fileClient.AppendAsync(fileStream, offset: 0);

            await fileClient.FlushAsync(position: fileSize);
        }

        private static async Task<Stream> GetStreamFromUrlAsync(string url)
        {
            MemoryStream mStream = new MemoryStream();

            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    streamToReadFrom.CopyTo(mStream);
                }
            }
            return mStream;
        }
    }      
}