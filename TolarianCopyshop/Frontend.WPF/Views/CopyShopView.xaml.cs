using MahApps.Metro.Controls;

namespace Tolarian.Copyshop.Fontend.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CopyShopView : MetroWindow
    {
        private static CopyShopView _copyShopView;

        public CopyShopView()
        {
            _copyShopView = this;
            InitializeComponent();
        }

        internal static CopyShopView GetInstance()
            => _copyShopView;
    }
}