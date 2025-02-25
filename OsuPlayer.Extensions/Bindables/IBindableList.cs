﻿using System.Collections.Specialized;

namespace OsuPlayer.Extensions.Bindables;

/// <summary>
/// A readonly interface which can be bound to other <see cref="IBindableList{T}" />s in order to watch for state and
/// content changes.
/// </summary>
/// <typeparam name="T">The type of value encapsulated by this <see cref="IBindableList{T}" />.</typeparam>
public interface IBindableList<T> : IReadOnlyList<T>, ICanBeDisabled, IHasDefaultValue, IUnbindable, IHasDescription, INotifyCollectionChanged
{
    /// <summary>
    /// An alias of <see cref="BindTo" /> provided for use in object initializer scenarios.
    /// Passes the provided value as the foreign (more permanent) bindable.
    /// </summary>
    sealed IBindableList<T> BindTarget
    {
        set => BindTo(value);
    }

    /// <summary>
    /// Binds self to another bindable such that we receive any values and value limitations of the bindable we bind width.
    /// </summary>
    /// <param name="them">The foreign bindable. This should always be the most permanent end of the bind (ie. a ConfigManager)</param>
    void BindTo(IBindableList<T> them);

    /// <summary>
    /// Bind an action to <see cref="INotifyCollectionChanged.CollectionChanged" /> with the option of running the bound action
    /// once immediately
    /// with an <see cref="NotifyCollectionChangedAction.Add" /> event for the entire contents of this
    /// <see cref="BindableList{T}" />.
    /// </summary>
    /// <param name="onChange">The action to perform when this <see cref="BindableList{T}" /> changes.</param>
    /// <param name="runOnceImmediately">
    /// Whether the action provided in <paramref name="onChange" /> should be run once
    /// immediately.
    /// </param>
    void BindCollectionChanged(NotifyCollectionChangedEventHandler onChange, bool runOnceImmediately = false);

    /// <inheritdoc cref="IBindable.GetBoundCopy" />
    IBindableList<T> GetBoundCopy();
}