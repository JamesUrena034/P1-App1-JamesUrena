using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P1_App1_JamesUrena.Models;

public class TiposHuacales
{
    [Key]
    public int TipoId {  get; set; }

    [Required(ErrorMessage ="Debe de tener una Descripcion Obligatoria")]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage ="La Existencia debe ser Mayor o igual a 0")]
    public int Existencia { get; set; }


    [InverseProperty("TipoHuacales")]
    public virtual ICollection<EntradasHuacalesDetalles> EntradasHuacalesDetalle { get; set; } = new List<EntradasHuacalesDetalles>();
}
