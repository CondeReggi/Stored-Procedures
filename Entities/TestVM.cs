using System.ComponentModel.DataAnnotations;

namespace SQL_StoreProcedure.Controllers
{
    public class TestVM
    {
        [Required]
        public int IdTest { get; set; }
        public string Descripcion { get; set; }
    }
}