
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


    }
}
