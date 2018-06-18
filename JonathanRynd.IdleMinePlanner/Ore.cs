using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    public class MinDamageComparer: IComparer<Ore>
    {
        public int Compare(Ore x, Ore y)
        {
            return x.MinHpcToConsider.CompareTo(y.MinHpcToConsider);
        }
    }

    public class MaxDamageComparer: IComparer<Ore>
    {
        public int Compare(Ore x, Ore y)
        {
            return x.MaxHpcToConsider.CompareTo(y.MaxHpcToConsider);
        }
    }

    public class OreOnehitComparer : IComparer<Ore>
    {
        public int Compare(Ore x, Ore y)
        {
            return (x.Hp + x.Defense).CompareTo(y.Hp + y.Defense);
        }
    }

    public class Ore
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public uint Clicks { get; set; } // clicks per victory
        public double ValuePerClick { get; set; }
        public double Hp { get; set; }
        public double Defense { get; set; }
        public double MaxHpcToConsider { get; set; }
        public double MinHpcToConsider { get; set; }
        public int MaxHitsToWorryAbout { get; set; }
        public double MaxHpcOfAllAbove {get; set; }

        public Ore()
        {
        }

        public Ore(string name, double hp, double defense, double value, double minToConsider, double maxToConsider, double maxOfAllAbove, int maxHits=0)
        {
            Name = name;
            Value = value;
            Hp = hp;
            Defense = defense;
            Clicks = 0;
            ValuePerClick = 0;
            MinHpcToConsider = minToConsider;
            MaxHpcToConsider = maxToConsider;
            MaxHpcOfAllAbove = maxOfAllAbove;
            MaxHitsToWorryAbout = maxHits;
        }

        public void UpdateClicks(double hpc)
        {
            if (hpc < Defense)
            {
                this.Clicks = uint.MaxValue;
                this.ValuePerClick = 0;
                return;
            }
            double clicks = Math.Ceiling(Hp / (hpc - Defense));
            this.Clicks = (uint)clicks;
            this.ValuePerClick = Value / clicks;
        }

        public static readonly List<Ore> oreList = new List<Ore>() {
#region ore definitions
            // this will be expanded as needed
            
            
new Ore("Poo",100,0,2,1,15,0),
new Ore("Paper",400,3,10,14,103,15),
new Ore("Salt",700,15,22,52,249,103),
new Ore("Clay",1400,35,50,113,210,249),
new Ore("Rock",2200,90,120,186,1190,249),
new Ore("Coal",4000,200,275,450,2200,1190),
new Ore("Bone",7000,380,580,964,3880,2200),
new Ore("Lead",12400,700,1100,2078,4834,3880),
new Ore("Iron",16000,1140,1850,2595,17140,4834),
new Ore("Copper",25000,1600,3200,3684,26600,17140),
new Ore("Carbonite",40000,2500,5200,7500,22500,26600),
new Ore("Quartz",64000,3800,8600,11800,25134,26600),
new Ore("Spooky Bone",92000,5400,14000,12477,23800,26600),
new Ore("Silver",128000,7200,20000,12478,12478,26600),
new Ore("Crystal",200000,9999,42000,20526,25384,135200),
new Ore("Topaz",500000,13500,140000,22429,39816,135200),
new Ore("Amethyst",1400000,18000,480000,35074,44416,135200),
new Ore("Aquamarine",4800000,24500,2200000,42891,80314,135200),
new Ore("Emerald",13000000,34000,7200000,77190,222406,135200),
new Ore("Ruby",42000000,50000,25500000,198410,261056,222406),
new Ore("Sapphire",120000000,80000,85000000,255696,277045,261056),
new Ore("Haunted Bone",200000000,130000,190000000,275986,570529,277045),
new Ore("Gold",360000000,200000,400000000,275987,275987,570529),
new Ore("Platinum",500000000,295000,760000000,569274,728276,360200000),
new Ore("Diamond",700000000,440000,1600000000,727475,1748412,360200000),
new Ore("Mithril",1000000000,680000,2800000000,1740446,2708398,360200000),
new Ore("Obsidian",1400000000,1050000,4800000000,2683606,3737141,360200000),
new Ore("Earth Essence",2000000000,1500000,8200000000,2683607,2683607,360200000),
new Ore("Orbium",2600000000,2000000,13800000000,3726428,5551913,2001500000),
new Ore("Novalite",3500000000,2800000,24000000000,5532241,8813746,2001500000),
new Ore("Magic Crystal",4900000000,4000000,42000000000,8771179,18369502,2001500000),
new Ore("Darkstone",7200000000,5800000,70000000000,8771180,8771180,2001500000),
new Ore("Adamantium",10000000000,8500000,125000000000,18275172,36748588,7205800000),
new Ore("Fire Essence",14000000000,12000000,200000000000,36179621,43390135,7205800000),
new Ore("Lunalite",20000000000,17000000,340000000000,43041667,75309038,7205800000),
new Ore("Mysterium",30000000000,24000000,580000000000,74251257,79658628,7205800000),
new Ore("Cursed Bone",45000000000,33500000,1050000000000,79138946,194214286,7205800000),
new Ore("Wind Essence",68000000000,48000000,1700000000000,79138947,79138947,7205800000),
new Ore("Unobtanium",100000000000,69000000,3000000000000,191850123,208082059,68048000000),
new Ore("Sollite",130000000000,95000000,4800000000000,207262522,518452769,68048000000),
new Ore("Water Essence",175000000000,138000000,7200000000000,508762712,517609545,68048000000),
new Ore("Absurdium",240000000000,205000000,12000000000000,514278351,682137177,68048000000),
new Ore("Cosmolite",320000000000,300000000,20000000000000,677358491,1774654378,68048000000),
new Ore("Shadow Essence",435000000000,440000000,30000000000000,1754199396,1758181819,68048000000),
new Ore("Demonite",590000000000,640000000,48000000000000,1753207548,3309683258,68048000000),
new Ore("Eternium",780000000000,950000000,72000000000000,3204335261,4792364533,68048000000),
new Ore("Mysticite",1050000000000,1400000000,110000000000000,4660869566,4995890411,68048000000),
new Ore("Light Essence",1350000000000,2000000000,170000000000000,4922077923,8958762887,68048000000),
new Ore("Soulstone",1800000000000,3100000000,270000000000000,8742633229,15262162163,68048000000),
new Ore("Arcanium",2400000000000,4800000000,420000000000000,14636065574,17365445027,68048000000),
new Ore("Hellstone Lv. 1",3200000000000,7000000000,680000000000000,17094637224,24679558012,68048000000),
new Ore("Hellstone Lv. 2",3840000000000,10500000000,1020000000000000,24362815885,37353146854,68048000000),
new Ore("Hellstone Lv. 3",4608000000000,15750000000,1530000000000000,36413677131,55819565218,68048000000),
new Ore("Hellstone Lv. 4",5529600000000,23625000000,2295000000000000,54690168540,84389835165,68048000000),
new Ore("Hellstone Lv. 5",6635520000000,35437500000,3442500000000000,81199706897,126335034247,84389835165),
new Ore("Hellstone Lv. 6",7962624000000,53156250000,5163750000000000,122396458696,192851407895,126335034247),
new Ore("Hellstone Lv. 7",9555148800000,79734375000,7745625000000000,181384894149,292071015000,192851407895),
new Ore("Hellstone Lv. 8",11466178560000,119601562500,11618437500000000,270472333027,447206664215,292071015000),
new Ore("Hellstone Lv. 9",13759414272000,179402343750,17427656250000000,404966512144,689010279750,447206664215),
new Ore("Hellstone Lv. 10",16511297126400,269103515625,26141484375000000,606068763103,1055355759740,689010279750),
new Ore("Hellstone Lv. 11",19813556551680,403655273437,39212226562500000,898994187229,1569158600007,1055355759740),
new Ore("Hellstone Lv. 12",23776267862016,605482910155,58818339843750000,1372459292801,2434426591849,1569158600007),
new Ore("Hellstone Lv. 13",28531521434419.2,908224365232,88227509765625000,2049485222609,4078393413501,2434426591849),
new Ore("Hellstone Lv. 14",34237825721303,1362336547848,132341264648437000,2918601353362,5642064763011,4078393413501),
new Ore("Hellstone Lv. 15",41085390865563.6,2043504821772,198511896972656000,4205893814697,6608548251280,5642064763011),


#endregion
        };

        public static int ComputeMostLucrativeOre(int[] upgradeState, WealthStatistics currentStatistics, out uint clicksForEachVictory, out double valueOfEachVictory)
        {
            double rawPickDamage = currentStatistics.CurrentP * currentStatistics.IdleActiveMultipliers() * upgradeState[(int)UpgradeID.ICAUTOPOWER & 127];
            Ore fakeOreForComparingRpd = new Ore() { Defense = 0, Hp = rawPickDamage, MinHpcToConsider = rawPickDamage, MaxHpcToConsider = rawPickDamage };
            int lowestFeasibleOre = Ore.oreList.BinarySearch(0, currentStatistics.HighestOre + 7,
                fakeOreForComparingRpd, new MinDamageComparer());
            int highestFeasibleOre = Ore.oreList.BinarySearch(lowestFeasibleOre, currentStatistics.HighestOre + 7 - lowestFeasibleOre,
                fakeOreForComparingRpd, new MaxDamageComparer());
            if (lowestFeasibleOre < 0)
            {
                lowestFeasibleOre = ~lowestFeasibleOre - 1;
            }
            if (highestFeasibleOre < 0)
            {
                highestFeasibleOre = ~highestFeasibleOre;
            }
            int oreIndexMaxRevenue = highestFeasibleOre;
            Ore oreAtMaxRevenue = Ore.oreList[oreIndexMaxRevenue];
            oreAtMaxRevenue.UpdateClicks(rawPickDamage);
            //double maxRevenuePerClick = oreAtMaxRevenue.ValuePerClick;
            for (int i = lowestFeasibleOre; i < highestFeasibleOre; i++)
            {
                Ore temp = Ore.oreList[i];
                temp.UpdateClicks(rawPickDamage);
                if (temp.ValuePerClick > oreAtMaxRevenue.ValuePerClick)
                {
                    oreAtMaxRevenue = temp;
                    oreIndexMaxRevenue = i;
                }
            }
            if (currentStatistics.HighestOre < oreIndexMaxRevenue)
            {
                oreIndexMaxRevenue = currentStatistics.HighestOre;
            }
            clicksForEachVictory = Ore.oreList[oreIndexMaxRevenue].Clicks;
            valueOfEachVictory = Ore.oreList[oreIndexMaxRevenue].Value;
            return oreIndexMaxRevenue;
        }


        public static int OptimalOreForCash(double hpc, int searchStartpoint)
        {
            return OptimalOreForCash(hpc, ref searchStartpoint);
        }

        public static int OptimalOreForCash(double hpc, ref int searchStartpoint)
        {
            Ore fakeOreForBinarySearch = new Ore("", hpc, 0, 0, hpc, hpc, 0);
            int maxOreIndex = oreList.BinarySearch(searchStartpoint, oreList.Count - searchStartpoint, fakeOreForBinarySearch, new MinDamageComparer());
            if (maxOreIndex > 0)
            {
                Ore o = oreList[maxOreIndex];
                if (o.MinHpcToConsider != o.MaxHpcToConsider)
                {
                    // exactly hit a good ore at its minimum value
                    oreList[maxOreIndex].UpdateClicks(hpc);
                    return maxOreIndex;
                }
                maxOreIndex--; // exactly hit a dud ore

            }
            else
            {
                // a negative return value means the ~ of the place where this value would be inserted
                maxOreIndex = ~maxOreIndex - 1;
            }

            Ore optimalOreSoFar = fakeOreForBinarySearch;
            int optimalIndex = -1;
            for (int candidateIndex = maxOreIndex; candidateIndex >= 0; candidateIndex--)
            {
                Ore candidateOre = oreList[candidateIndex];
                if (hpc < candidateOre.MaxHpcToConsider)
                {
                    candidateOre.UpdateClicks(hpc);
                    if (candidateOre.ValuePerClick > optimalOreSoFar.ValuePerClick)
                    {
                        optimalOreSoFar = candidateOre;
                        optimalIndex = candidateIndex;
                    }
                }

                if (hpc >= candidateOre.MaxHpcOfAllAbove) // past last possible point
                {
                    searchStartpoint = candidateIndex;
                    break;
                }
            }
            return optimalIndex;
        }
