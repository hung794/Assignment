using System.Text.RegularExpressions;

namespace Assignment.Util
{
    internal class Validate
    {
        public bool CheckText(string text)
        {
            return text != "";
        }

        public bool CheckNumber(string number)
        {
            Regex regex = new Regex("^[0-9]{8,10}$");
            return regex.IsMatch(number);
        }

        public bool CheckPassword(string password)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]{8,}$");
            return regex.IsMatch(password);
        }

        public bool CheckEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(email);
        }

        public bool CheckLinkImage(string url)
        {
            Regex regex = new Regex(@"/(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png)/g;");
            return regex.IsMatch(url);
        }

        public bool CheckLinkMusic(string url)
        {
            Regex regex = new Regex(@"^(https?|ftp|file):\/\/(www.)?(.*?)\.(mp3)$");
            return regex.IsMatch(url);
        }
    }
}
