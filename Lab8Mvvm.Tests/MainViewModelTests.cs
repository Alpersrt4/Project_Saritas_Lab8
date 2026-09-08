using Lab8Mvvm.Models;
using Lab8Mvvm.ViewModels;
using System;
using System.Linq;
using Xunit;

namespace Lab8Mvvm.Tests;

public class MainViewModelTests
{
    [Fact]
    public void PasswordCalculation_ShouldBeCorrect()
    {
        var pwd = new Pwd();
        Assert.Equal(3735927740u, pwd.GetPwd());
    }

    [Fact]
    public void EmptyOwner_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => new Pwd(""));
    }

    [Fact]
    public void AddCommand_ShouldAddObject()
    {
        var vm = new MainViewModel();
        int before = vm.Items.Count;

        vm.NewOwner = "Test User";
        vm.AddCommand.Execute(null);

        Assert.Equal(before + 1, vm.Items.Count);
        Assert.Contains(vm.Items, x => x.Owner == "Test User");
    }

    [Fact]
    public void DeleteCommand_ShouldDeleteObject()
    {
        var vm = new MainViewModel();
        int before = vm.Items.Count;

        vm.SelectedItem = vm.Items.First();
        vm.DeleteCommand.Execute(null);

        Assert.Equal(before - 1, vm.Items.Count);
    }

    [Fact]
    public void Filtering_ShouldFindCorrectObject()
    {
        var vm = new MainViewModel
        {
            NewOwner = "John Test"
        };

        vm.AddCommand.Execute(null);
        vm.FilterText = "John";

        var result = vm.FilteredItems.ToList();

        Assert.Single(result);
        Assert.Equal("John Test", result[0].Owner);
    }
}
