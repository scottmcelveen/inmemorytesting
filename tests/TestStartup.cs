using inmemorytesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureDatabases(IServiceCollection services)
        {
            
        }
    }
}