namespace QuanLyVanPhongCongChung.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("Access is denied.") { }
    public ForbiddenAccessException(string message) : base(message) { }
}
