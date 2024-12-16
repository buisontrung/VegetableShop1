namespace Order.Service
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? IpAddress { get; }
    }
}
