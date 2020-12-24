﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ATA.Library.Server.Data.Extensions
{
    public static class EnumerableExtensions
    {
        [return: MaybeNull]
        public static TElement ExtendedSingleOrDefault<TElement>(this IEnumerable<TElement> source, string message, Func<TElement, bool>? predicate = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message may not be empty or null", nameof(message));

            try
            {
                return predicate == null ? source.SingleOrDefault() : source.SingleOrDefault(predicate);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(message, ex);
            }
        }

        public static TElement ExtendedSingle<TElement>(this IEnumerable<TElement> source, string message, Func<TElement, bool>? predicate = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message may not be empty or null", nameof(message));

            try
            {
                return predicate == null ? source.Single() : source.Single(predicate);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}