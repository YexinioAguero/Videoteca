namespace Videoteca.Models
{
    public class EpisodeSeries
    {
        public int id_serie { get; set; }
        public string title { get; set; }

        public string url { get; set; }

        public Episode[] episodes { get; set; }

    }
}
