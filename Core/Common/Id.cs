using System;

namespace DotNEET
{
    public struct Id<T> : IEquatable<Id<T>>
    {
        private readonly long id;

        public Id(long id)
        {
            this.id = id;
        }

        public long Value
        {
            get
            {
                return this.id;
            }
        }

        public static bool operator !=(Id<T> first, Id<T> second)
        {
            return !(first == second);
        }

        public static bool operator ==(Id<T> first, Id<T> second)
        {
            if (object.ReferenceEquals(second, null))
            {
                if (object.ReferenceEquals(first, null))
                {
                    return true;
                }
                return false;
            }
            return first.Equals(second);
        }

        public static implicit operator Id<T>(long id)
        {
            return new Id<T>(id);
        }

        public static implicit operator long (Id<T> id)
        {
            return id.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Id<T> && this.Equals((Id<T>)obj);
        }

        public bool Equals(Id<T> other)
        {
            return this.id.Equals(other.id);
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
    }
}