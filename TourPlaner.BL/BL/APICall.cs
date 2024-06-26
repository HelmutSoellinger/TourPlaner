using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;


//Be sure to run "Install-Package Microsoft.Net.Http" from your nuget command line.
namespace TourPlaner.BL
{
    public class APICall
    {
        async public static Task<string> Call(string _start, string _finish) {
            var key = "5b3ce3597851110001cf62486dce1da273c04c438ea5462d4041e9a2";
            var baseAddress = new Uri("https://api.openrouteservice.org");

            using var httpClient = new HttpClient{BaseAddress = baseAddress};
            
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", key);

            float[] start = await Translate(_start);
            float[] finish = await Translate(_finish);
            if (start==null || finish==null)
            {
                Debug.WriteLine("Route not Found!");
                return "";
            }
            string route = $"{{\"coordinates\":[[{start[0].ToString(CultureInfo.InvariantCulture)},{start[1].ToString(CultureInfo.InvariantCulture)}],[{finish[0].ToString(CultureInfo.InvariantCulture)},{finish[1].ToString(CultureInfo.InvariantCulture)}]]}}";
            using var content = new StringContent(route, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync("/v2/directions/driving-car/geojson", content);
            
            string responseData = await response.Content.ReadAsStringAsync();
            string filename = _start + _finish + DateTime.Now.ToString("yyyyMMddHHmmss") +".json";
            File.WriteAllText($@"./Resources/{filename}", responseData);
            return filename;

        }
        async private static Task<float[]> Translate(string adr)
        {
            var key = "5b3ce3597851110001cf62486dce1da273c04c438ea5462d4041e9a2";
            var baseAddress = new Uri($"https://api.openrouteservice.org/geocode/search?api_key={key}&text={adr.Replace(" ","%20")}");
            using var httpClient = new HttpClient { BaseAddress = baseAddress };
            
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");

            using var response = await httpClient.GetAsync(baseAddress);
                
            string responseData = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<dynamic>(responseData);
            try
            {
                float a = data.features[0].geometry.coordinates[0];
                float b = data.features[0].geometry.coordinates[1];
                return [a, b];
            }
            catch
            {
                Debug.WriteLine("Adress not found!");
                return null;
            }
        }
    }
}
