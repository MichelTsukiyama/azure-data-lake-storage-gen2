# Estudo referente ao uso do Data Lake Storage no Azure

Este estudo exemplifica principalmente como:

- Conectar-se ao Azure DataLake Storage;
- Criar um Container;
- Criar diretórios;
- Fazer Upload de Arquivos;

Requisitos: Net 6.0, criar um DataLake Storage Gen2 no Azure;

--------

## Iniciar o projeto:

1. Clone este repositório;
2. Restaure o projeto(CLI):

        dotnet restore

3. Insira a string de conexão do seu Data Lake Storage em appsettings.json, em "DataLakeAccount"
4. Há um arquivo local chamado error.json pronto para upload no DataLake Storage;
5. Para fazer upload de um arquivo através de uma Url, insira a Url em appsettings.json, em "Url";
6. Rode o projeto(CLI):
   
        dotnet run

----

## Sobre o DataLake

O Data Lake Storage Gen2 permite que você gerencie de uma maneira fácil grandes quantidades de dados.

Para criar um Data Lake Storage Gen2 basta procurar por stoage na pesquisa do Azure, crie um novo recurso e em "Avançado" selecione a opção de ativar namespaces hierárquicos.

O Data Lake Storage Gen1 será descontinuado em 2024.

É possível trabalhar com Data Lake utilizando tanto a biblioteca do Blob como a do próprio Data Lake:

- using Azure.Storage.Files.DataLake;
- using Azure.Storage.Blobs;

> Obs. Retorna um erro se:
> - Ao criar um container e o mesmo já existir;
> - Tentar verificar a existência de um container pelo nome e o mesmo não existir ;
> - Fazer upload em um diretório inexistente, mas neste caso os diretórios são criados;

----

## Referências

- https://docs.microsoft.com/pt-br/azure/storage/blobs/data-lake-storage-introduction
- https://docs.microsoft.com/en-us/azure/storage/blobs/data-lake-storage-directory-file-acl-dotnet
- https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/storage/Azure.Storage.Files.DataLake/samples/Sample01b_HelloWorldAsync.cs#L93



