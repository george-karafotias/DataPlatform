using Data.Core.Greeklish.Interfaces;
using Data.Core.Greeklish.Models;
using Data.Core.Shared.Contracts;

namespace Data.Api.Endpoints;

public static class GreeklishEndpoints
{
    public static RouteGroupBuilder MapGreeklishEndpoints(
        this RouteGroupBuilder group)
    {
        var greeklish = group.MapGroup("/greeklish")
                             .WithTags("Greeklish");

        greeklish.MapPost("/convert",
            (GreeklishRequest request, IGreeklishConverter converter) =>
            {
                if (string.IsNullOrWhiteSpace(request.Text))
                {
                    return Results.BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Error = "Text cannot be empty"
                    });
                }

                var result = converter.Convert(request.Text);

                return Results.Ok(new ApiResponse<GreeklishResponse>
                {
                    Success = true,
                    Data = new GreeklishResponse
                    {
                        Data = result
                    }
                });
            })
            .WithName("ConvertGreeklish")
            .WithSummary("Converts Greeklish text to Greek")
            .WithDescription("Transforms Latin-based Greeklish input into Greek characters.");

        return group;
    }
}