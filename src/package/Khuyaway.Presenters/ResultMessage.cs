namespace Khuyaway.Presenters;

public class ResultMessage
{
    public const string SectionKey = nameof(ResultMessage);
    public Message ValidationError { get; set; }
    public Message ServerError { get; set; }

    public class Message
    {
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}