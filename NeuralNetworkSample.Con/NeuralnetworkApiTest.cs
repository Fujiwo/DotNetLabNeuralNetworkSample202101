using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NeuralNetworkSample.Con
{
    class NeuralNetworkApiTest
    {
        static readonly ApiInput[] inputs = new[] {
            new ApiInput { Latitude = 36.258288f, Longitude = 136.284582f },
            new ApiInput { Latitude = 36.274912f, Longitude = 136.329279f },
            new ApiInput { Latitude = 36.115784f, Longitude = 136.436160f },
            new ApiInput { Latitude = 35.921390f, Longitude = 136.887256f },
            new ApiInput { Latitude = 35.801149f, Longitude = 136.819202f },
            new ApiInput { Latitude = 35.837652f, Longitude = 136.695777f },
            new ApiInput { Latitude = 35.810712f, Longitude = 136.505430f },
            new ApiInput { Latitude = 35.742470f, Longitude = 136.561360f },
            new ApiInput { Latitude = 35.802909f, Longitude = 136.316039f },
            new ApiInput { Latitude = 35.665852f, Longitude = 136.343790f },
            new ApiInput { Latitude = 35.571157f, Longitude = 136.081772f },
            new ApiInput { Latitude = 35.503405f, Longitude = 136.098920f },
            new ApiInput { Latitude = 35.482062f, Longitude = 135.943618f },
            new ApiInput { Latitude = 35.333335f, Longitude = 135.606685f },
            new ApiInput { Latitude = 35.420561f, Longitude = 135.548270f },
            new ApiInput { Latitude = 35.473941f, Longitude = 135.436580f },
            new ApiInput { Latitude = 35.532702f, Longitude = 135.462497f },
            new ApiInput { Latitude = 35.557382f, Longitude = 135.465656f },
            new ApiInput { Latitude = 36.047508f, Longitude = 136.414003f },
            new ApiInput { Latitude = 35.437255f, Longitude = 135.641224f },
            new ApiInput { Latitude = 35.451298f, Longitude = 135.805629f },
            new ApiInput { Latitude = 35.535321f, Longitude = 135.986298f },
            new ApiInput { Latitude = 35.606896f, Longitude = 136.129897f },
            new ApiInput { Latitude = 35.798155f, Longitude = 136.297373f },
            new ApiInput { Latitude = 35.868748f, Longitude = 136.665986f },
            new ApiInput { Latitude = 36.054450f, Longitude = 136.587643f },
            new ApiInput { Latitude = 36.202522f, Longitude = 136.260118f }
        };

        public static async Task Run()
        {
            var input  = inputs[0];
            var output = await Predict(input);
            Console.WriteLine($"Input: (Latitude: {input.Latitude}, Longitude: {input.Longitude}) Prediction: {output.Prediction}");
        }

        //static HttpClient client = new HttpClient(
        //    new HttpClientHandler {
        //        ClientCertificateOptions                  = ClientCertificateOption.Manual,
        //        ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
        //    }
        // );

        static async Task<ApiOutput> Predict(ApiInput input)
        {
            var handler = new HttpClientHandler() {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };
            using var client = new HttpClient(handler);

            var scoreRequest = new {
                Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                    {
                        "WebServiceInput0",
                        new List<Dictionary<string, string>> {
                            new Dictionary<string, string> {
                                { "Latitude"    , $"{input.Latitude}"  }, // 35.9947998
                                { "Longitude"   , $"{input.Longitude}" }, // 136.1440818
                                { "CorrectValue", "0"                  },
                            }
                        }
                    },
                },
                GlobalParameters = new Dictionary<string, string> {}
            };

            const string apiKey = "iA3c12umDET9WfAaxCt1ZgvH1HCkMqe7"; // Replace this with the API key for the web service
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri("http://a27c049c-7f6a-4e94-8268-9c6a072bb33e.japaneast.azurecontainer.io/score");

            var requestString = JsonConvert.SerializeObject(scoreRequest);
            var content = new StringContent(requestString);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try {
                using var response = await client.PostAsync("", content);
                if (response.IsSuccessStatusCode) {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var resultObject = JsonConvert.DeserializeObject<System.Dynamic.ExpandoObject>(resultJson);
                    var outputObject = ((dynamic)resultObject).Results.WebServiceOutput0[0];
                    return new ApiOutput { Prediction = outputObject.CorrectValue };
                } else {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception("responseContent");
                }
            } catch (Exception ex) {
                throw;
            }
        }

        public class ApiInput
        {
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        public class ApiOutput
        {
            public double Prediction { get; set; }
        }
    }
}
