using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment1
{
    class Program
    {

        static string[] sc = { DriveService.Scope.DriveReadonly };
        static string AppName = "Uploaded File On Driver ";

        static void Main(string[] args)
        {

            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credentialpath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, sc, "user", CancellationToken.None, new FileDataStore(credentialpath, true)).Result;
                Console.WriteLine("Credential file saved:" + credentialpath);

            }


            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName,
            });


            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 20;
            listRequest.Fields = "nextPageToken, files(id, name)";


            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            Console.WriteLine("files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();

        }
    }
}
