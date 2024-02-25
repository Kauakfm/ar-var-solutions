using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Reflection.Emit;

using WebApiVar.Repository.Models;

namespace WebApiVar.Repository
{
    public class VAREntities : DbContext
    {
        public VAREntities(DbContextOptions<VAREntities> options) : base(options) { }
        public DbSet<Tab_inscricao_evento> Tab_inscricao_evento { get; set; }
        public DbSet<TabAula> TabAula { get; set; }
        public DbSet<TabAula_TabUsuario> TabAula_TabUsuario { get; set; }
        public DbSet<TabAulaComentario> TabAulaComentario { get; set; }
        public DbSet<TabAulaComentarioAnexo> TabAulaComentarioAnexo { get; set; }
        public DbSet<TabAulaComentarioInteracao> TabAulaComentarioInteracao { get; set; }
        public DbSet<TabAulaComentarioInteracaoAnexo> TabAulaComentarioInteracaoAnexo { get; set; }
        public DbSet<TabAulaMaterial> TabAulaMaterial { get; set; }
        public DbSet<TabConclusao> TabConclusao { get; set; }
        public DbSet<TabCurso> TabCurso { get; set; }
        public DbSet<TabCurso_TabAula> TabCurso_TabAula { get; set; }
        public DbSet<TabEstacaoTrabalho> TabEstacaoTrabalho { get; set; }
        public DbSet<TabEstacaoTrabalhoItens> TabEstacaoTrabalhoItens { get; set; }
        public DbSet<TabHistoricoAluno> TabHistoricoAluno { get; set; }
        public DbSet<TabMaterial> TabMaterial { get; set; }
        public DbSet<TabPosicaoAluno> TabPosicaoAluno { get; set; }
        public DbSet<TabPresenca> TabPresenca { get; set; }
        public DbSet<TabStatus> TabStatus { get; set; }
        public DbSet<TabTurma> TabTurma { get; set; }
        public DbSet<TabTurma_TabCurso> TabTurma_TabCurso { get; set; }
        public DbSet<TabUsuario> TabUsuario { get; set; }
        public DbSet<TabModulos> TabModulos { get; set; }
        public DbSet<TabUnidade> tabUnidade { get; set; }
        public DbSet<TabDoacao> TabDoacao { get; set; }
        public DbSet<TabAdoteUmAluno> TabAdoteUmAluno { get; set; }
        public DbSet<TabVarCast> TabVarCast { get; set; }
        public DbSet<TabPlano> TabPlano { get; set; }
        public DbSet<TabPlano_TabStatus> TabPlano_TabStatus { get; set; }
        public DbSet<TabUnidade_Curso> TabUnidade_Curso { get; set; }
        public DbSet<TabAviso> TabAviso { get; set; }
        public DbSet<tabComprasCurso> tabComprasCurso { get; set; }
        public DbSet<TabCupomCursoOnline> TabCupomCursoOnline { get; set; }
        public DbSet<Proc_EvolucaoAluno> Proc_EvolucaoAluno { get; set; }
        public DbSet<tabPagamentoPresencial> tabPagamentoPresencial { get; set; }
        public DbSet<tabMensalidade> tabMensalidade { get; set; }
        public DbSet<tabDiaPagamento> tabDiaPagamento { get; set; }
        public DbSet<tabCompraHistorico> tabCompraHistorico { get; set; }
        public DbSet<tabLogExcecao> tabLogExcecao { get; set; }
        public DbSet<tabCompraHistoricoDetalhes> tabCompraHistoricoDetalhes { get; set; }
        public DbSet<tabUsuarioAcessos> tabUsuarioAcessos { get; set; }
        public DbSet<tabCursoOnlineCancelamento> tabCursoOnlineCancelamento { get; set; }
        public DbSet<tabAulaUsuarioLog> tabAulaUsuarioLog { get; set; }        
        public DbSet<TabAlunoPresenca> TabAlunoPresenca { get; set; }
        public DbSet<tabUsuarioHistorico> tabUsuarioHistorico { get; set; }
        public DbSet<tabAulaSegundosAssistidos> tabAulaSegundosAssistidos { get; set; }
        public DbSet<TabPesquisa> tabPesquisa { get; set; }
        public DbSet<TabPerguntaPesquisa> tabPerguntaPesquisa { get; set; }
        public DbSet<TabPerguntaPesquisaResposta> tabPerguntaPesquisaResposta { get; set; }        
        public DbSet<TabUf> tabUf { get; set; }
        public DbSet<TabRaca> tabRaca { get; set; }
        public DbSet<TabGenero> tabGenero { get; set; }
        public DbSet<TabCertificadosAlunos> tabCertificadosAlunos { get; set; }
        public DbSet<TabAulaModuloCertificado> tabAulaModuloCertificado { get; set; }
        public DbSet<TabmodulosCertificados> tabmodulosCertificados { get; set; }
        public DbSet<Proc_AulasTurma> proc_AulasTurma { get; set; }
        public DbSet<Vw_SituacaoAtualVagasPorTurma> Vw_SituacaoAtualVagasPorTurma { get; set; }
        public DbSet<Proc_AulasIniciadasPorturmaPorDia> proc_AulasIniciadasPorturmaPorDia { get; set; }
        public DbSet<tabValorSecao> tabValorSecao { get; set; }
        public DbSet<tabSecao> tabSecao { get; set; }
        public DbSet<TabEstilizacao> TabEstilizacao { get; set; }

