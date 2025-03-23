namespace SopWebApp
{
    /// <summary>
    /// Represents the settings required to connect to the Event Store.
    /// </summary>
    public class EventStoreSettings
    {
        /// <summary>
        /// Gets or sets the connection string for the Event Store.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;
    }
}
