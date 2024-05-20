using ExtractDataFromCSV.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ServiceExtension.UpdateAppSettingsWithDevelopmentValuesService("ConnectionStrings");

var connectionString = builder.Configuration.GetConnectionString("Databasename01");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IInformationExpert, SInformationExpert>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    var isConnected = context.Database.CanConnect();

    if (isConnected)
    {
        Console.WriteLine("Connected to database");
    }
    else
    {
        Console.WriteLine("Could not connect to database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();