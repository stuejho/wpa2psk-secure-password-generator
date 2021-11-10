using System.Windows.Input;

namespace wpa2psk_secure_password_generator
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Exit", // text
                "Exit", // name
                typeof(CustomCommands), // type registering the command
                new InputGestureCollection() // button combination to trigger command
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );
    }
}
