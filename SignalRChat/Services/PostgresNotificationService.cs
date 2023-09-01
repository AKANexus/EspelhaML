using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Npgsql;
using SignalRChat.Hubs;

namespace SignalRChat.Services
{
    public class PostgresNotificationService
    {
        private readonly NpgsqlConnectionStringBuilder _csb;

        private readonly IHubContext<ChatHub> _chatHub;


        public PostgresNotificationService(IServiceProvider provider, IHubContext<ChatHub> chatHubContext, IConfiguration configuration)
        {
            _chatHub = chatHubContext;
            _csb = new NpgsqlConnectionStringBuilder
            {
                Database = "meliEspelho",
                Port = 5351,
                Username = "meliDBA",
                Password = configuration["SuperSecretSettings:NpgPassword"],
#if DEBUG
                Host = "192.168.10.215"
#else
                Host = "tinformatica.dyndns.org"
#endif
            };
        }

        public async Task MonitorForNotification()
        {
            await using var conn = new NpgsqlConnection(_csb.ConnectionString);
            await conn.OpenAsync();

            //e.Payload is string representation of JSON we constructed in NotifyOnDataChange() function
            conn.Notification += async (_, e) =>
            {
                await _chatHub.Clients.All.SendAsync("PostNotification", "Server", e.Payload);
            };

            await using (var cmd = new NpgsqlCommand("LISTEN dbnotification;", conn))
                cmd.ExecuteNonQuery();

            while (true)
                conn.Wait(); // wait for events

            Debug.WriteLine("Ended");
        }
    }
}
