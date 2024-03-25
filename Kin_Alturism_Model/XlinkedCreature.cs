using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kin_Alturism_Model
{
    public class XlinkedCreature
    {
        Parameters parameterlink;
        //parents and list of childern
        XlinkedCreature? parent1;
        XlinkedCreature? parent2;
        public List<XlinkedCreature> children;

        //amount of food it has and if it got food this loop
        private int food;
        public bool gotfood;

        //method to update food value without going over bounds
        int foodupdate
        {
            get { return food; }

            set
            {
                if (value > parameterlink.maxfood)
                {
                    food = parameterlink.maxfood;
                }
                else { food = value; }
            }
        }

        //phenotype
        Random rand;
        public physicalsex sex;
        private bool doubledominance;
        private int phenoaltruism;
        public bool fertile;
        public bool disability;
        int altruism
        {
            get
            {
                gene[] genes = new gene[] { gene1, gene2 };
                if (doubledominance) return (int)genes[rand.Next(0, 2)].altruism;
                else return phenoaltruism;
            }
        }

        //genotype
        gene gene1;
        gene gene2;

        //returns whether the creature starves
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

        //removes dead direct relatives
        public void RelationsUpdate()
        {
            if (parent1 != null) if (parent1.Dies()) parent1 = null;
            if (parent2 != null) if (parent2.Dies()) parent2 = null;
            List<XlinkedCreature> removedC = new List<XlinkedCreature>();
            foreach (XlinkedCreature child in children) if (child.Dies()) removedC.Add(child);
            foreach (XlinkedCreature child in removedC) children.Remove(child);
        }

        /// <summary>
        /// is just a phase
        /// </summary>
        public void RunIteration()
        {
            food = food - parameterlink.hunger;

            if (gotfood) Handout();

            //see if the creature can be fertile this phase
            if (food > parameterlink.mateBound)
            {
                fertile = true;
            }
            else fertile = false;
        }

        //deals with obtained food
        public void Handout()
        {
            //looks at family distance 1
            List<XlinkedCreature> currentdist = new List<XlinkedCreature>();
            if(parent1 != null) currentdist.Add(parent1);
            if(parent2 != null) currentdist.Add(parent2);
            foreach (XlinkedCreature child in children) currentdist.Add(child);
            bool found = false;

            //loops 
            for (int dist = 1; dist <= altruism; dist++)
            {
                //only hands out once
                if (found) break;
                HashSet<XlinkedCreature> nexthash = new HashSet<XlinkedCreature>();
                foreach (XlinkedCreature fam in currentdist)
                {
                    //if fam considered hungry, food handed out
                    int x = fam.foodupdate;
                    if (x < parameterlink.hungrybound)
                    {
                        fam.foodupdate = x + parameterlink.foodPerBundle;
                        if(parameterlink.selfishBonus >= rand.Next(1, 101)) foodupdate += parameterlink.bonusGain;
                        found = true;
                        break;
                    }
                    nexthash.UnionWith(fam.related);
                }
                //sets list of relatives' relatives to current parsing list
                currentdist = nexthash.ToList();

            }
            //if no hungry fam found, eat food itself
            if (!found) foodupdate = food + parameterlink.foodPerBundle;
            gotfood = false;
        }

        //returns a string to evaluate the creature as a whole
        public override string ToString()
        {
            string MorF;
            if(sex == physicalsex.male)
            {
                MorF = "M";
            }
            else
            {
                MorF = "F";
            }
            return MorF + " " + gene1.ToString() + " " + gene2.ToString();
        }

        //returns hashset of related creatures
        public HashSet<XlinkedCreature> related
        {
            get
            {
                HashSet<XlinkedCreature> found = new HashSet<XlinkedCreature>();
                if (parent1 != null) found.Add(parent1);
                if (parent2 != null) found.Add(parent2);
                foreach (XlinkedCreature child in children) found.Add(child);
                return found;
            }
        }

        //creates a new creature based on two parents
        public XlinkedCreature(XlinkedCreature mommy, XlinkedCreature daddy, Parameters parameterlink, Random rand)
        {
            //random number to see if mutated, mutation + (1) or - (2), mom (1) or dad (2) gene mutation, disabled, mommygene, daddygene,
            this.rand = rand;
            bool mutated = rand.Next(1, 101) <= parameterlink.mutationChance;
            bool disabled = rand.Next(1, 101) <= parameterlink.disabilityChance;
            int mutationDirection = rand.Next(1, 3);
            int momOrDad = rand.Next(1, 3);

            this.disability = disabled;
            this.food = 50;

            //sets parents
            this.parent1 = mommy;
            this.parent2 = daddy;
            int mommygene = rand.Next(1, 3);
            int daddygene = rand.Next(1, 3);
            //copy genes
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
            }
            else
            {
                this.gene2 = daddy.gene2;
            }

            //mutate
            if (mutated)
            {
                if (gene1.type == gene2.type)
                {
                    if (momOrDad == 1)
                    {
                        if (mutationDirection == 1)
                        {
                            if ((gene1.altruism >= parameterlink.altruismHalfwayPoint && gene1.altruism + 1 >= parameterlink.altruismHalfwayPoint) || (gene1.altruism < parameterlink.altruismHalfwayPoint && gene1.altruism + 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene1.dom == dominance.dominant) gene1.dom = dominance.recessive;
                                else gene1.dom = dominance.dominant;
                            }
                            gene1.altruism += 1;
                        }
                        else
                        {
                            if ((gene1.altruism >= parameterlink.altruismHalfwayPoint && gene1.altruism - 1 >= parameterlink.altruismHalfwayPoint) || (gene1.altruism < parameterlink.altruismHalfwayPoint && gene1.altruism - 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene1.dom == dominance.dominant) gene1.dom = dominance.recessive;
                                else gene1.dom = dominance.dominant;
                            }
                            gene1.altruism -= 1;
                        }
                    }
                    else
                    {
                        if (mutationDirection == 1)
                        {
                            if ((gene2.altruism >= parameterlink.altruismHalfwayPoint && gene2.altruism + 1 >= parameterlink.altruismHalfwayPoint) || (gene2.altruism < parameterlink.altruismHalfwayPoint && gene2.altruism + 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene2.dom == dominance.dominant) gene2.dom = dominance.recessive;
                                else gene2.dom = dominance.dominant;
                            }
                            gene2.altruism += 1;
                        }
                        else
                        {
                            if ((gene2.altruism >= parameterlink.altruismHalfwayPoint && gene2.altruism - 1 >= parameterlink.altruismHalfwayPoint) || (gene2.altruism < parameterlink.altruismHalfwayPoint && gene2.altruism - 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene2.dom == dominance.dominant) gene2.dom = dominance.recessive;
                                else gene2.dom = dominance.dominant;
                            }
                            gene2.altruism -= 1;
                        }
                    }
                }
                else if (gene1.type == sexgene.X)
                {
                    if (mutationDirection == 1)
                    {
                        if ((gene1.altruism >= parameterlink.altruismHalfwayPoint && gene1.altruism + 1 >= parameterlink.altruismHalfwayPoint) || (gene1.altruism < parameterlink.altruismHalfwayPoint && gene1.altruism + 1 < parameterlink.altruismHalfwayPoint))
                        {
                        }
                        else
                        {
                            if (gene1.dom == dominance.dominant) gene1.dom = dominance.recessive;
                            else gene1.dom = dominance.dominant;
                        }
                        gene1.altruism += 1;
                    }
                    else
                    {
                        if ((gene1.altruism >= parameterlink.altruismHalfwayPoint && gene1.altruism - 1 >= parameterlink.altruismHalfwayPoint) || (gene1.altruism < parameterlink.altruismHalfwayPoint && gene1.altruism - 1 < parameterlink.altruismHalfwayPoint))
                        {
                        }
                        else
                        {
                            if (gene1.dom == dominance.dominant) gene1.dom = dominance.recessive;
                            else gene1.dom = dominance.dominant;
                        }
                        gene1.altruism -= 1;
                    }
                }
                else
                {
                    if (mutationDirection == 1)
                    {
                        if ((gene2.altruism >= parameterlink.altruismHalfwayPoint && gene2.altruism + 1 >= parameterlink.altruismHalfwayPoint) || (gene2.altruism < parameterlink.altruismHalfwayPoint && gene2.altruism + 1 < parameterlink.altruismHalfwayPoint))
                        {
                        }
                        else
                        {
                            if (gene2.dom == dominance.dominant) gene2.dom = dominance.recessive;
                            else gene2.dom = dominance.dominant;
                        }
                        gene2.altruism += 1;
                    }
                    else
                    {
                        if ((gene2.altruism >= parameterlink.altruismHalfwayPoint && gene2.altruism - 1 >= parameterlink.altruismHalfwayPoint) || (gene2.altruism < parameterlink.altruismHalfwayPoint && gene2.altruism - 1 < parameterlink.altruismHalfwayPoint))
                        {
                        }
                        else
                        {
                            if (gene2.dom == dominance.dominant) gene2.dom = dominance.recessive;
                            else gene2.dom = dominance.dominant;
                        }
                        gene2.altruism -= 1;
                    }
                }
            }


            //decide sex and altruism phenotype
            if (gene1.type == sexgene.Y)
            {
                this.sex = physicalsex.male;
                this.doubledominance = false;
                this.phenoaltruism = gene2.altruism;
            }
            else if (gene2.type == sexgene.Y)
            {
                this.sex = physicalsex.male;
                this.doubledominance = false;
                this.phenoaltruism = gene1.altruism;
            }
            else
            {
                if (gene1.dom == gene2.dom)
                {
                    this.phenoaltruism = 69;
                    this.doubledominance = true;
                }
                else
                {
                    this.doubledominance = false;
                    if (gene1.dom == dominance.dominant)
                    {
                        this.phenoaltruism = gene1.altruism;
                    }
                    else this.phenoaltruism = gene2.altruism;
                }

                this.sex = physicalsex.female;
            }




            this.children = new List<XlinkedCreature>();
            this.fertile = false;
            this.gotfood = false;
            this.parameterlink = parameterlink;
        }

        //creates a creature based on simple parameters
        public XlinkedCreature(sexgene sex1, int gen1, dominance dom1, sexgene sex2, int gen2, dominance dom2, Parameters parameterlink, Random rand)
        {
            this.food = parameterlink.maxfood;
            this.disability = false;

            this.gene1 = new gene(gen1, dom1, sex1);
            this.gene2 = new gene(gen2, dom2, sex2);
            this.children = new List<XlinkedCreature>();
            this.fertile = false;
            this.gotfood = false;
            this.parameterlink = parameterlink;
            this.rand = rand;

            if (sex1 == sex2)
            {
                this.sex = physicalsex.female;
            }
            else
            {
                this.sex = physicalsex.male;
            }

            if (dom1 == dom2)
            {
                this.doubledominance = true;
                this.phenoaltruism = 69;
            }
            else if (dom1 == dominance.dominant)
            {
                this.doubledominance = false;
                this.phenoaltruism = gen1;
            }
            else
            {
                this.doubledominance = false;
                this.phenoaltruism = gen2;
            }
        }

    }

}
