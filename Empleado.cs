using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_de_Nomina
{ //Genfi Bencosme Polanco 23-SISN-2-023
    // Creamos la clase empleado.
    public class Empleado
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public decimal SalarioBase { get; set; }
        public int HorasTrabajadas { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal Bonos { get; set; }
        public decimal Deducciones { get; set; }
    }

}
