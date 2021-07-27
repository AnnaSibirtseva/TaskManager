using System;

namespace Task_Manager7
{
    public class Epic : Goal
    {
        public Epic(string name, DateTime date) : base(name, date)
        {
        }
        /// <summary>
        /// Empty constructor so that it can be serialized via JSON. 
        /// </summary>
        public Epic()
        {
        }
    }
}
