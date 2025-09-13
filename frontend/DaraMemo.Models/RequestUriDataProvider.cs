using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.Api {
    public class RequestUriDataProvider {
        public string BaseAddress { get; init; } = "http://localhost:5000";
        public string StatusCurrentEndpoint { get; init; } = "/api/status/current";
        public string StatusSetEndpoint { get; init; } = "/api/status/set";
        public string StatusResetEndpoint { get; init; } = "/api/status/reset";
        public string StatusKillEndpoint { get; init; } = "/api/status/kill";
        public string StatusRecordEndpoint { get; init; } = "/api/status/record";
    }
}
