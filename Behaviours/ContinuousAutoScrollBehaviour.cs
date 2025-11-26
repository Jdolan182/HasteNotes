using System;                        // TimeSpan, EventArgs, Math
using Avalonia;                      // Point, Vector
using Avalonia.Controls;             // ScrollViewer
using Avalonia.Input;                // PointerEventArgs
using Avalonia.Threading;            // DispatcherTimer
using Avalonia.Xaml.Interactivity;   // Behavior<T>

namespace HasteNotes.Behaviours;

public class ContinuousAutoScrollDuringDragBehaviour : Behavior<ScrollViewer>
{
    public double EdgeDistance { get; set; } = 20.0;
    public double ScrollDelta { get; set; } = 5.0;

    private DispatcherTimer _timer;
    private bool _isDragging;
    private Point _lastPointerPosition;

    protected override void OnAttached()
    {
        base.OnAttached();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
        _timer.Tick += Timer_Tick;
        _timer.Start();

        AssociatedObject.AddHandler(InputElement.PointerMovedEvent, OnPointerMoved, handledEventsToo: true);
        AssociatedObject.AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, handledEventsToo: true);
        AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, OnPointerReleased, handledEventsToo: true);
    }

    protected override void OnDetaching()
    {
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
        if (!_isDragging) return;

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
