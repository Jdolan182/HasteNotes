using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Avalonia.Threading;
using Gma.System.MouseKeyHook;

namespace HasteNotes.Services;

public class GlobalKeyService : IDisposable
{
    private readonly IKeyboardMouseEvents _hook;
    private readonly Dictionary<Keys, Action> _callbacks = new();

    public GlobalKeyService()
    {
        _hook = Hook.GlobalEvents();
        _hook.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
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

    public void Dispose()
    {
        _hook.KeyDown -= OnKeyDown;
        _hook.Dispose();
        _callbacks.Clear();
    }
}
