using System.ComponentModel.DataAnnotations;

namespace Alura.Filmes.API.Models
{
    public class Sessao
    {
        public int? FilmeId { get; set; }

        public int? CinemaId { get; set; }

        #region virtuals
        public virtual Filme Filme { get; set; }

        public virtual Cinema Cinema { get; set; }
        #endregion
    }
}
