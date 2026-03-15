#pragma warning disable SKEXP0070

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.SemanticKernel.ChatCompletion.OllamaKernelBuilderExtensions;
using System;

var builder = WebApplication.CreateBuilder(args);

//Register Semantic Kernel with Ollama 
builder.Services.AddSingleton<Kernel>(sp =>
{
    var kerneBuilder = Kernel.CreateBuilder();
    kerneBuilder.AddOllamaChatCompletion(modelId : "llama3.2", endpoint: new Uri("http://localhost:11434"));

    return kerneBuilder.Build();
});

var app = builder.Build();

app.MapGet("/chat", async ([FromBody]ChatRequest request, [FromServices] Kernel kernel) =>
{
    var chatService = kernel.GetRequiredService<IChatCompletionService>();

    var result = await chatService.GetChatMessageContentAsync(
         request.query);

    return Results.Ok(new { response = result.Content, model = "llama3.2" });
});

app.Run();

public record ChatRequest(string query);