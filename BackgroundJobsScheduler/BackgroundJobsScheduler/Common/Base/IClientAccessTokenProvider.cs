namespace BackgroundJobsScheduler.Common.Base
{
    public interface IClientAccessTokenProvider
    {
        string AccessToken { get; }
    }
}
