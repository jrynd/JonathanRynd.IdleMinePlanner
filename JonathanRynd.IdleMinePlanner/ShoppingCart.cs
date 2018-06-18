using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SolverFoundation.Services;

namespace JonathanRynd.IdleMinePlanner
{
    public struct Upgrade
    {
        public string name; // whatever is shown for it
        public int level;
        public double value;
        public double cumulativeCost; // cost to upgrade from nothing to here
        public double nextLevelCost;
        private static IEnumerable<Upgrade> GenerateBasePower()
        {
            Upgrade a = new Upgrade() { name = "20", cumulativeCost = 0, level = 0, nextLevelCost = 30, value = 20 };
            while (true)
            {
                yield return a;
                a.cumulativeCost += a.nextLevelCost;
                a.level++;
                a.nextLevelCost = Math.Floor(a.nextLevelCost * 1.2) + 100 * a.level;
                a.value = 0.1 * Math.Floor(a.value * 10.6 + 120);
                a.name = Math.Floor(a.value).ToString();
            }
        }

        static IList<Upgrade> ic(UpgradeID i)
        {
            if (i.HasFlag(UpgradeID.ICFLAG))
                return icUpgrades[(int)i & 127];
            else if (i.HasFlag(UpgradeID.BSFLAG))
                return bsUpgrades[(int)i & 127];
            return null;
        }

