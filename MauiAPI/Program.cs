using MauiAPI.Data;
using MauiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("api/todos", async (AppDbContext context) =>
{
    var items = await context.ToDos.ToListAsync();
    return Results.Ok(items);
});

app.MapPost("api/todos", async (AppDbContext context, ToDo toDo) =>
{
    await context.ToDos.AddAsync(toDo);
    await context.SaveChangesAsync();
    return Results.Created($"api/todos/{toDo.Id}", toDo);
});

app.MapPut("api/todos/{id}", async (AppDbContext context,int id, ToDo toDo) => {
    var item = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
    if(item == null)
    {
        return Results.NotFound();
    }
    item.ToDoName = toDo.ToDoName;
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("api/todos/{id}", async (AppDbContext context, int id) => { 
    var item = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
    if (item == null)
    {
        return Results.NotFound();
    }
    context.ToDos.Remove(item);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

//app.UseHttpsRedirection();




app.Run();
