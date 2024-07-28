using System;
using System.ComponentModel.DataAnnotations;
using ReactiveUI;

namespace CustomControlSample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        SelectedDate = DateOnly.FromDateTime(DateTime.Now);
    }
    
    private DateOnly _selectedDate;

    public DateOnly SelectedDate
    {
        get => _selectedDate;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDate, value);
        }
    }
    
    // the initial value is 6
    private int _NumberOfStars = 6;

    /// <summary>
    /// Gets or sets the number of stars
    /// </summary>
    public int NumberOfStars
    {
        get { return _NumberOfStars; }
        set { this.RaiseAndSetIfChanged(ref _NumberOfStars, value); }
    }


    // the initial value is 2
    private int _RatingValue = 2;

    /// <summary>
    /// Gets or sets the current rating value. 
    /// It must be between 0 and 5
    /// </summary>
    [Range(0, 4)]
    public int RatingValue
    {
        get { return _RatingValue; }
        set { this.RaiseAndSetIfChanged(ref _RatingValue, value); }
    }
}