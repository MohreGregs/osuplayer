﻿using DynamicData;
using OsuPlayer.Data.OsuPlayer.Enums;

namespace OsuPlayer.Modules.Services;

public interface ISortProvider
{
    public IObservable<IChangeSet<IMapEntryBase>>? SortedSongs { get; set; }
    public Bindable<SortingMode> SortingModeBindable { get; }
    public ObservableSorter SortingModeObservable { get; }

    public IComparable CustomSorter(IMapEntryBase map, SortingMode sortingMode);
}