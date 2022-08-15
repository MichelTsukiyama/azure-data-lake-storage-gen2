// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using DL;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

var configuration = builder.Build();

//Strings de conexão
var connectionStringDataLake = configuration.GetConnectionString("DataLakeAccount");
var url = configuration.GetSection("Values")["Url"];
// var url = Environment.GetEnvironmentVariable("Url");

//Variaveis
string containerName = "teste";
string fileName = "uploaded-file.json";
string fileName2 = "uploaded-file.xlsx";
string filePath = "../error.json";

//Instância e cria conexão com DataLake
DataLake dataLake = new DataLake();
var serviceClient = DataLake.GetDataLakeServiceClient(connectionStringDataLake);
var fileSystemClient = serviceClient.GetFileSystemClient(containerName);

// Cria container
await dataLake.CreateFileSystem(serviceClient, containerName);
 
// Cria diretórios com base na data atual
await dataLake.CreateDirectoryAsync(serviceClient, containerName);

// Upload de um Arquivo Local
await dataLake.UploadFile(fileSystemClient, fileName, filePath);

//Upload de uma Url arquivo tipo .xlsx
//Inserir a Url em appsettings.json
// await dataLake.UploadFileFromUrl(fileSystemClient, fileName2, url);

await dataLake.ListFilesInDirectory(fileSystemClient, "pme/2022/8");



//Transferir arquivos entre storages
// var blobAccount = DataMovement.ConfigureAcconut(connectionStringBlob);
// var dataLakeAccount = DataMovement.ConfigureAcconut(connectionStringDataLake);

//  await DataMovement.TransferAzureBlobToAzureBlob(blobAccount, dataLakeAccount);