        public DbSet<tabValorSecaoCodigo> tabValorSecaoCodigo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tab_inscricao_evento>().ToTable("tab_inscricao_evento");
            modelBuilder.Entity<TabAula>().ToTable("tabAula");
            modelBuilder.Entity<TabAula_TabUsuario>().ToTable("tabAula_TabUsuario");
            modelBuilder.Entity<TabAulaComentario>().ToTable("tabAulaComentario");
            modelBuilder.Entity<TabAulaComentarioAnexo>().ToTable("tabAulaComentarioAnexo");
            modelBuilder.Entity<TabAulaComentarioInteracao>().ToTable("tabAulaComentarioInteracao");
            modelBuilder.Entity<TabAulaComentarioInteracaoAnexo>().ToTable("tabAulaComentarioInteracaoAnexo");
            modelBuilder.Entity<TabAulaMaterial>().ToTable("tabAulaMaterial");
            modelBuilder.Entity<TabConclusao>().ToTable("tabConclusao");
            modelBuilder.Entity<TabCurso>().ToTable("tabCurso");
            modelBuilder.Entity<TabCurso_TabAula>().ToTable("tabCurso_TabAula");
            modelBuilder.Entity<TabEstacaoTrabalho>().ToTable("tabEstacaoTrabalho");
            modelBuilder.Entity<TabEstacaoTrabalhoItens>().ToTable("tabEstacaoTrabalhoItens");
            modelBuilder.Entity<TabHistoricoAluno>().ToTable("tabHistoricoAluno");
            modelBuilder.Entity<TabMaterial>().ToTable("tabMaterial");
            modelBuilder.Entity<TabPosicaoAluno>().ToTable("tabPosicaoAluno");
            modelBuilder.Entity<TabPresenca>().ToTable("tabPresenca");
            modelBuilder.Entity<TabStatus>().ToTable("tabStatus");
            modelBuilder.Entity<TabTurma>().ToTable("tabTurma");
            modelBuilder.Entity<TabTurma_TabCurso>().ToTable("tabTurma_TabCurso");
            modelBuilder.Entity<TabUsuario>().ToTable("tabUsuario");
            modelBuilder.Entity<TabModulos>().ToTable("TabModulos");
            modelBuilder.Entity<TabUnidade>().ToTable("tabUnidade");
            modelBuilder.Entity<TabVarCast>().ToTable("tabVarCast");
            modelBuilder.Entity<TabDoacao>().ToTable("TabDoacao");
            modelBuilder.Entity<TabAdoteUmAluno>().ToTable("TabAdoteUmAluno");
            modelBuilder.Entity<TabPlano>().ToTable("TabPlano");
            modelBuilder.Entity<TabPlano_TabStatus>().ToTable("TabPlano_TabStatus");
            modelBuilder.Entity<TabUnidade_Curso>().ToTable("TabUnidade_Curso");
            modelBuilder.Entity<tabComprasCurso>().ToTable("tabComprasCurso");
            modelBuilder.Entity<TabCupomCursoOnline>().ToTable("TabCupomCursoOnline");
            modelBuilder.Entity<TabAviso>().ToTable("tabAviso");
            modelBuilder.Entity<tabPagamentoPresencial>().ToTable("tabPagamentoPresencial");
            modelBuilder.Entity<tabMensalidade>().ToTable("tabMensalidade");
            modelBuilder.Entity<tabDiaPagamento>().ToTable("tabDiaPagamento");
            modelBuilder.Entity<tabCompraHistorico>().ToTable("tabCompraHistorico");
            modelBuilder.Entity<tabLogExcecao>().ToTable("tabLogExcecao");
            modelBuilder.Entity<tabCompraHistoricoDetalhes>().ToTable("tabCompraHistoricoDetalhes");
            modelBuilder.Entity<tabUsuarioAcessos>().ToTable("tabUsuarioAcessos");
            modelBuilder.Entity<tabAulaUsuarioLog>().ToTable("tabAulaUsuarioLog");
            modelBuilder.Entity<tabCursoOnlineCancelamento>().ToTable("tabCursoOnlineCancelamento");
            modelBuilder.Entity<Vw_SituacaoAtualVagasPorTurma>().ToView("Vw_SituacaoAtualVagasPorTurma").HasNoKey();
            modelBuilder.Entity<TabAlunoPresenca>().ToTable("tabAlunoPresenca");
            modelBuilder.Entity<tabUsuarioHistorico>().ToTable("tabUsuarioHistorico");
            modelBuilder.Entity<tabAulaSegundosAssistidos>().ToTable("tabAulaSegundosAssistidos");
            modelBuilder.Entity<TabPesquisa>().ToTable("tabPesquisa");
            modelBuilder.Entity<TabPerguntaPesquisa>().ToTable("tabPerguntaPesquisa");
            modelBuilder.Entity<TabPerguntaPesquisaResposta>().ToTable("tabPerguntaPesquisaResposta");
            modelBuilder.Entity<TabUf>().ToTable("tabUf");
            modelBuilder.Entity<TabRaca>().ToTable("tabRaca");
            modelBuilder.Entity<TabGenero>().ToTable("tabGenero");
            modelBuilder.Entity<TabCertificadosAlunos>().ToTable("tabCertificadosAlunos");
            modelBuilder.Entity<TabAulaModuloCertificado>().ToTable("tabAulaModuloCertificado");
            modelBuilder.Entity<TabmodulosCertificados>().ToTable("tabmodulosCertificados");
            modelBuilder.Entity<tabValorSecao>().ToTable("tabValorSecao");
            modelBuilder.Entity<tabSecao>().ToTable("tabSecao");
            modelBuilder.Entity<tabValorSecaoCodigo>().ToTable("tabValorSecaoCodigo");
            modelBuilder.Entity<TabEstilizacao>().ToTable("TabEstilizacao");
        }

    }
}

