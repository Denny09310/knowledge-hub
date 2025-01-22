using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.Components.Articles;

internal static class ArticlesEndpointRouteBuilderExtensions
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

            var imagesFolder = ArticleFilesHelper.GetUploadFolderPath(configuration, "Images");
            var imageFileName = ArticleFilesHelper.GenerateUniqueFileName(image.FileName);

            using var imageStream = new FileStream(Path.Combine(imagesFolder, imageFileName), FileMode.Create);
            await image.CopyToAsync(imageStream, ct);

            return Results.Ok(new
            {
                Data = new { FilePath = $"images/{imageFileName}" }
            });
        })
        .RequireAuthorization()
        .DisableAntiforgery();

        articlesGroup.MapPost("/{id}/Delete", async (
            string id,
            CancellationToken ct,
            [FromServices] ArticlesManager articlesManager) =>
        {
            await articlesManager.DeleteArticleAsync(id);
            return Results.LocalRedirect("~/");
        })
        .RequireAuthorization();

        return articlesGroup;
    }
}