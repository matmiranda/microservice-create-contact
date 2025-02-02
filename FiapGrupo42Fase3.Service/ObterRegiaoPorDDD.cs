using FiapGrupo42Fase3.Domain.Interface;

namespace FiapGrupo42Fase3.Service
{
    public class ObterRegiaoPorDDD : IObterRegiaoPorDDD
    {
        public string ObtemRegiaoPorDDD(int DDD)
        {
            string regiao = DDD switch
            {
                63 or 68 or 69 or 92 or 95 or 96 or 97 => "1",
                71 or 73 or 74 or 75 or 77 or 79 or 81 or 82 or 83 or 84 or 85 or 86 or 87 or 88 or 89 or 98 or 99 => "2",
                61 or 62 or 64 or 65 or 66 or 67 => "3",
                11 or 12 or 13 or 14 or 15 or 16 or 17 or 18 or 19 or 21 or 22 or 24 or 27 or 28 or 31 or 32 or 33 or 34 or 35 or 37 or 38 => "4",
                41 or 42 or 43 or 44 or 45 or 46 or 47 or 48 or 49 or 51 or 53 or 54 or 55 => "5",
                _ => $"DDD_INVALIDO",
            };
            return regiao;
        }
    }
}
