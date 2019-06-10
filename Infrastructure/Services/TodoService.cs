using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Linq;
using WebApiNHibernateCrudPagination.Data;
using WebApiNHibernateCrudPagination.Entities;
using WebApiNHibernateCrudPagination.Enums;

namespace WebApiNHibernateCrudPagination.Infrastructure.Services
{
    public class TodoService
    {
        private readonly ISessionFactoryBuilder _sessionFactoryBuilder;

        public TodoService()
        {
            _sessionFactoryBuilder = new FluentSessionFactoryBuilder();
        }

        public async Task<Tuple<int, List<Todo>>> FetchMany(int page = 1, int pageSize = 5,
            TodoShow show = TodoShow.All)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                var offset = (page - 1) * pageSize;
                IQueryable<Todo> queryable = null;

                if (show == TodoShow.Completed)
                    queryable = session.Query<Todo>().Where(t => t.Completed);
                else if (show == TodoShow.Pending)
                    queryable = session.Query<Todo>().Where(t => !t.Completed);


                int totalCount;
                List<Todo> todos;

                if (queryable != null)
                {
                    totalCount = await queryable.CountAsync();
                    todos = await queryable.Skip(offset).Take(pageSize).Select(t => new Todo
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Completed = t.Completed,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt
                    }).ToListAsync();
                }
                else
                {
                    totalCount = await session.Query<Todo>().CountAsync();
                    todos = await session.Query<Todo>().Skip(offset).Take(pageSize).Select(t => new Todo
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Completed = t.Completed,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt
                    }).ToListAsync();
                }

                return Tuple.Create(totalCount, todos);
            }
        }

        public async Task<Todo> GetById(int todoId)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                return await session.Query<Todo>().FirstOrDefaultAsync(t => t.Id == todoId);
            }
        }

        public async Task<Todo> CreateTodo(Todo todo)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.SaveAsync(todo);
                    return todo;
                }
            }
        }

        public async Task<Todo> Update(int id, Todo todoFromUserInput)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var todoFromDb = await GetById(id);
                    if (todoFromDb == null)
                        return null;

                    todoFromDb.Title = todoFromUserInput.Title;
                    if (todoFromUserInput.Description != null)
                        todoFromDb.Description = todoFromUserInput.Description;
                    todoFromDb.Completed = todoFromUserInput.Completed;

                    // Not needed, it is set in ApplicationDbContext
                    await session.UpdateAsync(todoFromDb);
                    await transaction.CommitAsync();
                    return todoFromDb;
                }
            }
        }

        public async Task<Todo> Update(Todo todoFromDb, Todo todoFromUserInput)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    todoFromDb.Title = todoFromUserInput.Title;

                    if (todoFromUserInput.Description != null)
                        todoFromDb.Description = todoFromUserInput.Description;

                    todoFromDb.Completed = todoFromUserInput.Completed;

                    await session.UpdateAsync(todoFromDb);
                    await transaction.CommitAsync();
                    return todoFromDb;
                }
            }
        }

        public async Task Delete(int todoId)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var todoFromDb = await session.Query<Todo>().FirstAsync(t => t.Id == todoId);
                    await session.DeleteAsync(todoFromDb);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task Delete(Todo todo)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.DeleteAsync(todo);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteAll()
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    // Approach 1
                    // session.Query<Todo>().Delete();

                    // Approach 2
                    await session.DeleteAsync("from Todo t");
                    await session.FlushAsync();
                    await transaction.CommitAsync();
                }
            }
        }
    }
}