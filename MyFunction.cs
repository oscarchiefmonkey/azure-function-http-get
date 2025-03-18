using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp12
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
            
            // Retrieve the connection string from environment variables
            string connectionString = Environment.GetEnvironmentVariable("storgdevsec001"); 
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Storage connection string is missing.");
            }

            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        [Function("ListFiles")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req)
        {
            _logger?.LogInformation("Fetching blob names from the 'test' container.");

            try
            {
                string containerName = "test";
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                List<string> blobNames = new List<string>();

                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    blobNames.Add(blobItem.Name);
                }

                return new OkObjectResult(blobNames);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error retrieving blobs: {ex.Message}");
                return new ObjectResult("Error retrieving blobs") { StatusCode = 500 };
            }
        }
    }
}
