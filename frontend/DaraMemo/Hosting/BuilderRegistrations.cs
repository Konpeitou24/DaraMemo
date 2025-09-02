using DaraMemo.Services.WebSocket;
using DaraMemo.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Hosting {
    public static class BuilderRegistrations {
        public static IHost SuperBuild(this IHostBuilder builder) {
            return builder.ConfigureServices(ServiceRegistrations.ConfigureServices)
                .Build();
        }

    }
}
