using System.Text.Json;

namespace Routing.gRPC.GoogleServices
{
    public class GoogleMapsService(IHttpClientFactory _httpClientFactory, IConfiguration _configuration) 
    {
        private readonly string _apiKey = _configuration["GoogleMaps:ApiKey"];


        private string BuildUrl(string endpoint, params (string key, string value)[] queryParams)
        {
            var uriBuilder = new UriBuilder(endpoint);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var param in queryParams)
            {
                query[param.key] = param.value;
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        public async Task<(double distanceInKm, TimeSpan estimatedTime)> GetRouteInfoAsync(string origin, string destination, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var url = BuildUrl(
            "https://maps.googleapis.com/maps/api/directions/json",
            ("origin", origin),
            ("destination", destination),
            ("key", _apiKey)
            );


            var httpResponse = await httpClient.GetAsync(url, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var json = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var directions = JsonDocument.Parse(json);

            var route = directions.RootElement.GetProperty("routes")[0];
            var leg = route.GetProperty("legs")[0];

            var distanceInMeters = leg.GetProperty("distance").GetProperty("value").GetDouble();
            var durationInSeconds = leg.GetProperty("duration").GetProperty("value").GetDouble();

            double distanceInKm = distanceInMeters / 1000.0;
            TimeSpan estimatedTime = TimeSpan.FromSeconds(durationInSeconds);

            return (distanceInKm, estimatedTime);
        }


     


       
    }
}

