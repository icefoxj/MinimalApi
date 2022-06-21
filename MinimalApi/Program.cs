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
app.MapGet("/api/cars", 
    (RaceDb db) =>
    {
        var cars = db.Cars.ToList();
        return Results.Ok(cars);
    })
    .WithName("GetCars")
    .WithTags("Cars");

// Get car
app.MapGet("/api/cars/{id}", 
    (int id, RaceDb db) =>
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
app.MapPost("/api/cars", 
    (CarCreateModel carModel, RaceDb db) =>
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
app.MapPut("/api/cars/{id}", 
    ([FromQuery]int id, [FromBody]CarCreateModel carModel, RaceDb db) =>
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
app.MapDelete("/api/cars/{id}", 
    (int id, RaceDb db) =>
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
// Get bikes
app.MapGet("/api/motorbikes", 
    (RaceDb db) =>
    {
        var motorbikes = db.Motorbikes.ToList();
        return Results.Ok(motorbikes);
    })  
    .WithName("GetMotorbikes")
    .WithTags("Motorbikes");

// Get bike
app.MapGet("/api/motorbikes/{id}", 
    (int id, RaceDb db) =>
    {
        var dbMotorbike = db.Motorbikes.FirstOrDefault(x => x.Id == id);
    
        if (dbMotorbike == null)
        {
            return Results.NotFound();
        }
    
        return Results.Ok(dbMotorbike);
    })
    .WithName("GetMotorbike")
    .WithTags("Motorbikes");

// Create bike
app.MapPost("/api/motorbikes", 
    (MotorbikeCreateModel motorbikeModel, RaceDb db) =>
    {
        var newMotorbike = new Motorbike
        {
            TeamName = motorbikeModel.TeamName,
            Speed = motorbikeModel.Speed,
            MelfunctionChance = motorbikeModel.MelfunctionChance
        };
        db.Motorbikes.Add(newMotorbike);
        db.SaveChanges();

        return Results.Ok(newMotorbike);
    })
    .WithName("CreateMotorbike")
    .WithTags("Motorbikes");

// Update bike
app.MapPut("/api/motorbikes/{id}", 
    ([FromQuery] int id, [FromBodyAttribute] MotorbikeCreateModel motorbikeModel, RaceDb db) =>
    {
        var dbMotorbike = db.Motorbikes.FirstOrDefault(x => x.Id == id);

        if (dbMotorbike == null)
        {
            return Results.NotFound($"Motorbike with id: {id} isn't found.");
        }

        dbMotorbike.TeamName = motorbikeModel.TeamName;
        dbMotorbike.Speed = motorbikeModel.Speed;
        dbMotorbike.MelfunctionChance = motorbikeModel.MelfunctionChance;
        db.SaveChanges();

        return Results.Ok(dbMotorbike);
    })
    .WithName("UpdateMotorbike")
    .WithTags("Motorbikes");

// Delete bike
app.MapDelete("/api/motorbikes/{id}", 
    (int id, RaceDb db) =>
    {
        var dbMotorbike = db.Motorbikes.FirstOrDefault(x => x.Id == id);
        if (dbMotorbike == null)
        {
            return Results.NotFound($"Motorbike with id: {id} isn't found.");
        }
        db.Remove(dbMotorbike);
        db.SaveChanges();
        return Results.Ok($"Motorbike with id: {id} was successfuly deleted");
    })
    .WithName("DeleteMotorbike")
    .WithTags("Motorbikes");
#endregion

#region CarRaces endpoints
// Get car races
app.MapGet("/api/carraces", 
    (RaceDb db) =>
    {
        var carRaces = db.CarRaces.Include(x => x.Cars).ToList();
        return Results.Ok(carRaces);
    })
    .WithName("GetCarRaces")
    .WithTags("CarRaces");

// Get car race
app.MapGet("/api/carraces/{id}", 
    (int id, RaceDb db) =>
    {
        var carRace = db
                    .CarRaces
                    .Include(x => x.Cars)
                    .FirstOrDefault(x => x.Id == id);
    
        if (carRace == null)
        {
            return Results.NotFound();
        }
    
        return Results.Ok(carRace);
    })
    .WithName("GetCarRace")
    .WithTags("CarRaces");

// Create car race
app.MapPost("/api/carraces", 
    (CarRaceCreateModel carRaceModel, RaceDb db) =>
    {
        var newCarRace = new CarRace
        {
            Name = carRaceModel.Name,
            Location = carRaceModel.Location,
            Distance = carRaceModel.Distance,
            TimeLimit = carRaceModel.TimeLimit,
            Status = "Created"
        };
        db.CarRaces.Add(newCarRace);
        db.SaveChanges();
        return Results.Ok(newCarRace);
    })
    .WithName("CreateCarRaces")
    .WithTags("CarRaces");

// Update car race
app.MapPut("/api/carraces/{id}", 
    ([FromQuery] int id, [FromBodyAttribute] CarRaceCreateModel carRaceModel, RaceDb db) =>
    {
        var dbCarRace = db
                    .CarRaces
                    .Include(x => x.Cars)
                    .FirstOrDefault(x => x.Id == id);
    
        if (dbCarRace == null)
        {
            return Results.NotFound($"CarRace with id: {id} isn't found.");
        }
    
        dbCarRace.Location = carRaceModel.Location;
        dbCarRace.Name = carRaceModel.Name;
        dbCarRace.TimeLimit = carRaceModel.TimeLimit;
        dbCarRace.Distance = carRaceModel.Distance;
        db.SaveChanges();
    
        return Results.Ok(dbCarRace);
    })
    .WithName("UpdateCarRaces")
    .WithTags("CarRaces");

// Delete car race
app.MapDelete("/api/carraces/{id}", 
    (int id, RaceDb db) =>
    {
        var dbCarRace = db
                    .CarRaces
                    .Include(x => x.Cars)
                    .FirstOrDefault(dbCarRace => dbCarRace.Id == id);
    
        if (dbCarRace == null)
        {
            return Results.NotFound($"CarRace with id: {id} isn't found.");
        }
    
        db.Remove(dbCarRace);
        db.SaveChanges();
    
        return Results.Ok($"CarRace with id: {id} was successfuly deleted");
    })
    .WithName("DeleteCarRaces")
    .WithTags("CarRaces");

// Start car race
app.MapPut("/api/carraces/{id}/start", 
    (int id, RaceDb db) =>
    {
        var carRace = db
            .CarRaces
            .Include(x => x.Cars)
            .FirstOrDefault(carRace => carRace.Id == id);
    
        if (carRace == null)
        {
            return Results.NotFound($"Car Race with id: {id} not found");
        }
    
        carRace.Status = "Started";
        db.SaveChanges();
    
        return Results.Ok(carRace);
    })
    .WithName("StartCarRaces")
    .WithTags("CarRaces");

// Add car to car race
app.MapPut("/api/carraces/{carRaceId}/addcar/{carId}", 
    (int carRaceId, int carId, RaceDb db) =>
    {
        var dbCarRace = db
            .CarRaces
            .Include(x => x.Cars)
            .SingleOrDefault(x => x.Id == carRaceId);
    
        if (dbCarRace == null)
        {
            return Results.NotFound($"Car Race with id: {carRaceId} not found");
        }
    
        var dbCar = db.Cars.SingleOrDefault(x => x.Id == carId);
    
        if (dbCar == null)
        {
            return Results.NotFound($"Car with id: {carId} not found");
        }
    
        dbCarRace.Cars.Add(dbCar);
        db.SaveChanges();
        return Results.Ok(dbCarRace);
    })
    .WithName("AddCarToCarRaces")
    .WithTags("CarRaces");
#endregion

#region MotorbikeRaces endpoints
// Get car races
app.MapGet("/api/motorbikeraces",
    (RaceDb db) =>
    {
        var motorbikeRaces = db.MotorbikeRaces.ToList();
        return Results.Ok(motorbikeRaces);
    })
    .WithName("GetMotorbikeRaces")
    .WithTags("MotorbikeRaces");

// Get car race
app.MapGet("/api/motorbikeraces/{id}",
    (int id, RaceDb db) =>
    {
        var motorbikeRace = db.MotorbikeRaces.FirstOrDefault(motorbikeRace => motorbikeRace.Id == id);

        if (motorbikeRace == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(motorbikeRace);
    })
    .WithName("GetMotorbikeRace")
    .WithTags("MotorbikeRaces");

// Create car race
app.MapPost("/api/motorbikeraces",
    (MotorbikeRaceCreateModel motorbikeRaceModel, RaceDb db) =>
    {
        var motorbikeRace = new MotorbikeRace
        {
            Name = motorbikeRaceModel.Name,
            Location = motorbikeRaceModel.Location,
            Distance = motorbikeRaceModel.Distance,
            TimeLimit = motorbikeRaceModel.TimeLimit
        };
        db.MotorbikeRaces.Add(motorbikeRace);
        db.SaveChanges();
        return Results.Ok();
    })
    .WithName("CreateMotorbikeRaces")
    .WithTags("MotorbikeRaces");

// Update car race
app.MapPut("/api/motorbikeraces/{id}",
    ([FromQuery] int id, [FromBodyAttribute] MotorbikeRaceCreateModel motorbikeRaceModel, RaceDb db) =>
    {
        var dbMotorbikeRace = db.MotorbikeRaces.FirstOrDefault(dbMotorbikeRace => dbMotorbikeRace.Id == id);

        if (dbMotorbikeRace == null)
        {
            return Results.NotFound($"MotorbikeRace with id: {id} isn't found, please try another id.");
        }

        dbMotorbikeRace.Location = motorbikeRaceModel.Location;
        dbMotorbikeRace.Name = motorbikeRaceModel.Name;
        dbMotorbikeRace.TimeLimit = motorbikeRaceModel.TimeLimit;
        dbMotorbikeRace.Distance = motorbikeRaceModel.Distance;
        db.SaveChanges();
        return Results.Ok(dbMotorbikeRace);
    })
    .WithName("UpdateMotorbikeRaces")
    .WithTags("MotorbikeRaces");

// Delete car race
app.MapDelete("/api/motorbikeraces/{id}",
    (int id, RaceDb db) =>
    {
        var dbMotorbikeRace = db.MotorbikeRaces.FirstOrDefault(dbMotorbikeRace => dbMotorbikeRace.Id == id);

        if (dbMotorbikeRace == null)
        {
            return Results.NotFound($"Motorbike Race with id: {id} isn't found, please try another id.");
        }
        db.Remove(dbMotorbikeRace);
        db.SaveChanges();
        return Results.Ok($"Motorbike Race with id: {id} was successfuly deleted");
    })
    .WithName("DeleteMotorbikeRaces")
    .WithTags("MotorbikeRaces");

// Start car race
app.MapPut("/api/motorbikeraces/{id}/start",
    (int id, RaceDb db) =>
    {
        var dbMotorbikeRace = db.MotorbikeRaces.FirstOrDefault(dbMotorbikeRace => dbMotorbikeRace.Id == id);

        if (dbMotorbikeRace == null)
        {
            return Results.NotFound();
        }

        dbMotorbikeRace.Status = "Started";
        db.SaveChanges();
        return Results.Ok(dbMotorbikeRace);
    })
    .WithName("StartMotorbikeRaces")
    .WithTags("MotorbikeRaces");

// Add car to car race
app.MapPut("/api/motorbikeraces/{motorbikecarRaceId}/addmotorbikecar/{motorbikeId}",
    (int motorbikeRaceId, int motorbikeId, RaceDb db) =>
    {
        var dbMotorbikeRace = db.MotorbikeRaces.SingleOrDefault(dbMotorbikeRace => dbMotorbikeRace.Id == motorbikeRaceId);

        if (dbMotorbikeRace == null)
        {
            return Results.NotFound($"Motorbike Race with id: {motorbikeRaceId} not found");
        }

        if (dbMotorbikeRace.Motorbikes == null)
        {
            dbMotorbikeRace.Motorbikes = new List<Motorbike>();
        }

        var dbMotorbike = db.Motorbikes.SingleOrDefault(dbMotorbike => dbMotorbike.Id == motorbikeId);

        if (dbMotorbike == null)
        {
            return Results.NotFound($"Motorbike with id: {motorbikeId} not found");
        }

        dbMotorbikeRace.Motorbikes.Add(dbMotorbike);
        db.SaveChanges();
        return Results.Ok(dbMotorbikeRace);
    })
    .WithName("AddMotorbikeToMotorbikeRaces")
    .WithTags("MotorbikeRaces");
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

public record MotorbikeCreateModel
{
    public string TeamName { get; set; }
    public int Speed { get; set; }
    public double MelfunctionChance { get; set; }
}

public record CarRace
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Distance { get; set; }
    public int TimeLimit { get; set; }
    public string Status { get; set; }
    public List<Car> Cars { get; set; } = new List<Car>();
}

public record CarRaceCreateModel
{
    public string Name { get; set; }
    public string Location { get; set; }
    public int Distance { get; set; }
    public int TimeLimit { get; set; }
}

public record MotorbikeRace
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Distance { get; set; }
    public int TimeLimit { get; set; }
    public string Status { get; set; }
    public List<Motorbike> Motorbikes { get; set; } = new List<Motorbike>();

}

public record MotorbikeRaceCreateModel
{
    public string Name { get; set; }
    public string Location { get; set; }
    public int Distance { get; set; }
    public int TimeLimit { get; set; }
}
#endregion

// Persistence

public class RaceDb : DbContext
{
    public RaceDb(DbContextOptions<RaceDb> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }

    public DbSet<Motorbike> Motorbikes { get; set; }

    public DbSet<CarRace> CarRaces { get; set; }

    public DbSet<MotorbikeRace> MotorbikeRaces { get; set; }
}