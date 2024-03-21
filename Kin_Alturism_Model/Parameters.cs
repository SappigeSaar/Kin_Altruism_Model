using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kin_Alturism_Model
{
    public static class Parameters
    {
        //amount of creatures given food for each loop
        //possible values numOfBundles [0 - 2147483647]
        public const int numOfBundles = 5;
        //amount of food handed to a creature when given food
        //possible values foodPerBundle [1 - 100]
        public const int foodPerBundle = 3;

        //food limit
        //possible values maxfood [0 - 2147483647]
        public const int maxfood = 100;
        //at what food value a creature is considered starving
        //possible values hungrybound [0 - 100]
        public const int hungrybound = 10;
        //at what rate food decreases per loop
        //possible values hunger [0 - 100]
        public const int hunger = 1;
        //lower bound for reproduction
        //possible values mateBound [0 - maxfood]
        public const int mateBound = 60;

        //chance of mutation in offspring
        //possible values mutationChance: [0 - 100]
        public const int mutationChance = 3;
        //chance of disability in offspring
        //possible values disabilityChance
        public const int disabilityChance = 0;

        //halfway point
        //possible values??
        public const int altruismHalfwayPoint = 8;

        //chance of selfish altruism bonus
        //possible values selfishBonus [0 - 100]
        public const int selfishBonus = 0;
        //amount of food gained from altruism bonus
        //possible values bonusGain [0 - maxfood]
        public const int bonusGain = 0;


    }
}
