using H.NotifyIcon.Core;
using Microsoft.Toolkit.Uwp.Notifications;
using Serilog;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CrossPlatformSync;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Month)
            .CreateLogger();

        try
        {
            using var icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream($"{nameof(CrossPlatformSync)}.copy_icon.ico")!);
            using var trayIcon = new TrayIconWithContextMenu
            {
                Icon = icon.Handle,
                ToolTip = "ת����֤��",
            };

            trayIcon.ContextMenu = new PopupMenu
            {
                Items =
                {
                    new PopupMenuItem("�˳�", (_, _) =>
                    {
                        trayIcon.Dispose();
                        Environment.Exit(0);
                    }),
                },
            };

            trayIcon.Create();

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            var app = builder.Build();

            ToastNotificationManagerCompat.OnActivated += toastArgs => Copy(toastArgs.Argument);
            app.MapGet("/verificationcode/{code}", (string code) =>
            {
                new ToastContentBuilder()
                    .AddText("��֤��")
                    .AddText(code)
                    .AddArgument(code)
                    .AddButton("copy", "������֤��", ToastActivationType.Background, code)
                    .Show(toast => toast.ExpirationTime = DateTime.Now.AddMinutes(5));

                return "ok";
            });

            app.Run();

            ToastNotificationManagerCompat.Uninstall();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "��������");
            throw;
        }
    }

    static void Copy(string text)
    {
        if (!Win32Helpers.OpenClipboard(IntPtr.Zero))
        {
            Log.Logger.Debug("�򿪼�����ʧ�ܡ�");
            Copy(text);
            return;
        }

        nint ptr = Marshal.StringToHGlobalUni(text);

        try
        {
            Win32Helpers.EmptyClipboard();
            Win32Helpers.SetClipboardData(13, ptr);
            Win32Helpers.CloseClipboard();
            Log.Logger.Debug("���Ƶ���������ɣ�{text}��", text);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Copy Error");
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}