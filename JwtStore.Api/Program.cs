using JwtStore.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.AddConfiguration();
builder.AddDataBase();
builder.AddJwtAuthentication();
builder.Services.AddOpenApi();


builder.AddAccountContext();

builder.AddMediator();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.AddAccountEndpoints();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
