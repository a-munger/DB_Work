using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace HttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string data = args.Length > 0 ? args[0] : "Test data from proxy client";

                // Get the service information from the configuration file
                var settings = (ServiceConfiguration)ConfigurationManager.GetSection("serviceSettings");

                // Create the Test URL
                string testUrl = string.Format("{0}{1}{2}",
                    settings.ServiceUrl,
                    settings.ServiceUrl.EndsWith("/") || settings.TestServiceMethod.StartsWith("/") ? "" : "/",
                    settings.TestServiceMethod);
                // Create the test request
                var testRequest = (HttpWebRequest)WebRequest.Create(testUrl);
                // Set headers
                testRequest.Method = "POST";
                testRequest.ContentLength = 0;
                testRequest.ContentType = "application/json";
                if (!string.IsNullOrEmpty(data))
                {
                    // Stream the parameters for the request
                    var bytes = Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(data));
                    testRequest.ContentLength = bytes.Length;
                    var testReqStream = testRequest.GetRequestStream();
                    testReqStream.Write(bytes, 0, bytes.Length);
                    testReqStream.Close();
                }
                Console.WriteLine("Set test data = [{0}]", data);
                Console.WriteLine("Calling {0}() Method...", settings.TestServiceMethod);
                // Retrieve the result from the web service
                var testResponse = testRequest.GetResponse();
                using (var testRespStream = testResponse.GetResponseStream())
                {
                    if (testRespStream == null)
                        throw new ApplicationException(string.Format("No response was returned from {0}", settings.TestServiceMethod));
                    using (var testReader = new StreamReader(testRespStream))
                    {
                        // Get the JSON string
                        string testJson = testReader.ReadToEnd();
                        // Console.WriteLine("Raw Results: {0}", testJson);
                        // Remove escape characters and outer quotes so the sting is deserializable
                        testJson = testJson.Substring(1, testJson.Length - 2).Replace(@"\", "");
                        // Console.WriteLine("JSON: {0}", testJson);
                        // Deserialize the JSON string
                        string testResult = new JavaScriptSerializer().Deserialize<string>(testJson);
                        Console.WriteLine(testResult + Environment.NewLine);
                    }
                }

                // Get a number to compute a Fibonacci value
                const string prompt = "Enter an integer (40 or less) to use for calculation: ";
                Console.WriteLine("{0}{1}", Environment.NewLine, prompt);
                int number;
                while (!int.TryParse(Console.ReadLine(), out number) || number > 40)
                {
                    Console.WriteLine("{0}{0}That entry was invalid!{0}{1}", Environment.NewLine, prompt);
                }
                Console.WriteLine("{0}Calculating Fibonacci value of {1}{0}", Environment.NewLine, number);

                // Create a request using the agreed-upon request data contract
                var request = new ServiceRequest
                {
                    Number = number,
                    UseRecursiveAlgorithm = true
                };

                // Create the Fibonacci URL
                string fibonacciUrl = string.Format("{0}{1}{2}",
                    settings.ServiceUrl,
                    settings.ServiceUrl.EndsWith("/") || settings.ServiceMethod.StartsWith("/") ? "" : "/",
                    settings.ServiceMethod);
                // Update the Fibonacci request
                var fibonacciRequest = (HttpWebRequest)WebRequest.Create(fibonacciUrl);
                // Set headers
                fibonacciRequest.Method = "POST";
                fibonacciRequest.ContentLength = 0;
                fibonacciRequest.ContentType = "application/json";
                // Stream the parameters for the request
                var fibonacciBytes = Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(request));
                fibonacciRequest.ContentLength = fibonacciBytes.Length;
                var fibonacciReqStream = fibonacciRequest.GetRequestStream();
                fibonacciReqStream.Write(fibonacciBytes, 0, fibonacciBytes.Length);
                fibonacciReqStream.Close();

                Console.WriteLine("Calling {0}() Method...", settings.ServiceMethod);
                // Retrieve the result from the web service
                var fibonacciResponse = fibonacciRequest.GetResponse();
                using (var fibonacciRespStream = fibonacciResponse.GetResponseStream())
                {
                    if (fibonacciRespStream == null)
                        throw new ApplicationException(string.Format("No response was returned from {0}", settings.ServiceMethod));
                    using (var fibonacciReader = new StreamReader(fibonacciRespStream))
                    {
                        // Get the JSON string
                        string fibonacciJson = fibonacciReader.ReadToEnd();
                        // Console.WriteLine("Raw Results: {0}", fibonacciJson);
                        // Remove escape characters and outer quotes so the sting is deserializable
                        fibonacciJson = fibonacciJson.Substring(1, fibonacciJson.Length - 2).Replace(@"\", "");
                        // Console.WriteLine("JSON: {0}", fibonacciJson);
                        // Deserialize the JSON string
                        var fibonacciResult = new JavaScriptSerializer().Deserialize<ServiceResult>(fibonacciJson);
                        if (fibonacciResult.Errors != null)
                            Console.WriteLine("The recusrsive calculation resulted in an error: {0}", fibonacciResult.Errors);
                        else Console.WriteLine("{0}Using the recursive formula: {1}{0}Calculated that f({2}) = {3}{0}In {4} iterations{0}{0}Press <ENTER> to continue",
                            Environment.NewLine, fibonacciResult.Data.Formula, number, fibonacciResult.Data.Value, fibonacciResult.Data.Iterations);
                        Console.ReadLine();
                    }
                }

                // Update the request object
                request.UseRecursiveAlgorithm = false;
                // Update the Fibonacci request
                fibonacciRequest = (HttpWebRequest)WebRequest.Create(fibonacciUrl);
                // Set headers
                fibonacciRequest.Method = "POST";
                fibonacciRequest.ContentLength = 0;
                fibonacciRequest.ContentType = "application/json";
                // Stream the parameters for the request
                fibonacciBytes = Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(request));
                fibonacciRequest.ContentLength = fibonacciBytes.Length;
                fibonacciReqStream = fibonacciRequest.GetRequestStream();
                fibonacciReqStream.Write(fibonacciBytes, 0, fibonacciBytes.Length);
                fibonacciReqStream.Close();

                Console.WriteLine("Calling {0}() Method...", settings.ServiceMethod);
                // Retrieve the result from the web service
                fibonacciResponse = fibonacciRequest.GetResponse();
                using (var fibonacciRespStream = fibonacciResponse.GetResponseStream())
                {
                    if (fibonacciRespStream == null)
                        throw new ApplicationException(string.Format("No response was returned from {0}", settings.ServiceMethod));
                    using (var fibonacciReader = new StreamReader(fibonacciRespStream))
                    {
                        // Get the JSON string
                        string fibonacciJson = fibonacciReader.ReadToEnd();
                        // Console.WriteLine("Raw Results: {0}", fibonacciJson);
                        // Remove escape characters and outer quotes so the sting is deserializable
                        fibonacciJson = fibonacciJson.Substring(1, fibonacciJson.Length - 2).Replace(@"\", "");
                        // Console.WriteLine("JSON: {0}", fibonacciJson);
                        // Deserialize the JSON string
                        var fibonacciResult = new JavaScriptSerializer().Deserialize<ServiceResult>(fibonacciJson);
                        if (fibonacciResult.Errors != null)
                            Console.WriteLine("The non-recusrsive calculation resulted in an error: {0}", fibonacciResult.Errors);
                        else Console.WriteLine("{0}Using the non-recursive formula: {1}{0}Calculated that f({2}) = {3}{0}In {4} iterations{0}",
                            Environment.NewLine, fibonacciResult.Data.Formula, number, fibonacciResult.Data.Value, fibonacciResult.Data.Iterations);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException("General", ex);
            }
            finally
            {
                Console.WriteLine("{0}Done! Press any key to exit...", Environment.NewLine);
                // Ensure the console doesn't close until we're done
                Console.ReadKey();
            }
        }

        static void HandleException(string exceptionLabel, Exception ex)
        {
            int innerCount = 0;
            while (ex != null)
            {
                string exceptionData = string.Format("{0}{0}{1} Exception{2}: {3}{0}{0}Stack Trace:{0}{4}",
                    Environment.NewLine,
                    innerCount == 0 ? exceptionLabel : "Inner",
                    innerCount == 0 ? "" : string.Format(" {0}", innerCount),
                    ex.Message,
                    ex.StackTrace);
                Console.WriteLine(exceptionData);
                ex = ex.InnerException;
                innerCount++;
            }
        }
    }

    // These are required because we are not working with a proxy class or service reference
    // This allows our program to serialize and deserialize the JSON data
    #region Data Types
    public class ServiceRequest
    {
        public bool UseRecursiveAlgorithm { get; set; }

        public int Number { get; set; }

        public ServiceRequest(bool useRecursiveAlgorithm, int number)
        {
            UseRecursiveAlgorithm = useRecursiveAlgorithm;
            Number = number;
        }

        public ServiceRequest() { }
    }

    public class ServiceResult
    {
        public string Errors { get; set; }

        public ServiceValue Data { get; set; }

        public ServiceResult(string errors, ServiceValue data)
        {
            Errors = errors;
            Data = data;
        }

        // Default constructor required to deserialize
        public ServiceResult() { }
    }

    public class ServiceValue
    {
        public string Formula { get; set; }

        public int Value { get; set; }

        public int Iterations { get; set; }

        public ServiceValue(string formula, int value, int iterations)
        {
            Formula = formula;
            Value = value;
            Iterations = iterations;
        }

        // Default constructor required to deserialize
        public ServiceValue() { }
    }
    #endregion
}
