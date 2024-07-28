using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace CustomControlSample.Controls;

[TemplatePart("PART_TimeCalendarPresenter", typeof(ItemsControl))]
public class TimeCalendarControl : TemplatedControl
{
    public TimeCalendarControl()
    {
        UpdateStars();
    }
    
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        
        if (!e.Handled)
        {
            if (e.Delta.Y < 0)
            {
                DatesOnlies.Remove(DatesOnlies.FirstOrDefault());
                DatesOnlies.Add(DatesOnlies.Last().AddDays(1));
            }
            else
            {
                DatesOnlies.Remove(DatesOnlies.LastOrDefault());
                DatesOnlies.Insert(0, DatesOnlies.FirstOrDefault().AddDays(-1));
            }
            
            e.Handled = true;
        }
        

    }
    
    private ItemsControl? _timeCalendarPresenter;
    
    public static readonly StyledProperty<int> MaxCountProperty = AvaloniaProperty.Register<TimeCalendarControl, int>
    (nameof(MaxCount),
        10,
        coerce: CoerceMaxCount);

    private static int CoerceMaxCount(AvaloniaObject avaloniaObject, int value)
    {
        return value > 15 ? 15 : value;
    }

    public static readonly DirectProperty<TimeCalendarControl, DateOnly> SelectedDateProperty =
        AvaloniaProperty.RegisterDirect<TimeCalendarControl, DateOnly>
        (nameof(SelectedDate),
            o => o.SelectedDate,
            (o, v) => o.SelectedDate = v);

    private DateOnly _selectedDate;

    public DateOnly SelectedDate
    {
        get => _selectedDate;
        set => SetAndRaise(SelectedDateProperty, ref _selectedDate, value);
    }

    public static readonly DirectProperty<TimeCalendarControl, ObservableCollection<DateOnly>> DatesOnliesProperty =
        AvaloniaProperty.RegisterDirect<TimeCalendarControl, ObservableCollection<DateOnly>>
        (nameof(DatesOnlies), 
            o => o.DatesOnlies,
            (o, v) => o.DatesOnlies = v);

    private ObservableCollection<DateOnly> _datesOnlies;

    public ObservableCollection<DateOnly> DatesOnlies
    {
        get => _datesOnlies;
        set => SetAndRaise(DatesOnliesProperty, ref _datesOnlies, value);
    }

    public int MaxCount
    {
        get => GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }
    
    private void UpdateStars()
    {
        var maxDate = SelectedDate.AddDays(MaxCount);
        
        var a = Enumerable
            .Range(1, MaxCount)
            .Select(i => SelectedDate.AddDays(i))
            .TakeWhile(d => d <= maxDate)
            .ToList();
        DatesOnlies = new ObservableCollection<DateOnly>(a);
    }

    // We override OnPropertyChanged of the base class. That way we can react on property changes
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        // if the changed property is the NumberOfStarsProperty, we need to update the stars
        if (change.Property == SelectedDateProperty)
        {
            UpdateStars();
        }
    }
    
    // We override what happens when the control template is being applied. 
    // That way we can for example listen to events of controls which are part of the template
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // if we had a control template before, we need to unsubscribe any event listeners
        if (_timeCalendarPresenter is not null)
        {
            _timeCalendarPresenter.PointerReleased -= StarsPresenter_PointerReleased;
        }

        // try to find the control with the given name
        _timeCalendarPresenter = e.NameScope.Find("PART_StarsPresenter") as ItemsControl;

        // listen to pointer-released events on the stars presenter.
        if (_timeCalendarPresenter != null)
        {
            _timeCalendarPresenter.PointerReleased += StarsPresenter_PointerReleased;
        }
    }
    
    private void StarsPresenter_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        // e.Source is the original source of this event. In our case, if the user clicked on a star, the original source is a Path.
        /*if (e.Source is Path star)
        {
            // The DataContext of the star is one of the numbers we have in the Stars-Collection. 
            // Let's cast the DataContext to an int. If that cast fails, use "0" as a fallback.
            Value = star.DataContext as int? ?? 0;
        }*/
    }
}