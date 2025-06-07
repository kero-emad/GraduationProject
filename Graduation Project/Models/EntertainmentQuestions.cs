using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class EntertainmentQuestions:Questions
    {
        
        public Section section { get; set; }

        public enum Section
        {
            MusicAndSongs,
            MoviesAndTVShows,
            ArtAndPaintings,
            GeneralKnowledge,
            SportsAndGames
        }

    }
}
