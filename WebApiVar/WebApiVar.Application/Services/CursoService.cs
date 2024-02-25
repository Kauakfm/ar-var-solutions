using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class CursoService
    {
        private readonly VAREntities _ctx;
        public CursoService(VAREntities ctx)
        {
            _ctx = ctx;
        }
        public GenericResponse<List<Cursos>> ObterTodosCursos()
        {
            try
            {
                var curso = _ctx.TabCurso.ToList();
                var aulascursos = _ctx.TabCurso_TabAula.ToList();
                var aulas = _ctx.TabAula.ToList();

                List<Cursos> cursos = new List<Cursos>();
                foreach (var aula in curso)
                {
                    List<TabAula> aulas1 = new();

                    foreach (var auladocurso in aulascursos.Where(x => x.cursoCodigo == aula.codigo))
                    {
                        aulas1.Add(aulas.FirstOrDefault(x => x.codigo == auladocurso.aulaCodigo));
                    }
                    cursos.Add(new Cursos
                    {
                        codigo = curso.FirstOrDefault(x => x.codigo == aula.codigo).codigo,
                        descricao = curso.FirstOrDefault(x => x.codigo == aula.codigo).descricao,
                        Logo = curso.FirstOrDefault(x => x.codigo == aula.codigo).Logo,
                        detalhes = curso.FirstOrDefault(x => x.codigo == aula.codigo).detalhes,
                        Atualizadoem = aulas1.OrderBy(x => x.dataCadastro).LastOrDefault()?.dataCadastro,
                        cargaHoraria = curso.FirstOrDefault(x => x.codigo == aula.codigo).cargaHoraria
                    });
                }
                return new GenericResponse<List<Cursos>>("sucess", cursos);
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<Cursos>>("failed", null);
            }
        }
        public List<TabCurso> InserirCurso(CursoRequest request)
        {
            try
            {
                _ctx.TabCurso.Add(new TabCurso
                {
                    descricao = request.titulo,
                    detalhes = request.detalhes,
                    Logo = request.Logo,
                    cargaHoraria = request.cargaHoraria
                });
                _ctx.SaveChanges();

                return _ctx.TabCurso.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Cursos> ObterCursos(int unidadecodigo)
        {
            var unidade = _ctx.TabUnidade_Curso.Where(t => t.unidadeCodigo == unidadecodigo).ToList().Select(c => c.cursoCodigo);
            var curso = _ctx.TabCurso.Where(y => unidade.Contains(y.codigo)).ToList();
            var aulascursos = _ctx.TabCurso_TabAula.ToList();
            var aulas = _ctx.TabAula.Where(x => x.isAtivo).ToList();

            List<Cursos> cursos = new List<Cursos>();
            foreach (var aula in curso)
            {
                List<TabAula> aulas1 = new();
                if (aulascursos.Any(x => x.cursoCodigo == aula.codigo))
                {
                    foreach (var auladocurso in aulascursos.Where(x => x.cursoCodigo == aula.codigo))
                    {
                        aulas1.Add(aulas.FirstOrDefault(x => x.codigo == auladocurso.aulaCodigo));
                    }
                    cursos.Add(new Cursos
                    {
                        codigo = curso.FirstOrDefault(x => x.codigo == aula.codigo).codigo,
                        descricao = curso.FirstOrDefault(x => x.codigo == aula.codigo).descricao,
                        Logo = curso.FirstOrDefault(x => x.codigo == aula.codigo).Logo,
                        detalhes = curso.FirstOrDefault(x => x.codigo == aula.codigo).detalhes,
                        Atualizadoem = aulas1.OrderBy(x => x.dataCadastro).Last()?.dataCadastro,
                        cargaHoraria = curso.FirstOrDefault(x => x.codigo == aula.codigo).cargaHoraria
                    });
                }
            }
            return cursos;
        }
        public Proc_EvolucaoAluno ObterPercentualConclusao(int codigocurso, int codigousuario)
        {
            var id = codigousuario.ToString();
            var curso = codigocurso.ToString();
            var proc = "Exec PROC_EVOLUCAO_ALUNO @curso , @id";
            return _ctx.Proc_EvolucaoAluno.FromSqlRaw(proc.Replace("@curso", curso).Replace("@id", id)).ToList().First();
        }

        public List<Proc_AulasTurma> ObterListaPresenca(ProcRequest request)
        {
            var objData = DateTime.Parse(request.data.ToString("yyyy-MM-dd"), new CultureInfo("en-US"));
            string objCodCurso = request.turmaCodigo.ToString();
            var proc = "Exec proc_AulasIniciadasPorturmaPorDia @dataAcesso , @turmaCodigo";
            return _ctx.proc_AulasTurma.FromSqlRaw(proc.Replace("@dataAcesso", objData.ToString()).Replace("@turmaCodigo", objCodCurso)).ToList();
            //  var dataParam = new SqlParameter("@dataAcesso", objData);
            //var codCursoParam = new SqlParameter("@turmaCodigo", objCodCurso);
            //return _ctx.Proc_AulasTurma.FromSqlRaw(proc, dataParam, codCursoParam).ToList().First();           
        }

        public async Task<List<string>> InserirMaterial(List<FileUploadModel> files, int id)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");

            var arquivosSalvas = new List<string>();

            foreach (var file in files)
            {
                if (file.Data.Length > 0)
                {
                    string filePath = "C:/inetpub/wwwroot/filesAula/" + file.FileName;

                    // Verificar se o diretório "Fotos" existe e criar se não existir
                    if (!Directory.Exists("C:/inetpub/wwwroot/filesAula"))
                    {
                        Directory.CreateDirectory("C:/inetpub/wwwroot/filesAula");
                    }

                    // Salvar o arquivo no diretório
                    await File.WriteAllBytesAsync(filePath, file.Data);
                    var user = _ctx.TabMaterial.Add(new TabMaterial
                    {
                        nome = file.FileName.Split('.')[0],
                        url = file.FileName
                    });
                    _ctx.SaveChanges();
                    _ctx.TabAulaMaterial.Add(new TabAulaMaterial
                    {
                        idAula = id,
                        idMaterial = user.Entity.id
                    });
                    _ctx.SaveChanges();

                    arquivosSalvas.Add(filePath);
                }
            }

            return arquivosSalvas;
        }

        public async Task<List<string>> InserirImgFormCurso(List<FileUploadModel> files, int id)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");

            var arquivosSalvas = new List<string>();

            foreach (var file in files)
            {
                if (file.Data.Length > 0)
                {   
                    string filePath = "C:/inetpub/wwwroot/CursosVar/" + file.FileName;

                    // Verificar se o diretório "Fotos" existe e criar se não existir
                    if (!Directory.Exists("C:/inetpub/wwwroot/CursosVar"))
                    {
                        Directory.CreateDirectory("C:/inetpub/wwwroot/CursosVar");
                    }

                    // Salvar o arquivo no diretório
                    await File.WriteAllBytesAsync(filePath, file.Data);
                    _ctx.TabCurso.FirstOrDefault(x => x.codigo == id).Logo = file.FileName;
                    _ctx.SaveChanges();
                                        

                    arquivosSalvas.Add(filePath);
                }
            }

            return arquivosSalvas;
        }
    }
}
