namespace Community_Portal
{
    public static class Services
    {
        public static string NotFoundMessage(string path)
        {
            return $"No {path} with that Id";
        }
        public static string UnauthorizedMessage(string path)
        {
            return "Your login credentials don't match an account in our system";
        }
    }
}