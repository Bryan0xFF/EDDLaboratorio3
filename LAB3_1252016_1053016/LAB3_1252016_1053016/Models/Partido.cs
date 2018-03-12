using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAB3_1252016_1053016.Models
{
    public class Partido: IComparable<Partido>
    {
        public int NoPartido { get; set; }
        public DateTime FechaPartido { get; set; }
        public string Grupo { get; set; }
        public string Pais1 { get; set; }
        public string Pais2 { get; set; }
        public string Estadio { get; set; }

        public int CompareTo(Partido other)
        {
            return this.NoPartido.CompareTo(other.NoPartido); 
        }

        //public int CompareTo(Partido other)
        //{
        //    return this.FechaPartido.CompareTo(other.FechaPartido); 
        //}
    }
}