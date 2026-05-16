namespace Personal_Blogging_Platform.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message, 400) { }
    }
}
