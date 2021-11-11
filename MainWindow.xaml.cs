using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace wpa2psk_secure_password_generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void genPwdBtn_Click(object sender, RoutedEventArgs e)
        {
            // Generate a 63 char wifi password as the plaintext
            string plaintext = Vault.GenerateWifiPassword();

            // Generate a key from the user's passphrase input
            byte[] key = Vault.GeneratePrivateKey(passphraseTextBox.Text);

            // Encrypt the new WiFi password with the generated key 
            byte[] ivPlusCipher = Vault.EncryptStringToBytes_AesCBC(plaintext, key);

            // Save the ciphertext to file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Cipher file (*.cipher)|*.cipher";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, ivPlusCipher);

            // Display password to user
            genPwdTextBox.Text = plaintext;
        }
    }
}
