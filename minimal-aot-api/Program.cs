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

app.Run();
