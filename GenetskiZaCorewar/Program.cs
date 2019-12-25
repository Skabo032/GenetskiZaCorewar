using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace GenetskiZaCorewar
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            Directory.CreateDirectory("../../../Populacija");   //Pravi folder gde ce se cuvati jedinke posebno
            Directory.CreateDirectory("../../../Podaci");       //Pravi folder za cuvanje podataka

            //dodavanje komentara zbog budjavog gita, spalicu te ako ne budes radio.
            int scoreKita = 0;      //skor iz jedne borbe
            float averageScore = 0; //prosecan skor cele generacije
            float scoreSum = 0;     //suma skorova cele generacije

            int populationSize = 50;    //misterija
            float mutationRate = 0.02f; //stepen mutacije
            int elitism = 2;            //broj najboljih jedinki koje nece biti promenjene u sledecoj generaciji

            GeneticAlgorithm<string> geneticAlgorithm = new GeneticAlgorithm<string>();
            Random random = new Random();
            string[] DozvoljeniOpCode = { "DAT", "MOV", "DJN", "ADD", "SUB", "MUL", "DIV", "JMP", "JMZ", "JMN", "SPL", "SEQ", "NOP" };  //jasno
            char[] DozvoljeniAdrModovi = { '#', '$', '*', '@', '<', '>', '{', '}' };                                                    //jasno

                    
            string podaciPath = "../../../Podaci/Podaci.txt";   //putanja do txt-a gde se cuvaju avg i max generacije
            string PopulacijaPath = "../../../Populacija/Pokemon";  //dep putanje do fajla gde se cuvaju jedinke

            geneticAlgorithm = new GeneticAlgorithm<string>(populationSize, 20, random, GetRandomGene, FitnessFunction, elitism, mutationRate); //pravi novu generaciju
            //geneticAlgorithm = new GeneticAlgorithm<string>(20, random, LoadGene, FitnessFunction, elitism, mutationRate);
            geneticAlgorithm.SaveGenerationToTxt(PopulacijaPath);


            float maxBestFitness = 0;   //najbolji skor ikada
           

        while(geneticAlgorithm.Generation < 5000)
        {
                scoreSum = 0;   //stavlja skor generacije na 0
                averageScore = 0;   //prosecan skor na 0
                geneticAlgorithm.NewGeneration(maxBestFitness); //pravi novu generaciju
                averageScore = scoreSum / geneticAlgorithm.Population.Count;    //racuna prosecan skor
                geneticAlgorithm.SaveGenerationToTxt(PopulacijaPath);                //cuva jedinke u fajlove posebno
                FileReadWrite.WritePodaci(podaciPath, geneticAlgorithm.BestFitness, averageScore, geneticAlgorithm.Generation); //cuva podatke u fajl
                Console.WriteLine("Average score: " + averageScore);
                Console.WriteLine("Najjaci pokemon u ovoj generaciji: " + geneticAlgorithm.BestFitness);
                if(geneticAlgorithm.BestFitness > maxBestFitness)
                {   //ako se u generaciji nasao neko sa vecim skorom od najboljeg do sada, onda taj skor sada postaje najbolji
                    maxBestFitness = geneticAlgorithm.BestFitness;
                }
                
        }
            Console.ReadKey();

        string GetRandomGene()  //pravi jednu random liniju red-code-a
            {
                string linija;
                int OpCodeIndex, AdrModeAIndex, AdrModeBIndex, OperandA, OperandB;

                    OpCodeIndex = random.Next(DozvoljeniOpCode.Length);
                    AdrModeAIndex = random.Next(DozvoljeniAdrModovi.Length);
                    AdrModeBIndex = random.Next(DozvoljeniAdrModovi.Length);

                   //OperandA = random.Next(-4000, 4000);
                   //OperandB = random.Next(-4000, 4000);

                OperandA = NormalDistributionRandom.NextGaussian(random, 0, 50);   //Operandi sa normalnom distribucijom
                OperandB = NormalDistributionRandom.NextGaussian(random, 0, 50);   //           -||-

                linija = DozvoljeniOpCode[OpCodeIndex] + " " + DozvoljeniAdrModovi[AdrModeAIndex] + OperandA + ", " + DozvoljeniAdrModovi[AdrModeBIndex] + OperandB + " ";
             
                return linija;               
            }

            float FitnessFunction(int i)    //izracunava fitness/skor jedne jedinke, msm
            {
                float score = 0;
                score = LaunchCommandLineApp(i);
                return score;
            }


            float LaunchCommandLineApp(int i)   //pokrece pmars-server za odrzavanje bitki
            {
                float scoreJedinke = 0;

                string line = "";

                // Odabrani ratnici za benchmark
                string gladijator1 = "BLUEFUNK.red";
                string gladijator2 = "CANNON.red";
                string gladijator3 = "FSTORM.red";
                string gladijator4 = "IRONGATE.red";
                string gladijator5 = "MARCIA13.red";
                string gladijator6 = "NOBODY.red";
                string gladijator7 = "PAPERONE.red";
                string gladijator8 = "PSWING.red";
                string gladijator9 = "RAVE.red";
                string gladijator10 = "THERMITE.red";
                string gladijator11 = "TIME.red";
                string gladijator12 = "TORNADO.red";

                // StartInfo klasa, za pokretanje nekog procesa iz cmd-a, jako korisna stvar!
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = "../../../pmars-server/pmars-server.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;

                //svaka while petlja je borba sa jednim gladijatorom, svaka borba ima broj rundi kao sto pise u startInfo.Arguments
                try
                {
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator1 + " -r 50";
                    Process exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))    //izvlaci podatak o skoru
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();       //saceka da se proces zavrsi
                    scoreJedinke += scoreKita;      //sracunava skor jedinke
                                                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator2 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator3 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator4 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator5 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator6 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator7 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator8 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator9 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator10 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator11 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------
                    startInfo.Arguments = "../../../Populacija/Pokemon" + i + ".red  ../../../pmars-server/Basic_Warriors/" + gladijator12 + " -r 50";
                    exeProcess = Process.Start(startInfo);
                    while (exeProcess.StandardOutput.EndOfStream == false)
                    {
                        line = exeProcess.StandardOutput.ReadLine();
                        if (line.Contains("scores"))
                        {
                            string[] data = line.Split(' ');
                            scoreKita = int.Parse(data[data.Length - 1]);
                            break;
                        }
                    }
                    exeProcess.WaitForExit();
                    scoreJedinke += scoreKita;
                    //--------------------------------------------------------------


                    scoreJedinke = scoreJedinke / 12 + 1;
                    scoreSum += scoreJedinke;

                    Console.WriteLine("Pokemon" + i + " Generacija " + geneticAlgorithm.Generation + ": " + scoreJedinke);
                    return scoreJedinke;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }

        }
      
    }
}
