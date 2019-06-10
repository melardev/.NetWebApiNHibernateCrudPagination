using System.Collections.Generic;
using WebApiNHibernateCrudPagination.Dtos.Responses.Shared;
using WebApiNHibernateCrudPagination.Entities;
using WebApiNHibernateCrudPagination.Models;

namespace WebApiNHibernateCrudPagination.Dtos.Responses.Todos
{
    public class TodoListResponse : PagedDto
    {
        public IEnumerable<TodoDto> Todos { get; set; }
//    public int SortBy {get; set;}


        public static TodoListResponse Build(List<Todo> todos, string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            List<TodoDto> todoDtos = new List<TodoDto>(todos.Count);

            foreach (var todo in todos)
                todoDtos.Add(TodoDto.Build(todo));


            return new TodoListResponse
            {
                PageMeta = new PageMeta(todos.Count, basePath, currentPageNumber: currentPage, pageSize: pageSize,
                    totalItemCount: totalItemCount),
                Todos = todoDtos
            };
        }
    }
}