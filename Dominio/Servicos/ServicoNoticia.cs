﻿using Dominio.Interfaces;
using Dominio.Interfaces.InterfaceServicos;
using Entidades.Entidades;
using Entidades.Entidades.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Servicos
{
    public class ServicoNoticia : IServicoNoticia
    {
        private readonly INoticia _INoticia;
        public ServicoNoticia(INoticia INoticia)
        {
            _INoticia = INoticia;
        }
        public async Task AdicionaNoticia(Noticia noticia)
        {
            var validarTitulo = noticia.ValidarPropriedadeString(noticia.Titulo, "Titulo");
            var validarInformacoes = noticia.ValidarPropriedadeString(noticia.Titulo, "Informacao"); ;

            if (validarTitulo && validarInformacoes)
            {
                noticia.DataAlteracao = DateTime.Now;
                noticia.DataCadastro = DateTime.Now;
                noticia.Ativo = true;

                await _INoticia.Adicionar(noticia);
            }
        }

        public async Task AtualizaNoticia(Noticia noticia)
        {
            var validarTitulo = noticia.ValidarPropriedadeString(noticia.Titulo, "Titulo");
            var validarInformacoes = noticia.ValidarPropriedadeString(noticia.Titulo, "Informacao"); ;

            if (validarTitulo && validarInformacoes)
            {
                noticia.DataAlteracao = DateTime.Now;
                noticia.DataCadastro = DateTime.Now;
                noticia.Ativo = true;

                await _INoticia.Atualizar(noticia);
            }
        }

        public async Task<List<Noticia>> ListarNoticiasAtivas()
        {
            return await _INoticia.ListaNoticias(n => n.Ativo);
        }

        public async Task<List<NoticiaViewModel>> ListarNoticiasCustomizada()
        {
            var listaNoticiaCustomizada = await _INoticia.ListaNoticiasCustomizada();
            var retorno = (
                from noticia in listaNoticiaCustomizada
                select new NoticiaViewModel
                {
                    Id = noticia.Id,
                    Titulo = noticia.Titulo,
                    Informacao = noticia.Informacao,
                    DataCadastro = string.Concat(noticia.DataCadastro.Day,"/", noticia.DataCadastro.Month,"/", noticia.DataCadastro.Year),
                    Usuario = SeparaEmail(noticia.ApplicationUser.Email)
                }).ToList();

            return retorno;
        }

        private string SeparaEmail(string email)
        {
            var stringEmail = email.Split('@');

            return stringEmail[0].ToString();
        }
    }   
    
}
