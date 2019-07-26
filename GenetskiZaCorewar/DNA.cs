using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetskiZaCorewar
{
    public class DNA<T>
    {
        public string[] Genes;
        public float Fitness;

        private Random random;
        private Func<string> getRandomGene;
        private Func<T> loadGene;
        private Func<int, float> fitnessFunction;

        public override string ToString()
        {
            string Geni = "";
            foreach(string el in Genes)
            {
                Geni += el + "\r\n";
            }
            return Geni;
        }

        public DNA(int size, Random random, Func<string> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
        {
            Genes = new string[size];
            this.random = random;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;
            if (shouldInitGenes)
            { 

                for (int i = 0; i < Genes.Length; i++)
                {
                    Genes[i] = getRandomGene();
                }
            }
        }
      /* public DNA(int size, Random random, Func<T> loadGene, Func<int, float> fitnessFunction)
        {
            Genes = new T[size];
            this.random = random;
            this.loadGene = loadGene;
            this.fitnessFunction = fitnessFunction;
            for(int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = loadGene();
            }
        }*/

        public float CalculateFitness(int index)
        {
            Fitness = fitnessFunction(index);
            return Fitness;
        }

        public DNA<T> Crossover(DNA<T> otherParent)
        {
            DNA<T> child = new DNA<T>(Genes.Length, random, getRandomGene,fitnessFunction, shouldInitGenes: false);

            for (int i = 0; i < Genes.Length; i++)
            {
                child.Genes[i] = random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
            }
            Console.WriteLine("Krosover: " + Genes + " " + otherParent.Genes);
            return child;
        }

        public void Mutate(float mutationRate)
        {
            for(int i=0; i <Genes.Length; i++)
            {
                if(random.NextDouble() < mutationRate)
                {
                    Console.WriteLine("Nesto je mutirano");
                    Genes[i] = getRandomGene();
                }
            }
        }
    }
}
