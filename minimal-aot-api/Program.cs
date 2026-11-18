var builder = WebApplication.CreateSlimBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.


app.MapGet("/", () => "Hello AOT world!");

app.Run();
