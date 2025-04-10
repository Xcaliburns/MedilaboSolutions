using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gateway.Events
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly ILogger<CustomJwtBearerEvents> _logger;

        public CustomJwtBearerEvents(ILogger<CustomJwtBearerEvents> logger)
        {
            _logger = logger;
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            var cookie = context.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(cookie))
            {
                context.Token = cookie;
            }
            return Task.CompletedTask;
        }
    }
}
