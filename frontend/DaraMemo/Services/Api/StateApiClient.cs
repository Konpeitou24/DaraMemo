using DaraMemo.Models.State;
using DaraMemo.Models.Timeline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.Api {
    public class StateApiClient {
        private readonly HttpClient _httpClient;

        public StateApiClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<CurrentStateResponse> GetCurrentStateAsync(string sessionId) {
            var response = await _httpClient.GetAsync($"/api/state/current?session_id={sessionId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CurrentStateResponse>(json);
            if (result == null) {
                throw new InvalidOperationException("Failed to deserialize CurrentStateResponse.");
            }
            return result;
        }

        public async Task<SetStateResponse> SetStateAsync(SetStateRequest request) {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/states/set", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SetStateResponse>(json);
            if (result == null) {
                throw new InvalidOperationException("Failed to deserialize SetStateResponse.");
            }
            return result;
        }

        public async Task<TimelineResponse> GetTimelineAsync(string sessionId) {

            var response = await _httpClient.GetAsync($"/api/sessions/timeline?session_id={sessionId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TimelineResponse>(json);
            if (result == null) {
                throw new InvalidOperationException("Failed to deserialize TimelineResponse.");
            }
            return result;
        }
    }
}
