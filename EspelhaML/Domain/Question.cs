namespace EspelhaML.Domain
{
    public class Question : EntityBase
    {
        public Question(long id, string questionText, long askerId, DateTime dateAsked, string questionStatus, string itemMlb)
        {
            Id = id;
            QuestionText = questionText;
            AskerId = askerId;
            DateAsked = dateAsked;
            QuestionStatus = questionStatus;
            ItemMlb = itemMlb;
        }
        public long Id { get; set; }
        public string QuestionText { get; set; }
        public long AskerId { get; set; }
        public DateTime DateAsked { get; set; }
        public DateTime? DateReplied { get; set; }
        public string QuestionStatus { get; set; }
        public string ItemMlb { get; set; }
        public string? AnswerStatus { get; set; }
        public string? AnswerText { get; set; }
    }
}
