using System;
using System.Collections.Generic;
using System.Linq;

namespace aaHelpers.Collections
{
    public class aaList : List<object>
    {
        private aaList(IEnumerable<object> enumerable) : base(enumerable)
        {
        }

        /// <summary>
        /// Creates a new empty <see cref="aaList"/> object.
        /// </summary>
        public aaList()
        {
        }

        /// <summary>
        /// Creates a new <see cref="aaList"/> with the provided object array.
        /// </summary>
        /// <param name="array">An array of objects to initialize the collection with.</param>
        public aaList(object[] array) : base(array)
        {
        }

        /// <summary>
        /// Creates a new <see cref="aaList"/> with the provided csv string.
        /// </summary>
        /// <param name="csv">A string containing values separated by a comma.</param>
        /// <returns>A new <see cref="aaList"/> with the provided values if any exist; otherwise a empty collection.</returns>
        public static aaList FromCsv(string csv)
        {
            if (string.IsNullOrEmpty(csv))
                throw new ArgumentException("The provided csv can not be null or empty.");

            var array = csv.Split(',').Cast<object>().ToArray();

            return new aaList(array);
        }

        /// <summary>
        /// Joins all the items of the collection into a single string separated by the provided string separator.
        /// </summary>
        /// <param name="separator">
        /// The optional separator string used to join the object values of the collection. If not provided, will default
        /// to a ',' character.
        /// </param>
        /// <returns>A string of join values separated by the provided separator string.</returns>
        public string Join(string separator = null) =>
            separator == null ? string.Join(",", this) : string.Join(separator, this);

        public aaList Where(object value, aaListMatchOption matchOption = default)
        {
            switch (matchOption)
            {
                case aaListMatchOption.Equal:
                    return new aaList(WhereEqual(value));
                case aaListMatchOption.Contains:
                    return new aaList(WhereContains(value));
                case aaListMatchOption.StartsWith:
                    return new aaList(WhereStartsWith(value));
                case aaListMatchOption.EndsWith:
                    return new aaList(WhereEndsWith(value));
                case aaListMatchOption.Greater:
                    return new aaList(WhereGreater(value));
                case aaListMatchOption.GreaterOrEqual:
                    return new aaList(WhereGreaterOrEqual(value));
                case aaListMatchOption.Less:
                    return new aaList(WhereLess(value));
                case aaListMatchOption.LessOrEqual:
                    return new aaList(WhereLessOrEqual(value));
                default:
                    throw new ArgumentOutOfRangeException(nameof(matchOption), matchOption, null);
            }
        }

        /// <summary>
        /// Determines if the current <see cref="aaList"/> collection contains only items of the same type.
        /// </summary>
        /// <returns>true if the collection is homogenous in terms of the type.</returns>
        public bool IsHomogenous() => this.GroupBy(x => x.GetType()).Count() == 1;

        private IEnumerable<object> WhereEqual(object value) =>
            FindAll(x => Equals(x, value));

        private IEnumerable<object> WhereContains(object value)
        {
            var str = GetValidString(value);
            return FindAll(x =>  ((string)x).Contains(str));
        }
        
        private IEnumerable<object> WhereStartsWith(object value)
        {
            var str = GetValidString(value);
            return FindAll(x =>  ((string)x).StartsWith(str));
        }
        
        private IEnumerable<object> WhereEndsWith(object value)
        {
            var str = GetValidString(value);
            return FindAll(x =>  ((string)x).EndsWith(str));
        }

        private IEnumerable<object> WhereGreater(object value) =>
            FindAll(x => GetValidComparable(value).CompareTo(x) > 0);

        private IEnumerable<object> WhereGreaterOrEqual(object value) =>
            FindAll(x => GetValidComparable(value).CompareTo(x) >= 0);

        private IEnumerable<object> WhereLess(object value) =>
            FindAll(x => GetValidComparable(value).CompareTo(x) < 0);

        private IEnumerable<object> WhereLessOrEqual(object value) =>
            FindAll(x => GetValidComparable(value).CompareTo(x) <= 0);

        private IComparable GetValidComparable(object value)
        {
            if (!(value is IComparable comparable))
                throw new ArgumentException(
                    $"The provided value does not implement {typeof(IComparable)}. " +
                    $"Values must be comparable in order to determine order");

            if (!IsHomogenous())
                throw new InvalidOperationException(
                    "The current collection contains more than one type. " +
                    "To compare object values, the collection must contain only items of the same type.");


            return comparable;
        }

        private string GetValidString(object value)
        {
            if (!(value is string s))
                throw new ArgumentException(
                    $"The provided value is not of type {typeof(string)}. " +
                    "Values must be a string type in order to perform compare functions 'Contains', 'StartsWith', 'EndsWith', or 'Match'");

            if (!this.All(x => x is string))
                throw new InvalidOperationException(
                    "The current collection contains non string types. " +
                    "To compare object values, the collection must contain only string values.");
            
            return s;
        }
    }
}