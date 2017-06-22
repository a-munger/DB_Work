using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfTrainingExample
{
    // The service contract defines the methods (operations) that will be exposed to the outside world
    [ServiceContract]
    public interface IExampleService
    {
        #region Simple Service Test Methods
        // This operation will be accessed over HTTP using the GET method
        // Use this to test directly from the browser
        // Example call = http://<HOST>/ExampleService.svc/Web/TestServiceGet/Test%20Data
        [OperationContract]
        [WebGet(UriTemplate = "/TestServiceGet/{data}", ResponseFormat = WebMessageFormat.Json)]
        string TestServiceWebGet(string data);

        // This operation will be accessed over HTTP using the GET method
        // Use this to test from a JavaScript/jQuery AJAX call
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/TestServicePost", ResponseFormat = WebMessageFormat.Json)]
        string TestServiceWebPost(string data);

        // This operation will be accessed using a client application
        // Used with a service reference or a proxy class
        [OperationContract]
        string TestService(string data);
        #endregion

        #region Service Test Methods with Data Contract
        // This operation will be accessed over HTTP using the GET method
        // Use this to test directly from the browser
        // Example call = http://<HOST>/ExampleService.svc/Web/FibonacciGet/3/false
        [OperationContract]
        [WebGet(UriTemplate = "/FibonacciGet/{number}/{useRecursiveAlgorithm}", ResponseFormat = WebMessageFormat.Json)]
        string FibonacciWebGet(string number, string useRecursiveAlgorithm);

        // This operation will be accessed over HTTP using the GET method
        // Use this to test from a JavaScript/jQuery AJAX call
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FibonacciPost", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string FibonacciWebPost(FibonacciRequest request);

        // This operation will be accessed using a client application
        // Used with a service reference or a proxy class
        [OperationContract]
        FibonacciResult Fibonacci(FibonacciRequest request);
        #endregion
    }

    // Data contracts define complex data types upon which the service and its clients agree
    #region Data Contracts
    [DataContract]
    public class FibonacciRequest
    {
        public FibonacciRequest()
        {
            UseRecursiveAlgorithm = false;
            Number = 0;
        }

        public FibonacciRequest(int number, bool useRecursiveAlgorithm = false)
        {
            UseRecursiveAlgorithm = useRecursiveAlgorithm;
            Number = number;
        }

        [DataMember]
        public bool UseRecursiveAlgorithm { get; set; }

        [DataMember]
        public int Number { get; set; }
    }

    [DataContract]
    public class FibonacciResult
    {
        [DataMember]
        public string Errors { get; set; }

        [DataMember]
        public FibonacciValue Data { get; set; }
    }

    [DataContract]
    public class FibonacciValue
    {
        [DataMember]
        public string Formula { get; set; }

        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public int Iterations { get; set; }
    }
    #endregion
}
