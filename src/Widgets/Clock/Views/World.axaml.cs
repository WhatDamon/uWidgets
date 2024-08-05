﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Clock.Models;
using Clock.ViewModels;
using uWidgets.Services;

namespace Clock.Views;

public partial class World : UserControl
{
    public World() : this(new WorldClockModel([])) {}
    
    public World(WorldClockModel worldClockModel)
    {
        DataContext = new WorldClockViewModel(worldClockModel);
        SizeChanged += OnSizeChanged;
        Unloaded += OnUnloaded;
        InitializeComponent();
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var wide = e.NewSize.AspectRatio >= 1.5;
        
        Grid.ColumnDefinitions = new ColumnDefinitions(wide ? "*,*,*,*" : "*,*");
        Grid.RowDefinitions = new RowDefinitions(wide ? "*" : "*,*");
        Grid.SetPosition(Third, wide ? 2 : 0, wide ? 0 : 1);
        Grid.SetPosition(Fourth, wide ? 3 : 1, wide ? 0 : 1);
    }

    private void OnUnloaded(object? sender, RoutedEventArgs e)
    {
        ((WorldClockViewModel)DataContext!).Dispose();
        SizeChanged -= OnSizeChanged;
        Unloaded -= OnUnloaded;
    }
}