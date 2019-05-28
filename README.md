# ElasticSearchPDFExample

Small example using Elasticsearch with .NET Core 2.2 and NEST for indexing PDF or document files.

## Usage ##

1. This example is based on Elasticsearch 6.7.0(https://www.elastic.co/pt/downloads/past-releases/elasticsearch-6-7-0), it won't work on 7.x because of many changes on that version. Install your preferable version, I made this using the MSI version non-service, check ingest-plugin on the installation if you are installing throught MSI.

In the PostmanCalls folder you can find some API calls where you can base yourself.

2. To start using it, create an index by calling /api/index/create to create the Article index
3. Next call /api/article/index?path= and put the path to some document for example /api/article/index?path=C:\temp\example.pdf
4. To search by OData query, call /api/article/search?query= to search all files you indexed.
