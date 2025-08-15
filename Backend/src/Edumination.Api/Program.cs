using Edumination.Api.Common.Extensions;

var b = WebApplication.CreateBuilder(args);
b.Services.AddControllers();
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();

// App services
b.Services.AddAppPersistence(b.Configuration)
          .AddJwtAuth(b.Configuration)
          .AddFeatureServices();

var app = b.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
