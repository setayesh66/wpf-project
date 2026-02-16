using AutoMapper;
using BeetleMovies.API.DBContexts;
using BeetleMovies.API.DTOs;
using BeetleMovies.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeetleMovies.API.EndpointHandlers
{
    public static class TaskHandlers
    {
   
        public static async Task<Results<Ok<List<TaskDTO>>, BadRequest<string>>> GetTasksAsync(
            BeetleMoviesContext context,
            IMapper mapper,
            [FromQuery] DateOnly? day,
            [FromQuery] DateTime? startTime,
            [FromQuery] DateTime? endTime)
        {
          
            if (day.HasValue && (startTime.HasValue || endTime.HasValue))
                return TypedResults.BadRequest("Use either 'day' OR 'startTime/endTime', not both");

            if (startTime > endTime)
                return TypedResults.BadRequest("endTime must be after startTime");

            var query = context.Tasks.AsNoTracking();

     
            if (day.HasValue)
            {
                var dayDate = day.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(t => t.StartTime.Date == dayDate.Date);
            }

            else
            {
                if (startTime.HasValue)
                    query = query.Where(t => t.StartTime >= startTime);

                if (endTime.HasValue)
                    query = query.Where(t => t.StartTime <= endTime);
            }

            var tasks = await query
                .OrderBy(t => t.StartTime)
                .ToListAsync();

            return TypedResults.Ok(mapper.Map<List<TaskDTO>>(tasks));
        }

        public static async Task<Results<Ok<TaskDTO>, NotFound>> GetTaskByIdAsync(
            BeetleMoviesContext context,
            IMapper mapper,
            int taskId)
        {
            var task = await context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == taskId);

            return task is null 
                ? TypedResults.NotFound() 
                : TypedResults.Ok(mapper.Map<TaskDTO>(task));
        }

        public static async Task<CreatedAtRoute<TaskDTO>> CreateTaskAsync(
            BeetleMoviesContext context,
            IMapper mapper,
            [FromBody] TaskForCreatingDTO taskDto)
        {
            var task = mapper.Map<TaskItem>(taskDto);
            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            return TypedResults.CreatedAtRoute(
                mapper.Map<TaskDTO>(task),
                "GetTaskById",
                new { taskId = task.Id });
        }

        public static async Task<Results<NoContent, NotFound>> UpdateTaskAsync(
            BeetleMoviesContext context,
            IMapper mapper,
            int taskId,
            [FromBody] TaskForUpdatingDTO taskDto)
        {
            var task = await context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task is null)
                return TypedResults.NotFound();

            mapper.Map(taskDto, task);
            await context.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        public static async Task<Results<NoContent, NotFound>> DeleteTaskAsync(
            BeetleMoviesContext context,
            int taskId)
        {
            var task = await context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task is null)
                return TypedResults.NotFound();

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    }
}