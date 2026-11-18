using MySqlConnector;

var builder = WebApplication.CreateSlimBuilder(args);

// Add services to the container.

builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("Default")!);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/", () => "Hello AOT world!");

app.MapGet("/db-check", async (MySqlDataSource db) =>
{
    await using var cmd = db.CreateCommand("SELECT VERSION();");
    var version = await cmd.ExecuteScalarAsync();

    return version?.ToString() ?? "No version found";
});

app.MapGet("/export/demo", (HttpResponse response) =>
{
    var csv = "id,name,city\n" +
    "1,Anna,Wien\n" +
    "2,Olli,Graz";

    response.Headers.ContentDisposition = new System.Net.Mime.ContentDisposition
    {
        FileName = "demo_data.csv",
        Inline = false
    }.ToString();

    return Results.Text(csv, "text/csv; charset=utf-8");
});

app.MapGet("/exports/reading", async (MySqlDataSource db, HttpResponse response, CancellationToken ct) =>
{
    response.ContentType = "text/csv; charset=utf-8";
    response.Headers.ContentDisposition = "attachment; filename=\"readings.csv\"";

    await using var cmd = db.CreateCommand("SELECT * FROM reading LIMIT 100");
    await using var reader = await cmd.ExecuteReaderAsync(ct);
    await using var writer = new StreamWriter(response.Body);

    // header row: column names come from query result
    var names = new string[reader.FieldCount];
    for (var i = 0; i < reader.FieldCount; i++) names[i] = reader.GetName(i);
    await writer.WriteLineAsync(string.Join(';', names));

    // data rows
    var values = new object[reader.FieldCount];
    while (await reader.ReadAsync(ct))
    {
        reader.GetValues(values);
        await writer.WriteLineAsync(string.Join(';', values));
        await Task.Delay(100, ct);   // TEMPORARY: simulate a slow export
    }
});

app.Run();
