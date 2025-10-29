using Newtonsoft.Json;
using System.Text;

namespace LibraryManagement.Services
{
    public class FirebaseService
    {
        // REPLACE WITH YOUR FIREBASE URL
        private readonly string _firebaseUrl = "https://library-management-1b490-default-rtdb.asia-southeast1.firebasedatabase.app:/";
        private readonly HttpClient _httpClient;

        public FirebaseService()
        {
            _httpClient = new HttpClient();
        }

        // Get single item
        public async Task<T?> GetAsync<T>(string path)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_firebaseUrl}{path}.json");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(json) || json == "null")
                    return default(T);
                    
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        // Get all items
        public async Task<Dictionary<string, T>> GetAllAsync<T>(string path)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_firebaseUrl}{path}.json");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(json) || json == "null")
                    return new Dictionary<string, T>();
                    
                return JsonConvert.DeserializeObject<Dictionary<string, T>>(json) ?? new Dictionary<string, T>();
            }
            catch
            {
                return new Dictionary<string, T>();
            }
        }

        // Post new item
        public async Task<string> PostAsync<T>(string path, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_firebaseUrl}{path}.json", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return obj?["name"] ?? "";
            }
            catch
            {
                return "";
            }
        }

        // Update item
        public async Task<bool> PutAsync<T>(string path, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_firebaseUrl}{path}.json", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Delete item
        public async Task<bool> DeleteAsync(string path)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_firebaseUrl}{path}.json");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}