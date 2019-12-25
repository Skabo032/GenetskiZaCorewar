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

            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
            }

        }


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
            
            string savedData = "../../../Populacija/Pokemon";
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
                    FileReadWrite.WriteNajbolji("../../../Podaci/Najbolji.red", best, Generation, Population[i].Fitness);
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
                        Console.WriteLine("parent je choosovan: Pokemon" + i);
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
