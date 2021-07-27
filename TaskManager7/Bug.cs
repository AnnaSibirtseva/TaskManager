using System;
using TaskManager7;

namespace Task_Manager7
{
    public class Bug : Goal, IAssignable
    {
        public Bug(string name, DateTime date) : base(name, date)
        {

        }
        /// <summary>
        /// Empty constructor so that it can be serialized via JSON. 
        /// </summary>
        public Bug()
        {
        }
        /// <summary>
        /// A method that adds a user to a problem of the Bug type. 
        /// </summary>
        /// <param name="user">User that we want to add.</param>
        public void AddUser(User user)
        {
            // If the number of users is more than the maximum, then we throw an error.  
            if (TaskUsers.Count < 1)
            {
                // If user did not exiast befour.
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
        /// A method that removes a user from a Bug type task. 
        /// </summary>
        /// <param name="user">User that need to be deleted.</param>
        public void DeleteUser(User user)
        {
            // Сhecking that the user has been deleted. 
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
        /// A method for assigning the maximum number of users to the task. 
        /// </summary>
        /// <param name="maximum">Maximum number of users in the task.</param>
        public void MaxiUsers(int maximum)
        {
            MaxOfUsers = maximum;
        }
    }
}
