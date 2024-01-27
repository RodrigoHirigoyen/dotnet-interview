using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<TodoContext>(
        // Use SQL Server
        // opt.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext"));
        opt => opt.UseInMemoryDatabase("TodoList")
    )
    .AddEndpointsApiExplorer()
    .AddControllers();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TodoContext>();

    context.TodoList.Add(
        new TodoApi.Models.TodoList
        {
            Id = 1,
            Name = "List1",
            Items = new List<TodoItem> { new TodoItem { Id = 1, Name = "Item1", Description = "Item1", OK = true } }
        }
    );

    context.SaveChanges();
}
app.UseAuthorization();
app.MapControllers();
app.Run();