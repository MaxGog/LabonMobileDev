using SQLite;

namespace ToDoList.Models;

public class ToDoModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
}