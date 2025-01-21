﻿using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KnowledgeHub.Components.Articles;

internal static class IdentityEndpointRouteBuilderExtensions
{
    internal static IEndpointConventionBuilder MapArticlesEndpoints(this IEndpointRouteBuilder builder)
    {
        var articlesGroup = builder.MapGroup("/Articles");

        articlesGroup.MapPost("/Image", async (
            CancellationToken ct,
            [FromServices] IConfiguration configuration,
            [FromForm] IFormFile image) =>
        {
            if (image.Length > 2 * 1024 * 1024)
            {
                return Results.StatusCode(StatusCodes.Status413PayloadTooLarge);
            }

            var uploadsFolder = configuration["Uploads:Images"] ?? throw new InvalidOperationException("Images path not set.");
            var fileName = $"{Guid.CreateVersion7()}{Path.GetExtension(image.FileName)}";
            
            using var fileStream = new FileStream(Path.Combine(uploadsFolder, fileName), FileMode.Create);
            await image.CopyToAsync(fileStream, ct);

            return Results.Ok(new
            {
                Data = new { FilePath = $"images/{fileName}" }
            });
        })
        .DisableAntiforgery();

        return articlesGroup;
    }
}
