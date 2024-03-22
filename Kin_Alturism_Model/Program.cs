// See https://aka.ms/new-console-template for more information

using Kin_Alturism_Model;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

Main main = new Main();
public class Main
{
    Stream file = File.Open("..\\..\\..\\outputs\\outputAt.txt", FileMode.OpenOrCreate);

    List<Creature> population = new List<Creature>();
    List<Creature> males = new List<Creature>();
    List<Creature> females = new List<Creature>();

    Random random = new Random();

    public Main()
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Storm here");

        string me = "EmmaReal?";
        Console.WriteLine(me);

        Initialise();

        RunLoop();

        PrintResults();

        Console.WriteLine("done :3");
    }

    /// <summary>
    /// sets up everything the program needs
    /// </summary>
    public void Initialise()
    {
        dominance firstHalf;
        dominance secondHalf;
        //set up all the creatures

        //read the initiattion Parameters from a file
        StreamReader reader = new StreamReader("..\\..\\..\\initParameters1.txt");

        string line = reader.ReadLine();


        if (line == "first")
        {
            firstHalf = dominance.dominant;
            secondHalf = dominance.recessive;
        }
        else
        {
            firstHalf = dominance.recessive;
            secondHalf = dominance.dominant;
        }
        

        line = reader.ReadLine();
        int halfwayPoint = int.Parse(line);

        line = reader.ReadLine();
        string[] distribution = line.Split(',');

        for (int altruism = 0; altruism < distribution.Length; altruism++)
        {
            dominance dominance;
            if (altruism < halfwayPoint)
                dominance = firstHalf;
            else
                dominance = secondHalf;

            int total = int.Parse(distribution[altruism]);

            //make the fems
            for (int j = 0; j < (total/2); j++)
            {
                Creature creature = new Creature(sexgene.X, altruism, dominance, sexgene.X, altruism, dominance);
                population.Add(creature);
                females.Add(creature);
            }

            //make the mans
            for (int j = (total / 2); j < total; j++)
            {
                Creature creature = new Creature(sexgene.X, altruism, dominance, sexgene.Y, altruism, dominance);
                population.Add(creature);
                males.Add(creature);
            }
        }

        Console.WriteLine("initialise complete");
    }

    /// <summary>
    /// runs the program itself
    /// </summary>
    public void RunLoop()
    {
        int phaseCount = 1000;

        while (phaseCount > 0)
        {
            //assign food
            AssignFood();

            //run the creature iteration
            foreach (Creature creature in population)
            {
                //creature phase\
                creature.RunIteration();//foodupdate

                
            }
            //only after all food is handed out, check population. otherwise a creature might die and get handed food afterwards
            foreach (Creature creature in population)
            {
                //kills the creatures 
                if (creature.Dies())
                {
                    population.Remove(creature);

                    //also remove from list of sex
                    if (creature.sex == physicalsex.male)
                    {
                        males.Remove(creature);
                    }
                    else
                    {
                        females.Remove(creature);
                    }
                }
            }


            List<Creature> newM = new List<Creature>();
            List<Creature> newF = new List<Creature>();

            //make new creatures
            foreach (Creature female in females)
            {
                if (female.fertile)
                    foreach (Creature male in males)
                        if (male.fertile)
                        {
                            Creature creature = new Creature(female, male);
                            
                            //add the creature to all the right lists
                            if (creature.sex == physicalsex.female)
                                newF.Add(creature);
                            else
                                newM.Add(creature);
                            male.children.Add(creature);
                            female.children.Add(creature);

                            //creatures cant make multiple babies in the same iteration
                            female.fertile = false;
                            male.fertile = false;
                        }
            }
            foreach (Creature gal in newF)
            {
                females.Add(gal);
                population.Add(gal);
            }
            foreach (Creature ew in newM)
            {
                males.Add(ew);
                population.Add(ew);
            }


            foreach (Creature creature in population)
            {
                creature.RelationsUpdate();
            }

            phaseCount--;
        }

        
    }

    /// <summary>
    /// put the results og the run into a file
    /// </summary>
    public void PrintResults()
    {

    }

    public void AssignFood()
    {
        List<int> indexes = new List<int>();
        //create list of random integers(representing the indexes
        for (int i = 0; i < Parameters.numOfBundles; i++)
        {
            int randomnumber = random.Next(population.Count());
            while (indexes.Contains(randomnumber))
            {
                randomnumber = random.Next(population.Count());
            }

            indexes.Add(randomnumber);

            Creature creature = population[randomnumber];
            creature.gotfood = true;
        }
        //foreach index getfood to true
    }
}


public class gene
{
    public int altruism;
    public dominance dom;
    public sexgene type;

    public gene(int alt, dominance dommy, sexgene sex)
    {
        if(sex == sexgene.X)
        {
            this.altruism = alt;
            this.dom = dommy;
        }
        else
        {
            this.altruism = 69;
            this.dom = dominance.dominant;
        }
        this.type = sex;
    }
}
public enum dominance
{
    dominant,
    recessive
}

public enum sexgene
{
    X,
    Y
}

public enum physicalsex
{
    female,
    male
}
