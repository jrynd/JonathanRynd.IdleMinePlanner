using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    class IdleMineOptimizer
    {
        public UpgradeState initialUpgradeState;
        public WealthStatistics initialStats;
        private ClickUpgradePath _optimalUpgradePath;

	    public ClickUpgradePath OptimalUpgradePath
	    {
		    get { return _optimalUpgradePath;}
		    private set { _optimalUpgradePath = value;}
	    }

        private ShoppingCart _optimalShoppingCart;

        public ShoppingCart OptimalShoppingCart
        {
            get { return _optimalShoppingCart; }
            private set { _optimalShoppingCart = value; }
        }
        
        public IdleMineOptimizer(UpgradeState ua, WealthStatistics inst)
        {
            initialUpgradeState = ua;
            initialStats = inst;
        }

        private const int defaultMaxItems = 6;

        IEnumerable<ShoppingCart> AllReasonableShoppingCarts(int maxItems=defaultMaxItems)
        {
            // use initialUpgradeState and current

            
//            var efficientFrontiers = new SortedList<double,double>();
//            double minPrice = double.MaxValue;
//            double maxValue = 0;
            if (maxItems==0) return new List<ShoppingCart>();

            int[] numBS = { 0, 1, 2 };
            int[] numBX = { 0, 1 };
            // TODO: 
            return (from bs in numBS
                    from bx in numBX
                    from bbp in Enumerable.Range(bs+bx,maxItems-bs-bx-bs-bx)
                    where bs + initialUpgradeState.bsLevel < Upgrade.ic(UpgradeID.BSSKILL).Count
                    && bx + initialUpgradeState.bxLevel < Upgrade.ic(UpgradeID.BSEXPERTISE).Count
                    select new ShoppingCart() { a = initialUpgradeState, NumBBP = bbp, NumBS = bs, NumBX = bx, NumBF = 0 });
                        // TODO: some method of paring it down
#if false
                        var price = sc.TotalCostWithoutAndWithGemwaster();
                        var value = sc.ChancesOfBeatingTargetAndExpectedPsIfYouDo();
                        if (price <= minPrice)
                        {
                            if (value > maxValue) maxValue = value;
                            efficientFrontiers.Add(price, value);
                            AddVariants(retval, sc);
                        }
                        else if (value >= maxValue)
                        {
                            maxValue = value;
                            efficientFrontiers.Add(price, value);
                            AddVariants(retval, sc);
                        }
                        else
                        {
                            // get index that it would be added at
                            // check that its value is beter than the value of lowerpriced item
                            if (true)
                            {
                                efficientFrontiers.Add(price, value);
                                // if both its price and value are better than higher-priced item, replace in both lists
                            }
                        }
#endif
        }

        IEnumerable<ClickUpgradePath> AllReasonableUpgradePaths(int maxItems = defaultMaxItems)
        {
            // use initialUpgradeState, cloning if necessary
            List<ClickUpgradePath> retval = new List<ClickUpgradePath>();
            retval.Add(new ClickUpgradePath(initialUpgradeState));
            for (int pathLength = 1; pathLength <= maxItems; pathLength++)
            {
                // the best thing to buy will either be the most efficient thing currently, or something cheaper
                // once we have a bunch of items, try a few different orderings to see if any are clear losers -- 
                // obviously the gps and rps at the end will be the same, but
                // the total earned gems and time spent will be different
            }
            return retval;
        }

        private void ARUPInternal(ClickUpgradePath soFar, double hpc, double currentGps, int af, int ap, int maxItems, List<ClickUpgradePath> retval)
        {
            if (maxItems == 0) return;
#if false
            // gemfinder addition is handled by simulateUpgradePath
            if (!soFar.UpgradeOrder.Contains(UpgradeID.ICGEMFINDER))
            {
                ClickUpgradePath withGemfinder = new ClickUpgradePath(soFar);
                withGemfinder.UpgradeOrder.Add(UpgradeID.ICGEMFINDER);
                retval.AddRange(ARUPInternal(withGemfinder, oldGemsPerSecond, af, ap, gf+1, maxItems - 1,retval));
            }
#endif
            var autoPowerList = Upgrade.ic(UpgradeID.ICAUTOPOWER);
            var autoSpeedList = Upgrade.ic(UpgradeID.ICAUTOSPEED);
            double costOfClickFaster = autoSpeedList[af].nextLevelCost;
            double costOfClickPower = autoPowerList[ap].nextLevelCost;
            bool clickFasterIsCheaper = costOfClickFaster < costOfClickPower;
            double hpcAfterPowerUpgrade = hpc * autoPowerList[ap+1].value/autoPowerList[ap].value; // old hpc * power increment
            // determine which is the optimal ore at that rawdamage level
            double gpsIfYouClickPowerful = Ore.oreList[Ore.OptimalOreForCash(hpcAfterPowerUpgrade, 0)].ValuePerClick * autoSpeedList[af].value;
            double clickFasterEfficiency = 1.0528/costOfClickFaster;
            double clickPowerEfficiency = gpsIfYouClickPowerful/(currentGps*costOfClickPower);
            bool clickFasterIsMoreEfficient = clickFasterEfficiency > clickPowerEfficiency;
            ClickUpgradePath withClickFaster = new ClickUpgradePath(soFar);
            withClickFaster.UpgradeOrder.Add(UpgradeID.ICAUTOSPEED);
            ClickUpgradePath withClickPower = new ClickUpgradePath(soFar);
            withClickPower.UpgradeOrder.Add(UpgradeID.ICAUTOPOWER);
            if (clickFasterIsCheaper || clickFasterIsMoreEfficient)
            {
                ARUPInternal(withClickFaster,hpc,currentGps*1.0528,af+1,ap,maxItems-1,retval);
            }
            if (!clickFasterIsCheaper || !clickFasterIsMoreEfficient)
            {
                ARUPInternal(withClickPower,hpcAfterPowerUpgrade,gpsIfYouClickPowerful,af,ap+1,maxItems-1,retval);
            }
        }

        private void AddVariants(List<ShoppingCart> retval,ShoppingCart sc)
        {
 	        //retval.Add(sc);
            int[] zeroOne = {0,1};
            retval.AddRange(from bf in zeroOne
                            select new ShoppingCart()
                   { NumBS = sc.NumBS, NumBBP = sc.NumBBP, a=sc.a, NumBF = bf, NumBX=sc.NumBX });
        }
        
        private static readonly double lnConfidenceLevel = Math.Log(.2);

        IEnumerable<double> GetPCollection()
        {
            // TODO: higher start point / higher spacing based on blacksmith upgrades
            // BEST would be to generate these AFTER the cart+path
            // to estimate chance of hitting each breakpoint
            return Enumerable.Range(0,20).Select(x=>initialStats.CurrentP*(1+x*.2));
        }
        


        public void Calc()
        {
            double bestEfficiency = 0;
            ShoppingCart cartForBestEfficiency = null;
            ClickUpgradePath pathForBestEfficiency = null;
            int gwForBestEfficiency = -1;

            var pathCollection = AllReasonableUpgradePaths();
            var cartCollection = AllReasonableShoppingCarts();
            var pCollection = GetPCollection(); // collection of P values to shoot for

            foreach (var upgradePath in pathCollection)
            {
                IList<ClickUpgradePath.UpgradeSimulationResult> pathSimulationGroup = upgradePath.Simulate(initialStats);
                foreach (ClickUpgradePath.UpgradeSimulationResult pathSimulation in pathSimulationGroup)
                {
                    WealthStatistics pathResult = pathSimulation.w;
                    UpgradeState completedInstantUpgrades = pathSimulation.u;
                    double timeSpent = pathSimulation.timeSpent;
                
                double clicksPerSecond = Upgrade.ic(UpgradeID.ICAUTOSPEED)[completedInstantUpgrades.afLevel].value;
                double gemFinderOdds = Upgrade.ic(UpgradeID.ICGEMFINDER)[completedInstantUpgrades.gfLevel].value;
                double autoPowerMultiplier = Upgrade.ic(UpgradeID.ICAUTOPOWER)[completedInstantUpgrades.apLevel].value;
                foreach (var sc in cartCollection)
                {
                    var costs = sc.TotalCostWithoutAndWithGemwaster();
                    //double timeSpentBuyingBSUpgradesOnceWeHaveEnoughMoney = (sc.NumBBP + sc.NumBF + sc.NumBS + sc.NumBX)/6.5 + (sc.NumBBP>0 ? 1 : 0) + (sc.NumBF<0 ? 1 : 0) + (sc.NumBS>0?1:0)+(sc.NumBX>0?1:0);
                    double efficiencyDiscount = 0.01 * (100 - completedInstantUpgrades.bfLevel - sc.NumBF);
                    foreach (var desiredP in pCollection)
                    {
                        int gw = 0;

                        foreach(var expect in sc.ChancesOfBeatingTargetAndExpectedPsIfYouDo(desiredP)) // each item corresponds to a different gemwaster
                        {
                            double costOfShoppingCart = (gw <= this.initialUpgradeState.gwLevel) ? costs.Item1 : costs.Item2;
                            double averageGemsForOneCraft = ShoppingCart.gwNumberOfGems[gw] * efficiencyDiscount;
                            double failChance = 1-expect.Item1;
                            // need to solve failChance^expectedNumCrafts < .2
                            // n ln failchance < ln .2
                            // n = ln .2 / ln failchance
                            double expectedNumCrafts = lnConfidenceLevel/Math.Log(failChance);
                            double expectedCraftingTime = expectedNumCrafts / 6.5;
                            double gemsRequired = expectedNumCrafts * averageGemsForOneCraft - pathResult.Gems; // still exact
                            // due to linearity of expectation
                            double expectedNeededOresMined = Math.Ceiling(gemsRequired / gemFinderOdds);
                            double hpc = pathResult.IdleActiveMultipliers()*pathResult.CurrentP*autoPowerMultiplier;
                            var miningList = EarnMoney.MiningPlanToEarnSpecifiedGoldAndGems(hpc, costOfShoppingCart, expectedNeededOresMined);
                            double timeSavingUpGoldAndGemsForCraft = miningList.Sum(x => x.Item2 * x.Item1.Clicks)*Upgrade.ic(UpgradeID.ICAUTOSPEED)[completedInstantUpgrades.afLevel].value;
                            double totalTime = timeSpentFollowingUpgradePath
                                + timeSavingUpGoldAndGemsForCraft
                                //+ timeSpentBuyingBSUpgradesOnceWeHaveEnoughMoney // can buy them on-the-run
                                + expectedCraftingTime;
                            pathResult.CurrentP = expect.Item2;
                            int clicksForEachVictory;
                            double valueOfEachVictory;
                            var stats = Ore.OptimalOreForCash( ); completedInstantUpgrades.instant, pathResult, out clicksForEachVictory, out valueOfEachVictory);

                            gw++;
                        }
                    }
                    
                    
                    
                }
                
            }

        }

    }
}
