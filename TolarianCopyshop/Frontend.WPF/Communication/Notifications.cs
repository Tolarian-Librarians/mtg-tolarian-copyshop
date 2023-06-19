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

            _notifyIcon = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
            };
            _notifyIcon.BalloonTipClosed += (_, __) => _notifyIcon.Visible = false;
        }

        public static void SendNotification(string header, string message, ToolTipIcon icon)
        {
            SetNotifyIcon();
            _notifyIcon.Visible = true;
            _notifyIcon.ShowBalloonTip(3000, header, message, icon);
        }
    }
}