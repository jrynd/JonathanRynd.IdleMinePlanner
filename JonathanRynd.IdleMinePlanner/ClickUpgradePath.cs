using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    class ClickUpgradePath
    {
        List<UpgradeID> _upgradeOrder;
        double efficiency;
        UpgradeState us;
        public List<UpgradeID> UpgradeOrder
        {
            private set { _upgradeOrder = value; }
            get { return _upgradeOrder; }
        }

        public UpgradeID this[int i]
        {
            private set { _upgradeOrder[i] = value; }
            get { return _upgradeOrder[i]; }
        }

        public ClickUpgradePath(UpgradeState us)
        {
            _upgradeOrder = new List<UpgradeID>();
            efficiency = 0;
            this.us = us;
        }

        public ClickUpgradePath(ClickUpgradePath x)
        {
            _upgradeOrder = new List<UpgradeID>();
            _upgradeOrder.AddRange(x._upgradeOrder); // deep copy
            us = x.us; // shallow is okay
        }

        private ClickUpgradePath()
        {
        }

        public struct UpgradeSimulationResult
        {
            public WealthStatistics w;
            public UpgradeState u;
            public double timeSpent;

            public UpgradeSimulationResult(WealthStatistics nw, UpgradeState nu, double nt)
            {
                // shallow copy
                this.w = nw;
                this.u = nu;
                this.timeSpent = nt;
            }
        }

        internal void Simulate(WealthStatistics initialStats,
            UpgradeState upgradesWithoutGemfinder,
            UpgradeState upgradesWithGemfinder,
            WealthStatistics wealthWithoutGemfinder,
            IList<WealthStatistics> wealthWithGemfinderBoughtAfterIthUpgrade)
        {
            var retval = new UpgradeSimulationResult[UpgradeOrder.Count + 1];
            List<WealthStatistics> currentStatistics = { new WealthStatistics(initialStats) };
            UpgradeState usWithoutGemfinder = new UpgradeState(us);
            UpgradeState usWithGemfinder;
            bool gfMaxed = usWithoutGemfinder.gfLevel == Upgrade.ic(UpgradeID.ICGEMFINDER).Count - 1;
            int itemsBoughtSoFar = 0;
            double secondsPerClick = 1/Upgrade.ic(UpgradeID.ICAUTOSPEED)[usWithoutGemfinder.afLevel].value;
            double gemsForEachVictoryBefore = Upgrade.ic(UpgradeID.ICGEMFINDER)[usWithoutGemfinder.gfLevel].value;
            double gemsForEachVictoryAfter = gfMaxed ? 0 : Upgrade.ic(UpgradeID.ICGEMFINDER)[usWithoutGemfinder.gfLevel + 1].value;
            int ore = -1;
            uint clicksForEachVictory=0;
            double timeForEachVictory = 0;
            double valueOfEachVictory=0;
            while (itemsBoughtSoFar < UpgradeOrder.Count)
            {
                UpgradeID pendingItem = _upgradeOrder[itemsBoughtSoFar];
                int upgradeLevelOfPendingItem = usWithoutGemfinder.instant[(int)pendingItem&127];

                Upgrade nextUpgrade = Upgrade.ic(pendingItem)[upgradeLevelOfPendingItem];
                double nextLevelCost = nextUpgrade.nextLevelCost;
                foreach (
                while (currentStatistics.Any(x=>x.Gold < nextLevelCost))
                {
                    if (ore == -1)
                    {
                        RecomputeOre(x, usWithoutGemfinder, secondsPerClick, out clicksForEachVictory, out valueOfEachVictory, out ore, out timeForEachVictory);
                    } 
                    currentStatistics.ForEach(x=>
                    {
                        x.Gold += valueOfEachVictory;
                        x.Gems += something ? gemsForEachVictoryBefore : gemsForEachVictoryAfter;
                        x.dataXp += clicksForEachVictory;
                        x.dataAge.Add(timeForEachVictory);
                    timeSpent += timeForEachVictory;
                    if (currentStatistics.dataXp > currentStatistics.dataXpThreshold || currentStatistics.dataAge.Sum() > currentStatistics.dataAgeThreshold)
                    {
                        currentStatistics.setThresholds();
                        ore = -1; // or recompute now
                    }
                    if (ore == currentStatistics.HighestOre)
                    {
                        currentStatistics.HighestOre++;
                        ore = -1; // or recompute now
                    }
                    if (ore == -1 && currentStatistics.Gold < nextLevelCost)
                    {
                        RecomputeOre(currentStatistics, usWithoutGemfinder, secondsPerClick, out clicksForEachVictory, out valueOfEachVictory, out ore, out timeForEachVictory);
                    }
                }

                // TODO: Handle this with a dictionary or list of delegates
                switch (pendingItem)
                {
                    case UpgradeID.ICAUTOSPEED:
                        usWithoutGemfinder.afLevel++;
                        secondsPerClick = Upgrade.ic(UpgradeID.ICAUTOSPEED)[usWithoutGemfinder.afLevel].value;
                        timeForEachVictory = clicksForEachVictory * secondsPerClick;
                        break;
                    case UpgradeID.ICAUTOPOWER:
                        usWithoutGemfinder.apLevel++;
                        ore = Ore.ComputeMostLucrativeOre(usWithoutGemfinder.instant, currentStatistics, out clicksForEachVictory, out valueOfEachVictory);
                        timeForEachVictory = clicksForEachVictory * secondsPerClick;
                        break;
                    case UpgradeID.ICGEMFINDER:
                        usWithoutGemfinder.gfLevel++;
                        gemsForEachVictoryBefore = Upgrade.ic(UpgradeID.ICGEMFINDER)[usWithoutGemfinder.gfLevel].value;
                        break;
                }
                currentStatistics.Gold -= nextLevelCost;
                timeSpent += 1.0; // it takes some time to click
                itemsBoughtSoFar++;
            }
            foreach (UpgradeSimulationResult thing in retval)
            {
                thing.u = usWithGemfinder;
            }
            retval[retval.Length()-1].u = usWithoutGemfinder;
            retval[retval.Length()-1].w = currentStatistics;
            return Tuple.Create(currentStatistics,usWithoutGemfinder);
        }

        private static void RecomputeOre(WealthStatistics currentStatistics, UpgradeState currentUpgradeState, double secondsPerClick, out uint clicksForEachVictory, out double valueOfEachVictory, out int ore, out double timeForEachVictory)
        {
            ore = Ore.ComputeMostLucrativeOre(currentUpgradeState.instant, currentStatistics, out clicksForEachVictory, out valueOfEachVictory);
            timeForEachVictory = VictorySeconds(secondsPerClick, clicksForEachVictory);
        }

        private static double VictorySeconds(double secondsPerClick, uint clicksForEachVictory)
        {
            return clicksForEachVictory * secondsPerClick;
        }
        
        
    }
}