        public static readonly IList<Upgrade>[] bsUpgrades = new IList<Upgrade>[]
        {
            // base power
            new MemoizeEnumerable<Upgrade>(GenerateBasePower()),
            // skill
            new Upgrade[]
            {
                new Upgrade(){level=0,value=0.8,name="100%",nextLevelCost=8.000000000000E+04,cumulativeCost=0.000000000000E+00},
                new Upgrade(){level=1,value=0.805,name="101%",nextLevelCost=3.360000000000E+05,cumulativeCost=8.000000000000E+04},
                new Upgrade(){level=2,value=0.81,name="102%",nextLevelCost=1.411200000000E+06,cumulativeCost=4.160000000000E+05},
                new Upgrade(){level=3,value=0.815,name="103%",nextLevelCost=5.927040000000E+06,cumulativeCost=1.827200000000E+06},
                new Upgrade(){level=4,value=0.82,name="104%",nextLevelCost=2.489356800000E+07,cumulativeCost=7.754240000000E+06},
                new Upgrade(){level=5,value=0.825,name="105%",nextLevelCost=1.045529850000E+08,cumulativeCost=3.264780800000E+07},
                new Upgrade(){level=6,value=0.83,name="106%",nextLevelCost=4.391225370000E+08,cumulativeCost=1.372007930000E+08},
                new Upgrade(){level=7,value=0.835,name="107%",nextLevelCost=1.844314655000E+09,cumulativeCost=5.763233300000E+08},
                new Upgrade(){level=8,value=0.84,name="108%",nextLevelCost=7.746121551000E+09,cumulativeCost=2.420637985000E+09},
                new Upgrade(){level=9,value=0.845,name="109%",nextLevelCost=3.253371051400E+10,cumulativeCost=1.016675953600E+10},
                new Upgrade(){level=10,value=0.85,name="110%",nextLevelCost=1.366415841580E+11,cumulativeCost=4.270047005000E+10},
                new Upgrade(){level=11,value=0.855,name="111%",nextLevelCost=5.738946534630E+11,cumulativeCost=1.793420542080E+11},
                new Upgrade(){level=12,value=0.86,name="112%",nextLevelCost=2.410357544544E+12,cumulativeCost=7.532367076710E+11},
                new Upgrade(){level=13,value=0.865,name="113%",nextLevelCost=1.012350168708E+13,cumulativeCost=3.163594252215E+12},
                new Upgrade(){level=14,value=0.87,name="114%",nextLevelCost=4.251870708575E+13,cumulativeCost=1.328709593930E+13},
                new Upgrade(){level=15,value=0.875,name="115%",nextLevelCost=1.785785697602E+14,cumulativeCost=5.580580302505E+13},
                new Upgrade(){level=16,value=0.88,name="116%",nextLevelCost=7.500299929927E+14,cumulativeCost=2.343843727852E+14},
                new Upgrade(){level=17,value=0.885,name="117%",nextLevelCost=3.150125970569E+15,cumulativeCost=9.844143657779E+14},
                new Upgrade(){level=18,value=0.89,name="118%",nextLevelCost=1.323052907639E+16,cumulativeCost=4.134540336347E+15},
                new Upgrade(){level=19,value=0.895,name="119%",nextLevelCost=5.556822212084E+16,cumulativeCost=1.736506941274E+16},
                new Upgrade(){level=20,value=0.9,name="120%",nextLevelCost=2.333865329075E+17,cumulativeCost=7.293329153358E+16},
                new Upgrade(){level=21,value=0.905,name="121%",nextLevelCost=9.802234382116E+17,cumulativeCost=3.063198244411E+17},
                new Upgrade(){level=22,value=0.91,name="122%",nextLevelCost=4.116938440489E+18,cumulativeCost=1.286543262653E+18},
                new Upgrade(){level=23,value=0.915,name="123%",nextLevelCost=1.729114145005E+19,cumulativeCost=5.403481703142E+18},
                new Upgrade(){level=24,value=0.92,name="124%",nextLevelCost=7.262279409022E+19,cumulativeCost=2.269462315319E+19},
                new Upgrade(){level=25,value=0.925,name="125%",nextLevelCost=3.050157351789E+20,cumulativeCost=9.531741724342E+19},
                new Upgrade(){level=26,value=0.93,name="126%",nextLevelCost=1.281066087752E+21,cumulativeCost=4.003331524224E+20},
                new Upgrade(){level=27,value=0.935,name="127%",nextLevelCost=5.380477568556E+21,cumulativeCost=1.681399240174E+21},
                new Upgrade(){level=28,value=0.94,name="128%",nextLevelCost=2.259800578794E+22,cumulativeCost=7.061876808730E+21},
                new Upgrade(){level=29,value=0.945,name="129%",nextLevelCost=9.491162430934E+22,cumulativeCost=2.965988259667E+22},
                new Upgrade(){level=30,value=0.95,name="130%",nextLevelCost=Double.PositiveInfinity,cumulativeCost=1.245715069060E+23},
            }, 
            // expertise
            new Upgrade[]
            {
                new Upgrade(){level=0,value=0,name="0",nextLevelCost=4.000000000000E+04,cumulativeCost=0.000000000000E+00},
                new Upgrade(){level=1,value=0.01,name="1",nextLevelCost=6.400000000000E+05,cumulativeCost=4.000000000000E+04},
                new Upgrade(){level=2,value=0.02,name="2",nextLevelCost=1.024000000000E+07,cumulativeCost=6.800000000000E+05},
                new Upgrade(){level=3,value=0.03,name="3",nextLevelCost=1.638400000000E+08,cumulativeCost=1.092000000000E+07},
                new Upgrade(){level=4,value=0.04,name="4",nextLevelCost=2.621440000000E+09,cumulativeCost=1.747600000000E+08},
                new Upgrade(){level=5,value=0.05,name="5",nextLevelCost=4.194304000000E+10,cumulativeCost=2.796200000000E+09},
                new Upgrade(){level=6,value=0.06,name="6",nextLevelCost=6.710886400000E+11,cumulativeCost=4.473924000000E+10},
                new Upgrade(){level=7,value=0.07,name="7",nextLevelCost=1.073741824000E+13,cumulativeCost=7.158278800000E+11},
                new Upgrade(){level=8,value=0.08,name="8",nextLevelCost=1.717986918400E+14,cumulativeCost=1.145324612000E+13},
                new Upgrade(){level=9,value=0.09,name="9",nextLevelCost=2.748779069440E+15,cumulativeCost=1.832519379600E+14},
                new Upgrade(){level=10,value=0.1,name="10",nextLevelCost=Double.PositiveInfinity,cumulativeCost=2.932031007400E+15},
            },
            // efficency
            new Upgrade[]{new Upgrade()},
            // gemwaster
            new Upgrade[]{new Upgrade()}
        };
        public static readonly Upgrade[][] icUpgrades = new Upgrade[][]
        {
            // idle power
            new Upgrade[]{new Upgrade()},
            // idle speeed
            new Upgrade[]{new Upgrade()},
            // gemfinder
            new Upgrade[]{new Upgrade()},
        };
    }
    class ShoppingCart
    {
 
        public UpgradeState a;
        //public bool gemWaster;
        public int[] cartList;
 
        public int NumBBP
        {
            get { return cartList[0]; }
            set { cartList[0] = value; }
        }
        public int NumBS
        {
            get { return cartList[1]; }
            set { cartList[1] = value; }
        }
        public int NumBX
        {
            get { return cartList[2]; }
            set { cartList[2] = value; }
        }
        public int NumBF
        {
            get { return cartList[3]; }
            set { cartList[3] = value; }
        }

