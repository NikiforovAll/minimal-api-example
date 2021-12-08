namespace MinimalAPI.Features;

using Dapper;
using Microsoft.Data.Sqlite;

public class HomeModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/home", (HttpResponse res) =>
        {
            return Results.Text("Minimal API + Carter = ❤");
        })
        .WithMetadata(new EndpointNameMetadata("home"))
        .AllowAnonymous();

        app.MapPost("/internal/migrate", async (SqliteConnection db, HttpResponse res) =>
        {
            var sql = $@"CREATE TABLE IF NOT EXISTS Todos (
                {nameof(Todo.Id)} INTEGER PRIMARY KEY AUTOINCREMENT,
                {nameof(Todo.Title)} TEXT NOT NULL,
                {nameof(Todo.IsComplete)} INTEGER DEFAULT 0 NOT NULL CHECK({nameof(Todo.IsComplete)} IN (0, 1))
            );";
            await db.ExecuteAsync(sql);
        });
    }
}
