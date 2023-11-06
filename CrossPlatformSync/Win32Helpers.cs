using System.Runtime.InteropServices;

namespace CrossPlatformSync;

internal static partial class Win32Helpers
{
    [LibraryImport("User32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool CloseClipboard();

    [LibraryImport("User32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool EmptyClipboard();

    [LibraryImport("User32")]
    public static partial IntPtr GetClipboardData(int uFormat);

    [LibraryImport("User32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool IsClipboardFormatAvailable(int format);

    [LibraryImport("User32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool OpenClipboard(IntPtr hWndNewOwner);

    [LibraryImport("User32")]
    public static partial IntPtr SetClipboardData(int uFormat, IntPtr hMem);
}