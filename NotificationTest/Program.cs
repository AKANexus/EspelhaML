using Npgsql;
using System.Data;

NpgsqlConnectionStringBuilder csb = new()
{
    Database = "meliEspelho",
    Port = 5351,
    Username = "meliDBA",
    Password = "Au9@449qOUuq",
#if DEBUG
    Host = "192.168.10.215"
#else
    Host = "tinformatica.dyndns.org"
#endif

};

await using var conn = new NpgsqlConnection(csb.ConnectionString);
await conn.OpenAsync();

//e.Payload is string representation of JSON we constructed in NotifyOnDataChange() function
conn.Notification += (o, e) => Console.WriteLine("Received notification: " + e.Payload);

await using (var cmd = new NpgsqlCommand("LISTEN dbnotification;", conn))
    cmd.ExecuteNonQuery();

while (true)
    conn.Wait(); // wait for events