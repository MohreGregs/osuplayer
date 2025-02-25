﻿namespace OsuPlayer.IO.DbReader.DataModels;

/// <summary>
/// a full beatmap entry with optionally used data
/// <remarks>only created on a <see cref="IMapEntryBase.ReadFullEntry" /> call</remarks>
/// </summary>
internal class RealmMapEntry : RealmMapEntryBase, IMapEntry
{
    public string BackgroundFileLocation { get; init; } = string.Empty;
    public string ArtistUnicode { get; init; } = string.Empty;
    public string TitleUnicode { get; init; } = string.Empty;
    public string AudioFileName { get; init; } = string.Empty;
    public string FolderName { get; init; } = string.Empty;
    public string FolderPath { get; init; } = string.Empty;
    public string FullPath { get; init; } = string.Empty;
    public bool UseUnicode { get; set; }

    public override string GetArtist()
    {
        if (UseUnicode)
            return string.IsNullOrEmpty(ArtistUnicode) ? Artist : ArtistUnicode;
        return Artist;
    }

    public override string GetTitle()
    {
        if (UseUnicode)
            return string.IsNullOrEmpty(TitleUnicode) ? Artist : TitleUnicode;
        return Title;
    }

    public Task<string?> FindBackground()
    {
        return Task.FromResult(File.Exists(BackgroundFileLocation) ? BackgroundFileLocation : null);
    }
}