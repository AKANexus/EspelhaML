using System.Text.Json.Serialization;

namespace EspelhaML.DTO
{
    public class QuestionRootDto : ErrorDto
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("answer")]
        public Answer? Answer { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("deleted_from_listing")]
        public bool DeletedFromListing { get; set; }

        [JsonPropertyName("hold")]
        public bool Hold { get; set; }

        [JsonPropertyName("item_id")]
        public string? ItemId { get; set; }

        [JsonPropertyName("seller_id")]
        public int SellerId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("from")]
        public From? From { get; set; }

    }

    public class Answer
    {
        public Answer(DateTime dateCreated, string status, string text)
        {
            DateCreated = dateCreated;
            Status = status;
            Text = text;
        }

        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

    }

    public class From
    {
        public From(int id, int answeredQuestions)
        {
            Id = id;
            AnsweredQuestions = answeredQuestions;
        }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("answered_questions")]
        public int AnsweredQuestions { get; set; }

    }
}
