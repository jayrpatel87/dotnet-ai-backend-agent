#pragma warning disable SKEXP0070

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
//using Microsoft.SemanticKernel.ChatCompletion.OllamaKernelBuilderExtensions;
using System;

var builder = WebApplication.CreateBuilder(args);

//Register Semantic Kernel with Ollama 
builder.Services.AddSingleton<Kernel>(sp =>
{
    var kerneBuilder = Kernel.CreateBuilder();
    kerneBuilder.AddOllamaChatCompletion(modelId : "phi3", endpoint: new Uri("http://localhost:11434"));

    return kerneBuilder.Build();
});

var app = builder.Build();

app.MapGet("/chat", async (ChatRequest request, Kernel kernel) =>
{
    var chatService = kernel.GetRequiredService<IChatCompletionService>();

    var result = await chatService.GetChatMessageContentAsync(
        "You are a helpful senior .NET backend engineer with deep SQL Server knowledge. \n\nUser: " + request.Query);

    return Results.Ok(new { response = result.Content, model = "phi3" });
});

app.Run();

public record ChatRequest(string Query);