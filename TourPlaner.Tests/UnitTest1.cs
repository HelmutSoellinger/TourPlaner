using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Windows.Input;
using TourPlaner.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

[TestFixture]
public class UnitTest1
{
    private EditButtonViewModel buttonViewModel;
    private EditButtonViewModel _buttonViewModel;
    private MainViewModel mainVM;

    [SetUp]
    public void SetUp()
    {
        buttonViewModel = new EditButtonViewModel();
        _buttonViewModel = new EditButtonViewModel();
        mainVM = new MainViewModel(buttonViewModel, _buttonViewModel);
    }

    [Test]
    public void InitialToursCount()
    {
        // Act
        int expected = 4;
        int actual = mainVM.Tours.Count;
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test] 
    public void Update_Button_CanExecute_On_SelectedTour()
    {
        mainVM.SelectedTour = null;
        bool canExecute = _buttonViewModel.DeleteTourCommand.CanExecute(null);
        Assert.IsFalse(canExecute);

        mainVM.SelectedTour = mainVM.Tours[0];

        canExecute = _buttonViewModel.DeleteTourCommand.CanExecute(null);
        Assert.IsTrue(canExecute);
    }

    [Test]
    public void DeleteTour()
    {
        int expectedCount = mainVM.Tours.Count - 1;
        mainVM.SelectedTour = mainVM.Tours[0];
        _buttonViewModel.DeleteTourCommand.Execute(null);
        Assert.IsTrue(expectedCount == mainVM.Tours.Count);
    }


    [Test]
    public void AddLog_SecondPrecision()
    {
        int logsCount = mainVM.Tours[0].Logs.Count;
        int expectedLogsCount = logsCount + 1;
        DateTime excpectedTime = DateTime.Now;
        // Act
        mainVM.SelectedTour = mainVM.Tours[0];
        buttonViewModel.AddNewLogCommand.Execute(null);    // simulate button click
        
        // Assert
        var actualLogsCount = mainVM.Tours[0].Logs.Count;

        Assert.That(actualLogsCount, Is.EqualTo(expectedLogsCount));
        var actualTime = mainVM.Tours[0].Logs[actualLogsCount - 1].Date;
        Assert.That(actualTime.Second, Is.EqualTo(excpectedTime.Second));
    }


}
