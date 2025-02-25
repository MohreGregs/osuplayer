using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using DynamicData;
using Material.Icons.Avalonia;
using OsuPlayer.Base.ViewModels;
using OsuPlayer.Data.OsuPlayer.Enums;
using OsuPlayer.Data.OsuPlayer.StorageModels;
using OsuPlayer.Extensions;
using OsuPlayer.IO.Storage.Playlists;
using OsuPlayer.Modules.Audio.Interfaces;
using ReactiveUI;

namespace OsuPlayer.Views;

public class PlaylistViewModel : BaseViewModel
{
    private readonly IObservable<Func<IMapEntryBase, bool>> _filter;
    private readonly List<MaterialIcon> _materialIcons = new();
    public readonly IPlayer Player;

    private IDisposable _currentBind;
    private ReadOnlyObservableCollection<IMapEntryBase>? _filteredSongEntries;
    private string _filterText;
    private ObservableCollection<Playlist> _playlists;
    private Playlist? _selectedPlaylist;

    public ObservableCollection<Playlist>? Playlists
    {
        get => _playlists;
        set => this.RaiseAndSetIfChanged(ref _playlists, value);
    }

    public Playlist? SelectedPlaylist
    {
        get => _selectedPlaylist;
        set
        {
            _selectedPlaylist = value;

            if (_filteredSongEntries != null)
                _currentBind.Dispose();

            if (_selectedPlaylist?.Songs != null)
                _currentBind = Player.SongSourceProvider.GetMapEntriesFromHash(_selectedPlaylist.Songs).ToSourceList().Connect()
                    .Filter(_filter, ListFilterPolicy.ClearAndReplace).ObserveOn(AvaloniaScheduler.Instance)
                    .Bind(out _filteredSongEntries).Subscribe();

            this.RaisePropertyChanged();
            this.RaisePropertyChanged(nameof(FilteredSongEntries));
        }
    }

    public ReadOnlyObservableCollection<IMapEntryBase>? FilteredSongEntries => _filteredSongEntries;
    public IMapEntryBase? SelectedSong { get; set; }

    public string FilterText
    {
        get => _filterText;
        set => this.RaiseAndSetIfChanged(ref _filterText, value);
    }

    public PlaylistViewModel(IPlayer player)
    {
        Player = player;

        _filter = this.WhenAnyValue(x => x.FilterText)
            .Throttle(TimeSpan.FromMilliseconds(20))
            .Select(BuildFilter);

        Player.SelectedPlaylist.BindValueChanged(d => RefreshAllIcons(), true);

        Player.RepeatMode.BindValueChanged(d =>
        {
            if (d.NewValue != RepeatMode.Playlist)
            {
                _materialIcons.ForEach(x => x.IsVisible = false);
                return;
            }

            RefreshAllIcons();
        }, true);

        Player.PlaylistChanged += (sender, args) =>
        {
            var selection = Player.SelectedPlaylist.Value;

            if (selection == default)
                return;

            Playlists = PlaylistManager.GetAllPlaylists()?.OrderBy(x => x.Name).ToObservableCollection() ?? new ObservableCollection<Playlist>();

            this.RaisePropertyChanged(nameof(Playlists));

            if (SelectedPlaylist == null)
                SelectedPlaylist = Playlists.First(x => x.Id == selection!.Id);
        };

        Activator = new ViewModelActivator();

        this.WhenActivated(disposables =>
        {
            Disposable.Create(() => { }).DisposeWith(disposables);

            _materialIcons.Clear();

            //if (Playlists != null) return;

            Playlists = PlaylistManager.GetAllPlaylists()?.OrderBy(x => x.Name).ToObservableCollection() ?? new ObservableCollection<Playlist>();

            if (Playlists.Count > 0 && SelectedPlaylist == default)
            {
                var config = new Config();
                SelectedPlaylist = Playlists.FirstOrDefault(x => x.Id == config.Container.SelectedPlaylist) ?? Playlists[0];
            }
        });
    }

    public void AddIcon(MaterialIcon icon)
    {
        _materialIcons.Add(icon);

        var indexOf = _materialIcons.IndexOf(icon);

        if (Player.RepeatMode.Value != RepeatMode.Playlist || indexOf >= Playlists?.Count)
        {
            icon.IsVisible = false;
            return;
        }

        icon.IsVisible = Player.SelectedPlaylist.Value?.Id == Playlists?[indexOf].Id;
    }

    private void RefreshAllIcons()
    {
        if (_materialIcons.Count != Playlists?.Count) return;

        for (var i = 0; i < _materialIcons.Count; i++) _materialIcons[i].IsVisible = Player.SelectedPlaylist.Value?.Id == Playlists?[i].Id;
    }

    /// <summary>
    /// Builds the filter to search songs from the song's <see cref="SourceList{T}" />
    /// </summary>
    /// <param name="searchText">the search text to search songs for</param>
    /// <returns>a function with input <see cref="IMapEntryBase" /> and output <see cref="bool" /> to select found songs</returns>
    private Func<IMapEntryBase, bool> BuildFilter(string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
            return _ => true;

        var searchQs = searchText.Split(' ');

        return song => searchQs.All(x =>
            song.Title.Contains(x, StringComparison.OrdinalIgnoreCase) ||
            song.Artist.Contains(x, StringComparison.OrdinalIgnoreCase));
    }
}