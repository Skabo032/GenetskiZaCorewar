using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetskiZaCorewar
{
    public class GeneticAlgorithm<T>
    {

        public List<DNA<T>> Population;
        public int Generation;
        public float BestFitness;
        public T[] BestGenes;

        private Func<string> getRandomGene;
        private Func<int, float> fitnessFunction;
        private Func<T> loadGene;

        public int dnaSize;
        public int Elitism;
        public float MutationRate;
        private List<DNA<T>> newPopulation;

        private Random random;
        private float fitnessSum;

        public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<string> getRandomGene, Func<int, float> fitnessFunction, int elitism, float mutationRate = 0.01f)
        {
            Generation = 0;
            Elitism = elitism;
            MutationRate = mutationRate;
            Population = new List<DNA<T>>(populationSize);
            newPopulation = new List<DNA<T>>(populationSize);
            this.random = random;
            this.dnaSize = dnaSize;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;

            BestGenes = new T[dnaSize];

          /* for(int i = 0; i<7;i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: false));
            }
            Population[0].Genes[0] = "spl $0, $0";
            Population[0].Genes[1] = "add #3039, $1";
            Population[0].Genes[2] = "mov $-2, $81";
            Population[0].Genes[3] = "djn $-2, <4800";
            Population[0].Genes[4] = "mov $1, <-1";

            Population[1].Genes[0] = "spl $0, $0";
            Population[1].Genes[1] = "add #3039, $1";
            Population[1].Genes[2] = "mov $-2, $81";
            Population[1].Genes[3] = "jmp $-2, $0";
            Population[1].Genes[4] = "mov $1, <-1";

            Population[2].Genes[0] = "mov <2, $3";
            Population[2].Genes[1] = "add $3, $-1";
            Population[2].Genes[2] = "jmp $-2, $0";
            Population[2].Genes[3] = "dat #0, #0";
            Population[2].Genes[4] = "dat #-5084, #5084";

            Population[3].Genes[0] = "mov $-1, @-1";
            Population[3].Genes[1] = "sub #28, $-2";
            Population[3].Genes[2] = "jmp $-2, #0";

            Population[4].Genes[0] = "mov #12, $-1";
            Population[4].Genes[1] = "mov @-2, <2";
            Population[4].Genes[2] = "djn $-1, $-3";
            Population[4].Genes[3] = "spl @3, $0";
            Population[4].Genes[4] = "add #653, $2";
            Population[4].Genes[5] = "jmz $-5, $-6";
            Population[4].Genes[6] = "dat #0, #833";

            Population[5].Genes[0] = "spl $0, $0";
            Population[5].Genes[1] = "mov $3, @3";
            Population[5].Genes[2] = "add $6, $2";
            Population[5].Genes[3] = "djn $-2, <2339";
            Population[5].Genes[4] = "jmp $1, $0";
            Population[5].Genes[5] = "spl $1, $-100";
            Population[5].Genes[6] = "mov $2, <-1";
            Population[5].Genes[7] = "jmp $-2, $0";
            Population[5].Genes[8] = "dat #2365, #-2365";

            Population[6].Genes[0] = "mov #6, $0";
            Population[6].Genes[1] = "mov <-1, <2";
            Population[6].Genes[2] = "jmp $-1, $-2";
            Population[6].Genes[3] = "spl @0, $1222";
            Population[6].Genes[4] = "sub #23, $-1";
            Population[6].Genes[5] = "jmz $-5, $-5";
            */



            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
            }

        }
        

     /*  public GeneticAlgorithm(int dnaSize, Random random, Func<T> loadGene, Func<int,float> fitnessFunction, int elitism, float mutationRate = 0.05f)
        {
            Generation = 0;
            Elitism = elitism;
            MutationRate = mutationRate;
            Population = new List<DNA<T>>(50);
            newPopulation = new List<DNA<T>>(50);
            this.random = random;
            this.dnaSize = dnaSize;
            this.loadGene = loadGene;
            this.fitnessFunction = fitnessFunction;
            for (int i = 0; i < 50; i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, loadGene, fitnessFunction));
            }

        }*/

        public GeneticAlgorithm()
        {

        }

        public void NewGeneration(float max)
        {
            if(Population.Count <= 0)
            {
                return;
            }

            CalculateFitness(max);
            Population.Sort(CompareDNA);
            newPopulation.Clear();
     
            for (int i = 0; i < Population.Count; i++)
            {
                if (i > 49 - Elitism) 
                    break;
                if (i < Elitism)
                {
                    newPopulation.Add(Population[i]);
                }
                DNA<T> parent1 = ChooseParent();
                DNA<T> parent2 = ChooseParent();

                DNA<T> child = parent1.Crossover(parent2);

                child.Mutate(MutationRate);

                newPopulation.Add(child);
               
            }
            List<DNA<T>> tmpList = Population;
            Population = newPopulation;
            newPopulation = tmpList;
            Generation++;
        }



        public void SaveGenerationToTxt(string filePath)
        {
            for(int i=0; i <Population.Count; i++)
            {
                FileReadWrite.WriteToTxtFile(filePath + i +".red", Population[i].ToString());
            }
        }


        public bool LoadGeneration(string filePath)
        {
            string savedData = "C:/Users/Skabo/source/repos/GenetskiZaCorewar/Populacija/Pokemon";
            if (System.IO.File.Exists(filePath))
                return false;

            for (int i = 0; i < 49; i++)
            {
                string path = savedData + i + ".red";
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: false));
            }
            return true;
        }

        public int CompareDNA(DNA<T> a, DNA<T> b) 
        {
            if (a.Fitness > b.Fitness)
                return -1;
            else if (a.Fitness < b.Fitness)
                return 1;
            else
                return 0;
        }

        public void CalculateFitness(float max)
        {
            fitnessSum = 0;
            DNA<T> best = Population[0];

            for(int i = 0; i < Population.Count; i++)
            {
                fitnessSum += Population[i].CalculateFitness(i);
                if(Population[i].Fitness > best.Fitness)
                {
                    best = Population[i];       
                }
                if(Population[i].Fitness > max)
                {
                    FileReadWrite.WriteNajbolji("C:/Users/Skabo/source/repos/GenetskiZaCorewar/Podaci/Najbolji.red", best, Generation, Population[i].Fitness);
                }
            }
            BestFitness = best.Fitness;
            best.Genes.CopyTo(BestGenes, 0);
        }

        private DNA<T> ChooseParent(int k = 0)
        {
            double randomNumber = random.NextDouble() * fitnessSum;

            
                for (int i = 0; i < Population.Count; i++)
                {
                    if (randomNumber < Population[i].Fitness)
                    {
                        Console.WriteLine("parent1 je choosovan");
                        return Population[i];
                    }
                    randomNumber -= Population[i].Fitness;
                }

                return null;
         
        }
        
       /* private DNA<T> ChooseParent2()
        {
            double randomNumber = random.NextDouble() * fitnessSum;

            for (int i = 2; i < Population.Count; i++)
            {
                if (randomNumber < Population[i].Fitness)
                {
                    Console.WriteLine("parent2 je choosovan");
                    return Population[i];
                }
                randomNumber -= Population[i].Fitness;
            }
            return null;
        }*/
        public override string ToString()
        {
            
            return Population[1].ToString();
        }
    }
}
