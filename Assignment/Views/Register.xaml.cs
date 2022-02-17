using Assignment.Entity;
using Assignment.Service;
using Assignment.Util;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        private string imageUrl = "";
        private int gender;
        public Register()
        {
            this.InitializeComponent();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            FirstName.Text = "";
            FirstNameError.Text = "";
            LastName.Text = "";
            LastNameError.Text = "";
            Phone.Text = "";
            PhoneError.Text = "";
            Address.Text = "";
            AddressError.Text = "";
            Email.Text = "";
            EmailError.Text = "";
            AddImageSuccessText.Text = "";
            imageUrl = "";
            AvatarError.Text = "";
            Birthday.Date = null;
            BirthdayError.Text = "";
            Male.IsChecked = true;
            Password.Password = "";
            PasswordError.Text = "";
            Introduction.Text = "";
        }

        private void LoginForm_Click(object sender, RoutedEventArgs e)
        {
            _ = Frame.Navigate(typeof(Login));
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            Validate validate = new Validate();
            int count = 0;
            if (!validate.CheckText(FirstName.Text))
            {
                FirstNameError.Text = "First Name is required!";
                count += 1;
            }
            else FirstNameError.Text = "";
            if (!validate.CheckText(LastName.Text))
            {
                LastNameError.Text = "Last Name is required!";
                count += 1;
            }
            else LastNameError.Text = "";
            if (!validate.CheckText(Phone.Text))
            {
                PhoneError.Text = "Phone Number is required!";
                count += 1;
            }
            else if (!validate.CheckNumber(Phone.Text))
            {
                PhoneError.Text = "Invalid Phone Number!";
                count += 1;
            }
            else PhoneError.Text = "";
            if (!validate.CheckText(Address.Text))
            {
                AddressError.Text = "Address is required!";
                count += 1;
            }
            else AddressError.Text = "";
            if (Birthday.Date == null)
            {
                BirthdayError.Text = "Birthday is required!";
                count += 1;
            }
            else BirthdayError.Text = "";
            if (!validate.CheckText(Email.Text))
            {
                EmailError.Text = "Email is required!";
                count += 1;
            }
            else if (!validate.CheckEmail(Email.Text))
            {
                EmailError.Text = "Invalid Email!";
                count += 1;
            }
            else EmailError.Text = "";
            if (imageUrl == "")
            {
                AvatarError.Text = "Avatar is required!";
                count += 1;
            }
            else AvatarError.Text = "";
            if (!validate.CheckText(Password.Password.ToString()))
            {
                PasswordError.Text = "Password is required!";
                count += 1;
            }
            else
            {
                if (!validate.CheckPassword(Password.Password.ToString()))
                {
                    PasswordError.Text = "Password must be longer than 8 characters!";
                    count += 1;
                }
                else PasswordError.Text = "";
            }
            if (count == 0)
            {
                Account account = new Account()
                {
                    firstName = FirstName.Text,
                    lastName = LastName.Text,
                    phone = Phone.Text,
                    address = Address.Text,
                    birthday = Birthday.Date.Value.ToString("yyyy-MM-dd"),
                    email = Email.Text,
                    avatar = imageUrl,
                    gender = gender,
                    password = Password.Password.ToString(),
                    introduction = Introduction.Text
                };
                AccountService accountService = new AccountService();
                bool isSuccess = await accountService.Register(account);
                if (isSuccess)
                {
                    Dialog.Content = "Register Successfully!";
                    await Dialog.ShowAsync();
                }
                else
                {
                    FirstNameError.Text = "";
                    LastNameError.Text = "";
                    PhoneError.Text = "";
                    AddressError.Text = "";
                    AddImageSuccessText.Text = "";
                    AvatarError.Text = "";
                    BirthdayError.Text = "";
                    PasswordError.Text = "";
                    EmailError.Text = "Email already exists!";
                    Dialog.Content = "Register Failed, Please Try Again!";
                    await Dialog.ShowAsync();
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton Gender = sender as RadioButton;
            string genderString = Gender.Content.ToString();
            switch (genderString)
            {
                case "Male":
                    gender = 1;
                    break;
                case "Female":
                    gender = 2;
                    break;
                default:
                    gender = 0;
                    break;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _ = Frame.Navigate(typeof(Login));
        }

        private async void AddImage_Click(object sender, RoutedEventArgs e)
        {
            AddImageSuccessText.Text = "";
            string result = await FileCloudinary.ChooseFile("image");
            if(result != "")
            {
                AddImageSuccessText.Text = "Success!";
                imageUrl = result;
            }
        }
    }
}
