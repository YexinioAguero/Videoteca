namespace Videoteca.Models
{
    public class CommentUser
    {

        public int comment_id { get; set; }

        public string userName { get; set; } = null!;

        public string? comment1 { get; set; }

        public string? image {get; set;}

        public int movies_series_id { get; set; }

        public DateTime? dateC { get; set; }
    }
}
