using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
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
            // Save the ciphertext to file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Cipher file (*.cipher)|*.cipher";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Generate a 63 char wifi password as the plaintext
                string plaintext = Vault.GenerateWifiPassword();

                // Generate a key from the user's passphrase input
                byte[] key = Vault.GeneratePrivateKey(passphraseGenTextBox.Text);

                // Encrypt the new WiFi password with the generated key 
                byte[] ivPlusCipher = Vault.EncryptStringToBytes_AesCBC(plaintext, key);

                File.WriteAllBytes(saveFileDialog.FileName, ivPlusCipher);

                // Display password to user
                genPwdTextBox.Text = plaintext;
            }
        }

        private void loadPwdFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                passwordFileTextBox.Text = openFileDialog.FileName;
            }
        }

        private void loadPwdBtn_Click(object sender, RoutedEventArgs e)
        {
            // Store decrypted password
            string password;

            // Get IV and ciphertext from file (return if file is not found)
            byte[] ivPlusCipher;
            try
            {
                ivPlusCipher = File.ReadAllBytes(passwordFileTextBox.Text);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is IOException)
                {
                    loadPwdTextBox.Text = "Error: Password file not found";
                    return;
                }
                throw;
            }

            // Generate key
            byte[] key = Vault.GeneratePrivateKey(passphraseLoadTextBox.Text);

            // Decrypt password using the generated key
            try
            {
                password = Vault.DecryptStringFromBytes_AesCBC(ivPlusCipher, key);
            }
            catch (CryptographicException)
            {
                loadPwdTextBox.Text = "Error: Invalid key or corrupted password file";
                return;
            }

            // Display password to user
            loadPwdTextBox.Text = password;
        }
    }
}
