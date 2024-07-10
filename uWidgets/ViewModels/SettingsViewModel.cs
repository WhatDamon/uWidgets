﻿using System;
using Avalonia.Controls;
using ReactiveUI;
using uWidgets.Core.Interfaces;
using uWidgets.Locales;
using uWidgets.Models;
using uWidgets.Views.Pages;

namespace uWidgets.ViewModels;

public class SettingsViewModel(IAppSettingsProvider appSettingsProvider) : ReactiveObject
{
    private UserControl? currentPage;
    public UserControl? CurrentPage
    {
        get => currentPage;
        set => this.RaiseAndSetIfChanged(ref currentPage, value);
    }

    private string? currentPageTitle;
    public string? CurrentPageTitle
    {
        get => currentPageTitle;
        set => this.RaiseAndSetIfChanged(ref currentPageTitle, value);
    }

    public ListItemTemplate[] MenuItems =>
    [
        new ListItemTemplate(typeof(Appearance), Locale.Settings_Appearance),
        new ListItemTemplate(typeof(Language), Locale.Settings_Language),
        new ListItemTemplate(typeof(About),  Locale.Settings_About)
    ];

    public void SetCurrentPage(ListItemTemplate? value)
    {
        CurrentPage = value?.Type != null
            ? (UserControl?)Activator.CreateInstance(value.Type, appSettingsProvider)
            : null;
        CurrentPageTitle = value?.Text;
    }
}