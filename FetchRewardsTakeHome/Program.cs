using FetchRewardsTakeHome.Business.Repositories.Payer;
using FetchRewardsTakeHome.Business.Repositories.Points;
using FetchRewardsTakeHome.Business.Services;
using FetchRewardsTakeHome.Data;

var builder = WebApplication.CreateBuilder(args);

//Dependency Injection
builder.Services.AddSingleton<FakeDatabase>();

//Repositories
builder.Services.AddScoped<IPayerRepository, PayerRepository>();
builder.Services.AddScoped<IPointsRepository, PointsRepository>();

//Services
builder.Services.AddTransient<IPayerService, PayerService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();