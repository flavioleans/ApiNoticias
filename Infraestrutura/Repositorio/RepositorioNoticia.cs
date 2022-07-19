using Dominio.Interfaces;
using Entidades.Entidades;
using Infraestrutura.Configuracoes;
using Infraestrutura.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio
{
    public class RepositorioNoticia : RepositorioGenerico<Noticia>, INoticia
    {
        private readonly DbContextOptions<Contexto> _optionsBuilder;
        public RepositorioNoticia()
        {
            _optionsBuilder = new DbContextOptions<Contexto>();
        }
        public async Task<List<Noticia>> ListaNoticias(Expression<Func<Noticia, bool>> exNoticia)
        {
            using (var banco = new Contexto(_optionsBuilder))
            {
                return await banco.Noticias.Where(exNoticia).AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<Noticia>> ListaNoticiasCustomizada()
        {
            using (var banco = new Contexto(_optionsBuilder))
            {
                var listaNoticias = await (from noticia in banco.Noticias
                                     join usuario in banco.ApplicationUsers
                                     on noticia.UserId equals usuario.Id
                                     select new Noticia
                                     {
                                         Id = noticia.Id,
                                         Informacao = noticia.Informacao,
                                         Titulo = noticia.Titulo,
                                         DataCadastro = noticia.DataCadastro,
                                         ApplicationUser = usuario
                                     }
                                     ).AsNoTracking().ToListAsync();
                return listaNoticias;
            }
        }
    }
}
