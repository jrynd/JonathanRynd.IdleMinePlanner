using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    class WealthStatistics
    {
        private double _gold = 0;

        /// <summary>
        /// Current liquid wealth.
        /// </summary>
        public double Gold
        {
            get { return _gold; }
            set { if (value >= 0) _gold = value; else throw new ArgumentOutOfRangeException("Gold"); }
        }
        private double _gems = 0;

        /// <summary>
        /// Current gem count.
        /// </summary>
        public double Gems
        {
            get { return _gems; }
            set { if (value >= 0) _gems = value; else throw new ArgumentOutOfRangeException("Gems"); }
        }

        private ushort _highestOre = 0;

        /// <summary>
        /// Keeps track of the maximum ore avilable to be mined.
        /// </summary>
        public ushort HighestOre
        {
            get { return _highestOre; }
            set { if (value > _highestOre) _highestOre = value; }
        }

        public StableSummer dataAge = new StableSummer(); // to get exact times
        public double dataAgeThreshold;
        public ulong dataXp = 0;
        public ulong dataXpThreshold;

        private double _currentP;

        /// <summary>
        /// Pick base power.
        /// </summary>
        public double CurrentP
        {
            get { return _currentP; }
            set { if (value > 0) _currentP = value; }
        }

        public void setThresholds()
        {
            double das = dataAge.Sum();
            dataAgeThreshold = NewtonMethod(IdleObjective, IdleDerivative, das, Math.Floor(IdleObjective(das) * 100) / 100, .0001);
            dataXpThreshold = (ulong)Math.Ceiling(NewtonMethod(ActiveObjective, ActiveDerivative, dataXp, Math.Floor(ActiveObjective(dataXp) * 100) / 100, .0001));
        }

        public WealthStatistics(WealthStatistics w)
        {
            dataAge= new StableSummer(w.dataAge);
            dataAgeThreshold = w.dataAgeThreshold;
            dataXp=w.dataXp;
            dataXpThreshold = w.dataXpThreshold;
            _currentP = w.CurrentP;
            _highestOre = w.HighestOre;
            _gems = w.Gems;
            _gold = w.Gold;
        }

        public WealthStatistics()
        {
            dataAge = new StableSummer();
            dataAgeThreshold = NewtonMethod(IdleObjective,IdleDerivative,0,0.01,0.0001)*25920000;
            dataXp = 0;
            dataXpThreshold = (uint)(Math.Ceiling(NewtonMethod(ActiveObjective,ActiveDerivative,0,0.01,0.0001)*10800));
            Gold = 0;
            Gems = 0;
            CurrentP = 20;
            HighestOre = 0;
        }

        /*
         * my_so.data.age = idiotDate.gettime() - my_so.data.birth;

         * my_so.data.xp starts at 1, add 1 for each autosmack, 5 for each manual


         *function managePower(n)
         *{
         *  _parent.life_txt.text = commalize(math.round(my_so.data.age / 6170));
         *  _parent.active_txt.text = commalize(math.round(my_so.data.xp / 20));
         *  lifeBonus = (math.floor((math.log(1 + (my_so.data.age / 25920000)) * math.sqrt(1 + (my_so.data.age / 25920000))) * 100) / 100) + 1;
         *  _parent.lifeBonus_txt.text = lifeBonus + "x";
         *  activeBonus = (math.floor((math.log(1 + (my_so.data.xp / 324000)) * math.sqrt(1 + (my_so.data.xp / 10800))) * 100) / 100) + 1;
         *  _parent.activeBonus_txt.text = activeBonus + "x"; return(((n * lifeBonus) * activeBonus) * my_so.data.weaponPower);
         *  } 

         */

        double IdleMultiplier(double x=-1)
        {
            //Math.Floor(Math.Log(1 + (dataAge / 25920000) * Math.Sqrt(1 + (dataAge / 25920000))) * 100) / 100 + 1;
            if (x == -1) x = dataAge.Sum();
            return Math.Floor(IdleObjective(x/25920000)*100)/100+1;
        }

        double ActiveMultiplier(double x=-1)
        {
            //Math.Floor(Math.Log(1 + (dataXp / 324000.0) * Math.Sqrt(1 + (dataXp / 10800))) * 100) / 100 + 1;
            if (x==-1) x = dataXp;
            return Math.Floor(ActiveObjective(x/10800.0)*100)/100+1;
        }

        internal double IdleActiveMultipliers()
        {
            return IdleMultiplier() * ActiveMultiplier();
        }

        public static double ProductLogForLargeX(double x)
        {
            double l1 = Math.Log(x);
            double l2= Math.Log(l1);

            return l1-l2*(1+1/l1+(-2+l2)/(2*l1*l1)+(6-9*l2+2*l2*l2)/(6*l1*l1*l1)+(-12+36*l2-22*l2*l2+3*l2*l2*l2)/(12*l1*l1*l1*l1));

            // hopefully the optimizer will be smart enough to evaluate this sensibly, e.g. use temps for power of l2 and 1/l1
        }

        public static double IdleObjective(double x)
        {
            return Math.Log(1 + x) * Math.Sqrt(1 + x);
        }

        public static double ActiveObjective(double x)
        {
            // note. this can be approximated by taking IdleObjective(x/factor) where factor varies depending on x
            // when x is small, 1+x/30 is so close to 1, that the objective is nearly 0
            // (a better approx is log(1+u) ~= u so a good approx is x/30 * sqrt(1+x)
            // when x is around 30, 1+x/30 is close to 2, can approximate by 2/e*sqrt(x)
            // when x is large, this is close to log(x/30)*sqrt(x) = sqrt(x)*(log x - log 30)
            // 
            return Math.Log(1 + x / 30) * Math.Sqrt(1 + x);
        }

        public static double IdleDerivative(double x)
        {
            // udv + vdu; u = log(1+x); v= sqrt(1+x); du = 1/(1+x); dv = 1/2 (1+x)^-0.5
            // log(1+x)*1/2 (1+x)^-0.5 + sqrt(1+x)/(1+x)
            // log(1+x)*1/2 / sqrt(1+x) + 1/sqrt(1+x)
            // (log(1+x)/2 + 1) / sqrt(1+x)
            return (Math.Log(x + 1)*0.5 + 1) / Math.Sqrt(x + 1); // or maybe * Math.Pow(x+1,-0.5) would be better?
            // or maybe calculate y=Log(x+1) and then calculate (y*0.5+1)*Exp(y/2)?
        }

        public static double ActiveDerivative(double x)
        {
            return (2*(x+1)+(x+30)*Math.Log(x/30+1)) / (2*Math.Sqrt(x+1)*(x+30));
        }

        public static double NewtonMethod(Func<double, double> objective, Func<double, double> derivative, double start, double target, double tolerance)
        {
            double candidate = start;
            double y = objective(candidate);
            while (Math.Abs(target - y) < tolerance)
            {
                candidate -= (y-target)/(derivative(candidate)); 
                y = objective(candidate);
            }
            return candidate;
        }

        // we set my_so.data.age = retval * 25920000
        public static double InverseOfLogSqrt(double x)
        {
            return x * Math.Exp(2*ProductLogForLargeX(x/2))-1;
        }
        
        public void SetIMDataFromMultiplier(double iMultiplier, double aMultiplier)
        {
            dataAgeThreshold = NewtonMethod(IdleObjective,IdleDerivative,Math.Exp(iMultiplier-1),iMultiplier-1,0.0001)*25920000;
            dataXpThreshold = (uint)(Math.Ceiling(NewtonMethod(ActiveObjective,ActiveDerivative,Math.Exp(aMultiplier-1),aMultiplier-1,0.0001)*10800));
        }
        
    }
}
