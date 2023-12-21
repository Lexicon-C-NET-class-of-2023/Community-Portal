namespace Community_Portal
{
    public static class Services
    {
        public static string NotFoundMessage(string path)
        {
            if (path == "login") return "Your login credentials don't match an account in our system";
            return $"No {path} with that Id";
        }
    }
}