        // would like this to be generic...
        public static double StableSum(IEnumerable<double> iterable)
        {
            // translation of http://code.activestate.com/recipes/393090/
            // In the event Licensee prepares a derivative work that is based on or incorporates Python 2.7.7 or any part thereof, and wants to make the derivative work available to others as provided herein, then Licensee hereby agrees to include in any such work a brief summary of the changes made to Python 2.7.7.
            // brief summary: translated from python to C#
            // Copyright © 2001-2014 Python Software Foundation; All Rights Reserved
    // "Full precision summation using multiple floats for intermediate values"
    // Rounded x+y stored in hi with the round-off stored in lo.  Together
    // hi+lo are exactly equal to x+y.  The inner loop applies hi/lo summation
    // to each partial so that the list of partial sums remains exact.
    // Depends on IEEE-754 arithmetic guarantees.  See proof of correctness at:
    // www-2.cs.cmu.edu/afs/cs/project/quake/public/papers/robust-arithmetic.ps

            var partials = new List<double>(); // sorted, non-overlapping partial sums
            foreach (double xp in iterable)
            {
                int i=0;
                double x = xp;
                foreach (var yp in partials)
                {
                    double y;
                    if (Math.Abs(xp) < Math.Abs(yp))
                    {
                        x=yp;
                        y=xp;
                    }
                    else
                    {
                        y=yp;
                    }
                    double hi=x+y;
                    double lo= y-(hi-x);
                    if (lo != 0.0)
                    {
                        if (i == partials.Count)
                        {
                            partials.Add(lo);
                        }
                        else
                        {
                            partials[i] = lo;
                        }
                        i++;
                    }
                    x = hi;
                }
                partials.RemoveRange(i,partials.Count-i);
                partials.Add(x);
            }
            return partials.Sum();
        }
 
#if false
        double StableSum(MinHeap<double> s)
        {
            while (s.Count > 1)
            {
                double first = s.ExtractDominating();
                double second = s.ExtractDominating();
                s.Add(first + second);
            }
            return s.FirstOrDefault();
        }
#endif
        
        #region precalculate bonuses and probabilities

        private static double[] rankBonuses = Enumerable.Range(0,17).Select(i => Math.Pow(1.3,i)).ToArray();

        static double GetPlusBonus(int numPluses)
        {
            return 1.0 + numPluses * 0.075; // the array lookup has to do a multadd too
#if false
            while (i >= plusBonuses.Count)
            {
                plusBonuses.Add(1.0 + i * 0.075);
            }
            return plusBonuses[i];
#endif
        }

        private static readonly double[] plusprob = new double[] {0.4, 0.48, 0.55, 0.6, 0.63, 0.66, 0.69, 0.734, 0.75};

        private static readonly double[][] rankprobsByGwlevel = // outer index is gemwaster; inner index is prob of getting that rank.
        {
            // TODO: Get these at higher precision
            new double[] {0.64943, 0.2451756, 0.069483806, 0.022953481, 0.0084103, 0.003041706, 0.001021728, 0.000332895, 0.000103608, 3.27332E-05, 1.00142E-05, 2.96398E-06, 8.47936E-07, 2.3424E-07, 6.24239E-08, 1.60333E-08, 5.18482E-09},
            new double[] {0, 0.69943, 0.1981813, 0.065454219, 0.023977368, 0.008669442, 0.002911193, 0.000948157, 0.000294968, 9.31427E-05, 2.84781E-05, 8.42279E-06, 2.40747E-06, 6.64349E-07, 1.76816E-07, 4.53419E-08, 1.46221E-08},
            new double[] {0, 0, 0.65943, 0.2177471, 0.079747274, 0.028826244, 0.009676723, 0.003150455, 0.000979661, 0.00030919, 9.44763E-05, 2.79221E-05, 7.97389E-06, 2.19806E-06, 5.84246E-07, 1.49581E-07, 4.8103E-08},
            new double[] {0, 0, 0, 0.63943, 0.2341414, 0.08461726, 0.028398185, 0.009242891, 0.002873168, 0.000906437, 0.00027684, 8.17725E-05, 2.33363E-05, 6.42746E-06, 1.70669E-06, 4.36408E-07, 1.40039E-07},
            new double[] {0, 0, 0, 0, 0.64943, 0.2346585, 0.07873662, 0.025620469, 0.007961868, 0.002511, 0.000766593, 0.000226328, 6.45525E-05, 1.77672E-05, 4.71377E-06, 1.20408E-06, 3.85677E-07},
            new double[] {0, 0, 0, 0, 0, 0.66943, 0.2245756, 0.073059296, 0.022698097, 0.007156314, 0.002183991, 0.00064452, 0.000183733, 5.05384E-05, 1.33979E-05, 3.4191E-06, 1.09338E-06}
        };
        #endregion
        internal static Dictionary<Tuple<byte,byte>,List<Tuple<double,double>>> chanceOfGettingEachPlusAtSpecificExpertiseAndRank =
            new Dictionary<Tuple<byte,byte>,List<Tuple<double,double>>>();

