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
using System.Collections.ObjectModel;


//Be sure to run "Install-Package Microsoft.Net.Http" from your nuget command line.
namespace TourPlaner.BL
{
    public class APICall
    {
        async public static Task<string> Call(string _start, string _finish) {
            var key = "";
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
            var key = "";
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
        async public static Task<AutoCompleteObject> Complete(string adr)
        {
            var completion = new AutoCompleteObject();
            var key = "";
            var baseAddress = new Uri($"https://api.openrouteservice.org/geocode/autocomplete?api_key={key}&text={adr.Replace(" ", "%20")}");
            using var httpClient = new HttpClient { BaseAddress = baseAddress };

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");

            using var response = await httpClient.GetAsync(baseAddress);

            string responseData = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<dynamic>(responseData);
            try
            {
                foreach( var feature in data.features)
                {
                    string label = feature.properties.label;
                    float a = feature.geometry.coordinates[0];
                    float b = feature.geometry.coordinates[1];
                    Tuple<float, float> coordinates = new Tuple<float, float>(a, b);
                    completion.Add(label,coordinates);
                }
            }
            catch
            {
                Debug.WriteLine("Adress not found!");
                return completion;
            }

            return completion;
        }
    }
    public class AutoCompleteObject
    {
        public List<Tuple<string, Tuple<float, float>>> Completion { get; set; } = new();
        public void Add(string completion, Tuple<float,float> coordinates)
        {
            Completion.Add(new Tuple<string, Tuple<float, float>>(completion, coordinates));
        }
        public ObservableCollection<string> GetObservable()
        {
            var Observable = new ObservableCollection<string>();
            foreach (var item in Completion)
            {
                Observable.Add(item.Item1);
            }
            return Observable;
        }
    }
}


