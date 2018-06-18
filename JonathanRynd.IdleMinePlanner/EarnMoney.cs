using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SolverFoundation.Services;


namespace JonathanRynd.IdleMinePlanner
{
 
    class EarnMoney
    {
        

        static IList<Ore> BuildFeasibleOreList(double hpc)
        {
            List<Ore> localOreList = new List<Ore>();
            // double bestValuePerClick = 0;
            Ore fakeOreToFindOneclick = new Ore() { Defense = 0, Hp = hpc };
            int oneClickIndex = Ore.oreList.BinarySearch(fakeOreToFindOneclick, new OreOnehitComparer());
            if (oneClickIndex < 0)
            {
                oneClickIndex = ~oneClickIndex - 1;
            }
            int bextRpc = Ore.OptimalOreForCash(hpc, oneClickIndex);
            // not sure, do I need to update ores here? Or can I just skip up to oneClickIndex and then take until bextRpc?
            for (int i=oneClickIndex; i <= bextRpc; i++)
            {
                Ore.oreList[i].UpdateClicks(hpc);
                localOreList.Add(Ore.oreList[i]);
            }
            return localOreList;
        }

        public static IList<Tuple<Ore,int>> MiningPlanToEarnSpecifiedGoldAndGems(double hpc, double goldNeeded, double gemsNeeded)
        {
            IList<Ore> localOreList = BuildFeasibleOreList(hpc);
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();
            Set items = new Set(Domain.Any, "items");
            Decision mine = new Decision(Domain.IntegerNonnegative, "mine", items);
            model.AddDecision(mine);
            Parameter value = new Parameter(Domain.RealNonnegative, "value", items);
            value.SetBinding(localOreList, "Value", "Name");
            Parameter clicks = new Parameter(Domain.IntegerNonnegative, "clicks", items);
            clicks.SetBinding(localOreList, "Clicks", "Name");

            model.AddParameters(value, clicks);
            model.AddConstraint("knap_value", Model.Sum(Model.ForEach(items, t => mine[t] * value[t])) >= goldNeeded);
            model.AddConstraint("knap_gems", Model.Sum(Model.ForEach(items, t => mine[t] )) >= gemsNeeded);
            model.AddGoal("knap_time", GoalKind.Minimize, Model.Sum(Model.ForEach(items, t => mine[t] * clicks[t])));
            var report = context.CheckModel();
            Console.Write(report);
            Solution solution = context.Solve(new SimplexDirective());
            //Report report = solution.GetReport();
            
            List<Tuple<Ore,int>> retval = new List<Tuple<Ore,int>>();
            for (int i=0; i<localOreList.Count; i++)
            {
                int temp = (int)mine.GetDouble(i);
                retval.Add(Tuple.Create(localOreList[i], temp));
            }
            return retval;
        }

    }
}
