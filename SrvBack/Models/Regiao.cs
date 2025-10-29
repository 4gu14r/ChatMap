namespace SrvBack.Models
{
    public class Regiao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public int PaisId { get; set; }
    }
}
