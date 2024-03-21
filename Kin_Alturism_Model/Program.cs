// See https://aka.ms/new-console-template for more information

using Kin_Alturism_Model;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System;
using System.Reflection.Metadata.Ecma335;

Main main = new Main();
public class Main
{
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

        Console.WriteLine(":3");
    }

    /// <summary>
    /// sets up everything the program needs
    /// </summary>
    public void Initialise()
    {
        //set up all the creatures
        
        //read the initiattion Parameters from a file
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

                //kills the creatures 
                if (creature.Dies())
                {
                    population.Remove(creature);
                }
            }

            //make new creatures
            foreach (Creature female in females)
            {
                if (female.fertile)
                    foreach (Creature male in males)
                        if (male.fertile)
                        {
                            Creature creature = new Creature(female, male);
                            
                            //add the creature to all the right lists
                            population.Add(creature);
                            if (creature.sex == physicalsex.female)
                                females.Add(creature);
                            else
                                males.Add(creature);

                            //creatures cant make multiple babies in the same iteration
                            female.fertile = false;
                            male.fertile = false;
                        }
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
    public int ?altruism;
    public dominance ?dom;
    public sexgene type;

    public gene(int alt, dominance dommy, sexgene sex)
    {
        if(sex == sexgene.X)
        {
            this.altruism = alt;
            this.dom = dommy;
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
