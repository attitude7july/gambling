using gambling.Application.Common.Mappings;
using gambling.Domain.Entities;

namespace gambling.Application.TodoLists.Queries.GetTodos;

public class TodoListDto : IMapFrom<ApplicationUser>
{
    public TodoListDto()
    {
        Items = new List<TodoItemDto>();
    }

    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Colour { get; set; }

    public IList<TodoItemDto> Items { get; set; }
}
