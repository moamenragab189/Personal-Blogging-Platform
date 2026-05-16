namespace Personal_Blogging_Platform.Exceptions
{
    public class UnauthorizedException: AppException
    {
        public UnauthorizedException(string message) : base(message, 401) { }
    }
}
