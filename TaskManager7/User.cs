using System.Collections.Generic;

namespace TaskManager7
{
    public class User
    {
        public string UserName { get; set; }
        public static List<User> Users = new List<User>();
        /// <summary>
        /// Constructor for assigning a name to the user. 
        /// </summary>
        /// <param name="name">Username.</param>
        public User(string name)
        {
            UserName = name;
        }
        /// <summary>
        /// Empty constructor so that it can be serialized via JSON. 
        /// </summary>
        public User()
        {
        }
    }
}
