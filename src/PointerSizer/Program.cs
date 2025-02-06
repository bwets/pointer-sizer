using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PointerSizer;

internal static class Program
{
    private static NotifyIcon? _icon;
    private static int _addKey1;
    private static int _subKey1;
    private static int _resetKey;
    private static int _addKey2;
    private static int _subKey2;

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var context = new ApplicationContext();

        _addKey1 = HotKeyManager.RegisterHotKey(Keys.Add, KeyModifiers.Windows | KeyModifiers.Shift);
        _addKey2 = HotKeyManager.RegisterHotKey(Keys.I, KeyModifiers.Windows | KeyModifiers.Shift);
        _subKey1 = HotKeyManager.RegisterHotKey(Keys.Subtract, KeyModifiers.Windows | KeyModifiers.Shift);
        _subKey2 = HotKeyManager.RegisterHotKey(Keys.D, KeyModifiers.Windows | KeyModifiers.Shift);
        _resetKey = HotKeyManager.RegisterHotKey(Keys.Multiply, KeyModifiers.Windows | KeyModifiers.Shift);

        HotKeyManager.HotKeyPressed += HotKeyManager_HotKeyPressed;

        _icon = new NotifyIcon();
        _icon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("PointerSizer.cursor-256.ico"));
        _icon.Visible = true;
        _icon.Text = "Pointer Sizer";
        _icon.ContextMenuStrip = new ContextMenuStrip();
        _icon.ContextMenuStrip.Items.Add("Exit", null, (sender, args) =>
        {
            context.ExitThread();
        });
        
        Application.Run(context);
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        Release();
    }

    
   
    private static void Release()
    {
        if (_icon == null) return;
        _icon.Visible = false;
        _icon.Dispose();
        _icon = null;
        try
        {
            HotKeyManager.UnregisterHotKey(_addKey1);
            HotKeyManager.UnregisterHotKey(_subKey1);
            HotKeyManager.UnregisterHotKey(_addKey2);
            HotKeyManager.UnregisterHotKey(_subKey2);
            HotKeyManager.UnregisterHotKey(_resetKey);

        }
        catch (Exception e)
        {
            // ignored
        }
    }
    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        Release();
    }

    private static void HotKeyManager_HotKeyPressed(object? sender, HotKeyEventArgs e)
    {
        switch (e.Key)
        {
            case Keys.Add:
            case Keys.I:
                Pointer.Increase();
                break;
            case Keys.Subtract:
            case Keys.D:
                Pointer.Decrease();
                break;
            case Keys.Multiply:
                Pointer.SetSize(20);
                break;
        }
    }

 
 
}