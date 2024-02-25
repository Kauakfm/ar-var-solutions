using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class AulaService
    {
        private readonly VAREntities _ctx;

        public AulaService(VAREntities context)
        {
            _ctx = context;
        }
        public GenericResponse<int> ObterQtdAulas(int cursoId)
        {
            try
            {
                var qtd = _ctx.TabCurso_TabAula.Where(x => x.cursoCodigo == cursoId).Count();
                return new GenericResponse<int>("success", qtd);
            }
            catch (Exception ex)
            {
                return new GenericResponse<int>("failed", 0);
            }
        } public GenericResponse<List<TabAula>> ObterTodasAulas()
        {
            try
            {
                var aulas = _ctx.TabAula.ToList();
                return new GenericResponse<List<TabAula>>("success", aulas);
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<TabAula>>("failed", null);
            }
        }
        public GenericResponse<List<TabModulos>> ObterModulos()
        {
            try
            {
                var modulos = _ctx.TabModulos.ToList();
                var response = new GenericResponse<List<TabModulos>>("sucess", modulos);
                return response;

            }
            catch (Exception ex)
            {
                var response = new GenericResponse<List<TabModulos>>("failed", null);
                return response;
            }
        }
        public List<TabModulos> InserirModulo(ModuloRequest request)
        {
            try
            {
                _ctx.TabModulos.Add(new TabModulos
                {
                    descricao = request.titulo,
                });
                _ctx.SaveChanges();
                return _ctx.TabModulos.ToList();
            }
            catch
            {
                return null;
            }

        }

        //public List<TabAula> InserirAula(List<AulaRequest> requests, int cursoCodigo)
        //{
        //    try
        //    {
        //        var aulas = _ctx.TabAula.ToList();
        //        var aulasCurso = _ctx.TabCurso_TabAula.ToList();
        //        for (int c = 0; c < requests.Count(); c++)
        //        {
        //            TabAula aula = _ctx.TabAula.FirstOrDefault(x => x.urlAula == requests[c].urlAula || x.urlAulapanda == requests[c].urlAulaPanda);
        //            if (aula == null)
        //            {
        //                var aulaSalva = _ctx.TabAula.Add(new TabAula
        //                {
        //                    descricao = requests[c].descricao,
        //                    detalhe = requests[c].detalhe,
        //                    urlAula = requests[c].urlAula,
        //                    isAtivo = requests[c].isAtivo,
        //                    dataCadastro = DateTime.Now,
        //                    tempoVideoSegundos = requests[c].tempoVideoSegundos,
        //                    isDesafio = requests[c].isDesafio,
        //                    moduloId = requests[c].moduloId,
        //                    urlAulapanda = requests[c].urlAulaPanda,
        //                });
        //                _ctx.SaveChanges();
        //                _ctx.TabCurso_TabAula.Add(new TabCurso_TabAula
        //                {
        //                    aulaCodigo = aulaSalva.Entity.codigo,
        //                    cursoCodigo = cursoCodigo,
        //                    ordem = c
        //                });
        //            }
        //            else
        //            {
        //                var ordem = _ctx.TabCurso_TabAula.FirstOrDefault(x => x.aulaCodigo == aula.codigo && x.cursoCodigo == cursoCodigo);
        //                if (ordem == null)
        //                {
        //                    _ctx.TabCurso_TabAula.Add(new TabCurso_TabAula
        //                    {
        //                        aulaCodigo = aula.codigo,
        //                        cursoCodigo = cursoCodigo,
        //                        ordem = c
        //                    });
        //                }

        //                else
        //                    ordem.ordem = c;
        //            }
        //        }
        //        for (int i = 0; i < aulas.Count(); i++)
        //        {
        //            if (requests.Any(x => x.urlAula != aulas[i].urlAula || x.urlAulaPanda != aulas[i].urlAulapanda))
        //            {
        //                aulas[i].isAtivo = false;
        //            }
        //        }

        //        _ctx.SaveChanges();

        //        return _ctx.TabAula.ToList();


        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public bool MarcarSegundosAssistidos(int usuarioCodigo, MarcarSegundosRequest request)
        {
            try
            {
                _ctx.tabAulaSegundosAssistidos.Add(new tabAulaSegundosAssistidos
                {
                    aulaCodigo = request.aulaCodigo,
                    segundos = request.segundos,
                    usuarioCodigo = usuarioCodigo
                });
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public tabAulaSegundosAssistidos ObterSegundosAssistidos(int usuarioCodigo, int aulaCodigo)
        {
            try
            {
                return _ctx.tabAulaSegundosAssistidos.FirstOrDefault(x => x.aulaCodigo == aulaCodigo && x.usuarioCodigo == usuarioCodigo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<AulasPorModulo> ObterAulasDoCurso(int id)
        {
            var aulasCurso = _ctx.TabCurso_TabAula.Where(x => x.cursoCodigo == id).OrderBy(x => x.ordem).ToList();
            var aulasdDb = _ctx.TabAula.Where(x => x.isAtivo).ToList();
            var aulas = new List<TabAula>();

            foreach (var aula in aulasCurso.Select(x => x.aulaCodigo))
            {
                aulas.Add(aulasdDb.FirstOrDefault(x => x.codigo == aula));
            }
            //         var aulas = new List<TabAula>();

            //foreach(var aula in aulasCurso.Select(x => x.aulaCodigo))
            //{
            //    aulas.Add(_ctx.TabAula.FirstOrDefault(x => x.codigo == aula));
            //}

            List<AulasPorModulo> aulasporModulos = new List<AulasPorModulo>();
            List<TabModulos> modulosComDescricao = _ctx.TabModulos.ToList();
            List<int> modulos = new List<int>();

            foreach (var modulo in aulas.Select(x => x.moduloId))
            {
                if (Convert.ToInt32(modulo) == 0)
                    break;
                modulos.Add(Convert.ToInt32(modulo));

            }
            foreach (var aula in modulos.GroupBy(x => x))
            {
                aulasporModulos.Add(new AulasPorModulo
                {
                    nome = modulosComDescricao.FirstOrDefault(x => x.codigo == aula.Key).descricao,
                    aulas = aulas.Where(x => x.moduloId == aula.Key).ToList()
                });
            }
            return aulasporModulos;
        }
        public TabAula ObterAula(int id)
        {
            var codigoAula = _ctx.TabAula.FirstOrDefault(x => x.codigo == id);
            return codigoAula;
        }

        public TabAula ObterPrimeiraAula(int cursoId)
        {
            var codigoAula = _ctx.TabCurso_TabAula.Where(x => x.cursoCodigo == cursoId).OrderBy(c => c.ordem).FirstOrDefault().aulaCodigo;
            var Aula = _ctx.TabAula.FirstOrDefault(x => x.codigo == codigoAula);
            return Aula;
        }

        public Conclusao MarcarConcluido(int aulaId, int usuarioId)
        {
            Conclusao conclu = new Conclusao();
            var cert = new CertificadoService(_ctx);

            if (_ctx.TabConclusao.Any(x => x.aulaId == aulaId && x.usuarioId == usuarioId))
            {
                conclu.status = false;
                conclu.isCertificado = false;
                return conclu;
            }
            var concluido = _ctx.TabConclusao.Add(new TabConclusao()
            {
                aulaId = aulaId,
                usuarioId = usuarioId,
                data = DateTime.Now
            });
            _ctx.SaveChanges();
            conclu.status = true;
            var concluidos = _ctx.TabConclusao.Where(x => x.usuarioId == usuarioId)
                .Select(c => c.aulaId).ToList();
            var moduloscertificados = _ctx.tabCertificadosAlunos
                .Where(x => x.usuarioCodigo == usuarioId).ToList();
            var modulocertificados = _ctx.tabAulaModuloCertificado.Where(x => x.aulaCodigo == aulaId).ToList();

            var modulos = modulocertificados
                .Where(x => !moduloscertificados
                .Any(u => u.modulosCerficadosCodigo == x.codigo))
                .Select(y => y.modulocertificadoCodigo)
                .Distinct()
                .ToList();
            //var modulos = _ctx.tabAulaModuloCertificado
            //.Where(x => x.aulaCodigo == aulaId && 
            //!moduloscertificados.Any(u => u.modulosCerficadosCodigo == x.codigo)).ToList()
            //.GroupBy(c => c.modulocertificadoCodigo).Select(y => y.Key).ToList();

            var aulaModuloCertificado = _ctx.tabAulaModuloCertificado
                .Where(c => modulos
                .Any(i => i == c.modulocertificadoCodigo)).ToList();

            foreach (var modulo in modulos)
            {
                if (aulaModuloCertificado.Where(c => c.modulocertificadoCodigo == modulo).All(y => concluidos.Contains(y.aulaCodigo)))
                {
                    var objNomeModulo = _ctx.tabmodulosCertificados.FirstOrDefault(x => x.codigo == modulo)?.nomeModulo;
                    conclu.isCertificado = true;
                    conclu.nomeModulo = objNomeModulo;
                    var certificadoAluno = new TabCertificadosAlunos()
                    {
                        usuarioCodigo = usuarioId,
                        modulosCerficadosCodigo = modulo,
                        dataGeracao = DateTime.Now
                    };
                    _ctx.tabCertificadosAlunos.Add(certificadoAluno);
                    _ctx.SaveChanges();
                    cert.GerarCertificadoPDF(certificadoAluno.codigo);
                    return conclu;
                }
            }
            conclu.isCertificado = false;
            return conclu;
        }
        public TabConclusao ObterConcluido(int aulaId, int usuarioId)
        {
            var concluido = _ctx.TabConclusao.FirstOrDefault(x => x.aulaId == aulaId && x.usuarioId == usuarioId);
            return concluido;

        }
        public List<TabConclusao> ObterConcluidos(int id)
        {
            var concluido = _ctx.TabConclusao.Where(x => x.usuarioId == id).ToList();
            return concluido;
        }

        public void obterAulaIniciada(int id, int aulaid)
        {
            try
            {
                tabAulaUsuarioLog objTab = new tabAulaUsuarioLog
                {
                    usuarioCodigo = id,
                    aulaCodigo = aulaid,
                    dataAcesso = DateTime.Now
                };
                _ctx.Add(objTab);
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                LogExcecaoService logExcecaoService = new LogExcecaoService(_ctx);
                logExcecaoService.gravarLog(ex, "Erro ao salvar aula");
            }

        }
        public List<CursoAmostra> ObterNomeAulaOnline(int id)
        {
            var unidadeCurso = _ctx.TabUnidade_Curso.Where(x => x.unidadeCodigo == id).ToList().Select(y => y.cursoCodigo).ToList();
            var cursos = _ctx.TabCurso.ToList();
            var aulasCurso = _ctx.TabCurso_TabAula.Where(x => unidadeCurso.Contains(x.cursoCodigo)).OrderBy(x => x.ordem).ToList();
            var aulasdDb = _ctx.TabAula.ToList();

            var cursoAulasAmostra = new List<CursoAmostra>();
            foreach (var aulaCurso in aulasCurso.GroupBy(x => x.cursoCodigo))
            {
                var aulas = new List<TabAula>();
                foreach (var aula in aulasCurso.Where(y => y.cursoCodigo == aulaCurso.Key).ToList().Select(x => x.aulaCodigo))
                {
                    aulas.Add(aulasdDb.FirstOrDefault(x => x.codigo == aula));
                }

                List<AulasPorModuloAmostra> aulasporModulos = new List<AulasPorModuloAmostra>();
                List<TabModulos> modulosComDescricao = _ctx.TabModulos.ToList();
                List<int> modulos = new List<int>();
                foreach (var modulo in aulas.Select(x => x.moduloId))
                {
                    if (Convert.ToInt32(modulo) == 0)
                        break;
                    modulos.Add(Convert.ToInt32(modulo));

                }
                foreach (var aula in modulos.GroupBy(x => x))
                {
                    List<Aula> aulass = new();
                    foreach (var aulaa in aulas.Where(x => x.moduloId == aula.Key).ToList())
                    {
                        aulass.Add(new Aula
                        {
                            descricao = aulaa.descricao,
                            moduloId = aula.Key,
                            arquivo = aulaa.arquivo,
                            codigo = aulaa.codigo,
                            dataCadastro = aulaa.dataCadastro,
                            detalhe = aulaa.detalhe,
                            isDesafio = aulaa.isDesafio,
                            tempoVideoSegundos = aulaa.tempoVideoSegundos
                        });
                    }


                    aulasporModulos.Add(new AulasPorModuloAmostra
                    {


                        nome = modulosComDescricao.FirstOrDefault(x => x.codigo == aula.Key).descricao,
                        aulas = aulass

                    });
                }
                cursoAulasAmostra.Add(new CursoAmostra
                {
                    Curso = cursos.FirstOrDefault(x => x.codigo == aulaCurso.Key).descricao,
                    aulas = aulasporModulos
                });
            }
            return cursoAulasAmostra;
        }

        public bool InserirAula(AulaRequest request)
        {
            try
            {
                TabAula aula = new()
                {
                    descricao = request.descricao,
                    detalhe = request.detalhe,
                    urlAula = request.urlAula,
                    isAtivo = request.isAtivo,
                    tempoVideoSegundos = request.tempoVideoSegundos,
                    moduloId = request.moduloId,
                    urlAulapanda = request.urlAulaPanda,
                    dataCadastro = DateTime.Now
                };

                _ctx.TabAula.Add(aula);
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool VincularAulaCurso(TabCursoTabAulaRequest request)
        {
            try
            {
                var aulascurso = _ctx.TabCurso_TabAula.Where(x => x.cursoCodigo == request.cursoCodigo && x.ordem >= request.ordem).ToList();
                foreach (var aula in aulascurso)
                {
                    aula.ordem++;
                }
                _ctx.TabCurso_TabAula.Add(new TabCurso_TabAula
                {
                    ordem = request.ordem,
                    cursoCodigo = request.cursoCodigo,
                    aulaCodigo = request.aulaCodigo,
                });
                _ctx.SaveChanges();
                int index = 1;
                foreach (var aulinha in _ctx.TabCurso_TabAula.Where(x => x.cursoCodigo == request.cursoCodigo).OrderBy(y => y.ordem).ToList())
                {
                    aulinha.ordem = index;
                    index++;
                }
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}

