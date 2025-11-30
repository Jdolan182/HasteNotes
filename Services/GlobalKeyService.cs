using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Avalonia.Threading;
using Gma.System.MouseKeyHook;

namespace HasteNotes.Services;
public class GlobalKeyService : IDisposable
{
    private readonly IKeyboardMouseEvents _hook;
    private readonly Dictionary<Keys, Action> _callbacks = [];
    private bool _disposed;

    public GlobalKeyService()
    {
        _hook = Hook.GlobalEvents();
        _hook.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (_callbacks.TryGetValue(e.KeyCode, out var callback))
            Dispatcher.UIThread.Post(callback);
    }

    public void RegisterKey(Keys key, Action callback)
    {
        _callbacks[key] = callback;
    }

    public void UnregisterKey(Keys key)
    {
        _callbacks.Remove(key);
    }

    public void UnregisterAll()
    {
        _callbacks.Clear();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Suppress finalizer
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _hook.KeyDown -= OnKeyDown;
            _hook.Dispose();
            _callbacks.Clear();
        }

        _disposed = true;
    }

    ~GlobalKeyService()
    {
        Dispose(false);
    }
}
