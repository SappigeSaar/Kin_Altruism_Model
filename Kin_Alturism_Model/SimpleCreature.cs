using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kin_Alturism_Model
{
    public class SimpleCreature
    {
        Parameters parameterlink;
        //parents and list of childern
        SimpleCreature? parent1;
        SimpleCreature? parent2;
        public List<SimpleCreature> children;

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
        bool doubledominance;
        private int phenoaltruism;
        public bool fertile;
        public bool disability;

        int altruism
        {
            get
            {
                int[] genes = new int[] { gene1val, gene2val };
                if (doubledominance) return (int)genes[rand.Next(0, 2)];
                else return phenoaltruism;
            }

            set
            {
                phenoaltruism = value;
            }
        }

        //genotype
        public int gene1val;
        dominance gene1dom;
        public int gene2val;
        dominance gene2dom;

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
            List<SimpleCreature> removedC = new List<SimpleCreature>();
            foreach (SimpleCreature child in children) if (child.Dies()) removedC.Add(child);
            foreach (SimpleCreature child in removedC) children.Remove(child);
        }

        /// <summary>
        /// is just a phase
        /// </summary>
        public void RunIteration()
        {
            food = food - parameterlink.hunger;

            if (gotfood)
                Handout();

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
            List<SimpleCreature> currentdist = new List<SimpleCreature>();
            if (parent1 != null) currentdist.Add(parent1);
            if (parent2 != null) currentdist.Add(parent2);
            foreach (SimpleCreature child in children) currentdist.Add(child);
            bool found = false;

            //loops 
            for (int dist = 1; dist <= altruism; dist++)
            {
                //only hands out once
                if (found) break;
                HashSet<SimpleCreature> nexthash = new HashSet<SimpleCreature>();
                foreach (SimpleCreature fam in currentdist)
                {
                    //if fam considered hungry, food handed out
                    int x = fam.foodupdate;
                    if (x < parameterlink.hungrybound)
                    {
                        fam.foodupdate = x + parameterlink.foodPerBundle;
                        if (parameterlink.selfishBonus >= rand.Next(1, 101)) foodupdate += parameterlink.bonusGain;
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
            if (sex == physicalsex.male)
            {
                MorF = "M";
            }
            else
            {
                MorF = "F";
            }
            return MorF + " " + altruism.ToString();
        }

        //returns hashset of related creatures
        public HashSet<SimpleCreature> related
        {
            get
            {
                HashSet<SimpleCreature> found = new HashSet<SimpleCreature>();
                if (parent1 != null) found.Add(parent1);
                if (parent2 != null) found.Add(parent2);
                foreach (SimpleCreature child in children) found.Add(child);
                return found;
            }
        }

        //creates a new creature based on two parents
        public SimpleCreature(SimpleCreature mommy, SimpleCreature daddy, Parameters parameterlink, Random rand)
        {
            //random number to see if mutated, mutation + (1) or - (2), mom (1) or dad (2) gene mutation, disabled, mommygene, daddygene,
            this.rand = rand;
            bool mutated = rand.Next(1, 101) <= parameterlink.mutationChance;
            bool disabled = rand.Next(1, 101) <= parameterlink.disabilityChance;
            int mutationDirection = rand.Next(1, 3);
            int momOrDad = rand.Next(1, 3);

            this.disability = disabled;
            this.food = parameterlink.startingFood;


            //sets parents
            this.parent1 = mommy;
            this.parent2 = daddy;
            int mommygene = rand.Next(1, 3);
            int daddygene = rand.Next(1, 3);
            //copy genes
            if (mommygene == 1)
            {
                this.gene1val = mommy.gene1val;
            }
            else
            {
                this.gene1val = mommy.gene2val;
            }
            if (daddygene == 1)
            {
                this.gene2val = daddy.gene1val;
            }
            else
            {
                this.gene2val = daddy.gene2val;
            }

            //mutate
            if (mutated)
            {
                if (momOrDad == 1)
                {
                    if (mutationDirection == 1)
                    {
                        if(gene1val == 10)
                        {

                        }
                        else
                        {
                            if ((gene1val >= parameterlink.altruismHalfwayPoint && gene1val + 1 >= parameterlink.altruismHalfwayPoint) || (gene1val < parameterlink.altruismHalfwayPoint && gene1val + 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene1dom == dominance.dominant) gene1dom = dominance.recessive;
                                else gene1dom = dominance.dominant;
                            }
                            this.gene1val += 1;
                        }
                    }
                    else
                    {
                        if (gene1val == 0)
                        {

                        }
                        else
                        {
                            if ((gene1val >= parameterlink.altruismHalfwayPoint && gene1val - 1 >= parameterlink.altruismHalfwayPoint) || (gene1val < parameterlink.altruismHalfwayPoint && gene1val - 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene1dom == dominance.dominant) gene1dom = dominance.recessive;
                                else gene1dom = dominance.dominant;
                            }
                            this.gene1val -= 1;
                        }
                    }
                }
                else
                {
                    if (mutationDirection == 1)
                    {
                        if (gene2val == 10)
                        {

                        }
                        else
                        {
                            if ((gene2val >= parameterlink.altruismHalfwayPoint && gene2val + 1 >= parameterlink.altruismHalfwayPoint) || (gene2val < parameterlink.altruismHalfwayPoint && gene2val + 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene2dom == dominance.dominant) gene2dom = dominance.recessive;
                                else gene2dom = dominance.dominant;
                            }
                            this.gene2val += 1;
                        }
                    }
                    else
                    {
                        if (gene2val == 0)
                        {

                        }
                        else
                        {
                            if ((gene2val >= parameterlink.altruismHalfwayPoint && gene2val - 1 >= parameterlink.altruismHalfwayPoint) || (gene2val < parameterlink.altruismHalfwayPoint && gene2val - 1 < parameterlink.altruismHalfwayPoint))
                            {
                            }
                            else
                            {
                                if (gene2dom == dominance.dominant) gene2dom = dominance.recessive;
                                else gene2dom = dominance.dominant;
                            }
                            this.gene2val -= 1;
                        }
                    }
                }
            }

            //decide sex
            if(daddygene == 1)
            {
                this.sex = physicalsex.female;
            }
            else
            {
                this.sex = physicalsex.male;
            }

            //domfucky?
            if(gene1dom == gene2dom)
            {
                this.doubledominance = true;
            }else if(gene1dom == dominance.dominant)
            {
                this.doubledominance = false;
                this.phenoaltruism = gene1val;
            }
            else
            {
                this.doubledominance = false;
                this.phenoaltruism = gene2val;
            }


            this.children = new List<SimpleCreature>();
            this.fertile = false;
            this.gotfood = false;
            this.parameterlink = parameterlink;

        }

        //creates a creature based on simple parameters
        public SimpleCreature(physicalsex sex, int altruism, dominance dom, Parameters parameterlink, Random rand)
        {
            this.sex = sex;
            this.altruism = altruism;
            this.food = parameterlink.maxfood;
            this.disability = false;

            this.doubledominance = true;
            this.gene1val = altruism;
            this.gene2val = altruism;
            this.gene1dom = dom;
            this.gene2dom = dom;
            

            this.children = new List<SimpleCreature>();
            this.fertile = false;
            this.gotfood = false;
            this.parameterlink = parameterlink;
            this.rand = rand;
        }

    }

}
