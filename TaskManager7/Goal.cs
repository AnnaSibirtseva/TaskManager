using System;
using System.Collections.Generic;
using TaskManager7;

namespace Task_Manager7
{
    public class Goal
    {
        public Status GoalStatus { get; set; }
        public string GoalName { get; set; }
        public DateTime CreationDate { get; set; }
        // List of users attached to the task. 
        public List<User> TaskUsers = new List<User>();
        // List of subtasks for tasks of Epic type. 
        public List<Goal> SubTasks { get; set; } = new List<Goal>();
        public int MaxOfUsers { get; set; } = 0;
        public Goal(string name, DateTime date)
        {
            GoalName = name;
            CreationDate = date;
        }
        /// <summary>
        /// Empty constructor so that it can be serialized via JSON. 
        /// </summary>
        public Goal()
        {
        }
    }
    /// <summary>
    /// Creating a separate type for the status. 
    /// </summary>
    public enum Status
    {
        Open,
        InProgress,
        Completed
    }
}
