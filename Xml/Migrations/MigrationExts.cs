using DotNEET.Extensions;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DotNEET.Xml.Migrations
{
    public static class MigrationExts
    {
        /// <summary>
        /// Return a non-changing guid based on type classname. 
        /// SO BE CAREFUL WHEN RENAMING CLASSES !
        /// </summary>
        /// <typeparam name="T">The migration (used for constraint to avoid code completion pollution)</typeparam>
        /// <param name="type">The type of the class to retrieve the Guid from</param>
        /// <returns>A Guid representing the class</returns>
        public static Guid GetGuid<T>(this T type) where T : IMigration
        {
            return new Guid(MD5.Create().ComputeHash(Encoding.Default.GetBytes(type.ClassName())));
        }
    }
}