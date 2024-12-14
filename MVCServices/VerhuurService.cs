using MVCData.Models;
using MVCData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCServices
{
    public class VerhuurService
    {
        private readonly IVerhuringRepository verhuurRepository;
        public VerhuurService(IVerhuringRepository verhuurRepository)
        {
            this.verhuurRepository = verhuurRepository;
        }

        public Klant? SearchKlant(int postcode, string naam)
        {
            return verhuurRepository.SearchForInlog(postcode, naam);
        }

        public IEnumerable<Genre> GetGenres()
        {
            return verhuurRepository.GetAllGenres();
        }

        public IEnumerable<Film> GetFilmByGenre(int genreId)
        {
            return verhuurRepository.GetFilmsByGenre(genreId);
        }

        public void GetMovieFromStock(int id)
        {
            Film? film = verhuurRepository.GetFilm(id);
            verhuurRepository.RemoveMovieFromStock(id);
        }

        public void AddMovieStock(int id)
        {
            Film? film = verhuurRepository.GetFilm(id);
            verhuurRepository.AddMovieToStock(id);
        }

        public Film? GetMovie(int id)
        {
           return verhuurRepository.GetFilm(id);
        }

        public string GetGenreNaam(int id)
        {
           return verhuurRepository.GetGenreNaam(id);
        }

        public void AddVerhuur(Verhuring verhuur)
        {
            verhuurRepository.AddVerhuur(verhuur);
        }
    }

}

