using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;

namespace HasteNotes.Behaviours;

public class ContinuousAutoScrollDuringDragBehaviour : Behavior<ScrollViewer>
{
    public double EdgeDistance { get; set; } = 20.0;
    public double ScrollDelta { get; set; } = 5.0;

    private readonly DispatcherTimer _timer = new();
    private bool _isDragging;
    private Point _lastPointerPosition;

    protected override void OnAttached()
    {
        if (AssociatedObject is null)
            return;

        base.OnAttached();

        _timer.Interval = TimeSpan.FromMilliseconds(20);
        _timer.Tick += Timer_Tick;
        _timer.Start();

        AssociatedObject.AddHandler(InputElement.PointerMovedEvent, OnPointerMoved, handledEventsToo: true);
        AssociatedObject.AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, handledEventsToo: true);
        AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, OnPointerReleased, handledEventsToo: true);
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is null)
            return;

        _timer.Stop();
        AssociatedObject.RemoveHandler(InputElement.PointerMovedEvent, OnPointerMoved);
        AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnPointerPressed);
        AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnPointerReleased);
        base.OnDetaching();
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _isDragging = true;
        _lastPointerPosition = e.GetPosition(AssociatedObject);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        _lastPointerPosition = e.GetPosition(AssociatedObject);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (!_isDragging || AssociatedObject is null) 
            return;

        var pos = _lastPointerPosition;
        var height = AssociatedObject.Bounds.Height;

        if (pos.Y < EdgeDistance)
        {
            AssociatedObject.Offset = new Vector(0, Math.Max(0, AssociatedObject.Offset.Y - ScrollDelta));
        }
        else if (pos.Y > height - EdgeDistance)
        {
            AssociatedObject.Offset = new Vector(0, AssociatedObject.Offset.Y + ScrollDelta);
        }
    }
}
