using TubeAppBackend.Services;
using TubeAppBackend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITypeService, TypeService>();
builder.Services.AddScoped<INomenclatureService, NomenclatureService>();
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IRemnantService, RemnantService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IPriceCalculationService, PriceCalculationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
