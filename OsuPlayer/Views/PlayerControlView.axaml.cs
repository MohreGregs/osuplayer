using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OsuPlayer.IO;
using OsuPlayer.ViewModels;
using ReactiveUI;

namespace OsuPlayer.Views;

public partial class PlayerControlView : ReactiveUserControl<PlayerControlViewModel>
{
    public PlayerControlView()
    {
        InitializeComponent();
    }
    
    private Slider ProgressSlider => this.FindControl<Slider>("SongProgressSlider");
    
    private void InitializeComponent()
    {
        this.WhenActivated(disposables =>
        {
            ProgressSlider.AddHandler(PointerPressedEvent, SongProgressSlider_OnPointerPressed,
                RoutingStrategies.Tunnel);
            ProgressSlider.AddHandler(PointerReleasedEvent, SongProgressSlider_OnPointerReleased,
                RoutingStrategies.Tunnel);
        });
        AvaloniaXamlLoader.Load(this);
    }

    private void SongProgressSlider_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        Core.Instance.Engine.ChannelPosition = Core.Instance.MainWindow.ViewModel!.PlayerControl.SongTime;
        Core.Instance.Player.Play();
    }

    private void SongProgressSlider_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Core.Instance.Player.Pause();
    }
    
    private void Volume_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        Core.Instance.Player.Mute();
    }

    private void PlaybackSpeedBtn_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Settings_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.Instance.MainWindow.ViewModel!.MainView = Core.Instance.MainWindow.ViewModel.SettingsView;
    }

    private void Blacklist_OnClick(object? sender, RoutedEventArgs e)
    {
        // throw new NotImplementedException();
    }

    private void Favorite_OnClick(object? sender, RoutedEventArgs e)
    {
        // throw new NotImplementedException();
    }
    
    private void SongControl(object? sender, RoutedEventArgs e)
    {
        switch ((sender as Control)?.Name)
        {
            case "Repeat":
                Core.Instance.Player.Repeat = !Core.Instance.Player.Repeat;
                break;
            case "Previous":
                Core.Instance.Player.PreviousSong();
                break;
            case "PlayPause":
                Core.Instance.Player.PlayPause();
                break;
            case "Next":
                Core.Instance.Player.NextSong();
                break;
            case "Shuffle":
                Core.Instance.Player.Shuffle = !Core.Instance.Player.Shuffle;
                break;
        }
    }

    private void Volume_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.Instance.Player.Mute();
    }
    
    private void PlaybackSpeed_OnClick(object? sender, RoutedEventArgs e)
    {
        ViewModel!.PlaybackSpeed = 0;
    }

    private void VolumePopupHandler(object? sender, PointerEventArgs e)
    {
        if (e.RoutedEvent!.Name == "PointerEnter")
        {
            ViewModel!.IsVolumeVisible = true;
        }
        else
        {
            if (ViewModel!.VolumePopupPointerOver)
                return;
            ViewModel!.IsVolumeVisible = false;
        }
            
    }
    
    private void SpeedPopupHandler(object? sender, PointerEventArgs e)
    {
        if (e.RoutedEvent!.Name == "PointerEnter")
        {
            ViewModel!.IsSpeedVisible = true;
        }
        else
        {
            if (ViewModel!.SpeedPopupPointerOver)
                return;
            ViewModel!.IsSpeedVisible = false;
        }
            
    }

    private void VolumePopup_OnPointerLeave(object? sender, PointerEventArgs e)
    {
        if (ViewModel!.VolumePointerOver)
            return;
        ViewModel!.IsVolumeVisible = false;
    }

    private void PlaybackSpeedPopup_OnPointerLeave(object? sender, PointerEventArgs e)
    {
        if (ViewModel!.SpeedPointerOver)
            return;
        ViewModel!.IsSpeedVisible = false;
    }
}