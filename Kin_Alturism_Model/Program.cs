// See https://aka.ms/new-console-template for more information

using System.Net.Security;
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
        
        //read the initiattion parameters from a file
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
            creature.gotFood = true;
        }
        //foreach index getfood to true
    }
}

public class Creature
{
    Creature ?parent1;
    Creature ?parent2;
    List<Creature> children;

    private int food;

    int foodupdate
    {
        get { return food; }

        set { if(value > 100)
            {
                food = 100;
            } else { food = value; }
        }
    }

    //phenotype
    public physicalsex sex;
    private bool domfucky;
    private int ?phenoalt;
    Random rand = new Random();
    public bool fertile;
    int altruism
    {
        get
        {
            gene[] genes = new gene[] { gene1, gene2 };
            if (domfucky) return genes[rand.Next(1, 3)].altruism;
            else return (int)phenoalt;
        }
    }

    //genotype
    gene gene1;
    gene gene2;

    public bool Dies()
    {
        if (food > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void RelationsUpdate()
    {
        if (parent1 != null) if (parent1.Dies()) parent1 = null;
        if (parent2 != null) if (parent2.Dies()) parent2 = null;
        foreach(Creature child in children)
        {
            if(child.Dies()) children.Remove(child);
        }
    }

    /// <summary>
    /// is just a phase
    /// </summary>
    public void RunIteration()
    {
        food = food - Parameters.hunger;

        if (gotFood)
            HandOut();

        //see if the creature can be fertile this phase
        if (food > Parameters.mateBound)
        {
            fertile = true;
        }
        else fertile = false;
    }




    public Creature(Creature mommy, Creature daddy)
    {
        this.food = 100;
        this.parent1 = mommy;
        this.parent2 = daddy;
        Random rand = new Random();
        int mommygene = rand.Next(1, 3);
        int daddygene = rand.Next(1, 3);
        if (mommygene == 1)
        {
            this.gene1 = mommy.gene1;
        }
        else
        {
            this.gene1 = mommy.gene2;
        }
        if (daddygene == 1)
        {
            this.gene2 = daddy.gene1;
            this.sex = physicalsex.female;
        }
        else
        {
            this.gene2 = daddy.gene2;
            this.sex = physicalsex.male;
        }
        this.children = new List<Creature>();
        this.fertile = false;

    }
    public Creature(physicalsex sex, int gen1, dominance dom1, int gen2, dominance dom2)
    {
        this.food = 100;
        this.sex = sex;
        sexgene sexgene2;
        if (sex == physicalsex.male)
        {
            sexgene2 = sexgene.Y;
        }
        else
        {
            sexgene2 = sexgene.X;
        }
        this.gene1 = new gene(gen1, dom1, sexgene.X);
        this.gene2 = new gene(gen2, dom2, sexgene2);
        this.children = new List<Creature>();
        this.fertile = false;
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

public struct gene
{
    public int altruism;
    public dominance dom;
    public sexgene type;

    public gene(int alt, dominance dommy, sexgene sex)
    {
        this.altruism = alt;
        this.dom = dommy;
        this.type = sex;
    }
}
public enum physicalsex
{
    female,
    male
}