        double GetChanceOfGettingSpecificPlus(byte expertise, byte rank, int whichplus)
        {
            List<Tuple<double,double>> lv;
            if (rank > 8) return (whichplus==0 ? 1.0 : 0.0);
            double adjustedPlusprob = plusprob[rank] + expertise*.01;
            Tuple<byte,byte> indexer = Tuple.Create(expertise,rank);
            if (!chanceOfGettingEachPlusAtSpecificExpertiseAndRank.TryGetValue(indexer,out lv))
            {
                lv = new List<Tuple<double,double>>();
                chanceOfGettingEachPlusAtSpecificExpertiseAndRank[indexer] = lv;
                lv.Add(Tuple.Create(1-plusprob[rank],plusprob[rank]));
            }
            while (lv.Count <= whichplus)
            {
                double lastLeft = lv.Last().Item2;
                // double continueleft = lastLeft*plusprob[rank];
                lv.Add(Tuple.Create(lastLeft*(1-plusprob[rank]),lastLeft*plusprob[rank]));
            }
            return lv[whichplus].Item1;
        }

        public IEnumerable<Tuple<double,double>> ChancesOfBeatingTargetAndExpectedPsIfYouDo(double targetPValue)
        {
            return TryForEachGemwaster(targetPValue,ChanceOfBeatingTargetAndExpectedPIfYouDo);
        }

        IEnumerable<Tuple<double, double>> TryForEachGemwaster(double targetPValue, Func<int, double, Tuple<double, double>> func)
        {
            int maxGw = Math.Min(5, (int)a.gwLevel + 1);
            return Enumerable.Range(0, maxGw + 1).Select(
                gwLevel => func(gwLevel,targetPValue));
        }

        double SimpleExpectation(double x0, double x1)
        {
            // this is trying to deal with overflow
            if (Math.Sign(x0) == Math.Sign(x1))
            {
                if (Math.Abs(x0) < Math.Abs(x1))
                    return x0 + (x1 - x0) / 2;
                else
                    return x1 + (x0 - x1) / 2;
            }
            return (x0 + x1) / 2;
        }

        double ExpectationOfRange(double x0, double x1)
        {
            double i0=Math.Floor(x0), i1=Math.Floor(x1), y0=x0-i0, y1=x1-i1;
            if (i0==0 && i1==0) { return SimpleExpectation(x0,x1);} // handles actual integers, and also rounded-to-int due to running out of precision
            double firstIntervalSize, firstIntervalValue, lastIntervalSize, lastIntervalValue;
            if (y0 < 0.5)
            {
                firstIntervalValue = i0;
                firstIntervalSize = 0.5 - y0;
            }
            else
            {
                firstIntervalValue = i0 + 1;
                // check for loss of precision
                if (firstIntervalValue == i0 || firstIntervalValue == i0+2) { return SimpleExpectation(x0,x1); }
                firstIntervalSize = 1.5 - y0;
            }
            if (y1 < 0.5)
            {
                lastIntervalValue = i1;
                lastIntervalSize = y1 + 0.5;
            }
            else
            {
                lastIntervalValue = i1 + 1;
                // check for loss of precision
                if (lastIntervalValue == i1 || lastIntervalValue == i1 + 2) { return SimpleExpectation(x0, x1); }
                lastIntervalSize = y1 - 0.5;
            }
            return (
                (lastIntervalValue-firstIntervalValue-1)*(firstIntervalValue+lastIntervalValue)/2
                + firstIntervalSize * firstIntervalValue
                + lastIntervalSize * lastIntervalValue)/(x1-x0);
        }

        private static readonly int firstGodlyRank = 9;
        private static readonly int overallNumberRanks = 17;

