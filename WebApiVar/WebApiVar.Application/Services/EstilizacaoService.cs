using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SixLabors.ImageSharp.MetaData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class EstilizacaoService
    {
        private readonly VAREntities _ctx;

        public EstilizacaoService(VAREntities context)
        {
            _ctx = context;
        }

        public GenericResponse<TabEstilizacao> ObterCSS()
        {
            try
            {
                var response = _ctx.TabEstilizacao.FirstOrDefault(x => x.isAtivo == true);

                return new GenericResponse<TabEstilizacao>("success", response);
            }
            catch (Exception ex)
            {
                return new GenericResponse<TabEstilizacao>("failed", null);

            }
        }
        public List<SecaoValor> ObterTelaInicio()
        {
            try
            {
                List<SecaoValor> secaoValors = new List<SecaoValor>();

                var secoesAtivas = _ctx.tabSecao.ToList();
                var valoresSecaoCodigo = _ctx.tabValorSecaoCodigo.ToList();
                var valoresSecao = _ctx.tabValorSecao.ToList();

                foreach (var secao in secoesAtivas)
                {
                    var valoresAssociados = valoresSecaoCodigo
                        .Where(x => x.secaoCodigo == secao.codigo)
                        .Select(x => new SecaoValor
                        {
                            codigoSecao = secao.codigo,
                            secao = secao.descricao,
                            tipo = valoresSecao.FirstOrDefault(c => c.codigo == x.valorSecaoCodigo)?.tipo,
                            valor = valoresSecao.FirstOrDefault(c => c.codigo == x.valorSecaoCodigo)?.valor,
                            isativo = secao.isAtivo
                        })
                        .ToList();

                    secaoValors.AddRange(valoresAssociados);
                }
                return secaoValors;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> atualizarInicio(List<FileUploadModel> files, string urlVideo)
        {
            try
            {
                if (files == null || files.Count == 0)
                    throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");
                var fotosSalvas = new List<string>();

                var secoes = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "INICIO");

                foreach (var file in files)
                {
                    if (file.Data.Length > 0)
                    {
                        file.FileName = secoes?.codigo.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + $".{file.FileName.Split(".")[1]}";
                        // Salvar a foto em um diretório (por exemplo, "Fotos") e obter o caminho completo do arquivo salvo
                        string filePath = "C:/inetpub/wwwroot/images/telaInicio/" + file.FileName;

                        // Verificar se o diretório "Fotos" existe e criar se não existir
                        if (!Directory.Exists("C:/inetpub/wwwroot/images/telaInicio"))
                        {
                            Directory.CreateDirectory("C:/inetpub/wwwroot/images/telaInicio");
                        }

                        // Salvar o arquivo no diretório
                        File.WriteAllBytes(filePath, file.Data);

                        fotosSalvas.Add(filePath);
                        atualizarLogoUrlVideo(file.FileName, urlVideo);
                    }
                }
                return fotosSalvas;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void atualizarLogoUrlVideo(string nomeFoto, string urlVideo)
        {
            try
            {
                var secoes = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "INICIO");
                var tabvalorSecaoCodigo = _ctx.tabValorSecaoCodigo.Where(x => x.secaoCodigo == secoes.codigo).Select(c => c.valorSecaoCodigo).ToList();
                var valorSecao = _ctx.tabValorSecao.Where(c => tabvalorSecaoCodigo.Contains(c.codigo)).ToList();

                foreach (var item in valorSecao)
                {
                    if (item.tipo == "Imagem Logo")
                        item.valor = nomeFoto;
                    else if (item.tipo == "Url Video")
                        item.valor = urlVideo;

                    _ctx.tabValorSecao.Update(item);
                    _ctx.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public bool atualizarQuemSomos(QuemSomosRequest request)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "QUEM SOMOS");
                var codvaluesession = _ctx.tabValorSecaoCodigo.Where(x => x.secaoCodigo == secao.codigo).Select(x => x.valorSecaoCodigo);
                var valores = _ctx.tabValorSecao.Where(x => codvaluesession.Contains(x.codigo)).ToList();

                foreach (var item in valores)
                {
                    if (item.tipo == "Titulo Quem Somos")
                        item.valor = request.titulo;
                    else if (item.tipo == "Subtitulo Quem Somos")
                        item.valor = request.subtitulo;
                    else if (item.tipo == "Texto Quem Somos")
                        item.valor = request.texto;

                    _ctx.tabValorSecao.Update(item);
                    _ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool isAtivar(IsAtivoSecaoRequest request)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "QUEM SOMOS");
                secao.isAtivo = request.isAtivo;

                _ctx.tabSecao.Update(secao);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool atualizarNossoProposito(NossoPropositoRequest request)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "NOSSO PROPÓSITO");
                var codvaluesession = _ctx.tabValorSecaoCodigo.Where(x => x.secaoCodigo == secao.codigo).Select(x => x.valorSecaoCodigo);
                var valores = _ctx.tabValorSecao.Where(x => codvaluesession.Contains(x.codigo)).ToList();

                foreach (var item in valores)
                {
                    if (item.tipo == "Titulo Nosso Proposito")
                        item.valor = request.titulo;
                    else if (item.tipo == "Texto Nosso Proposito")
                        item.valor = request.texto;
                    else if (item.tipo == "Dilema 1")
                        item.valor = request.dilema1;
                    else if (item.tipo == "Dilema 2")
                        item.valor = request.dilema2;
                    else if (item.tipo == "Dilema 3")
                        item.valor = request.dilema3;
                    else if (item.tipo == "Dilema 4")
                        item.valor = request.dilema4;

                    _ctx.tabValorSecao.Update(item);
                    _ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool isAtivarNossoProposito(IsAtivoSecaoRequest request)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "NOSSO PROPÓSITO");
                secao.isAtivo = request.isAtivo;

                _ctx.tabSecao.Update(secao);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool atualizarNossaHistoria(NossaHistoriaRequest request)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "NOSSA HISTÓRIA");
                var codigoValorSecao = _ctx.tabValorSecaoCodigo.Where(x => x.secaoCodigo == secao.codigo).Select(x => x.valorSecaoCodigo).ToList();
                var valorSecao = _ctx.tabValorSecao.Where(c => codigoValorSecao.Contains(c.codigo)).ToList();

                foreach (var item in valorSecao)
                {
                    if (item.tipo == "Titulo Nossa Historia")
                        item.valor = request.titutlo;
                    else if (item.tipo == "Texto Nossa Historia")
                        item.valor = request.texto;

                    _ctx.tabValorSecao.Update(item);
                    _ctx.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool isAtivarNossaHistoria(IsAtivoSecaoRequest request)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "NOSSA HISTÓRIA");
                secao.isAtivo = request.isAtivo;

                _ctx.tabSecao.Update(secao);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> atualizarImagem1NossaHistoria(List<FileUploadModel> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");
                var fotosSalvas = new List<string>();

                var secoes = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "INICIO");
                var valorSecao = _ctx.tabValorSecao.FirstOrDefault(x => x.tipo == "Imagem 1");
                foreach (var file in files)
                {
                    if (file.Data.Length > 0)
                    {
                        file.FileName = secoes?.codigo.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + $".{file.FileName.Split(".")[1]}";
                        // Salvar a foto em um diretório (por exemplo, "Fotos") e obter o caminho completo do arquivo salvo
                        string filePath = "C:/inetpub/wwwroot/images/telaInicio/" + file.FileName;

                        // Verificar se o diretório "Fotos" existe e criar se não existir
                        if (!Directory.Exists("C:/inetpub/wwwroot/images/telaInicio"))
                        {
                            Directory.CreateDirectory("C:/inetpub/wwwroot/images/telaInicio");
                        }

                        // Salvar o arquivo no diretório
                        File.WriteAllBytes(filePath, file.Data);

                        fotosSalvas.Add(filePath);

                        valorSecao.valor = file.FileName;
                        _ctx.tabValorSecao.Update(valorSecao);
                        _ctx.SaveChanges();
                    }
                }
                return fotosSalvas;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> atualizarImagem2NossaHistoria(List<FileUploadModel> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");
                var fotosSalvas = new List<string>();

                var secoes = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "INICIO");
                var valorSecao = _ctx.tabValorSecao.FirstOrDefault(x => x.tipo == "Imagem 2");
                foreach (var file in files)
                {
                    if (file.Data.Length > 0)
                    {
                        file.FileName = secoes?.codigo.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + $".{file.FileName.Split(".")[1]}";
                        // Salvar a foto em um diretório (por exemplo, "Fotos") e obter o caminho completo do arquivo salvo
                        string filePath = "C:/inetpub/wwwroot/images/telaInicio/" + file.FileName;

                        // Verificar se o diretório "Fotos" existe e criar se não existir
                        if (!Directory.Exists("C:/inetpub/wwwroot/images/telaInicio"))
                        {
                            Directory.CreateDirectory("C:/inetpub/wwwroot/images/telaInicio");
                        }

                        // Salvar o arquivo no diretório
                        File.WriteAllBytes(filePath, file.Data);

                        fotosSalvas.Add(filePath);

                        valorSecao.valor = file.FileName;
                        _ctx.tabValorSecao.Update(valorSecao);
                        _ctx.SaveChanges();
                    }
                }
                return fotosSalvas;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> atualizarImagem3NossaHistoria(List<FileUploadModel> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");
                var fotosSalvas = new List<string>();

                var secoes = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "INICIO");
                var valorSecao = _ctx.tabValorSecao.FirstOrDefault(x => x.tipo == "Imagem 3");
                foreach (var file in files)
                {
                    if (file.Data.Length > 0)
                    {
                        file.FileName = secoes?.codigo.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + $".{file.FileName.Split(".")[1]}";
                        // Salvar a foto em um diretório (por exemplo, "Fotos") e obter o caminho completo do arquivo salvo
                        string filePath = "C:/inetpub/wwwroot/images/telaInicio/" + file.FileName;

                        // Verificar se o diretório "Fotos" existe e criar se não existir
                        if (!Directory.Exists("C:/inetpub/wwwroot/images/telaInicio"))
                        {
                            Directory.CreateDirectory("C:/inetpub/wwwroot/images/telaInicio");
                        }

                        // Salvar o arquivo no diretório
                        File.WriteAllBytes(filePath, file.Data);

                        fotosSalvas.Add(filePath);

                        valorSecao.valor = file.FileName;
                        _ctx.tabValorSecao.Update(valorSecao);
                        _ctx.SaveChanges();
                    }
                }
                return fotosSalvas;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> atualizarImagem4NossaHistoria(List<FileUploadModel> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");
                var fotosSalvas = new List<string>();

                var secoes = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "INICIO");
                var valorSecao = _ctx.tabValorSecao.FirstOrDefault(x => x.tipo == "Imagem 4");
                foreach (var file in files)
                {
                    if (file.Data.Length > 0)
                    {
                        file.FileName = secoes?.codigo.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + $".{file.FileName.Split(".")[1]}";
                        // Salvar a foto em um diretório (por exemplo, "Fotos") e obter o caminho completo do arquivo salvo
                        string filePath = "C:/inetpub/wwwroot/images/telaInicio/" + file.FileName;

                        // Verificar se o diretório "Fotos" existe e criar se não existir
                        if (!Directory.Exists("C:/inetpub/wwwroot/images/telaInicio"))
                        {
                            Directory.CreateDirectory("C:/inetpub/wwwroot/images/telaInicio");
                        }

                        // Salvar o arquivo no diretório
                        File.WriteAllBytes(filePath, file.Data);

                        fotosSalvas.Add(filePath);

                        valorSecao.valor = file.FileName;
                        _ctx.tabValorSecao.Update(valorSecao);
                        _ctx.SaveChanges();
                    }
                }
                return fotosSalvas;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string atualizarFotos(List<FileUploadModel> files, CasosSucessoRequest request)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    atualizarCasoSucesso1(request);

                    return "success";
                }
                var fotosSalvas = new List<string>();

                var secoes = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "CASES DE SUCESSO");

                foreach (var file in files)
                {
                    if (file.Data.Length > 0)
                    {
                        file.FileName = secoes?.codigo.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + $".{file.FileName.Split(".")[1]}";
                        // Salvar a foto em um diretório (por exemplo, "Fotos") e obter o caminho completo do arquivo salvo
                        string filePath = "C:/inetpub/wwwroot/images/CasesSucesso/" + file.FileName;

                        // Verificar se o diretório "Fotos" existe e criar se não existir
                        if (!Directory.Exists("C:/inetpub/wwwroot/images/CasesSucesso"))
                        {
                            Directory.CreateDirectory("C:/inetpub/wwwroot/images/CasesSucesso");
                        }

                        // Salvar o arquivo no diretório
                        File.WriteAllBytes(filePath, file.Data);
                        atualizarCasoSucesso1(request, file.FileName);
                        fotosSalvas.Add(filePath);

                    }
                }
                return fotosSalvas.First(); ;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public void atualizarCasoSucesso1(CasosSucessoRequest request, string nomeImage = null)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "CASES DE SUCESSO");
                var codigoValor = _ctx.tabValorSecaoCodigo.Where(x => x.secaoCodigo == secao.codigo).Select(x => x.valorSecaoCodigo).ToList();
                var valor = _ctx.tabValorSecao.Where(x => codigoValor.Contains(x.codigo)).ToList();
                if (request.numero == "1")
                {

                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 1")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 1")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 1")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 1")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 1")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 1")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
                if (request.numero == "2")
                {
                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 2")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 2")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 2")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 2")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 2")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 2")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
                if (request.numero == "3")
                {
                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 3")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 3")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 3")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 3")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 3")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 3")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
                if (request.numero == "4")
                {
                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 4")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 4")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 4")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 4")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 4")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 4")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
                if (request.numero == "5")
                {
                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 5")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 5")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 5")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 5")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 5")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 5")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
                if (request.numero == "6")
                {
                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 6")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 6")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 6")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 6")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 6")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 6")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
                if (request.numero == "7")
                {
                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 7")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 7")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 7")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 7")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 7")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 7")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
                if (request.numero == "8")
                {
                    foreach (var item in valor)
                    {
                        if (item.tipo == "Foto 8")
                        {
                            if (nomeImage != null)
                                item.valor = nomeImage;
                        }
                        else if (item.tipo == "Nome 8")
                            item.valor = request.nome;
                        else if (item.tipo == "Cargo 8")
                            item.valor = request.cargo;
                        else if (item.tipo == "Idade 8")
                            item.valor = request.idade;
                        else if (item.tipo == "Url Likedin 8")
                            item.valor = request.linkedin;
                        else if (item.tipo == "Texto 8")
                            item.valor = request.texto;

                        _ctx.tabValorSecao.Update(item);
                        _ctx.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public GenericResponse<List<SecaoValor>> ObterHomeAluno()
        {
            try
            {
                List<SecaoValor> secaoValores = new List<SecaoValor>();

                var secao = _ctx.tabSecao.Where(x => x.isAtivo == true && x.descricao == "CARDS HOME ALUNO").FirstOrDefault();
                var valoresSecaoCodigo = _ctx.tabValorSecaoCodigo.Where(x => x.secaoCodigo == secao.codigo).ToList();
                var valoresSecao = _ctx.tabValorSecao.Where(x => valoresSecaoCodigo.Select(i => i.valorSecaoCodigo).Contains(x.codigo)).ToList();

                foreach (var valorsecao in valoresSecao)
                {
                    secaoValores.Add(new SecaoValor()
                    {
                        codigoSecao = secao.codigo,
                        secao = secao.descricao,
                        tipo = valorsecao.tipo,
                        valor = valorsecao.valor
                    });
                }
                return new GenericResponse<List<SecaoValor>>("success", secaoValores);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool AtualizarHomeAluno(HomeAlunoRequest request)
        {
            try
            {
                var secao = _ctx.tabSecao.FirstOrDefault(x => x.descricao == "CARDS HOME ALUNO");
                var secoesValorCodigo = _ctx.tabValorSecaoCodigo.Where(x => x.secaoCodigo == secao.codigo).ToList();
                var secoesValores = _ctx.tabValorSecao.Where(x => secoesValorCodigo.Select(x => x.valorSecaoCodigo).Contains(x.codigo)).ToList();

                secoesValores.FirstOrDefault(x => x.tipo == "Titulo Card 1").valor = request.TitleCard1;
                secoesValores.FirstOrDefault(x => x.tipo == "Titulo Card 2").valor = request.TitleCard2;
                secoesValores.FirstOrDefault(x => x.tipo == "Body Card 1").valor = request.BodyCard1;
                secoesValores.FirstOrDefault(x => x.tipo == "Body Card 2").valor = request.BodyCard2;
                secoesValores.FirstOrDefault(x => x.tipo == "Button Card 1").valor = request.ButtonCard1;
                secoesValores.FirstOrDefault(x => x.tipo == "Button Card 2").valor = request.ButtonCard2;
                secoesValores.FirstOrDefault(x => x.tipo == "Button Link Card 1").valor = request.ButtonLinkCard1;
                secoesValores.FirstOrDefault(x => x.tipo == "Button Link Card 2").valor = request.ButtonLinkCard2;

                _ctx.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AlterarLayoutFront(int id)
        {
            try
            {
                var estilizacao = _ctx.TabEstilizacao.FirstOrDefault(x => x.codigo == id);
                if(estilizacao == null)
                {
                    return false;
                }

                estilizacao.isAtivo = true;

                var todasEstilizacoes = _ctx.TabEstilizacao.Where(x => x.codigo != estilizacao.codigo).ToList();
                foreach (var item in todasEstilizacoes)
                {
                    item.isAtivo = false;
                }

                _ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<TabEstilizacao> ObterTodosEstilosCSS()
        {
            try
            {
                var estilizacao = _ctx.TabEstilizacao.ToList();
                if (estilizacao == null)
                {
                    return null;
                }

                return estilizacao;
            }
            catch
            {
                return null;
            }
        }
    }
}