#if false
        public static List<KeyValuePair<double, int>> possibleRanges;
        struct OreBreakpoint
        {
            public Ore ore;
            public double rawDamage;
            public double revenuePerHit;
        }

        public static bool BeatsMax(OreBreakpoint a, ref double b)
        {
            if (a.revenuePerHit > b)
            {
                b = a.revenuePerHit;
                return true;
            }
            return false;
        }

        public static void ComputeOptimalOres()
        {
            possibleRanges = (
                from ore in oreList
                from hitsToMine in Enumerable.Range(1,ore.MaxHitsToWorryAbout)
                select new OreBreakpoint()
                {
                    ore = ore,
                    rawDamage = Math.Ceiling(ore.Hp/hitsToMine) + ore.Defense,
                    revenuePerHit = ore.Value / hitsToMine 
                } into oreBreakpoints
                group oreBreakpoints by oreBreakpoints.rawDamage into bkptsGroupedByDamage
                select new KeyValuePair<double,int>
                (
                    bkptsGroupedByDamage.Key,
                    bkptsGroupedByDamage.Aggregate((x, y) => x.revenuePerHit > y.revenuePerHit ? x : y).oreNumber
                )).ToList();
#if false
                        double bestSoFar = 0;
            possibleRanges = (
                from o in Enumerable.Range(0,oreList.Length)
                from hits in Enumerable.Range(1,500)
                select new thingInfo()
                {
                    oreNumber=o,
                    hitNumber = hits,
                    rawDamage = Math.Ceiling(oreList[o].Hp/hits) + oreList[o].Defense,
                    revenuePerHit = oreList[o].Value / hits 
                } into things
                ).Where(v => BeatsMax(v, ref bestSoFar)).Select(v=>new KeyValuePair<double,int>(v.rawDamage,v.oreNumber)).ToList();

#elif false
            foreach (var v in crossProduct)
            {
                if (v.revenuePerHit > bestSoFar)
                {
                    bestSoFar = v.revenuePerHit;
                    possibleRanges.Add(v.rawDamage, v.oreNumber);
                }
            }
#endif
        }
#endif
    }
}
