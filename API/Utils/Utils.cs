using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace API.Utils
{
    public static class Utils
    {
        private static bool RegexMatch(string pattern, string text)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(text);
            return match.Success;
        }
        public static bool IsValidEmail(string email)
        {
            return RegexMatch(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",email);
        }
        public static int IsPasswordValid(string password)
        {

            if (password.Length < 8)
                return 900;
            if (password.Any(char.IsUpper) == false)
                return 901;
            if (password.Any(char.IsLower) == false)
                return 902;
            if (password.Any(char.IsNumber) == false)
                return 903;
            if (RegexMatch(@"(?=.*\W)",password) == false)
                return 904;
            return 200;
        }
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!?.,><}{|@#$%^&*";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}