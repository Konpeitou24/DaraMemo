using DaraMemo.Models.Dtos;
using System.Net.Http;
using System.Net.Http.Json;

namespace DaraMemo.Services.Api {

    public class StateApiClient {
        private readonly HttpClient _httpClient;

        public StateApiClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        // /api/status/current
        public async Task<StatusCurrentResponse> GetCurrentStatusAsync() {
            var state = await _httpClient.GetStringAsync("/api/status/current");
            return new StatusCurrentResponse { State = state };
        }

        // /api/status/set
        public async Task<SimpleOkResponse> SetBreakStatusAsync() {
            var result = await _httpClient.GetStringAsync("/api/status/set");
            return new SimpleOkResponse { Result = result };
        }

        // /api/status/record
        public async Task<StatusRecordResponse> GetStatusRecordAsync() {
            var response = await _httpClient.GetFromJsonAsync<StatusRecordResponse>("/api/status/record");
            if (response == null) {
                throw new InvalidOperationException("The response from /api/status/record was null.");
            }
            return response;
        }

        // /api/status/reset
        public async Task<SimpleOkResponse> ResetStatusAsync() {
            var result = await _httpClient.GetStringAsync("/api/status/reset");
            return new SimpleOkResponse { Result = result };
        }

        // /api/status/kill
        public async Task<SimpleOkResponse> KillServerAsync() {
            var result = await _httpClient.GetStringAsync("/api/status/kill");
            return new SimpleOkResponse { Result = result };
        }
    }

}
