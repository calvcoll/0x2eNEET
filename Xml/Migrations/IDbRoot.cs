namespace DotNEET.Xml.Migrations
{
    public interface IDbRoot
    {
        /// <summary>
        /// The version of this db
        /// </summary>
        IMigrationHistory MigrationHistory { get; }
    }
}