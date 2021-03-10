﻿using System.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    public static class ILinkedBucketListExtensions
    {
        private static bool ValuesAreEqual<ValueType>(ValueType x, ValueType y, IEqualityComparer<ValueType> equalityComparer) =>
            equalityComparer.Equals(x, y);

        public static LinkedBucketListNode<KeyType, ValueType>? FindFirst<KeyType, ValueType>(
            this ILinkedBucketList<KeyType, ValueType> bucketList,
            ValueType value,
            IEqualityComparer<ValueType>? equalityComparer)
            where KeyType : notnull
        {
            equalityComparer ??= EqualityComparer<ValueType>.Default;
            return bucketList.FindFirst(otherValue => ValuesAreEqual(value, otherValue, equalityComparer));
        }

        public static LinkedBucketListNode<KeyType, ValueType>? FindFirst<KeyType, ValueType>(
            this ILinkedBucketList<KeyType, ValueType> bucketList,
            ValueType value)
            where KeyType : notnull =>
            FindFirst(bucketList, value, equalityComparer: null);

        public static LinkedBucketListNode<KeyType, ValueType>? FindLast<KeyType, ValueType>(
            this ILinkedBucketList<KeyType, ValueType> bucketList,
            ValueType value,
            IEqualityComparer<ValueType>? equalityComparer)
            where KeyType : notnull
        {
            equalityComparer ??= EqualityComparer<ValueType>.Default;
            return bucketList.FindLast(otherValue => ValuesAreEqual(value, otherValue, equalityComparer));
        }

        public static LinkedBucketListNode<KeyType, ValueType>? FindLast<KeyType, ValueType>(
            this ILinkedBucketList<KeyType, ValueType> bucketList,
            ValueType value)
            where KeyType : notnull =>
            FindLast(bucketList, value, equalityComparer: null);
    }
}