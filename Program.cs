using Microsoft.Microsoft.SemanticKernel;
using Microsoft.Microsoft.SemanticKernel.ChatCompletion;

var builder = WebApplication.CreateBuilder(args);

//Register Semantic Kernel with Ollama 
builder.Services.AddSingleton<IKernel>(sp =>
{
    var kerneBuilder = Kernel.CreateBuilder();
    kerneBuilder.AddOllamaChatCompletion("Ollama", "http://localhost:11434");

    return kerneBuilder.Build();
});

var app = builder.Build();

app.MapGet("/chat", async (ChatRequest request, Kernel kernel) =>
{
    var chatService = kernel.GetRequiredService<IChatCompletionService>();

    var result = await chatService.GetChatMessageCompletionsAsync(
        "You are a helpful senior .NET backend engineer with deep SQL Server knowledge. \n\nUser: " + request.Query);

    return Results.Ok(new { response = result.Content, model = "phi3" });
});

app.Run();

public record ChatRequest(string Query);