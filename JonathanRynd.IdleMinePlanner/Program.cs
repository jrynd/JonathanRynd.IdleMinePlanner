using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    class Program
    {
        static void PromptForCurrentStatistics(UpgradeState ua, WealthStatistics wealth)
        {
            PromptForCurrentUpgradeState(ua);
            PromptForCurrentWealth(wealth);
        }

        private static void PromptForCurrentWealth(WealthStatistics wealth)
        {
            Console.WriteLine("Enter current gems");
            wealth.Gems = ulong.Parse(Console.ReadLine());
            Console.WriteLine("Enter current idle multiplier x 100 (no decimal point)");
            wealth.dataAge = ulong.Parse(Console.ReadLine());
            Console.WriteLine("Enter current active multiplier x 100 (no decimal point)");
            wealth.dataXp = ulong.Parse(Console.ReadLine());
            Console.WriteLine("Enter current gold");
            wealth.Gold = double.Parse(Console.ReadLine());
        }

        private static void BS(uint temp)
        {
            Console.WriteLine("Blacksmith skill displayed as {0}%, actual range {1}%-{2}%, average {3}%",
                temp + 100,
                temp * .5 + 80.0,
                temp * .5 + 130.0,
                temp * .5 + 105.0);
        }

        private static void BX(uint upgradeLevel)
        {
            Console.WriteLine("Skill level {0} means plus prob ranges from {1}%-{2}% depending on rank",
                upgradeLevel,
                40 + upgradeLevel,
                75 + upgradeLevel);
        }

        private static void GW(uint upgradeLevel)
        {
            Console.WriteLine("GW level {0} means max gems to use is {1}",
                upgradeLevel,
                Math.Floor(Math.Pow(3.4, upgradeLevel)));
        }

        private static void BF(uint upgradeLevel)
        {
            Console.WriteLine("Efficiency {0} means effective gem costs are {1},{2},{3},{4},{5},{6}",
                upgradeLevel,
                100.0 / (100 - upgradeLevel),
                300.0 / (100 - upgradeLevel),
                1100.0 / (100 - upgradeLevel),
                3900.0 / (100 - upgradeLevel),
                13300.0 / (100 - upgradeLevel),
                45400.0 / (100 - upgradeLevel));
        }

        private static void AP(uint upgradeLevel)
        {
            Console.WriteLine("AP level {0} means a {1}% multiplier",
                upgradeLevel,
                50 + upgradeLevel * 8);
        }

        private static void AF(uint upgradeLevel)
        {
            double ticks = Math.Pow(1.0528, -upgradeLevel);
            Console.WriteLine("AF level {0} means {1} clicks per second",
                upgradeLevel,
                ticks);
        }

        private static void GF(uint upgradeLevel)
        {
            double level = 4 + .4 * upgradeLevel;
            Console.WriteLine("GF level {0} means {1}% chance of finding",
                upgradeLevel,
                level);
        }

        private static readonly StateMachine[] m = new StateMachine[]
        {
            null,
            new StateMachine("blacksmith skill",30,UpgradeID.BSSKILL,BS),
            new StateMachine("blacksmith expertise",10,UpgradeID.BSEXPERTISE,BX),
            new StateMachine("blacksmith efficiency",50,UpgradeID.BSEFFICIENCY,BF),
            new StateMachine("gem waster",5,UpgradeID.BSGEMWASTER,GW),
            new StateMachine("autoclick power",250,UpgradeID.ICAUTOPOWER,AP),
            new StateMachine("autoclick faster",100,UpgradeID.ICAUTOSPEED,AF),
            new StateMachine("gemfinder",100,UpgradeID.ICGEMFINDER,GF),
        };

        private static uint TransitionFunction(uint currentState, UpgradeState ua)
        {
            StateMachine s = m[currentState];
            Console.WriteLine("Enter {0}, 0 for no upgrades, max {1}, negative to go back",
                s.prompt,
                s.max);
            int temp;
            if (!int.TryParse(Console.ReadLine(),out temp) || temp > s.max) return currentState;
            if (temp < 0) return currentState-1;
            ua.blacksmith[currentState] = temp;
            if (s.f != null) s.f.Invoke((uint)temp);
            return currentState+1;
        }

        private static void PromptForCurrentUpgradeState(UpgradeState ua)
        {
        BBP:
            Console.WriteLine("Enter blacksmith base power level, 0 for no upgrades, must be nonnegative");
            if (!int.TryParse(Console.ReadLine(),out ua.blacksmith[0])) goto BBP;
            // optionally show actual level/price from array
            uint state=1;
            do 
            {
                state = TransitionFunction(state,ua);
                if (state==0) goto BBP;
            } while (state < 8);
        }

        static void Main(string[] args)
        {
            UpgradeState ua = new UpgradeState();
            WealthStatistics wealth = new WealthStatistics();
            PromptForCurrentStatistics(ua,wealth);
            var x = new IdleMineOptimizer(ua, wealth);
            x.Calc();
            OutputUpgradePath(x.OptimalUpgradePath);
            OutputShoppingCart(x.OptimalShoppingCart);

        }

        private static void OutputShoppingCart(ShoppingCart s)
        {
            throw new NotImplementedException();
        }
        private static void OutputUpgradePath(ClickUpgradePath clickUpgradePath)
        {
            throw new NotImplementedException();
        }
    }
}
