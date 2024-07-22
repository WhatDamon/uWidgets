using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Notes.Locales;
using Notes.Models;
using Notes.ViewModels;
using uWidgets.Core.Interfaces;

namespace Notes.Views;

public partial class Note : UserControl
{
    private readonly NoteModel noteModel;
    private readonly IWidgetLayoutProvider widgetLayoutProvider;
    
    public Note(IWidgetLayoutProvider widgetLayoutProvider) : this(new NoteModel(Locale.Notes_Title), widgetLayoutProvider) {}
    
    public Note(NoteModel noteModel, IWidgetLayoutProvider widgetLayoutProvider)
    {
        this.noteModel = noteModel;
        this.widgetLayoutProvider = widgetLayoutProvider;
        DataContext = new NoteViewModel(noteModel);
        
        PointerExited += OnPointerExited;
        SizeChanged += OnSizeChanged;
        Unloaded += OnUnloaded;
        InitializeComponent();
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var width = VisualRoot?.ClientSize.Width ?? 0;
        
        (DataContext as NoteViewModel)!.LineEnd = new Point(width, 0);
    }

    private void OnUnloaded(object? sender, RoutedEventArgs e)
    {
        PointerExited -= OnPointerExited;
        SizeChanged -= OnSizeChanged;
    }

    private void UpdateContent(object? sender, RoutedEventArgs e)
    {
        var newText = (sender as TextBox)!.Text;

        var newModel = noteModel with { Content = newText, Updated = DateTime.Now };
        var newSettings = JsonSerializer.SerializeToElement(newModel);
        var newLayout = widgetLayoutProvider.Get() with { Settings = newSettings };
        
        widgetLayoutProvider.Save(newLayout);
    }
    
    private void UpdateTitle(object? sender, RoutedEventArgs e)
    {
        var newText = (sender as TextBox)!.Text;

        var newModel = noteModel with { Title = newText, Updated = DateTime.Now };
        var newSettings = JsonSerializer.SerializeToElement(newModel);
        var newLayout = widgetLayoutProvider.Get() with { Settings = newSettings };
        
        widgetLayoutProvider.Save(newLayout);
    }

    private void OverlayClick(object? sender, PointerPressedEventArgs e) => Overlay.IsVisible = false;
    private void OnPointerExited(object? sender, PointerEventArgs e) => Overlay.IsVisible = true;
}