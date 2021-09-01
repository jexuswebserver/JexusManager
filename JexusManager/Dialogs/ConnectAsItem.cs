using JexusManager.Features;
using Microsoft.Web.Administration;

namespace JexusManager.Dialogs
{
    public class ConnectAsItem : IItem<ConnectAsItem>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public ConfigurationElement Element
        {
            get { return _virtualDirectory; }
            set { _virtualDirectory = (VirtualDirectory)value; }
        }

        public string Flag { get; set; }

        private VirtualDirectory _virtualDirectory;

        public ConnectAsItem(VirtualDirectory virtualDirectory)
        {
            UserName = virtualDirectory?.UserName ?? string.Empty;
            Password = virtualDirectory?.Password ?? string.Empty;
            _virtualDirectory = virtualDirectory;
        }

        public void Apply()
        {
            if (Element == null)
            {
                return;
            }

            _virtualDirectory.UserName = UserName;

            // IMORTANT: force creating the config elements.
            _virtualDirectory.Application.Server.CommitChanges();
            _virtualDirectory.SetPassword(Password);
        }

        public bool Match(ConnectAsItem other)
        {
            return other != null && other.UserName == UserName && other.Password == Password;
        }

        public bool Equals(ConnectAsItem other)
        {
            return Match(other);
        }
    }
}
