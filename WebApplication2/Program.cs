using WebApplication2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MoviesDatabaseSettings>(builder.Configuration.GetSection("MoviesDatabaseSettings"));
builder.Services.AddSingleton<MoviesService>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Movies API!");

/// <summary>
/// Get all movies
/// </summary>
app.MapGet("/api/movies", async (MoviesService moviesService) => await moviesService.Get());

/// <summary>
/// Get a movie by id
/// </summary>
app.MapGet("/api/movies/{id}", async (MoviesService moviesService, string id) =>
{
var movie = await moviesService.Get(id);
return movie is null ? Results.NotFound() : Results.Ok(movie);
});

/// <summary>
/// Create a new movie
/// </summary>
app.MapPost("/api/movies", async (MoviesService moviesService, Movie movie) =>
{
await moviesService.Create(movie);
return Results.Ok();
});

/// <summary>
/// Update a movie
/// </summary>
app.MapPut("/api/movies/{id}", async (MoviesService moviesService, string id, Movie updatedMovie) =>
{
var movie = await moviesService.Get(id);
if (movie is null) return Results.NotFound();

updatedMovie.Id = movie.Id;
await moviesService.Update(id, updatedMovie);

return Results.NoContent();
});

/// <summary>
/// Delete a movie
/// </summary>
app.MapDelete("/api/movies/{id}", async (MoviesService moviesService, string id) =>
{
var movie = await moviesService.Get(id);
if (movie is null) return Results.NotFound();

await moviesService.Remove(movie.Id);

    return Results.NoContent();
});

app.Run();
