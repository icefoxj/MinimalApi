var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Cars endpoint

app.MapGet("/api/cars", () =>
{
    var car1 = new Car
    {
        TeamName = "Team A"
    };
    var car2 = new Car
    {
        TeamName = "Team B"
    };

    var cars = new List<Car>
    {
        car1,
        car2
    };

    return cars;
}).WithName("GetCars")
.WithTags("Cars");

app.MapGet("/api/cars/{id}", (int id) =>
{
    var car1 = new Car
    {
        TeamName = "Team A",
        Id = id
    };

    return car1;
}).WithName("GetCar")
.WithTags("Cars");

app.MapPost("/api/cars", (Car car) =>
{
    return car;
}).WithName("CreateCar")
.WithTags("Cars");

app.MapPut("/api/cars/{id}", (Car car) =>
{
    return car;
}).WithName("UpdateCar")
.WithTags("Cars");

app.MapDelete("/api/cars/{id}", (int id) =>
{
    return $"Car with id: {id} was succesfully deteted";
}).WithName("DeleteCar")
.WithTags("Cars");

// Motorbikes endpoints

app.MapGet("/api/motorbikes", () =>
{
    var bike1 = new Motorbike
    {
        TeamName = "Team A"
    };
    var bike2 = new Motorbike
    {
        TeamName = "Team B"
    };

    var bikes = new List<Motorbike>
    {
        bike1,
        bike2
    };

    return bikes;
}).WithName("GetMotorbikes")
.WithTags("Motorbikes");

app.MapGet("/api/motorbikes/{id}", (int id) =>
{
    var bike1 = new Motorbike
    {
        TeamName = "Team A",
        Id = id
    };

    return bike1;
}).WithName("GetMotorbike")
.WithTags("Motorbikes");

app.MapPost("/api/motorbikess", (Motorbike bike) =>
{
    return bike;
}).WithName("CreateMotorbike")
.WithTags("Motorbikes");

app.MapPut("/api/motorbikes/{id}", (Motorbike bike) =>
{
    return bike;
}).WithName("UpdateMotorbike")
.WithTags("Motorbikes");

app.MapDelete("/api/motorbikes/{id}", (int id) =>
{
    return $"Motorbike with id: {id} was succesfully deteted";
}).WithName("DeleteMotorbike")
.WithTags("Motorbikes");

app.Run();

// Models

public record Car
{
    public int Id { get; set; }
    public string TeamName { get; set; }
    public int Speed { get; set; }
    public double MelfunctionChance { get; set; }
    public int MelfunctionsOccured { get; set; }
    public int DistanceCoverdInMiles { get; set; }
    public bool FinishedRace { get; set; }
    public int RacedForHours { get; set; }
}

public record Motorbike
{
    public int Id { get; set; }
    public string TeamName { get; set; }
    public int Speed { get; set; }
    public double MelfunctionChance { get; set; }
    public int MelfunctionsOccured { get; set; }
    public int DistanceCoverdInMiles { get; set; }
    public bool FinishedRace { get; set; }
    public int RacedForHours { get; set; }
}