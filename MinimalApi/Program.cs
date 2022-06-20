#region builder
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RaceDb>(options => options.UseSqlServer(connectionString));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
#endregion

#region app
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
#endregion

#region Cars endpoints

// Get cars
app.MapGet("/api/cars", (RaceDb db) =>
{
    var cars = db.Cars.ToList();
    return Results.Ok(cars);
})
    .WithName("GetCars")
    .WithTags("Cars");

// Get car
app.MapGet("/api/cars/{id}", (int id, RaceDb db) =>
{
    var car = db.Cars.FirstOrDefault(x => x.Id == id);

    if (car == null)
    {
        return Results.NotFound($"Car with id:{id} isn't found");
    }

    return Results.Ok(car);
})
    .WithName("GetCar")
    .WithTags("Cars");

// Create car
app.MapPost("/api/cars", (CarCreateModel carModel, RaceDb db) =>
{
    var newCar = new Car
    {
        TeamName = carModel.TeamName,
        Speed = carModel.Speed,
        MelfunctionChance = carModel.MelfunctionChance
    };

    db.Cars.Add(newCar);
    db.SaveChanges();

    return Results.Ok(newCar);
})
    .WithName("CreateCar")
    .WithTags("Cars");

// Edit car
app.MapPut("/api/cars/{id}", ([FromQuery]int id, [FromBody]CarCreateModel carModel, RaceDb db) =>
{
    var dbCar = db.Cars.FirstOrDefault(x => x.Id == id);

    if (dbCar == null)
    {
        return Results.NotFound($"Car with id:{id} isn't found");
    }

    dbCar.TeamName = carModel.TeamName;
    dbCar.Speed = carModel.Speed;
    dbCar.MelfunctionChance = carModel.MelfunctionChance;
    db.SaveChanges();

    return Results.Ok(dbCar);
})
    .WithName("UpdateCar")
    .WithTags("Cars");

// Delete car
app.MapDelete("/api/cars/{id}", (int id, RaceDb db) =>
{
    var dbCar = db.Cars.FirstOrDefault(x => x.Id == id);

    if (dbCar == null)
    {
        return Results.NotFound($"Car with id:{id} isn't found");
    }

    db.Remove(dbCar);
    db.SaveChanges();

    return Results.Ok($"Car with id: {id} was successfuly deleted");
})
    .WithName("DeleteCar")
    .WithTags("Cars");

#endregion

#region Motorbikes endpoints
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
#endregion

app.Run();

#region Models
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

public record CarCreateModel
{
    public string TeamName { get; set; }
    public int Speed { get; set; }
    public double MelfunctionChance { get; set; }
}
#endregion

// Persistence

public class RaceDb : DbContext
{
    public RaceDb(DbContextOptions<RaceDb> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
}