using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UserManagementTriggers
{
    public static class PatientsTrigger
    {
        [FunctionName("PatientsTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "DaaS-Users",
            collectionName: "Patients",
            ConnectionStringSetting = "ConStrCollection",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                var logicAppUrl = GetEnvironmentVariable("LA-PatientTrigger");
                using (var client = new HttpClient())
                {
                    var jsonString = JsonConvert.SerializeObject(input);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(logicAppUrl, content).Result;
                }
            }
        }
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
