using ApiTicketAWS.Services;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddHttpClient("Jira").ConfigurePrimaryHttpMessageHandler(()=> {
    var httpClientHandler = new HttpClientHandler()
    {
        SslProtocols = SslProtocols.None
       // ClientCertificateOptions = ClientCertificateOption.Manual
    };

    httpClientHandler.ServerCertificateCustomValidationCallback = OnServerCertificateValidation;

    return httpClientHandler;
});

bool OnServerCertificateValidation(HttpRequestMessage arg1, X509Certificate2? arg2, X509Chain? arg3, SslPolicyErrors arg4)
{
    return true;
}

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseHsts();
app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
