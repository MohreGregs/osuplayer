﻿using OsuPlayer.Data.OsuPlayer.Enums;

namespace OsuPlayer.Modules.Audio;

public interface IShuffleProvider
{
    /// <summary>
    /// Provides a method to generate a shuffled index based on simple parameters.
    /// </summary>
    /// <param name="currentIndex">The current song index the shuffle algorithm is based on</param>
    /// <param name="direction">The <see cref="ShuffleDirection" /> to shuffle to</param>
    /// <param name="rangeMax">The maximum allowed index for the shuffle algorithm</param>
    /// <returns>a random generated (shuffled) index</returns>
    int DoShuffle(int currentIndex, ShuffleDirection direction, int rangeMax);
}