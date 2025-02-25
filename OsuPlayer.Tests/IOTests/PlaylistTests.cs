﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OsuPlayer.Data.OsuPlayer.StorageModels;
using OsuPlayer.IO.Storage.Playlists;

namespace OsuPlayer.Tests;

public class PlaylistTests
{
    [Test]
    public void GetAllPlaylistsTest()
    {
        PlaylistManager.GetAllPlaylists();
    }

    [Test]
    public async Task GetAllPlaylistsAsyncTest()
    {
        await PlaylistManager.GetAllPlaylistsAsync();
    }

    [Test]
    public async Task AddPlaylistAsync()
    {
        var id = Guid.NewGuid();

        await PlaylistManager.AddPlaylistAsync(new Playlist
        {
            Id = id,
            Name = "Unit Test"
        });

        await PlaylistManager.AddPlaylistAsync(new Playlist
        {
            Id = id,
            Name = "Unit Test"
        });
    }

    [Test]
    public async Task RenamePlaylist()
    {
        var id = Guid.NewGuid();

        var playlist = new Playlist
        {
            Id = id,
            Name = id.ToString()
        };

        await PlaylistManager.AddPlaylistAsync(playlist);

        playlist.Name = "New Name";

        await PlaylistManager.RenamePlaylist(playlist);

        var playlists = await PlaylistManager.GetAllPlaylistsAsync();

        Assert.IsTrue(playlists.Any(x => x.Name == "New Name"));
    }

    [Test]
    public async Task DeletePlaylist()
    {
        var id = Guid.NewGuid();

        var playlist = new Playlist
        {
            Id = id,
            Name = id.ToString()
        };

        await PlaylistManager.AddPlaylistAsync(playlist);

        await PlaylistManager.DeletePlaylistAsync(playlist);

        var playlists = await PlaylistManager.GetAllPlaylistsAsync();

        Assert.IsFalse(playlists.Any(x => x.Name == id.ToString()));
    }

    [Test]
    public void CreateContainer()
    {
        var obj = new PlaylistContainer().Init();

        Assert.IsNotNull(obj);
    }
}