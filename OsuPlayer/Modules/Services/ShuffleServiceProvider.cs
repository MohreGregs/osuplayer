﻿using OsuPlayer.Extensions;
using OsuPlayer.Modules.ShuffleImpl;

namespace OsuPlayer.Modules.Services;

/// <summary>
/// Provides a service for shuffle implementation registering the services using Splat.
/// </summary>
public class ShuffleServiceProvider : IShuffleServiceProvider
{
    public List<IShuffleImpl> ShuffleAlgorithms { get; }
    public IShuffleImpl? ShuffleImpl { get; private set; }

    public ShuffleServiceProvider()
    {
        using var config = new Config();

        var shuffleType = typeof(IShuffleImpl);
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => shuffleType.IsAssignableFrom(p) && p != shuffleType);

        ShuffleAlgorithms = types.Select(x => Activator.CreateInstance(x) as IShuffleImpl).ToList();

        ShuffleAlgorithms.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCulture));

        var shuffleAlgo = ShuffleAlgorithms.FirstOrDefault(x => string.Equals(x.GetType().Name, config.Container.ShuffleAlgorithm, StringComparison.InvariantCultureIgnoreCase));

        ShuffleImpl = shuffleAlgo ?? ShuffleAlgorithms.FirstOrDefault(x => x.GetType().IsDefined(typeof(DefaultImplAttr), false));
    }

    public void SetShuffleImpl(IShuffleImpl? algorithm)
    {
        using var config = new Config();
        config.Container.ShuffleAlgorithm = algorithm?.GetType().Name;

        ShuffleImpl = algorithm;
    }
}