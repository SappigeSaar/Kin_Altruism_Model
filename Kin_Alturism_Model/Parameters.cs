using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kin_Alturism_Model
{
    public class Parameters
    {
        //amount of creatures given food for each loop
        //possible values numOfBundles [0 - 2147483647]
        public int numOfBundles = 5;
        //amount of food handed to a creature when given food
        //possible values foodPerBundle [1 - 100]
        public int foodPerBundle = 10;

        //food limit
        //possible values maxfood [0 - 2147483647]
        public int maxfood = 100;
        //at what food value a creature is considered starving
        //possible values hungrybound [0 - 100]
        public int hungrybound = 40;
        //at what rate food decreases per loop
        //possible values hunger [0 - 100]
        public int hunger = 15;
        //lower bound for reproduction
        //possible values mateBound [0 - maxfood]
        public int mateBound = 60;

        //chance of mutation in offspring
        //possible values mutationChance: [0 - 100]
        public int mutationChance = 3;
        //chance of disability in offspring
        //possible values disabilityChance
        public int disabilityChance = 0;

        //halfway point
        //possible values??
        public int altruismHalfwayPoint;

        //chance of selfish altruism bonus
        //possible values selfishBonus [0 - 100]
        public int selfishBonus = 0;
        //amount of food gained from altruism bonus
        //possible values bonusGain [0 - maxfood]
        public int bonusGain = 0;

        public Parameters(int halfwaypoint, string[] parameters)
        {
            this.altruismHalfwayPoint = halfwaypoint;

            if (parameters != null)
                foreach (string p in parameters)
                {
                    string[] ps = p.Split(' ');
                    string paramType = ps[0];
                    int value = int.Parse(ps[1]);

                    switch (paramType)
                    {
                        case ("numOfBundles"):
                            this.numOfBundles = value;
                            break;
                        case ("foodPerBundle"):
                            this.foodPerBundle = value;
                            break;
                        case ("maxfood"):
                            this.maxfood = value;
                            break;
                        case ("hungrybound"):
                            this.hungrybound = value;
                            break;
                        case ("hunger"):
                            this.hunger = value;
                            break;
                        case ("mateBound"):
                            this.mateBound = value;
                            break;
                        case ("mutationChance"):
                            this.mutationChance = value;
                            break;
                        case ("disabilityChance"):
                            this.disabilityChance = value;
                            break;
                        case ("selfishBonus"):
                            this.selfishBonus = value;
                            break;
                        case ("bonusGain"):
                            this.bonusGain = value;
                            break;

                    }
                }
        }
    }
}
