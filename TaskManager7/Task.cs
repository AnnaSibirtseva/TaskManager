using System;
using TaskManager7;

namespace Task_Manager7
{
    public class Task : Goal, IAssignable
    {
        public Task(string name, DateTime date) : base(name, date)
        {

        }
        /// <summary>
        /// Empty constructor so that it can be serialized via JSON. 
        /// </summary>
        public Task()
        {

        }
        /// <summary>
        /// A method that adds a user to the task of the Task type.
        /// </summary>
        /// <param name="user">User that we want to add.</param>
        public void AddUser(User user)
        {
            if (TaskUsers.Count < 1)
            {
                if (!TaskUsers.Contains(user))
                {
                    for (int i = 0; i < TaskUsers.Count; i++)
                    {
                        if (TaskUsers[i].UserName == user.UserName)
                        {
                            throw new ArgumentException(" *** THE USER IS ALREADY ATTACHED TO THE TASK *** ");
                        }
                    }
                    TaskUsers.Add(user);
                }
                else
                {
                    throw new ArgumentException(" *** THE USER IS ALREADY ATTACHED TO THE TASK *** ");
                }
            }
            else
            {
                throw new ArgumentException($" *** MAXIMUM NUMBER OF USERS ON THIS TASK IS 1 *** ");
            }
        }
        /// <summary>
        /// A method that removes a user from a Task type task. 
        /// </summary>
        /// <param name="user">User that we need to delete.</param>
        public void DeleteUser(User user)
        {
            bool removed = false;
            if (TaskUsers.Count > 0)
            {
                for (int i = 0; i < TaskUsers.Count; i++)
                {
                    if (TaskUsers[i].UserName == user.UserName)
                    {
                        removed = true;
                        TaskUsers.Remove(TaskUsers[i]);
                    }
                }
                if (!removed)
                {
                    throw new ArgumentException(" *** THE USER IS NOT ATTACHED TO THE TASK *** ");
                }
            }
            else
            {
                throw new ArgumentException(" *** THIS TASK HAS NO USERS *** ");
            }
        }
        /// <summary>
        /// A method for assigning the maximum number of users to a task. 
        /// </summary>
        /// <param name="maximum">Maximum number of users in the task.</param>
        public void MaxiUsers(int maximum)
        {
            MaxOfUsers = maximum;
        }
    }
}
