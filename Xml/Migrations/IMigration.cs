using System;
using System.Collections.Generic;

namespace DotNEET.Xml.Migrations
{
    /// <summary>
    /// Maybe a generic version of this class would be useful (for different db types)
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// The version after this migration
        /// </summary>
        Guid MigrationId { get; }

        /// <summary>
        /// The migrations needed to apply this migration
        /// </summary>
        IEnumerable<Guid> MigrationsNeeded { get; }

        /// <summary>
        /// Apply the migration (this DOES NOT CHECK for version before ! (It's handled in MigrationExecuter))
        /// </summary>
        /// <param name="filePath"></param>
        void Apply(IDbRoot root, string filePath);

        /// <summary>
        /// Revert the changes
        /// </summary>
        /// <param name="filePath"></param>
        void Revert(IDbRoot root, string filePath);
    }
}