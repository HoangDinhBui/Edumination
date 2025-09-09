namespace Edumination.Api.Dtos
{
    public class QuestionAnswerKeyCreateDto
    {
        public string KeyJson { get; set; }
    }

    public class QuestionAnswerKeyUpdateDto
    {
        public string KeyJson { get; set; }
    }

    public class QuestionAnswerKeyDto
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string KeyJson { get; set; }
    }
}