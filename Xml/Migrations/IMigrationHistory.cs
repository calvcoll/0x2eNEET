using System;

namespace DotNEET.Xml.Migrations
{
    public interface IMigrationHistory
    {
        /// <summary>
        /// Add a migration in history so it won't be applied twice
        /// </summary>
        /// <param name="migration">The migration id to add to history</param>
        void AddMigration(Guid migration);

        /// <summary>
        /// Check if migration has been applied or not
        /// </summary>
        /// <param name="migrationId">The migration id</param>
        /// <returns>True if the migration has been applied (aka in the history), otherwise false</returns>
        bool HasMigration(Guid migrationId);
    }
}