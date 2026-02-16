using BeetleMovies.API.DTOs;
using BeetleMovies.API.EndpointHandlers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace BeetleMovies.API.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterTaskEndpoints(this IEndpointRouteBuilder builder)
        {
            var tasksGroup = builder.MapGroup("/tasks")
                .WithTags("Tasks")
                .WithOpenApi();

            tasksGroup.MapGet("", TaskHandlers.GetTasksAsync)
                .Produces<List<TaskDTO>>(StatusCodes.Status200OK)
                .Produces<BadRequest<string>>(StatusCodes.Status400BadRequest)
                .WithOpenApi(o =>
                {
                    o.Summary = "Get tasks with date filters";
                    o.Description = "Filter by: 1) day (?day=YYYY-MM-DD) OR 2) time range (?startTime=...&endTime=...)";
                    
                    o.Parameters.Clear();
                    
                    o.Parameters.Add(new OpenApiParameter
                    {
                        Name = "day",
                        In = ParameterLocation.Query,
                        Description = "Filter by specific date (e.g., 2025-05-20)",
                        Schema = new OpenApiSchema { Type = "string", Format = "date" }
                    });

                    o.Parameters.Add(new OpenApiParameter
                    {
                        Name = "startTime",
                        In = ParameterLocation.Query,
                        Description = "Start of range (e.g., 2025-05-20T09:00:00)",
                        Schema = new OpenApiSchema { Type = "string", Format = "date-time" }
                    });

                    o.Parameters.Add(new OpenApiParameter
                    {
                        Name = "endTime",
                        In = ParameterLocation.Query,
                        Description = "End of range (e.g., 2025-05-20T17:00:00)",
                        Schema = new OpenApiSchema { Type = "string", Format = "date-time" }
                    });

                    return o;
                });

            var tasksWithId = tasksGroup.MapGroup("/{taskId:int}");
            tasksWithId.MapGet("", TaskHandlers.GetTaskByIdAsync)
                .WithName("GetTaskById")
                .Produces<TaskDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

            tasksGroup.MapPost("", TaskHandlers.CreateTaskAsync)
                .Produces<TaskDTO>(StatusCodes.Status201Created)
                .ProducesValidationProblem()
                .WithOpenApi();

            tasksWithId.MapPut("", TaskHandlers.UpdateTaskAsync)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .ProducesValidationProblem()
                .WithOpenApi();

            tasksWithId.MapDelete("", TaskHandlers.DeleteTaskAsync)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();
        }
    }
}