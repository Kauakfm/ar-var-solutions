using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Services
{
    public static class FeriadosBrasileiros
    {
        private static readonly HashSet<DateTime> feriados = new HashSet<DateTime>
    {
        // Feriados fixos
        new DateTime(DateTime.Now.Year, 1, 1), // Ano Novo
        new DateTime(DateTime.Now.Year, 4, 21), // Tiradentes
        new DateTime(DateTime.Now.Year, 5, 1), // Dia do Trabalhador
        new DateTime(DateTime.Now.Year, 9, 7), // Independência do Brasil
        new DateTime(DateTime.Now.Year, 10, 12), // Nossa Senhora Aparecida
        new DateTime(DateTime.Now.Year, 11, 2), // Finados
        new DateTime(DateTime.Now.Year, 11, 15), // Proclamação da República
        new DateTime(DateTime.Now.Year, 12, 25), // Natal

        // Feriados móveis - Páscoa (considerando as datas relativas à Páscoa)
        GetEaster(DateTime.Now.Year).AddDays(-2), // Sexta-feira Santa
        GetEaster(DateTime.Now.Year).AddDays(60), // Corpus Christi (60 dias após a Páscoa)
    };

        private static DateTime GetEaster(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;
            return new DateTime(year, month, day);
        }

        public static bool IsHoliday(DateTime date)
        {
            return feriados.Contains(date.Date);
        }
    }
}
