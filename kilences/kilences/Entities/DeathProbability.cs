using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kilences.Entities
{
    class DeathProbability
    {
        //nem, kor és a halálozási valószínűség
        public Gender Gender { get; set; }
        public int Kor { get; set; }
        public double HaValószínűség { get; set; }
    }
}
