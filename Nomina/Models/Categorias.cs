using System;
using System.Collections.Generic;

namespace Nomina.Models;

public partial class Categorias
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? TotalEmpleados { get; set; }

    public decimal? SueldoMaximo { get; set; }

    public virtual ICollection<Empleados> Empleados { get; } = new List<Empleados>();
}
