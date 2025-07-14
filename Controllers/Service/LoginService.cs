namespace firstWeb.Controllers.Service
{
    public class LoginService
    {
        public void checkLogin(string username, string password)
        {
            

            Console.WriteLine($"Username: {username}, Password: {password}");

        }

        public bool IsValidUser(string username, string password)
        {
         

            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }

    }
    }
