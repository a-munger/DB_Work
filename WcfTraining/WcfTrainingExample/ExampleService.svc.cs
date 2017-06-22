using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace WcfTrainingExample
{
    public class ExampleService : IExampleService
    {
        #region Globals
        private int _iterationCounter;
        private Dictionary<int, int> _calculatedFibonaccis;
        #endregion

        #region Simple Service Test Methods
        /// <summary>
        /// Uses GET method from HTML/Javascript client to access the service test method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string TestServiceWebGet(string data)
        {
            // Since all three methods perform the same operation pass call along to internal method
            return new JavaScriptSerializer().Serialize(TestService(data));
        }

        /// <summary>
        /// Uses POST method from HTML/Javascript client to access the service test method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string TestServiceWebPost(string data)
        {
            // Since all three methods perform the same operation pass call along to internal method
            return new JavaScriptSerializer().Serialize(TestService(data));
        }

        /// <summary>
        /// Access from a .NET client application
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string TestService(string data)
        {
            return string.Format("The service is running and received data: [{0}]", data).Replace(Environment.NewLine, "");
        }
        #endregion

        #region Service Test Methods with Data Contract
        public string FibonacciWebGet(string number, string useRecursiveAlgorithm)
        {
            // Convert the parameters to the request object and pass
            int integer;
            if (!int.TryParse(number, out integer)) integer = 0;
            var request = new FibonacciRequest(integer, useRecursiveAlgorithm.ToLower() == "true");
            return FibonacciWebPost(request);
        }

        public string FibonacciWebPost(FibonacciRequest request)
        {
            // Return a JSON version of the result object
            return new JavaScriptSerializer().Serialize(Fibonacci(request));
        }

        public FibonacciResult Fibonacci(FibonacciRequest request)
        {
            _calculatedFibonaccis = new Dictionary<int, int>();
            var result = new FibonacciResult();
            if (request.Number > 40) throw new ApplicationException(string.Format("The submitted value [{0}] is greater than the max value allowed [10]!", request.Number));
            var data = new FibonacciValue();

            try
            {
                data.Formula = request.UseRecursiveAlgorithm
                    ? "f(n) = f(n - 1) + f(n - 2)"
                    : string.Format("f(n) = (phi^n - ( -1^n / phi^n )) / √5{0}[where phi = (1 + √5) / 2]", Environment.NewLine);
                data.Value = request.UseRecursiveAlgorithm
                    ? FibonacciRecursive(request.Number)
                    : FibonacciNonRecursive(request.Number);
                data.Iterations = _iterationCounter;
                result.Data = data;
            }
            catch (Exception ex)
            {
                result = new FibonacciResult
                {
                    Errors = ex.Message
                };
            }

            return result;
        }

        #region Internal Fibonacci Methods
        // High School Algebra Formula: f(n) = f(n-1) + f(n-2)

        // Iteration count = n^(n-1)
        // Just putting the instructins into code
        private int FibonacciRecursive(int number)
        {
            _iterationCounter++;
            if (number < 2) return number;
            return FibonacciRecursive(number - 1) + FibonacciRecursive(number - 2);
        }

        // Iterationcount ~ n^2
        // Recognizing that the formula as-is is expensive (but not really changing it - hence sophomoric)
        private int FibonacciRecursiveBetter(int number)
        {
            _iterationCounter++;
            if (number < 2) return number;
            if (_calculatedFibonaccis.ContainsKey(number)) return _calculatedFibonaccis[number];
            int fibonacciResult = FibonacciRecursiveBetter(number - 1) + FibonacciRecursiveBetter(number - 2);
            _calculatedFibonaccis[number] = fibonacciResult;
            return fibonacciResult;
        }

        // Iteration count = n
        // Understanding that the goal is the output, not a representation of the steps between it and the input
        // First point at which you are "programming"
        private int FibonacciIterative(int number)
        {
            _iterationCounter++;
            if (number < 2) return number;

            int currentResult = 0;
            int lastResult = 1;
            int secondToLastResult = 0;

            for (int i = 2; i <= number; i++)
            {
                currentResult = lastResult + secondToLastResult;
                secondToLastResult = lastResult;
                lastResult = currentResult;
            }
            return currentResult;
        }

        // Iteration count = 1
        // Truly understanding where in your code the costs occur and optimizing
        private int FibonacciNonRecursive(int number)
        {
            _iterationCounter++;
            if (number < 2) return number;
            double phi = (1 + Math.Sqrt(5)) / 2;
            double fibonacci = (Math.Pow(phi, number) - Math.Pow(-1, number) / Math.Pow(phi, number)) / Math.Sqrt(5);
            return (int)fibonacci;
        }
        #endregion

        #endregion
    }
}
