using Microsoft.Web.Administration;

namespace JexusManager.Dialogs
{
    public class ConnectAsItem
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public VirtualDirectory Element { get; set; }

        public ConnectAsItem(VirtualDirectory virtualDirectory)
        {
            UserName = virtualDirectory?.UserName ?? string.Empty;
            Password = virtualDirectory?.Password ?? string.Empty;
            Element = virtualDirectory;
        }

        public void Apply()
        {
            if (Element == null)
            {
                return;
            }

            Element.UserName = UserName;
            Element.Password = Password;
        }
    }
}
