using Microsoft.EntityFrameworkCore;
using MVCData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories
{
    public class SQLVerhuringRepository : IVerhuringRepository
    {
        private readonly VideoVerhuurContext context;
        public SQLVerhuringRepository(VideoVerhuurContext context)
        {
            this.context = context;
        }
        public void AddVerhuur(Verhuring verhuring)
        {
            context.Verhuringen.Add(verhuring);
            context.SaveChanges();
        }

        public Film? GetFilm(int id)
        {
            return context.Films.Find(id);
        }
        public void RemoveMovieFromStock(int id)
        {
            Film? film = context.Films.Find(id);
            film!.InVoorraad -= 1;
            film.UitVoorraad += 1;
            context.SaveChanges();
        }

        public void AddMovieToStock(int id)
        {
            Film? film = context.Films.Find(id);
            film!.InVoorraad += 1;
            film.UitVoorraad -= 1;
            context.SaveChanges();
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return context.Genres.AsNoTracking();
        }

        public Klant? SearchForInlog(int p, string k)
        {
            return context.Klanten.Where(klant => klant.Postcode == p && klant.VoorNaam == k ).FirstOrDefault();
        }

        public IEnumerable<Film>? GetFilmsByGenre(int genreId)
        {
            return context.Films.Include(g => g.Genre).Where(film => film.GenreId == genreId).AsNoTracking();
        }

        public string? GetGenreNaam(int id)
        {
            return context.Genres.Where(g => g.GenreId == id).Select(n => n.GenreNaam).FirstOrDefault();
        }
    }
}
