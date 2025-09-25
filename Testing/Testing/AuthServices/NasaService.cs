namespace Testing.AuthServices
{
    public class NasaService
    {
        private readonly HttpClient _httpClient;
        public NasaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private async Task LoginAsync()
        {
            var loginContent = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("identity", "ahmedzaher75802004@gmail.com"),
            new KeyValuePair<string, string>("password", "ahmedTAREKZAHER2004******")
        });

            var response = await _httpClient.PostAsync("https://www.space-track.org/ajaxauth/login", loginContent,default);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetSatellitesAsync()
        {
            await LoginAsync();

            var url = "https://www.space-track.org/basicspacedata/query/class/tle_latest/ORDINAL/1/perigee/<200/format/json";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}
