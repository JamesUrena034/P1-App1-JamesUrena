using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P1_App1_JamesUrena.Models;

public class EntradasHuacalesDetalles
{
    [Key]
    public int DetalleId { get; set; }

    public int EntradaId { get; set; }

    public int TipoId { get; set; }

    public int CantidadId { get; set; }
    public double Precio { get; set; }

    [ForeignKey("EntradaId")]
    [InverseProperty("EntradasHuacalesDetalle")]
    public virtual EntradasHuacales EntradaHuacales { get; set; } = null!;

    [ForeignKey("TipoId")]
    [InverseProperty("EntradasHuacalesDetalle")]
    public virtual TiposHuacales? TipoHuacales { get; set; }


}
