namespace Personal_Blogging_Platform.Exceptions
{
    public class NotFoundException: AppException
    {
        public NotFoundException(string message) : base(message, 404) { }
   
    }
}
