using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Task_Manager7;
using TaskManager7;

namespace Manager
{
    class Program
    {
        // Arrays for creating menus. 
        static string[] MainMenuItems = new string[3];
        static string[] ProjectMenuItems = new string[7];
        static string[] UserMenuItems = new string[4];
        static string[] TaskMenuItems = new string[9];
        // User's current position in the menu. 
        static int CurrentPosition = 0;
        // The user-selected project in which the work will take place. 
        static Project CurrentProject = null;
        // The current project of Epic type to which you will need to add subtasks. 
        static Goal CurrentEpic = null;
        static ConsoleKeyInfo key;
        // Username list. 
        public static List<string> UsersNames = new List<string>();
        // List for saving all projects names. 
        static List<string> ProjectsNames = new List<string>();
        // A list for storing the names of all tasks. 
        static List<string> TasksNames = new List<string>();
        // A list for storing all existing projects. 
        static List<Project> Projects = new List<Project>();
        /// <summary>
        /// The main method of the program.
        /// </summary>
        static void Main(string[] args)
        {
            // If an unexpected exeption suddenly pops up. 
            try
            {
                OpenSaved();
                Menu();
                SaveEverything();
            }
            catch
            {
                Error(" *** SORRY SOMETHING WENT WRONG *** ");
            }
        }
        /// <summary>
        /// The main menu of the program.
        /// </summary>
        static void Menu()
        {
            CurrentPosition = 0;
            MainMenuItems[0] = "    Projects ";
            MainMenuItems[1] = "     Users ";
            MainMenuItems[2] = "     Exit ";
            // Continue until the user wants to quit. 
            do
            {
                // Displaying the menu.
                DrawMenu(MainMenuItems, "MENU");
                // If you press enter then go to the next menu. 
                if (key.Key == ConsoleKey.Enter)
                {
                    NextMenuPosition(CurrentPosition);
                }
            } while (key.Key != ConsoleKey.Escape);
        }
        /// <summary>
        /// Method for navigating the menu.
        /// </summary>
        /// <param name="menu">Array with the desired menu.</param>
        /// <param name="key">The pressed key.</param>
        /// <param name="CurrentPosition">The current position of the arrow.</param>
        /// <returns></returns>
        static int SelectedCommand(string[] menu, ConsoleKeyInfo key, int CurrentPosition)
        {
            if (key.Key == ConsoleKey.UpArrow)
            {
                // If we were already at the beginning of the menu, then we go down to the end. 
                if (CurrentPosition == 0)
                {
                    CurrentPosition = menu.Length - 1;
                }
                else
                {
                    CurrentPosition -= 1;
                }
            }
            if (key.Key == ConsoleKey.DownArrow)
            {
                // If we were already at the end of the menu, then we go up to the beginning. 
                if (CurrentPosition == menu.Length - 1)
                {
                    CurrentPosition = 0;
                }
                else
                {
                    CurrentPosition += 1;
                }
            }
            return CurrentPosition;
        }
        /// <summary>
        /// Method for drawing the arrow in the menu. 
        /// </summary>
        /// <param name="menu">Array with the desired menu.</param>
        /// <param name="CurrentPosition">The current position of the arrow.</param>
        static void CurrentCommand(string[] menu, int CurrentPosition)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                // Draw arrows at the current location.
                if (i == CurrentPosition)
                {
                    Console.Write(menu[i]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("<<");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(menu[i]);
                }
            }
        }
        /// <summary>
        /// Determine which menu to show next. 
        /// </summary>
        /// <param name="CurrentPosition">The current position of the arrow.</param>
        static void NextMenuPosition(int CurrentPosition)
        {
            switch (CurrentPosition)
            {
                case 0:
                    ProjectMenu();
                    break;
                case 1:
                    UserMenu();
                    break;
                case 2:
                    SaveEverything();
                    Environment.Exit(0);
                    break;
            }
        }
        /// <summary>
        /// Menu for working with projects. 
        /// </summary>
        static void ProjectMenu()
        {
            // Set the values for the menu elements. 
            ProjectMenuItems[0] = "  Create a project ";
            ProjectMenuItems[1] = "  View a list of projects ";
            ProjectMenuItems[2] = "  Change the projects name ";
            ProjectMenuItems[3] = "  Delete the project ";
            ProjectMenuItems[4] = "  Select the project ";
            ProjectMenuItems[5] = "  Manage Tasks ";
            ProjectMenuItems[6] = "  Go Back ";
            CurrentPosition = 0;
            // Continue until Esc is pressed. 
            do
            {
                DrawMenu(ProjectMenuItems, "MENU");
                if (key.Key == ConsoleKey.Enter)
                {
                    // Calling the method that determines where the user is clicking. 
                    ProjectSwitch(CurrentPosition);
                }
            } while (key.Key != ConsoleKey.Escape);
        }
        /// <summary>
        /// Redrawing the menu when pressing the Up and Down arrows. 
        /// </summary>
        /// <param name="menu">Array with the desired menu.</param>
        /// <param name="menuName">Menu name.</param>
        static void DrawMenu(string[] menu, string menuName)
        {
            // Rendering the menu. 
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n  === {menuName} ===");
            Console.ForegroundColor = ConsoleColor.White;
            CurrentCommand(menu, CurrentPosition);
            key = Console.ReadKey(true);
            CurrentPosition = SelectedCommand(menu, key, CurrentPosition);
        }
        /// <summary>
        /// Menu for working with users. 
        /// </summary>
        static void UserMenu()
        {
            // Assigning menu values to an array. 
            UserMenuItems[0] = "  Create the user ";
            UserMenuItems[1] = "  View the users list ";
            UserMenuItems[2] = "  Delete the user ";
            UserMenuItems[3] = "  Go Back ";
            do
            {
                DrawMenu(UserMenuItems, "MENU");
                if (key.Key == ConsoleKey.Enter)
                {
                    // Passing to the selected item. 
                    UserSwitch(CurrentPosition);
                }
            } while (key.Key != ConsoleKey.Escape);
        }
        /// <summary>
        /// Menu for working with tasks. 
        /// </summary>
        /// <param name="projectName">The name of the project to work with.</param>
        static void TaskMenu(string projectName)
        {
            TaskMenuItems[0] = "  Add task to the project ";
            TaskMenuItems[1] = "  Assign users for the tasks ";
            TaskMenuItems[2] = "  Remove user from the task ";
            TaskMenuItems[3] = "  Change task status ";
            TaskMenuItems[4] = "  View the list of tasks ";
            TaskMenuItems[5] = "  Group tasks by status ";
            TaskMenuItems[6] = "  Delete task from the project ";
            TaskMenuItems[7] = "  Subtask management ";
            TaskMenuItems[8] = "  Go Back ";
            CurrentPosition = 0;
            do
            {
                DrawMenu(TaskMenuItems, $"PROJECT -{projectName}- MENU");
                if (key.Key == ConsoleKey.Enter)
                {
                    TaskSwitch(CurrentPosition);
                }
            } while (key.Key != ConsoleKey.Escape);
        }
        /// <summary>
        /// Doing the selected action with the user. 
        /// </summary>
        /// <param name="Current">Selected command.</param>
        static void UserSwitch(int Current)
        {
            switch (Current)
            {
                case 0:
                    CreateUser();
                    break;
                case 1:
                    ViewUsersList();
                    break;
                case 2:
                    DeleteUser();
                    break;
                case 3:
                    Menu();
                    break;
            }
        }
        /// <summary>
        /// Doing the selected action with the project. 
        /// </summary>
        /// <param name="Current">Selected command.</param>
        static void ProjectSwitch(int Current)
        {
            // Executing the command selected by the user. 
            switch (Current)
            {
                case 0:
                    CreateProject();
                    break;
                case 1:
                    ListOfProjects();
                    break;
                case 2:
                    ChangeProjectName();
                    break;
                case 3:
                    DeleteProject();
                    break;
                case 4:
                    SelectProject();
                    break;
                case 5:
                    ReadyForTasks();
                    break;
                case 6:
                    Menu();
                    break;

            }
        }
        /// <summary>
        /// Carring out the selected action with the task. 
        /// </summary>
        /// <param name="Current">Selected command.</param>
        static void TaskSwitch(int Current)
        {
            // Executing the command selected by the user. 
            switch (Current)
            {
                case 0:
                    AddTask();
                    break;
                case 1:
                    AssignUser();
                    break;
                case 2:
                    UnpinUser();
                    break;
                case 3:
                    ChangeStatus();
                    break;
                case 4:
                    ViewTaskList();
                    break;
                case 5:
                    GroupByStatus();
                    break;
                case 6:
                    DeleteTask();
                    break;
                case 7:
                    CreatingCurrentEpic();
                    break;
                case 8:
                    ProjectMenu();
                    break;
            }
        }
        /// <summary>
        /// Method for defining a command with a subtask. 
        /// </summary>
        static void DeleteOrAdd()
        {
            // Showing the options for subtasks.
            Console.WriteLine("\n  === SUBTASKS MENU === ");
            Console.WriteLine("    1. Add Subtask ");
            Console.WriteLine("    2. Delete Subtask\n ");
            Console.Write(" Enter the number of comand you wnt to do: ");
            string number = Console.ReadLine();
            // Checking if it is correct.
            if (int.TryParse(number, out int IntNumber) && IntNumber >= 1 && IntNumber <= 2)
            {
                switch (IntNumber)
                {
                    case 1:
                        SubTaskAdd();
                        break;
                    case 2:
                        SubTaskDelete();
                        break;
                }
            }
            else
            {
                Error(" *** NUMBER MUST BE INTEGER 1 OR 2 *** ");
            }
        }
        /// <summary>
        /// Method for adding subtasks to Epic. 
        /// </summary>
        static void SubTaskAdd()
        {
            if (CurrentProject.EpicTasks.Count < 1)
            {
                Error(" *** YOU DO NOT HAVE ANY 'EPIC' TASKS *** ");
            }
            else
            {
                // Checking that we still have 'space' for new subtask.
                if (CurrentProject.TaskList.Count + CurrentEpic.SubTasks.Count < CurrentProject.MaximumTaskNumber)
                {
                    CreateSubTask();
                }
                else
                {
                    Error($"*** MAXIMUM NUMBER OF TASKS IS {CurrentProject.MaximumTaskNumber} ***");
                }
            }
        }
        /// <summary>
        /// Method for deleting subtasks from the Epic. 
        /// </summary>
        static void SubTaskDelete()
        {
            // Checking if there is any 'Epic' tasks in the project.
            if (CurrentProject.EpicTasks.Count < 1)
            {
                Error(" *** YOU DO NOT HAVE ANY 'EPIC' TASKS *** ");
            }
            else
            {
                if (CurrentEpic.SubTasks.Count > 0)
                {
                    // Calling the method for deleting the sybtask.
                    DeleteSubTask();
                }
                else
                {
                    Error($"*** YOU DO NOT HAVE ANY SUBTASKS IS THIS TASK ***");
                }
            }
        }
        /// <summary>
        /// A small menu with a choice of follow-up actions. 
        /// </summary>
        static void CreatingCurrentEpic()
        {
            Console.Write("\n Enter the name of the task you want to work with: ");
            string name = Console.ReadLine();
            bool flag = false;
            for (int i = 0; i < CurrentProject.EpicTasks.Count; i++)
            {
                // If tasks with that name exists we are deleting it.
                if (CurrentProject.EpicTasks[i].GoalName == name)
                {
                    flag = true;
                    CurrentEpic = CurrentProject.EpicTasks[i];
                    DeleteOrAdd();
                }
            }
            if (!flag)
            {
                Error(" *** EPIC TASK WAS NOT FOUND *** ");
            }
        }
        /// <summary>
        /// Creating a subtask based on the selected settings. 
        /// </summary>
        static void CreateSubTask()
        {
            // Showing the options for the subtasks. 
            bool flag = false;
            Console.WriteLine("\n === SUBTASKS TYPES === ");
            Console.WriteLine("       1. Story ");
            Console.WriteLine("       2. Task\n ");
            Console.Write(" Enter the number of the selected task type:  ");
            string number = Console.ReadLine();
            // Checing that users input was correct.
            if (int.TryParse(number, out int IntNumber) && IntNumber >= 1 && IntNumber <= 2)
            {
                Console.Write("\n Enter task name: ");
                string name = Console.ReadLine();
                for (int i = 0; i < CurrentEpic.SubTasks.Count; i++)
                {
                    // If task already exist then throwing an error.
                    if (CurrentEpic.SubTasks[i].GoalName == name)
                    {
                        flag = true;
                        Error(" *** THIS TASK ALREADY EXISTS  *** ");
                        break;
                    }
                }
                if (!flag)
                {
                    if (!TasksNames.Contains(name))
                    {
                        // Creating the subtask depending on users input.
                        switch (IntNumber)
                        {
                            case 1:
                                Story story = new Story(name, DateTime.Now);
                                CurrentEpic.SubTasks.Add(story);
                                CurrentProject.TaskList.Add(story);
                                Success(" Subtask was added successfully ");
                                break;
                            case 2:
                                Task task = new Task(name, DateTime.Now);
                                CurrentEpic.SubTasks.Add(task);
                                CurrentProject.TaskList.Add(task);
                                Success(" Subtask was added successfully ");
                                break;
                        }
                    }
                    else
                    {
                        Error(" *** THIS TASK ALREADY EXISTS  *** ");
                    }
                }
            }
        }
        /// <summary>
        /// Deleting a subtask by name. 
        /// </summary>
        static void DeleteSubTask()
        {
            bool flag = false;
            Console.Write("\n Enter task name: ");
            string name = Console.ReadLine();
            for (int i = 0; i < CurrentEpic.SubTasks.Count; i++)
            {
                // If task exist then we delete it otherwise throw an error.
                if (CurrentEpic.SubTasks[i].GoalName == name)
                {
                    CurrentEpic.SubTasks.Remove(CurrentEpic.SubTasks[i]);
                    CurrentProject.TaskList.Remove(CurrentEpic.SubTasks[i]);
                    Success(" Subtask was deleted successfully ");
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                Error(" *** THIS TASK DO NOT EXISTS  *** ");
            }

        }
        /// <summary>
        /// Method for adding a new task to the project. 
        /// </summary>
        static void AddTask()
        {
            if (CurrentProject.TaskList.Count < CurrentProject.MaximumTaskNumber)
            {
                Console.Write("\n Enter task name: ");
                string name = Console.ReadLine();
                if (!TasksNames.Contains(name))
                {
                    // Сreating a project based on the selected type of the task. 
                    SelectedType(name);
                }
                else
                {
                    Error(" *** THIS TASK ALREADY EXISTS  *** ");
                }
            }
            else
            {
                Error($"*** MAXIMUM NUMBER OF TASKS IS {CurrentProject.MaximumTaskNumber} ***");
            }
        }
        /// <summary>
        /// Method for creating a task with selected type.
        /// </summary>
        /// <param name="name">Project name.</param>
        static void SelectedType(string name)
        {
            // Show the task tipes.
            Console.WriteLine(" === TASKS TYPES === ");
            Console.WriteLine("       1. Epic ");
            Console.WriteLine("       2. Story ");
            Console.WriteLine("       3. Task ");
            Console.WriteLine("       4. Bug ");
            Console.Write(" Enter the number of the selected task type:  ");
            string number = Console.ReadLine();
            // Check if users input was correct.
            if (int.TryParse(number, out int IntNumber) && IntNumber >= 1 && IntNumber <= 4)
            {
                TasksNames.Add(name);
                // Creating the task with chosen type.
                switch (IntNumber)
                {
                    case 1:
                        Epic epic = new Epic(name, DateTime.Now);
                        CurrentProject.EpicTasks.Add(epic);
                        CurrentProject.TaskList.Add(epic);
                        break;
                    case 2:
                        Story story = new Story(name, DateTime.Now);
                        CurrentProject.TaskList.Add(story);
                        break;
                    case 3:
                        Task task = new Task(name, DateTime.Now);
                        CurrentProject.TaskList.Add(task);
                        break;
                    case 4:
                        Bug bug = new Bug(name, DateTime.Now);
                        CurrentProject.TaskList.Add(bug);
                        break;
                }
                Success(" The task was successfully added to the project ");
            }
            else
            {
                Error("*** INPUT WAS INCORRECT ***");
            }
        }
        /// <summary>
        /// Method for removing users from a task. 
        /// </summary>
        /// <param name="elem">Сurrent task.</param>
        /// <param name="user">The user to be deleted.</param>
        static void TypeForUnpin(Goal elem, User user)
        {
            // Catching the errors that the interface throws. 
            try
            {
                if (elem is Epic)
                {
                    throw new ArgumentException(" *** TASKS 'EPIK' TYPE CANNOT DELETE USERS *** ");
                }
                if (elem is Story)
                {
                    (elem as Story).DeleteUser(user);
                }
                if (elem is Task)
                {
                    (elem as Task).DeleteUser(user);
                }
                if (elem is Bug)
                {
                    (elem as Bug).DeleteUser(user);
                }
                // Checking that task was deleted.
                bool removed = false;
                for (int i = 0; i < elem.TaskUsers.Count; i++)
                {
                    if (elem.TaskUsers[i].UserName == user.UserName)
                    {
                        removed = true;
                    }
                }
                if (!removed)
                {
                    Success(" The user was successfully deleted from the project ");
                }
            }
            catch (ArgumentException ex)
            {
                Error($"{ex.Message}");
            }
        }
        /// <summary>
        /// Method that removes the selected user. 
        /// </summary>
        /// <param name="elem">The current task.</param>
        static void DeleteUserFromTask(Goal elem)
        {
            // Showing all available users.
            Console.WriteLine("\n === Users ===");
            for (int i = 0; i < UsersNames.Count; i++)
            {
                Console.WriteLine($" {i + 1}. {UsersNames[i]}");
            }
            Console.Write("\n Enter the name of the user you want to delete from the task: ");
            string Name = Console.ReadLine();
            // Checking that users input was correct and user with that name exist.
            if (UsersNames.Contains(Name))
            {
                User user = new User(Name);
                TypeForUnpin(elem, user);
            }
            else
            {
                Error(" *** USER DOES NOT EXIST ***");
            }
        }
        /// <summary>
        /// The main method for deleting a user. 
        /// </summary>
        static void UnpinUser()
        {
            bool Used = false;
            if (CurrentProject.TaskList.Count != 0)
            {
                Console.Write("\n Enter the name of the task which you want to delete users from: ");
                string name = Console.ReadLine();
                foreach (Goal elem in CurrentProject.TaskList)
                {
                    if (elem.GoalName == name)
                    {
                        Used = true;
                        if (UsersNames.Count == 0)
                        {
                            Error(" *** THE LIST OF USERS IS EMPTY ***");
                            break;
                        }
                        else
                        {
                            try
                            {
                                DeleteUserFromTask(elem);
                            }
                            catch (ArgumentException ex)
                            {
                                Error($"{ex.Message}");
                            }
                        }
                    }
                }
                if (!Used)
                {
                    Error(" *** TASK DOES NOT EXIST *** ");
                }
            }
            else
            {
                Error(" *** THE LIST OF TASKS IS EMPTY *** ");
            }
        }
        /// <summary>
        /// A method that attaches a user to a task. 
        /// </summary>
        static void AssignUser()
        {
            bool Used = false;
            if (CurrentProject.TaskList.Count != 0)
            {
                Console.Write("\n Enter the name of the task which you want to attach users: ");
                string name = Console.ReadLine();
                foreach (Goal elem in CurrentProject.TaskList)
                {
                    if (elem.GoalName == name)
                    {
                        Used = true;
                        if (UsersNames.Count == 0)
                        {
                            Error(" *** THE LIST OF USERS IS EMPTY ***");
                            break;
                        }
                        else
                        {
                            try
                            {
                                AdditingUser(elem);
                            }
                            catch (ArgumentException ex)
                            {
                                Error($"{ex.Message}");
                            }
                        }
                    }
                }
                if (!Used)
                {
                    Error(" *** TASK DOES NOT EXIST *** ");
                }
            }
            else
            {
                Error(" *** THE LIST OF TASKS IS EMPTY *** ");
            }
        }
        /// <summary>
        /// Method for adding a user to a task. 
        /// </summary>
        /// <param name="elem">The current task.</param>
        static void AdditingUser(Goal elem)
        {
            if (elem is Story)
            {
                if (elem.MaxOfUsers == 0)
                {
                    Console.Write(" Enter maximum number of users: ");
                    string max = Console.ReadLine();
                    // Checking that number was correct.
                    if (int.TryParse(max, out int intMax) && intMax > 0 && intMax < 15)
                    {
                        elem.MaxOfUsers = intMax;
                    }
                    else
                    {
                        throw new ArgumentException("*** NUMBER MUST BE INTEGER BETWEEN 0 AND 15 ***");
                    }
                }
            }
            Console.WriteLine("\n === Users ===");
            for (int i = 0; i < UsersNames.Count; i++)
            {
                Console.WriteLine($" {i + 1}. {UsersNames[i]}");
            }
            Console.Write("\n Enter the name of the user you want to attach to the task: ");
            string Name = Console.ReadLine();
            if (UsersNames.Contains(Name))
            {
                User user = new User(Name);
                WhichType(elem, user);
                if (elem.TaskUsers.Contains(user))
                {
                    Success(" The user was successfully attached to the project ");
                }
            }
            else
            {
                Error(" *** USER DOES NOT EXIST ***");
            }
        }
        /// <summary>
        /// Method for creating a user depending on the type of task. 
        /// </summary>
        /// <param name="elem">The current task.</param>
        /// <param name="user">New user.</param>
        static void WhichType(Goal elem, User user)
        {
            // Catching the errors that the interface throws. 
            try
            {
                if (elem is Epic)
                {
                    throw new ArgumentException(" *** TASKS 'EPIK' TYPE CANNOT ASSIGN USERS *** ");
                }
                if (elem is Story)
                {
                    (elem as Story).AddUser(user);
                }
                if (elem is Task)
                {
                    (elem as Task).AddUser(user);
                }
                if (elem is Bug)
                {
                    (elem as Bug).AddUser(user);
                }
            }
            catch (ArgumentException ex)
            {
                Error($"{ex.Message}");
            }
        }
        /// <summary>
        /// Method for removing tasks from a project. 
        /// </summary>
        static void DeleteTask()
        {
            // If projects exist, then delete. 
            bool Used = false;
            if (CurrentProject.TaskList.Count != 0)
            {
                Console.Write("\n Enter the name of the task you want to delete: ");
                string name = Console.ReadLine();
                foreach (Goal elem in CurrentProject.TaskList)
                {
                    if (elem.GoalName == name)
                    {
                        // Remove from all lists. 
                        if (elem is Epic)
                        {
                            CurrentProject.EpicTasks.Remove((Epic)elem);
                        }
                        if (CurrentEpic != null)
                        {
                            if (CurrentEpic.SubTasks.Contains(elem))
                            {
                                CurrentEpic.SubTasks.Remove(elem);
                            }
                        }
                        Used = true;
                        CurrentProject.TaskList.Remove(elem);
                        TasksNames.Remove(name);
                        Success(" Task has been successfully deleted from the project");
                        break;
                    }
                }
                if (!Used)
                {
                    Error(" *** TASK DOES NOT EXIST *** ");
                }

            }
            else
            {
                Error(" *** YOU DO NOT HAVE ANY TASKS *** ");
            }
        }
        /// <summary>
        /// Method for sorting tasks in groups.
        /// </summary>
        static void GroupByStatus()
        {
            if (CurrentProject.TaskList.Count == 0)
            {
                Error(" *** THE LIST OF TASKS IS EMPTY ***");
            }
            else
            {
                // Selecting a specific status from all tasks and stuff it into lists.  
                List<Goal> ComletedTasks = CurrentProject.TaskList.Where(x => x.GoalStatus == Status.Completed).ToList();
                List<Goal> InProgresTasks = CurrentProject.TaskList.Where(x => x.GoalStatus == Status.InProgress).ToList();
                List<Goal> OpenTasks = CurrentProject.TaskList.Where(x => x.GoalStatus == Status.Open).ToList();
                Console.WriteLine("");
                // Writing all lists to console.
                PrintComlete(ComletedTasks, "COMPLETED");
                PrintComlete(InProgresTasks, "IN PROGRESS");
                PrintComlete(OpenTasks, "OPEN");
                Wait();
            }
        }
        /// <summary>
        /// Method for printing tasks by groups.
        /// </summary>
        /// <param name="TaskListic">The current group of tasks.</param>
        /// <param name="GroupName">The name of crrent group.</param>
        static void PrintComlete(List<Goal> TaskListic, string GroupName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" === {GroupName} ===");
            Console.ResetColor();
            if (TaskListic.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" THIS GROUP IS EMPTY ");
                Console.ResetColor();
            }
            else
            {
                for (int i = 0; i < TaskListic.Count; i++)
                {
                    Console.WriteLine($" NAME: {TaskListic[i].GoalName} ");
                    Console.WriteLine($" STATUS: {TaskListic[i].GoalStatus} ");
                    Console.WriteLine($" TIME: {TaskListic[i].CreationDate} ");
                    if (TaskListic[i] is Epic)
                    {
                        Console.Write($" SUBTASKS:");
                        for (int j = 0; j < (CurrentProject.TaskList[i]).SubTasks.Count; j++)
                        {
                            Console.Write($"  {CurrentEpic.SubTasks[j].GoalName  }");
                        }
                    }
                    else
                    {
                        Console.Write($" USERS:");
                        for (int j = 0; j < TaskListic[i].TaskUsers.Count; j++)
                        {
                            Console.Write($"  {TaskListic[i].TaskUsers[j].UserName   }");
                        }
                    }
                    Console.WriteLine("\n");
                }
            }
        }
        /// <summary>
        /// Method that changes the status of tasks. 
        /// </summary>
        static void ChangeStatus()
        {
            // If the list is not empty, then we execute.
            if (CurrentProject.TaskList.Count != 0)
            {
                Console.Write("\n Enter the name of the task you want to change: ");
                string name = Console.ReadLine();
                StatusType(name);
            }
            else
            {
                Error(" *** THE LIST OF TASKS IS EMPTY *** ");
            }
        }
        /// <summary>
        /// Determine what status you want to change. 
        /// </summary>
        /// <param name="name">Task name.</param>
        static void StatusType(string name)
        {
            // Checking that task exists.
            bool used = false;
            foreach (Goal elem in CurrentProject.TaskList)
            {
                if (elem.GoalName == name)
                {
                    used = true;
                    Console.WriteLine(" === STATUSES === ");
                    Console.WriteLine("   1. Open ");
                    Console.WriteLine("   2. In Progress ");
                    Console.WriteLine("   3. Completed ");
                    Console.Write(" Enter the new status: ");
                    string number = Console.ReadLine();
                    if (int.TryParse(number, out int IntNumber) && IntNumber >= 1 && IntNumber <= 3)
                    {
                        switch (IntNumber)
                        {
                            case 1:
                                elem.GoalStatus = Status.Open;
                                break;
                            case 2:
                                elem.GoalStatus = Status.InProgress;
                                break;
                            case 3:
                                elem.GoalStatus = Status.Completed;
                                break;
                        }
                        Success(" The task status was successfully changed ");
                        break;
                    }
                    else
                    {
                        Error(" *** INPUT WAS INCORRECT ***");
                        break;
                    }
                }
            }
            if (!used)
            {
                Error(" *** TASK DOES NOT EXIST *** ");
            }
        }
        /// <summary>
        /// Method for outputting tasks to the console. 
        /// </summary>
        static void ViewTaskList()
        {
            // Сhecking that the list of tasks is not empty. 
            if (CurrentProject.TaskList.Count == 0)
            {
                Error("\n *** THE LIST OF TASKS IS EMPTY ***");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine($" > MAXIMUM NUMBER OF TASKS IS { CurrentProject.MaximumTaskNumber}");
                Console.WriteLine("\n === TASKS ===");
                Console.WriteLine("");
                for (int i = 0; i < CurrentProject.TaskList.Count; i++)
                {
                    Console.WriteLine($" NAME: {CurrentProject.TaskList[i].GoalName} ");
                    Console.WriteLine($" STATUS: {CurrentProject.TaskList[i].GoalStatus} ");
                    Console.WriteLine($" TIME: {CurrentProject.TaskList[i].CreationDate} ");
                    if (CurrentProject.TaskList[i] is Epic)
                    {
                        Console.Write($" SUBTASKS:");
                        for (int j = 0; j < (CurrentProject.TaskList[i]).SubTasks.Count; j++)
                        {
                            Console.Write($"  {CurrentEpic.SubTasks[j].GoalName  }");
                        }
                    }
                    else
                    {
                        Console.Write($" USERS:");
                        for (int j = 0; j < CurrentProject.TaskList[i].TaskUsers.Count; j++)
                        {
                            Console.Write($"  {CurrentProject.TaskList[i].TaskUsers[j].UserName  }");
                        }
                    }
                    Console.WriteLine("\n");
                }
                Wait();
            }
        }
        /// <summary>
        /// Creating a new project. 
        /// </summary>
        static void CreateProject()
        {
            Console.Write("\n Enter project name: ");
            string name = Console.ReadLine();
            // If it didn't exist before than create.
            if (!ProjectsNames.Contains(name))
            {
                Console.Write(" Enter maximum number of tasks : ");
                string max = Console.ReadLine();
                if (int.TryParse(max, out int intMax) && intMax > 0 && intMax < 50)
                {
                    Project project = new Project(name);
                    Projects.Add(project);
                    ProjectsNames.Add(name);
                    project.MaximumTaskNumber = intMax;
                    project.ProjectName = name;
                    Success(" Project has been successfully created ");
                }
                else
                {
                    Error("*** NUMBER MUST BE INTEGER BETWEEN 0 AND 50 ***");
                }
            }
            else
            {
                Error("*** PROJECT ALREADY EXISTS  ***");

            }
        }
        /// <summary>
        /// We enter a list of all projects. 
        /// </summary>
        static void ListOfProjects()
        {
            if (Projects.Count == 0)
            {
                Error(" *** THE LIST IS EMPTY ***");
            }
            else
            {
                Console.WriteLine("\n === Projects ===");
                // We show the name of the project and the number of tasks in it. 
                for (int i = 0; i < Projects.Count; i++)
                {
                    Console.WriteLine($" {i + 1}. {Projects[i].ProjectName} - {Projects[i].TaskList.Count} tasks");
                }
                Wait();
            }
        }
        /// <summary>
        /// Changing the project name. 
        /// </summary>
        static void ChangeProjectName()
        {
            // If the list is not empty, then we execute.
            if (ProjectsNames.Count != 0)
            {
                Console.Write("\n Enter the name of the project you want to change: ");
                string name = Console.ReadLine();
                if (ProjectsNames.Contains(name))
                {
                    foreach (var elem in Projects)
                    {
                        if (elem.ProjectName == name)
                        {
                            Console.Write(" Enter the new name for the project: ");
                            string newName = Console.ReadLine();
                            elem.ProjectName = newName;
                            ProjectsNames.Remove(name);
                            ProjectsNames.Add(newName);
                            Success(" Name has been successfully changed ");
                        }
                    }
                }
                else
                {
                    Error(" *** PROJECT DOES NOT EXIST ***");
                }
            }
            else
            {
                Error(" *** YOU DO NOT HAVE ANY PROJECTS ***");
            }
        }
        /// <summary>
        /// A method that deletes a project by its name. 
        /// </summary>
        static void DeleteProject()
        {
            // If projects exist, then delete. 
            if (ProjectsNames.Count != 0)
            {
                Console.Write("\n Enter the name of the project you want to delete: ");
                string name = Console.ReadLine();
                if (ProjectsNames.Contains(name))
                {
                    foreach (var elem in Projects)
                    {
                        if (elem.ProjectName == name)
                        {
                            // Remove from all lists. 
                            Projects.Remove(elem);
                            ProjectsNames.Remove(name);
                            Success(" Project has been successfully deleted ");
                            break;
                        }
                    }
                }
                else
                {
                    Error(" *** PROJECT DOES NOT EXIST *** ");
                }
            }
            else
            {
                Error(" *** YOU DO NOT HAVE ANY PROJECTS *** ");
            }
        }
        /// <summary>
        /// Method for choosing a project for further work. 
        /// </summary>
        static void SelectProject()
        {
            if (ProjectsNames.Count != 0)
            {
                Console.Write("\n Enter the name of the project you want to choose: ");
                string name = Console.ReadLine();
                if (ProjectsNames.Contains(name))
                {
                    foreach (var elem in Projects)
                    {
                        if (elem.ProjectName == name)
                        {
                            // Saving the selected project into a separate variable. 
                            CurrentProject = elem;
                            Success(" Project has been successfully selected ");
                            break;
                        }
                    }
                }
                else
                {
                    Error(" *** PROJECT DOES NOT EXIST *** ");
                }
            }
            else
            {
                Error(" *** YOU DO NOT HAVE ANY PROJECTS *** ");
            }
        }
        /// <summary>
        /// A method that verifies that a project is selected. 
        /// </summary>
        static void ReadyForTasks()
        {
            // Сhecking that the project still exists. 
            if (CurrentProject != null && ProjectsNames.Contains(CurrentProject.ProjectName))
            {
                TaskMenu(CurrentProject.ProjectName);
            }
            else
            {
                Error(" *** YOU NEED TO CHOOSE A PROJECT *** ");
            }
        }
        /// <summary>
        /// The method that creates a new user.
        /// </summary>
        static void CreateUser()
        {
            Console.Write("\n Enter username: ");
            string name = Console.ReadLine();
            // If the user didn't exist already. then we create. 
            if (!UsersNames.Contains(name))
            {
                User user = new User(name);
                User.Users.Add(user);
                UsersNames.Add(name);
                Success(" User has been successfully added ");
            }
            else
            {
                Error(" *** USER ALREADY EXISTS ***");
            }
        }
        /// <summary>
        /// A method that lists all users. 
        /// </summary>
        static void ViewUsersList()
        {
            // Сhecking that the list of users is not empty. 
            if (UsersNames.Count == 0)
            {
                Error(" *** THE LIST OF USERS IS EMPTY ***");
            }
            else
            {
                Console.WriteLine("\n === Users ===");
                for (int i = 0; i < UsersNames.Count; i++)
                {
                    Console.WriteLine($" {i + 1}. {UsersNames[i]}");
                }
                Wait();
            }
        }
        /// <summary>
        /// The method that removes the user. 
        /// </summary>
        static void DeleteUser()
        {
            // Сhecking that we have someone to delete. 
            if (UsersNames.Count != 0)
            {
                Console.Write("\n Enter the name of the user you want to delete: ");
                string name = Console.ReadLine();
                if (UsersNames.Contains(name))
                {
                    UsersNames.Remove(name);
                    for (int i = 0; i < User.Users.Count; i++)
                    {
                        if (User.Users[i].UserName == name)
                        {
                            User.Users.Remove(User.Users[i]);
                        }
                    }
                    Success(" User has been successfully deleted ");
                }
                else
                {
                    Error(" *** USER DOES NOT EXIST ***");
                }
            }
            else
            {
                Error(" *** THE USERS LIST IS EMPTY ***");
            }

        }
        /// <summary>
        /// Method for saving all the projects and user names to the .json file.
        /// </summary>
        static void SaveEverything()
        {
            // Saving all task to the lict with thic method.
            SavingTypeOfTask();
            // Write all users to the file. 
            using (StreamWriter streamWriter = new StreamWriter("Users.json"))
            {
                for (int i = 0; i < UsersNames.Count; i++)
                {
                    streamWriter.WriteLine(JsonSerializer.Serialize(UsersNames[i]));
                }
            }
            // Write all projects to the file. 
            using (StreamWriter streamWriter = new StreamWriter("Projects.json"))
            {
                for (int i = 0; i < Projects.Count; i++)
                {
                    streamWriter.WriteLine(JsonSerializer.Serialize(Projects[i]));
                }
            }
            // Write all project names to the file. 
            using (StreamWriter streamWriter = new StreamWriter("ProjectsNames.json"))
            {
                for (int i = 0; i < ProjectsNames.Count; i++)
                {
                    streamWriter.WriteLine(JsonSerializer.Serialize(ProjectsNames[i]));
                }
            }
        }
        /// <summary>
        /// Method for opening all saved files. 
        /// </summary>
        static void OpenSaved()
        {
            // Reading all users from the file.
            using (StreamReader streamReader = new StreamReader("Users.json"))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    UsersNames.Add(JsonSerializer.Deserialize<string>(line));
                }
            }
            // Reading all projects from the file.
            using (StreamReader streamReader = new StreamReader("Projects.json"))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Projects.Add(JsonSerializer.Deserialize<Project>(line));
                }
            }
            // Since my program was almost entirely built on project names, I had to save them separately. 
            using (StreamReader streamReader = new StreamReader("ProjectsNames.json"))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    ProjectsNames.Add(JsonSerializer.Deserialize<string>(line));
                }
            }
            // Saving all task to the lict with thic method.
            SavingTypeOfTask();
        }
        /// <summary>
        /// Method for saving tasks depending on their type 
        /// </summary>
        static void SavingTypeOfTask()
        {
            // Going through all projects and all tasks in them
            for (int i = 0; i < Projects.Count; i++)
            {
                for (int j = 0; j < Projects[i].TaskList.Count; j++)
                {
                    if (Projects[i].TaskList[j] is Epic)
                    {
                        Projects[i].SavedTasks.Add(Projects[i].TaskList[j] as Epic);
                    }
                    if (Projects[i].TaskList[j] is Story)
                    {
                        Projects[i].SavedTasks.Add(Projects[i].TaskList[j] as Story);
                    }
                    if (Projects[i].TaskList[j] is Task)
                    {
                        Projects[i].SavedTasks.Add(Projects[i].TaskList[j] as Task);
                    }
                    if (Projects[i].TaskList[j] is Bug)
                    {
                        Projects[i].SavedTasks.Add(Projects[i].TaskList[j] as Bug);
                    }
                }
            }
        }
        /// <summary>
        /// Method that displays an error. 
        /// </summary>
        /// <param name="error">The message that will be displayed as an error.</param>
        public static void Error(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{error}");
            Console.ResetColor();
            Wait();
        }
        /// <summary>
        /// A method that prevents saz from cleaning up the console.
        /// </summary>
        public static void Wait()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
        }
        /// <summary>
        /// A method that notifies that the action was completed successfully. 
        /// </summary>
        /// <param name="whatHappened">A test that shows what happened.</param>
        static void Success(string whatHappened)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{whatHappened}");
            Wait();
        }
    }
}
