#region Using

using System;

#endregion

namespace VkAPIAsync.Authorization
{
    /// <summary>
    ///     Логика взаимодействия для WpfAuthWindow.xaml
    /// </summary>
    public partial class WpfAuthWindow
    {
        public WpfAuthWindow()
        {
            InitializeComponent();

            var page = new WpfAuthPage(this);
            page.Authorized += (sender, args) =>
                {
                    if (AuthorizedCopy != null)
                    {
                        AuthorizedCopy(this, EventArgs.Empty);
                    }
                };

            MainFrame.Navigate(page);
        }

        public event EventHandler AuthorizedCopy;
    }
}