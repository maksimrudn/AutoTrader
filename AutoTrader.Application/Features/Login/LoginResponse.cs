namespace AutoTrader.Application.Features.Login
{
    public class LoginResponse
    {
        public List<string>? SeccodeList { get; internal set; }
        public string? SelectedSeccode { get; internal set; }
        public string? ClientId { get; internal set; }
        public string? FreeMoney { get; internal set; }
        public string? Union { get; internal set; }
    }
}
