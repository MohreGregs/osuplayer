using System.ComponentModel;
using Avalonia.Markup.Xaml;
using OsuPlayer.IO.Storage.Config;
using OsuPlayer.Modules.Audio;
using ReactiveUI;

namespace OsuPlayer.Windows;

public partial class MainWindow : ReactivePlayerWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel viewModel, Player player)
    {
        ViewModel = viewModel;
        player.MainWindow = this;
        Import(player);
        InitializeComponent();
    }

    private async void Import(Player player)
    {
        await player.ImportSongs();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(disposables =>
        {
            if (ViewModel != null)
                ViewModel.MainView = ViewModel.HomeView;
        });

        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        using var config = new Config();
        var configContainer = config.Read();
        configContainer.Volume = ViewModel.Player.Volume;
    }
}