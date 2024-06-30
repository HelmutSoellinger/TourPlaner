using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Windows.Input;
using TourPlaner.BL;
using TourPlaner.DAL;
using TourPlaner.ViewModels;
using TourPlaner.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows;

[TestFixture]
public class UnitTest1
{
    private EditButtonViewModel logButtonViewModel;
    private EditButtonViewModel tourButtonViewModel;
    private ITourManager _tourManager;
    private ITourRepository _tourRepository;
    private MainViewModel mainVM;

    [SetUp]
    public void SetUp()
    {
        logButtonViewModel = new EditButtonViewModel();
        tourButtonViewModel = new EditButtonViewModel();
        _tourRepository = new TourRepository(new TourPlanerDbContext());
        _tourManager = new TourManager(_tourRepository);
        
        mainVM = new MainViewModel(_tourManager, logButtonViewModel, tourButtonViewModel);
    }

    [Test]
    public void Update_Button_CantExecute_On_SelectedTour()
    {
        mainVM.SelectedTour = null;
        bool canExecute = tourButtonViewModel.DeleteTourCommand.CanExecute(null);
        Assert.That(!canExecute);
    }
    [Test]
    public void Update_Button_CanExecute_On_SelectedTour() { 
        mainVM.SelectedTour = mainVM.Tours[0];

        bool canExecute = tourButtonViewModel.DeleteTourCommand.CanExecute(null);
        Assert.That(canExecute);
    }
    [Test]
    public void DeleteTour()
    {
        tourButtonViewModel.EventTriggerUnitTest(new TourModel() { FileName = "" }, "AddTourButtonClicked");
        int expectedCount = mainVM.Tours.Count - 1;
        
        mainVM.SelectedTour = mainVM.Tours.Last();
        tourButtonViewModel.EventTriggerUnitTest(mainVM.SelectedTour, "DeleteTourButtonClicked");
        Assert.That(expectedCount == mainVM.Tours.Count);
    }
    [Test]
    public void AddLog_SecondPrecision()
    {
        DateTime excpectedTime = DateTime.Now;
        // Act
        tourButtonViewModel.EventTriggerUnitTest(new TourModel() { FileName=""}, "AddTourButtonClicked");
        mainVM.SelectedTour = mainVM.Tours.Last();
        int logsCount = mainVM.SelectedTour.Logs.Count;
        int expectedLogsCount = logsCount + 1;
        logButtonViewModel.EventTriggerUnitTest(new LogModel(),"AddLogButtonClicked");

        //buttonViewModel.AddNewLogCommand.Execute(null);    // simulate button click
        
        // Assert
        var actualLogsCount = mainVM.SelectedTour.Logs.Count;

        Assert.That(actualLogsCount, Is.EqualTo(expectedLogsCount));
        var actualTime = DateTime.Parse(mainVM.SelectedTour.Logs[actualLogsCount - 1].Date);
        Assert.That(actualTime.Second, Is.EqualTo(excpectedTime.Second));
    }
    [Test]
    public void Export_Tour_CantExecute_On_SelectedTour()
    {
        mainVM.SelectedTour = null;
        bool canExecute = mainVM.TourExport.CanExecute(null);
        Assert.That(!canExecute);
    }
    [Test]
    public void Export_Tour_CanExecute_On_SelectedTour()
    {
        mainVM.SelectedTour = mainVM.Tours[0];

        bool canExecute = mainVM.TourExport.CanExecute(null);
        Assert.That(canExecute);
    }
    [Test]
    public void TourPopUp_OkCommand_Test_Fail()
    {
        var popUpViewModel = new NewTourPopupViewModel(new TourModel()
        {
            Name="Test",
            Description="Test",
            StartLocation="Test",
            EndLocation=""
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(!popUpViewModel.Result);
    }
    [Test]
    public void TourPopUp_OkCommand_Test_Sucess()
    {
        var popUpViewModel = new NewTourPopupViewModel(new TourModel()
        {
            Name = "Test",
            Description = "Test",
            StartLocation = "Test",
            EndLocation = "Test"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(popUpViewModel.Result);
    }
    [Test]
    public void TourPopUp_CancelCommand_Test()
    {
        var popUpViewModel = new NewTourPopupViewModel(new TourModel()
        {
            Name = "Test",
            Description = "Test",
            StartLocation = "Test",
            EndLocation = "Test"
        });
        popUpViewModel.CancelCommand.Execute(null);
        Assert.That(!popUpViewModel.Result);
    }
    [Test]
    public void LogPopUp_OkCommand_Test_Fail()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            TotalTime = "Test",
            TotalDistance = "",
            Comment = "Test",
            Difficulty = "1",
            Rating = "1"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(!popUpViewModel.Result);
    }
    [Test]
    public void LogPopUp_OkCommand_Test_Sucess()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "1",
            Rating = "1"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(popUpViewModel.Result);
    }
    [Test]
    public void LogPopUp_CancelCommand_Test()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "1",
            Rating = "1"
        });
        popUpViewModel.CancelCommand.Execute(null);
        Assert.That(!popUpViewModel.Result);
    }
    [Test]
    public void LogPopUp_Big_Number_Test()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "12",
            Rating = "1"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(popUpViewModel.LogModel.Difficulty=="10");
    }
    [Test]
    public void LogPopUp_Small_Number_Test()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "1",
            Rating = "-10"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(popUpViewModel.LogModel.Rating == "0");
    }
    [Test]
    public void LogPopUp_Unparsable_Number_Test()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "1a",
            Rating = "1"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(!popUpViewModel.Result);
    }
    [Test]
    public void LogPopUp_Decimal_Point_To_Comma_Number_Test()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "5.5",
            Rating = "1"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(popUpViewModel.LogModel.Difficulty == "5,5");
    }
    [Test]
    public void LogPopUp_Date_Parse_Test()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            Date= "01.01.2001 00:00:01",
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "1",
            Rating = "1"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(popUpViewModel.Result);
    }
    [Test]
    public void LogPopUp_Date_Fail_Parse_Test()
    {
        var popUpViewModel = new NewLogPopupViewModel(new LogModel()
        {
            Date = "01.111.2001 00:00:01",
            TotalTime = "Test",
            TotalDistance = "Test",
            Comment = "Test",
            Difficulty = "1",
            Rating = "1"
        });
        popUpViewModel.OkCommand.Execute(null);
        Assert.That(!popUpViewModel.Result);
    }
    [Test]
    public void Export_PDF_CantExecute_On_SelectedTour()
    {
        mainVM.SelectedTour = null;
        bool canExecute = mainVM.PdfGenerierenCommand.CanExecute(null);
        Assert.That(!canExecute);
    }
    [Test]
    public void Export_PDF_CanExecute_On_SelectedTour()
    {
        mainVM.SelectedTour = mainVM.Tours[0];

        bool canExecute = mainVM.PdfGenerierenCommand.CanExecute(null);
        Assert.That(canExecute);
    }
}
