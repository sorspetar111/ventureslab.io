namespace UserTaskApi.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public UserModel User { get; set; }
    }


    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<TaskModel> Tasks { get; set; }
    }

    
}
