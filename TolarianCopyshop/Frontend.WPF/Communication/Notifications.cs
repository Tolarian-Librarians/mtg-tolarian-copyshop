using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Tolarian.Copyshop.Fontend.WPF.Communication
{
    public static class Notifications
    {
        private static NotifyIcon _notifyIcon;

        private static void SetNotifyIcon()
        {
            if (_notifyIcon != null)
            {
                return;
            }

#pragma warning disable DF0024 // Marks undisposed objects assinged to a field, originated in an object creation.
            _notifyIcon = new NotifyIcon
            {
#pragma warning disable DF0033 // Marks undisposed objects assinged to a property, originated from a method invocation.
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
#pragma warning restore DF0033 // Marks undisposed objects assinged to a property, originated from a method invocation.

            };
            _notifyIcon.BalloonTipClosed += (_, __) => _notifyIcon.Visible = false;
#pragma warning restore DF0024 // Marks undisposed objects assinged to a field, originated in an object creation.
        }

        public static void SendNotification(string header, string message, ToolTipIcon icon)
        {
            SetNotifyIcon();
            _notifyIcon.Visible = true;
            _notifyIcon.ShowBalloonTip(3000, header, message, icon);
        }
    }
}
