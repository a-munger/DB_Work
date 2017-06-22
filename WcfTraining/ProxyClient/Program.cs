using System;
using System.Configuration;
using System.ServiceModel;
using WcfTrainingExample;
// WcfTrainingExample  is a proxy class for the service.
// To create it, I did the following
// Save the service WSDL: http://localhost:30659/ExampleService.svc?singleWsdl as C:\Temp\WcfTrainingExample.wsdl
// Generate a proxy class from it:
// 1. Launch the Developer Command Prompt
//    START > All Programs > Visual Studio 2013 > Visual Studio Tools > Developer Command Prompt for VS2013
// 2. Run the SVCUTIL command
//    svcutil /l:cs /o:"C:\Temp\WcfTrainingExample.cs" "C:\Temp\WcfTrainingExample.wsdl"
// 3. Include the resulting class WcfTrainingExample.cs in the project

namespace ProxyClient
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

                // Here, we create the endpoint using ChannelFactory
                var binding = new BasicHttpBinding();
                if (settings.UseSsl) binding.Security.Mode = BasicHttpSecurityMode.Transport;
                var address = new EndpointAddress(settings.ServiceUrl);
                var factory = new ChannelFactory<IExampleService>(binding, address);
                var channel = factory.CreateChannel();

                // Execute the test method
                Console.WriteLine("Set test data = [{0}]", data);
                Console.WriteLine("Calling TestService() Method...");
                Console.WriteLine(channel.TestService(data));

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
                var request = new FibonacciRequest
                {
                    Number = number,
                    UseRecursiveAlgorithm = true
                };

                // Return a result data contract object by calling the operation
                var result = channel.Fibonacci(request);
                if (result.Errors != null)
                    Console.WriteLine("The recusrsive calculation resulted in an error: {0}", result.Errors);
                else Console.WriteLine("{0}Using the recursive formula: {1}{0}Calculated that f({2}) = {3}{0}In {4} iterations{0}{0}Press <ENTER> to continue",
                    Environment.NewLine, result.Data.Formula, number, result.Data.Value, result.Data.Iterations);

                Console.ReadLine();

                // Call the operation again
                request.UseRecursiveAlgorithm = false;
                result = channel.Fibonacci(request);
                if (result.Errors != null)
                    Console.WriteLine("The non-recusrsive calculation resulted in an error: {0}", result.Errors);
                else Console.WriteLine("Using the non-recursive formula: {1}{0}Calculated that f({2}) = {3}{0}In {4} iterations",
                    Environment.NewLine, result.Data.Formula, number, result.Data.Value, result.Data.Iterations);

                // Close the endpoint
                factory.Close();
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
}
