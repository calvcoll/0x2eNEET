using DotNEET.Extensions;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNEET.Xml.Migrations
{
    /// <summary>
    /// This class retrieve migrations stored in code (the classes implementing IMigration in the constructor caller assembly)
    /// Warning : This class most likely will try to apply ALL migration available, so if you have two different contexts
    /// within one assembly, there might be collisions (you must check at runtime with casts or something)
    /// Providing a namespace to filter migration is a todo
    /// </summary>
    public class MigrationExecuter
    {
        private readonly IReadOnlyDictionary<Guid, IMigration> migrations;
        private readonly Action persistVersionCallback;

        public MigrationExecuter(Action persistVersionCallback)
        {
            this.persistVersionCallback = persistVersionCallback;
            this.migrations = Assembly.GetCallingAssembly().ExportedTypes.Where(x => x.GetInterfaces().Contains(typeof(IMigration)))
                .Select(x => x.GetInstanceOrNull() as IMigration).Where(x => x != null).ToDictionary(x => x.MigrationId);
        }

        public void ApplyMigrations(IDbRoot root, string filePath)
        {
            var hasDoneSomething = false;
            this.migrations.ForEach(x =>
                {
                    if (!root.MigrationHistory.HasMigration(x.Key))
                    {
                        this.SolveDependency(root, filePath, x.Value);
                        x.Value.Apply(root, filePath);
                        root.MigrationHistory.AddMigration(x.Key);
                        hasDoneSomething = true;
                    }
                });
            // It's useless to serialize / write stuff when you've done nothing, right ?
            if (hasDoneSomething)
            {
                this.persistVersionCallback();
            }
        }

        private void SolveDependency(IDbRoot root, string filePath, IMigration migration)
        {
            foreach (var migrationId in migration.MigrationsNeeded.Where(x => !root.MigrationHistory.HasMigration(x)))
            {
                // For each dependency that needs to be solved, we solve the subdependencies
                var dependency = this.migrations[migrationId];
                this.SolveDependency(root, filePath, dependency);
                dependency.Apply(root, filePath);
                root.MigrationHistory.AddMigration(migrationId);
            }
        }
    }
}