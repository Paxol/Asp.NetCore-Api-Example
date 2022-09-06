using DAMWeb.Shared;
using MediatR;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapPost("/api", async ctx =>
{
    StreamReader reader = new StreamReader(ctx.Request.Body);
    var json = await reader.ReadToEndAsync();

    JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Include,
        TypeNameHandling = TypeNameHandling.All
    };

    object? requestObj = JsonConvert.DeserializeObject(json, settings);

    if (requestObj is null)
    {
        // Error out
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        await ctx.Response.WriteAsync(JsonConvert.SerializeObject(ApiResponse.Problem("Dati forniti non validi"), settings));
        return;
    }

    var mediator = ctx.RequestServices.GetService<IMediator>();

    if (mediator is null)
    {
        // Error out
        Console.Error.WriteLine("Missing MediatR service");
        await ctx.Response.WriteAsync(JsonConvert.SerializeObject(ApiResponse.Problem("Errore interno del server, ripova più tardi"), settings));
        return;
    }

    object? handlerResponse = await mediator.Send(requestObj);

    string responseJson = JsonConvert.SerializeObject(handlerResponse, settings);

    await ctx.Response.WriteAsync(responseJson);
});

app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();
