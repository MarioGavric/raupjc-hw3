using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Zad1.TodoItem;

namespace Zad1
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TodoItem todoItem)
        {
            if(_context.TodoItem.Any(s => s.Id == todoItem.Id))
                throw new DuplicateTodoItemException("Item with the id: " + todoItem.Id + " already exists." );

            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoItem> GetAsync(Guid todoId, Guid userId)
        {
            var task = await _context.TodoItem.FirstAsync(s => s.Id.Equals(todoId));

            if (task == null) return null;

            if (!task.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User " + userId + " is not the owner of the TodoItem.");

            return task;
        }

        public async Task<List<TodoItem>> GetActiveAsync(Guid userId)
        {
            ///return await _context.TodoItem.Where(s => !s.IsCompleted && s.UserId.Equals(userId)).ToListAsync();
            return await _context.TodoItem.Include(s => s.Labels).Where(s => !s.IsCompleted && s.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<List<TodoItem>> GetAllAsync(Guid userId)
        {
            ///return await _context.TodoItem.Where(s => s.UserId.Equals(userId)).OrderByDescending(s => s.DateCreated).ToListAsync();
            return await _context.TodoItem.Include(s => s.Labels).Where(s => s.UserId.Equals(userId)).OrderByDescending(s => s.DateCreated).ToListAsync();
        }

        public async Task<List<TodoItem>> GetCompletedAsync(Guid userId)
        {
            ///return await _context.TodoItem.Where(s => s.IsCompleted && s.UserId.Equals(userId)).ToListAsync();
            return await _context.TodoItem.Include(s => s.Labels).Where(s => s.IsCompleted && s.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<List<TodoItem>> GetFilteredAsync(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return await  _context.TodoItem.Where(s => s.UserId.Equals(userId) && filterFunction(s)).ToListAsync();

        }

        public async Task<bool> MarkAsCompletedAsync(Guid todoId, Guid userId)
        {
            var task = await _context.TodoItem.FirstOrDefaultAsync(s => s.Id.Equals(todoId));

            if (task == null) return false;

            if (!task.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User " + userId + " is not the owner of the TodoItem.");

            task.MarkAsCompleted();
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> RemoveAsync(Guid todoId, Guid userId)
        {
            var task = await _context.TodoItem.FirstAsync(s => s.Id.Equals(todoId));

            if (task == null) return false;

            if (!task.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User " + userId + " is not the owner of the TodoItem.");

            _context.TodoItem.Remove(task);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task UpdateAsync(TodoItem todoItem, Guid userId)
        {

            if (!_context.TodoItem.Contains(todoItem))
                await AddAsync(todoItem);

            if (!todoItem.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User " + userId + " is not the owner of the TodoItem.");

            _context.Entry(GetAsync(todoItem.Id, userId)).CurrentValues.SetValues(todoItem);
            await _context.SaveChangesAsync();

        }

        public void AddLabel(TodoItemLabel item)
        {
            // ensuring that the label won't be put to the database if the same label allready exists
            TodoItemLabel todoItemLabel = _context.TodoItemLabel.FirstOrDefault(s => s.Value == item.Value);
            if (todoItemLabel == null)
                _context.TodoItemLabel.Add(item);
                _context.SaveChanges();
        }
    }
}