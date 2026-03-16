namespace ToDoList.Service.Common.Exception
{
    public class DomainValidationException(IEnumerable<string> errors) : System.Exception("Validation failed")
    {
        public IEnumerable<string> Errors { get; } = errors;
    }
}
