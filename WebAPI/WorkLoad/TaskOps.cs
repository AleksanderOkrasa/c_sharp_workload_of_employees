using Microsoft.EntityFrameworkCore;

namespace WorkLoad
{
    public class TaskOps
    {
        public static async Task<IResult> getAllTasks(DbInter db)
        {
            return Results.Ok(await db.Tasks.ToListAsync()); 
        }

        public static async Task<IResult> insertTask(Task task, DbInter db)
        {
            db.Tasks.Add(task);
            await db.SaveChangesAsync();

            return Results.Created($"/tasks/{task.Id}", task);
        }
    }
}
