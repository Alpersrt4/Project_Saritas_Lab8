using Lab8Mvvm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;

namespace Lab8Mvvm.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Pwd> Items { get; } = new();

    private string newOwner = string.Empty;
    public string NewOwner
    {
        get => newOwner;
        set
        {
            if (newOwner == value) return;
            newOwner = value;
            OnPropertyChanged();
        }
    }

    private string filterText = string.Empty;
    public string FilterText
    {
        get => filterText;
        set
        {
            if (filterText == value) return;
            filterText = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(FilteredItems));
        }
    }

    private Pwd? selectedItem;
    public Pwd? SelectedItem
    {
        get => selectedItem;
        set
        {
            if (ReferenceEquals(selectedItem, value)) return;
            selectedItem = value;
            OnPropertyChanged();
            deleteCommand.RaiseCanExecuteChanged();
        }
    }

    private string statusMessage = string.Empty;
    public string StatusMessage
    {
        get => statusMessage;
        private set
        {
            statusMessage = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Pwd> FilteredItems
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return Items;

            return Items.Where(x =>
                x.Owner.Contains(FilterText, StringComparison.OrdinalIgnoreCase));
        }
    }

    public ICommand AddCommand { get; }
    public ICommand DeleteCommand => deleteCommand;
    public ICommand SaveCommand { get; }

    private readonly RelayCommand deleteCommand;

    public MainViewModel()
    {
        Items.Add(new Pwd());

        AddCommand = new RelayCommand(_ => AddObject());
        deleteCommand = new RelayCommand(_ => DeleteObject(), _ => SelectedItem is not null);
        SaveCommand = new RelayCommand(_ => SaveData());

        Items.CollectionChanged += (_, _) => OnPropertyChanged(nameof(FilteredItems));
    }

    private void AddObject()
    {
        try
        {
            var item = new Pwd(NewOwner);
            Items.Add(item);
            NewOwner = string.Empty;
            StatusMessage = "Object added successfully.";
        }
        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void DeleteObject()
    {
        if (SelectedItem is null)
            return;

        Items.Remove(SelectedItem);
        SelectedItem = null;
        StatusMessage = "Object deleted successfully.";
    }

    private void SaveData()
    {
        try
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Lab8_MVVM_Data.json");

            var data = Items.Select(x => new
            {
                x.Owner,
                x.Password
            }).ToList();

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
            StatusMessage = $"Saved: {path}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Save error: {ex.Message}";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
