using System.Runtime.InteropServices;

namespace PointerSizer;

internal static class Pointer
{
    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    public static extern bool SystemParametersInfoRead(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

    public static void SetSize(uint size)
    {
        // https://stackoverflow.com/questions/60104778/change-and-update-the-size-of-the-cursor-in-windows-10-via-powershell
        SystemParametersInfo(0x2029, 0, size, 0x01);
            
    }

    public static uint GetSize()
    {
        uint xx = 0;
        SystemParametersInfoRead(0x2028, 0, ref xx, 0x01);
        return xx;
    }

    public static void Increase()
    {
        var size = GetSize();
        size += 20;
        SetSize(size);
    }

    public static void Decrease()
    {
        var size = GetSize();
        size -= 20;
        SetSize(size);
    }

}