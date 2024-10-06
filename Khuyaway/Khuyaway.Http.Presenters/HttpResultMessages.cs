namespace Khuyaway.Http.Presenters;

public class HttpResultMessages
{
    public const string SectionKey = nameof(HttpResultMessages);
    public Messages ServerError { get; set; }
    public Messages ValidationError { get; set; }
    public Messages Unauthorized { get; set; }

    public class Messages
    {
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}