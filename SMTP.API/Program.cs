using Microsoft.EntityFrameworkCore;
using SMTP.API.DataContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// -------------x-------X ------SWAGGER ---------X---------------------------------------
builder.Services.AddSwaggerGen();

// -------------x-------X ------CONEXAO BANCO----X---------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// -------------x-------X ------MEDIATR-----X---------------------------------------
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);


// -------------x-------X ------Cors-----X---------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarTudo", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Origens do seu frontend
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Permite envio de credenciais/cookies
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // -------------x-------X ------SWAGGER-----X---------------------------------------
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("LiberarTudo");

app.UseAuthorization();

app.MapControllers();

app.Run();
