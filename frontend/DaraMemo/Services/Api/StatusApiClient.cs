using DaraMemo.Models.Dtos;
using System.Net.Http;
using System.Net.Http.Json;

namespace DaraMemo.Services.Api {

    public class StatusApiClient {
        private readonly HttpClient _httpClient;
        public StatusApiClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        // /api/status/current
        public async Task<StatusCurrentResponse> GetCurrentStatusAsync() {
            try {
                var state = await _httpClient.GetStringAsync(RequestUriData.BuildUri(RequestUriData.StatusCurrentEndpoint));
            return new StatusCurrentResponse { Value = state.Replace("\"", string.Empty) };
            } catch (Exception) {
                throw;
            }
        }

        // /api/status/set
        public async Task<SimpleOkResponse> SetBreakStatusAsync() {
            try {
                var result = await _httpClient.GetStringAsync(RequestUriData.BuildUri(RequestUriData.StatusSetEndpoint));
                return new SimpleOkResponse { Value = result.Replace("\"", string.Empty) };
            } catch (Exception) {
                throw;
            }
        }

        // /api/status/record
        public async Task<StatusRecordResponse> GetStatusRecordAsync() {
            try {
                var response = await _httpClient.GetFromJsonAsync<StatusRecordResponse>(RequestUriData.BuildUri(RequestUriData.StatusRecordEndpoint));
                if (response == null) {
                    throw new InvalidOperationException("The response from /api/status/record was null.");
                }
                return response;
            } catch (Exception) {

                throw;
            }
        }

        // /api/status/reset
        public async Task<SimpleOkResponse> ResetStatusAsync() {
            try {
                var result = await _httpClient.GetStringAsync(RequestUriData.BuildUri(RequestUriData.StatusResetEndpoint));
                return new SimpleOkResponse { Value = result };
            } catch (Exception) {

                throw;
            }
        }

        // /api/status/kill
        public async Task<SimpleOkResponse> KillServerAsync() {
            try {
                var result = await _httpClient.GetStringAsync(RequestUriData.BuildUri(RequestUriData.StatusKillEndpoint));
                return new SimpleOkResponse { Value = result };
            } catch (Exception) {

                throw;
            }
        }
    }

}