        private void ChanceInternal(double rankprob,
            double probStoppingHere,
            double rankBonus,
            double leSkillEffect,
            double plusBonus,
            double targetPValue,
            ref double maxContributionThisRound,
            List<double> expectations,
            StableSummer probabilities)
        {
            double prob = rankprob * probStoppingHere;
            // TODO: Properly handle the 2x rounding that happens
            // first, calculates basepower * rankbonus * quality
            double expectedPAt100NoPlusbonus = BasePower(a.bbLevel + this.NumBBP) * rankBonus;
            double expectedPAtLowEndNoPlusbonus = Math.Round(expectedPAt100NoPlusbonus * leSkillEffect);
            double expectedPAtHighEndNoPlusbonus = Math.Round(expectedPAt100NoPlusbonus * (0.5 + leSkillEffect));
            double expectedPAtHighEnd = Math.Round(expectedPAtHighEndNoPlusbonus * plusBonus);
            if (expectedPAtHighEnd > targetPValue)
            {
                double expectedPAtLowEnd = Math.Round(expectedPAtLowEndNoPlusbonus * plusBonus);
                double value;
                if (expectedPAtLowEnd > targetPValue)
                {
                    // APPROXIMATION
                    value = SimpleExpectation(expectedPAtLowEnd, expectedPAtHighEnd);
                }
                else
                {
                    // APPROXIMATION
                    // only portion between targetPValue and expectedPAtHighEnd
                    value = SimpleExpectation(targetPValue, expectedPAtHighEnd);
                    prob = prob * (expectedPAtHighEnd - targetPValue) / (expectedPAtHighEnd - expectedPAtLowEnd);
                }
                double contribution = prob * value;
                if (contribution > maxContributionThisRound) maxContributionThisRound = contribution;
                expectations.Add(contribution);
                probabilities.Add(prob);
            }
        }

        private Tuple<double, double> ChanceOfBeatingTargetAndExpectedPIfYouDo(int gwLevel, double targetPValue)
        {
            List<double> expectations = new List<double>();
            StableSummer probabilities = new StableSummer();
            // start with godly ranks with 0 pluses
            // (no reliable reports of godly ranks ever getting pluses)
            double prob;
            double leSkillEffect = Upgrade.bsUpgrades[(int)UpgradeID.BSSKILL & 127][a.bsLevel + NumBS].value;
            double heSkillEffect = 0.5 + leSkillEffect;
            double basePower = Upgrade.bsUpgrades[(int)UpgradeID.BSBASEPOWER & 127][a.bbLevel + NumBBP].value;
            int rank;
            for (rank = firstGodlyRank; rank<overallNumberRanks; rank++)
            {
                prob = rankprobsByGwlevel[gwLevel][rank];
                double expectedPAt100 = basePower*rankBonuses[rank];
                double expectedPAtLowEnd = expectedPAt100 * leSkillEffect;
                double expectedPAtHighEnd = expectedPAt100 * heSkillEffect;
                // no plus bonus multiplier, makes things easy
                if (targetPValue < expectedPAtLowEnd)
                {
                    // whole thing contributes
                    // don't have to adjust prob
                    // CODE IS CURRENTLY EXACT
                    expectations.Add(prob*ExpectationOfRange(expectedPAtLowEnd,expectedPAtHighEnd));
                    probabilities.Add(prob);
                }
                else if (targetPValue < expectedPAtHighEnd)
                {
                    // not all of the range contributes, so we have to adjust prob
                    prob *= (expectedPAtHighEnd-targetPValue)/(expectedPAtHighEnd-expectedPAtLowEnd);
                    // SLIGHT APPROXIMATION:
                    // we may lose a small bit of accuracy here since something smaller than targetValue might be rounded to it
                    expectations.Add(prob*ExpectationOfRange(targetPValue,expectedPAtHighEnd));
                    probabilities.Add(prob);
                }
            }
            // now we can do it for the mortal ranks (which can get plusbonuses after rounding)
            double maxContributionThisRound;
            // outer loop over possible plusbonuses
            int numPluses = 0;

            var modifiedPlusProbs = plusprob.Select(x=>x+(a.bsLevel+this.NumBX)*.01).ToArray();
            var probReachingHere = Enumerable.Repeat(1.0, plusprob.Count());
            do {
                var probStoppingHere = probReachingHere.Zip(modifiedPlusProbs, (prh, mpp) => prh * (1 - mpp));
                double plusBonus = GetPlusBonus(numPluses);
                maxContributionThisRound = 0;
                // this could be LINQed with a subfunction, zipping rankprob[gwlevel] and probStoppingHere
                CustomSequenceOperators.TripleForEach(rankprobsByGwlevel[gwLevel], probStoppingHere, rankBonuses,
                    (rp, psh, rb) => ChanceInternal(rp, psh, rb, leSkillEffect, plusBonus, targetPValue, ref maxContributionThisRound, expectations, probabilities));
                numPluses++;
                // might be faster without LINQ?
                probReachingHere = probReachingHere.Zip(probStoppingHere, (prh, psh) => prh - psh);
            } while (maxContributionThisRound > 1e-10);
            // all possible ranks, all possible enchantments
            double chance = probabilities.Sum();
            return Tuple.Create(chance,StableSum(expectations) / chance);
        }

#if false
        // not very useful as it ignores other multipliers, as well as nonlinear effects for high P
        IEnumerable<double> ExpectedPValuesAfterOneCraft(double currentPValue)
        {
            return TryForEachGemwaster(currentPValue, ExpectedPValueAfterOneCraft);
        }

