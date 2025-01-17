using KnowledgeHub.Services;

namespace KnowledgeHub.Components.Articles;

public static class ArticlesEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapArticlesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/Articles");

        group.MapPost("/Delete/{id}", async (string id, ArticleService articles) =>
        {
            await articles.DeleteArticleAsync(id);
            return Results.LocalRedirect("/");
        });

        return group;
    }
}