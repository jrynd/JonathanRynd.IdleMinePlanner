using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    enum Upgrade : int
    {
	BSFLAG = 256,
        BSBASEPOWER=0|256,
        BSSKILL=1|256,
        BSEXPERTISE=2|256,
        BSEFFICIENCY=3|256,
        BSGEMWASTER=4|256,
        BSMAX=4|256,
        BSCOUNT=5|256
	ICFLAG=256,
        ICAUTOPOWER = 0|512,
        ICAUTOSPEED = 1|512,
        ICGEMFINDER = 2|512,
        ICMAX=2|512,
        ICCOUNT=3|512
    }

    class UpgradeState
    {
        public static readonly int[] bsMaxes = {0,30,10,50,6};
        public int[] blacksmith = new int[(int)Upgrade.BSCOUNT];
        public static readonly int[] icMaxes = { 200, 50, 50 };
        public int[] instant = new int[(int)Upgrade.ICCOUNT];
        /// <summary>
        /// Called by the public setters
        /// </summary>
        /// <param name="which">which blacksmith upgrade to set</param>
        /// <param name="value">how many of it</param>
        private void setter(BSUpgrade which, int value)
        {
            if (value > bsMaxes[(int)which] && value != -1)
                throw new ArgumentOutOfRangeException(String.Format("Value {0} is larger than max {1} for {2}",value,which,bsMaxes[(int)which]));
            blacksmith[(int)which] = value;
        }
        /// <summary>
        /// Called by the public setters
        /// </summary>
        /// <param name="which">which instant upgrade to set</param>
        /// <param name="value">how many of it</param>
        private void setter(Upgrade which, int value)
        {
            if (value > icMaxes[(int)which] && value != -1)
                throw new ArgumentOutOfRangeException(String.Format("Value {0} is larger than max {1} for {2}",value,which,icMaxes[(int)which]));
            instant[(int)which] = value;
        }

        public int bbLevel
	    {
		    get { return blacksmith[(int)Upgrade.BSBASEPOWER];}
		    set { blacksmith[(int)Upgrade.BSBASEPOWER]=value; } // no maximum
 	    }
        public int bsLevel
	    {
            get { return blacksmith[(int)Upgrade.BSSKILL]; }
		    set { setter(Upgrade.BSSKILL,value); }
	    }
        public int bxLevel
	    {
            get { return blacksmith[(int)Upgrade.BSEXPERTISE]; }
		    set { setter(Upgrade.BSEXPERTISE,value); }
	    }
        public int bfLevel
	    {
		    get { return blacksmith[(int)Upgrade.BSEFFICIENCY];}
		    set { setter(Upgrade.BSEFFICIENCY,value); }
	    }
        public int gwLevel
	    {
		    get { return blacksmith[(int)Upgrade.BSGEMWASTER];}
		    set { setter(Upgrade.BSGEMWASTER,value); }
	    }
        public int gfLevel
        {
            get { return instant[(int)Upgrade.ICGEMFINDER];}
            set { setter(Upgrade.ICGEMFINDER,value); }
        }
        public int apLevel
        {
            get { return instant[(int)Upgrade.ICAUTOPOWER];}
            set { setter(Upgrade.ICAUTOPOWER,value); }
        }
        public int afLevel
        {
            get { return instant[(int)Upgrade.ICAUTOSPEED];}
            set { setter(Upgrade.ICAUTOSPEED,value); }
        }

        public int this[BSUpgrade i]
        {
            get { return blacksmith[(int)i]; }
            set { setter(i, value); }
        }

        public int this[Upgrade i]
        {
            get { return instant[(int)i]; }
            set { setter(i, value); }
        }

        public UpgradeState()
        {

        }

        public UpgradeState(UpgradeState us)
        {
            us.blacksmith.CopyTo(blacksmith, 0);
            us.instant.CopyTo(instant, 0);
        }
    }
}
