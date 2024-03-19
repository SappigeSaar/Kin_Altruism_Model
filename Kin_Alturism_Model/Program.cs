// See https://aka.ms/new-console-template for more information

using System.Net.Security;
using System.Runtime.InteropServices;

public class Main
{
    public Main()
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Storm here");

        string me = "EmmaReal?";
        Console.WriteLine(me);

    }
}

public class Creature
{
    Creature parent1;
    Creature parent2;
    List<Creature> children;

    private int food;
    public physicalsex sex;

    int foodupdate
    {
        get { return food; }

        set { if(value > 100)
            {
                food = 100;
            } else { food = value; }
        }
    }   
    int gene1;
    dominance gene1dom;
    sexgene sexgene1;
    int gene2;
    dominance gene2dom;
    sexgene sexgene2;

    public Creature(physicalsex sex, int gen1, dominance dom1, int gen2, dominance dom2)
    {
        this.food = 100;
        this.gene1 = gen1;
        this.gene1dom = dom1;
        this.gene2 = gen2;
        this.gene2dom = dom2;
        if(sex == physicalsex.male)
        {
            this.sexgene1 = sexgene.X;
            this.sexgene2 = sexgene.Y;
        }
        else
        {
            this.sexgene1 = sexgene.X;
            this.sexgene2 = sexgene.X;
        }
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
            this.gene1dom = mommy.gene1dom;
            this.sexgene1 = mommy.sexgene1;
        }
        else
        {
            this.gene1 = mommy.gene2;
            this.gene1dom = mommy.gene2dom;
            this.sexgene1 = mommy.sexgene2;
        }
        if (daddygene == 1)
        {
            this.gene1 = daddy.gene1;
            this.gene1dom = daddy.gene1dom;
            this.sexgene1 = daddy.sexgene1;
        }
        else
        {
            this.gene1 = daddy.gene2;
            this.gene1dom = daddy.gene2dom;
            this.sexgene1 = daddy.sexgene2;
        }

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