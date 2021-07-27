using System.Collections.Generic;
using Task_Manager7;

namespace TaskManager7
{
    public class Project
    {
        public string ProjectName { get; set; }
        public int MaximumTaskNumber { get; set; }
        // List of tasks in the current project. 
        public List<Goal> TaskList { get; set; } = new List<Goal>();
        // List of projects of type Epic in the selected project. 
        public List<Epic> EpicTasks { get; set; } = new List<Epic>();
        // List for saving data in the end.
        public List<Goal> SavedTasks { get; set; } = new List<Goal>();
        /// <summary>
        /// Constructor for naming the project. 
        /// </summary>
        /// <param name="name">The name for the project.</param>
        public Project(string name)
        {
            ProjectName = name;
        }
        /// <summary>
        /// Empty constructor so that it can be serialized via JSON. 
        /// </summary>
        public Project()
        {
        }
    }
}
