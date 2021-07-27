using TaskManager7;

namespace Task_Manager7
{
    public interface IAssignable
    {
        /// <summary>
        /// A method for assigning the maximum number of users to a task. 
        /// </summary>
        /// <param name="maximum">Integer number of maximum users.</param>
        public void MaxiUsers(int maximum);
        /// <summary>
        /// A method that adds a user to the tasks.
        /// </summary>
        /// <param name="name">User that we need to add.</param>
        public void AddUser(User name);
        /// <summary>
        /// A method that removes a user from a Bug type task.
        /// </summary>
        /// <param name="name">User that we re removing.</param>
        public void DeleteUser(User name);
    }
}
