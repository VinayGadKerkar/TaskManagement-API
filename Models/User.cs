namespace TaskManager.Models;

public class User {
    public int Id {get ; set;}
    public string Name {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string HashedPassword {get; set;} = string.Empty;
    public List<TaskItem> Tasks {get; set;} = new List<TaskItem>();
}