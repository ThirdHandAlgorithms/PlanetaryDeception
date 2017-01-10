namespace PlanetaryDeception
{
    using System;

    /// <summary>
    /// Exceptions when Item stuff goes wrong
    /// </summary>
    public class ItemException : Exception
    {
        /// <summary>
        /// Exception constructor, supply itemId
        /// </summary>
        /// <param name="message"></param>
        /// <param name="itemId"></param>
        public ItemException(string message, KnownItem itemId) : base(message + " (" + itemId + ")")
        {
        }
    }
}
