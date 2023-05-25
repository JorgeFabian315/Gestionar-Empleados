using System;
using System.Collections.Generic;

namespace Nomina.Models;

public partial class Empleados
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal? Sueldo { get; set; }

    public bool? Activo { get; set; }

    public int CategoriaId { get; set; }

    public virtual Categorias Categoria { get; set; } = null!;
}