        double ExpectedPValueAfterOneCraft(double currentPValue, int gwSetting)
        {
            // this is not very useful; most of the time we won't be crafting once.
            throw new NotImplementedException();
        }
#endif

        private static readonly Func<int,int,int> Plus = (x,y)=>x+y;
        //private static readonly Func<double,double,double> Minus = (x,y)=>x-y;
        private static readonly Func<double, double> Negate = x => -x;
        private static readonly Func<IList<Upgrade>, int, double> GetCumulativeCost = (array, index) => array[index].cumulativeCost;
        public Tuple<double,double> TotalCostWithoutAndWithGemwaster()
        {
            var finalBSLevelsAfterUpgrade = a.blacksmith.Zip(cartList, Plus);
            IEnumerable<double> bookValueBeforePurchase = Upgrade.bsUpgrades.Zip(finalBSLevelsAfterUpgrade, GetCumulativeCost);
            IEnumerable<double> bookValueAfterPurchase = Upgrade.bsUpgrades.Zip(a.blacksmith, GetCumulativeCost);

            double costWithoutGemwaster = StableSum(bookValueBeforePurchase.Select(Negate).Union(bookValueAfterPurchase));
            double costWithGemwaster = costWithoutGemwaster + Upgrade.bsUpgrades[4][a.gwLevel].nextLevelCost;
            return Tuple.Create(costWithoutGemwaster,costWithGemwaster);
        }

        public static readonly int[] gwNumberOfGems = { 1, 3, 11, 39, 133, 454 };

#if false // expected power is not a useful measure
        private static readonly double[,] expectedGwPower = new double[6, 11]
        {
            {}, // gw level 0 (1 gem)
            {}, // gw level 1 (3 gems)
            {}, // gw level 2 (11 gems)
            {}, // gw level 3 (39 gems)
            {}, // gw level 4 (133 gems)
            {}
        };
#endif

        private static IList<double> memoizedBasePower = new List<double> { 20 };
        private static double BasePower(int level)
        {
            double last = memoizedBasePower.Last();
            while (memoizedBasePower.Count <= level)
            {
                memoizedBasePower.Add(last=Math.Floor(last * 1.06 + 12));
            } 
            return memoizedBasePower[level];
        }

#if false  // not used by anyone, and "expected" is a poor measure
        private static readonly double[] expectedQuality = new double[]
        {
            1.05, 1.055,
            1.06, 1.065,
            1.07, 1.075,
            1.08, 1.085,
            1.09, 1.095,
            1.10, 1.105,
            1.11, 1.115,
            1.12, 1.125,
            1.13, 1.135,
            1.14, 1.145,
            1.15, 1.155,
            1.16, 1.165,
            1.17, 1.175,
            1.18, 1.185,
            1.19, 1.195,
            1.20
        };
#endif

#if false
        internal double GetExpectedP(int gwLevel)
        {
            return BasePower(a.bbLevel + NumBBP)
                * expectedGwPower[gwLevel, a.bxLevel + NumBX] // includes rank and enchantment
                * expectedQuality[a.bsLevel + NumBS];
        }
#endif

        public override string ToString()
        {
            return string.Format("Base power: {0}\nSkill: {1}\nExpertise: {2}\nEfficiency: {3}\n}",
                this.NumBBP, NumBS, NumBX, NumBF);
        }
    }
}
