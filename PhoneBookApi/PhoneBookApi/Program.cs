using Microsoft.OpenApi.Models;
using PhoneBookApi.Extensions;
using PhoneBookApi.Repositories;
using PhoneBookApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.AddDbConnection(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper();
builder.Services.AddRedisConnection(builder.Configuration);
builder.Services.AddSingleton<KafkaService>();
builder.Services.AddScoped<FakeDataGenerator>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "PhoneBook API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");
using (var scope = app.Services.CreateScope())
{
    var dataGenerator = scope.ServiceProvider.GetRequiredService<FakeDataGenerator>();
    dataGenerator.EnsureData();
}

app.Run();