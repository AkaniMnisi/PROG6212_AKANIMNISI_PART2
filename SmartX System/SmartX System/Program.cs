using SmartX.Shared.Models;
using SmartX_System.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TelemetryService>();


builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
//app.UseHttpsRedirection();

app.MapGet("/api/menu", () => new[] {
    "Sensor Data Ingestion and Telemetry",
    "Real-Time Command Stream (Disabled)",
    "Network Topology (Disabled)"
});


app.MapPost("/api/telemetry/ingest", (TelemetryPacket<object> packet, TelemetryService service) =>
{
    try
    {
        service.ProcessPacket(packet);
        return Results.Ok(new { Message = "Packet received successfully", Timestamp = DateTime.UtcNow });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
});


app.MapPost("/api/telemetry/upload", async (IFormFile file, string sensorMac, TelemetryService service) =>
{
    if (file == null || file.Length == 0) return Results.BadRequest("File is empty.");

    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

    var filePath = Path.Combine(uploadsFolder, file.FileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    service.AttachLogToSensor(sensorMac, filePath);
    return Results.Ok(new { Message = "File uploaded successfully", Path = filePath });
}).DisableAntiforgery();

app.Run();