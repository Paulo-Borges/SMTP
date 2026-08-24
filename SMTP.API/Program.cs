using Microsoft.EntityFrameworkCore;
using SMTP.API.DataContext;
using SMTP.API.SendEmail;

//var outlook = new Email("smtp.office365.com", "borgespaulo72@yahoo.com.br", "");

//outlook.SendEmail(
//    emailsTo: new List<string>
//{
//    "borgespaulo08@gmail.com"
//},
//subject: "Teste",
//body: "Degue Anexo",
//attachments: new List<string>
//{
//    @"C:/Users/USER/Downloads/Usuarios.xlsx"
//});

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


// Registrar serviço de email com injeção de dependência
builder.Services.AddScoped<IEmailService, Email>();

// Configure CORS - Liberar Tudo
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarTudo", policy =>
    {
        policy.AllowAnyOrigin()        // Permite qualquer origem
              .AllowAnyMethod()         // Permite qualquer método HTTP (GET, POST, PUT, DELETE, etc)
              .AllowAnyHeader();        // Permite qualquer header
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
