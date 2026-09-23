var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseRouting();


app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/info", () =>
{
    return Results.Ok(new
    {
        MachineName = Environment.MachineName,
        ServerTime = DateTime.Now,
        Message = "Hello from Cloud"
    });
});

app.MapPost("/api/stress", (int? durationSeconds) =>
{
    int seconds = durationSeconds ?? 5;

    if (seconds > 30)
    {
        seconds = 30;
    }
    else if (seconds < 1)
    {
        seconds = 1;
    }

    var stopWatch = System.Diagnostics.Stopwatch.StartNew();

    while (stopWatch.Elapsed.TotalSeconds < seconds)
    {
        double x = 0.0001;
        for (int i = 0; i < 1_000_000; i++)
        {
            x += Math.Sqrt(i) * Math.Sin(i);
        }
    }

    stopWatch.Stop();
    return Results.Ok(new
    {
        MachineName = Environment.MachineName,
        Status = "Stress test finished",
        durationSeconds = seconds
    });
});

app.Run();
