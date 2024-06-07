


using Microsoft.EntityFrameworkCore;
using UserTaskApi.Data;
using UserTaskApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

 
DatabaseProviderFactory.ConfigureDatabase(builder.Services, builder.Configuration);

var app = builder.Build();

 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed data if using in-memory database
if (builder.Configuration.GetValue<bool>("UseInMemoryDatabase"))
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<UserTaskContext>();
        SeedData(context);
    }
}

app.Run();

void SeedData(UserTaskContext context)
{
    var users = new List<UserModel>
    {
        new UserModel { Id = 1, Name = "John Doe" },
        new UserModel { Id = 2, Name = "Jane Smith" }
    };

    var tasks = new List<TaskModel>
    {
        new TaskModel { Id = 1, UserId = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Subject = "Task 1", Description = "Description 1" },
        new TaskModel { Id = 2, UserId = 2, Start = DateTime.Now.AddHours(2), End = DateTime.Now.AddHours(3), Subject = "Task 2", Description = "Description 2" }
    };

    context.Users.AddRange(users);
    context.Tasks.AddRange(tasks);
    context.SaveChanges();
}


/*
using Microsoft.EntityFrameworkCore;
using UserTaskApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserTaskContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/