﻿namespace OsuPlayer.IO.Storage.LazerModels.Extensions;

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.
public static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        // List<T> has a potentially more optimal path to adding a range.
        if (collection is List<T> list)
            list.AddRange(items);
        else
            foreach (var obj in items)
                collection.Add(obj);
    }
}