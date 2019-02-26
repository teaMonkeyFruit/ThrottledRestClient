using System;
using System.Reflection;
using RestSharp;
using RestSharp.Serialization.Json;

namespace TeaMonkeyThrottledRestClient
{
    public interface IThrottledRestClient
    {
        ThrottledResponse Get(string uri);
        ThrottledResponse Post(string uri, object body);
        ThrottledResponse Patch(string uri, object body);
    }

    public class ThrottledRestClient : IThrottledRestClient
    {
        private int _requestCount = 0;
        private EasyStopwatch _easyStopWatch = new EasyStopwatch();

        public int MaxRequestsPerSecond { get; set; } = 5;

        public ThrottledRestClient()
        {
            _easyStopWatch.Start();
        }

        public ThrottledResponse Get(string uri)
        {
            RequestRateThrottle();

            var client = new RestClient(uri);

            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage);

            return new ThrottledResponse(response);
        }

        public ThrottledResponse Post(string uri, object body)
        {
            RequestRateThrottle();

            var client = new RestClient(uri);

            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };

            request.AddBody(body);

            IRestResponse response = client.Execute(request);
            
            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage);

            return new ThrottledResponse(response);
        }
        
        public ThrottledResponse Patch(string uri, object body)
        {
            RequestRateThrottle();

            var client = new RestClient(uri);

            var request = new RestRequest(Method.PATCH) { RequestFormat = DataFormat.Json };

            request.AddBody(body);

            IRestResponse response = client.Execute(request);
            
            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage);
            
            return new ThrottledResponse(response);
        }

        private void RequestRateThrottle()
        {

            if (_requestCount < MaxRequestsPerSecond)
            {
                _requestCount++;
                return;
            }

            if (_requestCount > MaxRequestsPerSecond)
                throw new Exception($"Requests should never be greater than {MaxRequestsPerSecond}");

            while (_easyStopWatch.SecondsElapsed() < 1)
            {
                System.Threading.Thread.Sleep(100);
            }
            
            _requestCount = 1;
            _easyStopWatch.Restart();
        }

    }

    public class ThrottledResponse
    {
        public IRestResponse Response { get; }

        public ThrottledResponse(IRestResponse response)
        {
            Response = response;
        }
        
        public T AsObject<T>()
        { 

            var record = new JsonDeserializer().Deserialize<T>(Response);

            return record;
        }
    }
   
}
