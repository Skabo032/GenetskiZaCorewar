using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetskiZaCorewar
{
    [Serializable]
    public class GeneticSaveData<T>
    {
        public List<T[]> PopulationGenes;
        public int Generation;
    }
}
