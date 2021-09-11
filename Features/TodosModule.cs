namespace MinimalAPI;

using Dapper;
using Microsoft.Data.Sqlite;

public class TodosModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/todos", GetTodos);
        app.MapGet("/api/todos/{id}", GetTodo);
        app.MapPost("/api/todos", CreateTodo);
        app.MapPut("/api/todos/{id}/mark-complete", MarkComplete);
        app.MapDelete("/api/todos/{id}", DeleteTodo);
    }
    private static async Task<IResult> GetTodo(int id, SqliteConnection db) =>
        await db.QuerySingleOrDefaultAsync<Todo>(
            "SELECT * FROM Todos WHERE Id = @id", new { id })
            is Todo todo
                ? Results.Ok(todo)
                : Results.NotFound();

    private async Task<IEnumerable<Todo>> GetTodos(SqliteConnection db) =>
        await db.QueryAsync<Todo>("SELECT * FROM Todos");

    private static async Task<IResult> CreateTodo(Todo todo, SqliteConnection db)
    {
        var newTodo = await db.QuerySingleAsync<Todo>(
            "INSERT INTO Todos(Title, IsComplete) Values(@Title, @IsComplete) RETURNING * ", todo);

        return Results.Created($"/todos/{newTodo.Id}", newTodo);
    }
    private static async Task<IResult> DeleteTodo(int id, SqliteConnection db) =>
        await db.ExecuteAsync(
            "DELETE FROM Todos WHERE Id = @id", new { id }) == 1
            ? Results.NoContent()
            : Results.NotFound();
    private static async Task<IResult> MarkComplete(int id, SqliteConnection db) =>
        await db.ExecuteAsync(
            "UPDATE Todos SET IsComplete = true WHERE Id = @Id", new { Id = id }) == 1
            ? Results.NoContent()
            : Results.NotFound();
}

public class Todo
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
}
