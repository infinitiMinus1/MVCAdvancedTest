using MVCData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories
{
    public interface IVerhuringRepository
    {
        Film? GetFilm(int id);
        void AddVerhuur(Verhuring verhuring);
        void RemoveMovieFromStock(int id);
        Klant? SearchForInlog(int p, string k);
        IEnumerable<Genre> GetAllGenres();
        IEnumerable<Film> GetFilmsByGenre(int genreId);
        public void AddMovieToStock(int id);
        string GetGenreNaam(int id);
    }
}
