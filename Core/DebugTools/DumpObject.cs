using MoreLinq;
using System;
using System.Collections;
using System.Diagnostics;

namespace DotNEET.Debug
{
    public static class DumpObject
    {
        /// <summary>
        /// Dump attributes and properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toDump"></param>
        public static T Dump<T>(this T toDump, bool followRef = false) where T : class
        {
#if DEBUG
            if (toDump.GetType().IsValueType || (toDump.GetType().GetMethod("ToString", new Type[0]).DeclaringType != typeof(object)))
            {
                toDump.DumpValue();
            }
            else
            {
                var castedEnumerable = toDump as IEnumerable;
                if (castedEnumerable != null)
                {
                    foreach (var subToDump in castedEnumerable)
                    {
                        subToDump.Dump();
                    }
                }
                else
                {
                    Trace.TraceInformation("Properties of " + toDump.GetType().Name);
                    Trace.Indent();
                    toDump.GetType().GetProperties().ForEach(x =>
                        {
                            // If field type is class and has not implemented tostring and not a
                            // value we do a recursive search
                            if ((x.PropertyType.IsClass) && (x.PropertyType.GetMethod("ToString", new Type[0]).DeclaringType == typeof(object)))
                            {
                                if (followRef)
                                {
                                    x.GetValue(toDump).Dump();
                                }
                            }
                            else if (x.PropertyType.GetMethod("ToString", new Type[0]) != null) // value type implement tostring themselves (yeah magic)
                            {
                                x.GetValue(toDump).DumpValue(x.Name);
                            }
                        });
                    Trace.Unindent();
                    Trace.TraceInformation("Fields of " + toDump.GetType().Name);
                    Trace.Indent();
                    toDump.GetType().GetFields().ForEach(x =>
                        {
                            if ((x.FieldType.IsClass) && (x.FieldType.GetMethod("ToString", new Type[0]).DeclaringType == typeof(object)))
                            {
                                if (followRef)
                                {
                                    x.GetValue(toDump).Dump();
                                }
                            }
                            else if (x.FieldType.GetMethod("ToString", new Type[0]) != null)
                            {
                                x.GetValue(toDump).DumpValue(x.Name);
                            }
                        });
                    Trace.Unindent();
                }
            }
#endif
            return toDump;
        }

        public static T DumpValue<T>(this T toDump, string name = "")
        {
            if (toDump != null)
            {
                Trace.TraceInformation(toDump.GetType().ToString() + " " + name + " => " + toDump.ToString());
            }
            else
            {
                Trace.TraceInformation(name + " is null");
            }
            return toDump;
        }
    }
}