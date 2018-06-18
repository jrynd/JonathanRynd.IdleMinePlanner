using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    enum UpgradeID : int
    {
	BSFLAG = 256,
        BSBASEPOWER=0|256,
        BSSKILL=1|256,
        BSEXPERTISE=2|256,
        BSEFFICIENCY=3|256,
        BSGEMWASTER=4|256,
        BSMAX=4|256,
        BSCOUNT=5|256,
	ICFLAG=512,
        ICAUTOPOWER = 0|512,
        ICAUTOSPEED = 1|512,
        ICGEMFINDER = 2|512,
        ICMAX=2|512,
        ICCOUNT=3|512,
        MASK=255
    }

    class UpgradeState
    {
        // TODO fix these max levels
        public static readonly int[] bsMaxes = {0,30,10,50,6};
        public int[] blacksmith = new int[(int)UpgradeID.BSCOUNT];
        public static readonly int[] icMaxes = { 200, 50, 50 };
        public int[] instant = new int[(int)UpgradeID.ICCOUNT];
        /// <summary>
        /// Called by the public setters
        /// </summary>
        /// <param name="which">which blacksmith UpgradeID to set</param>
        /// <param name="value">how many of it</param>
        private void setter(UpgradeID which, int value)
        {
            if (which.HasFlag(UpgradeID.BSFLAG))
            {
                if (value > bsMaxes[(int)which&127] && value != -1)
                    throw new ArgumentOutOfRangeException(String.Format("Value {0} is larger than max {1} for {2}",value,which,bsMaxes[(int)which&127]));
                blacksmith[(int)which&127] = value;
            } else if (which.HasFlag(UpgradeID.ICFLAG))
            {
                if (value > icMaxes[(int)which&127] && value != -1)
                    throw new ArgumentOutOfRangeException(String.Format("Value {0} is larger than max {1} for {2}",value,which,icMaxes[(int)which&127]));
                blacksmith[(int)which&127] = value;
            }
        }

        public int bbLevel
	    {
		    get { return blacksmith[(int)UpgradeID.BSBASEPOWER & 127];}
		    set { blacksmith[(int)UpgradeID.BSBASEPOWER & 127]=value; } // no maximum
 	    }
        public int bsLevel
	    {
            get { return blacksmith[(int)UpgradeID.BSSKILL & 127]; }
		    set { setter(UpgradeID.BSSKILL,value); }
	    }
        public int bxLevel
	    {
            get { return blacksmith[(int)UpgradeID.BSEXPERTISE & 127]; }
		    set { setter(UpgradeID.BSEXPERTISE,value); }
	    }
        public int bfLevel
	    {
		    get { return blacksmith[(int)UpgradeID.BSEFFICIENCY & 127];}
		    set { setter(UpgradeID.BSEFFICIENCY,value); }
	    }
        public int gwLevel
	    {
		    get { return blacksmith[(int)UpgradeID.BSGEMWASTER & 127];}
		    set { setter(UpgradeID.BSGEMWASTER,value); }
	    }
        public int gfLevel
        {
            get { return instant[(int)UpgradeID.ICGEMFINDER & 127];}
            set { setter(UpgradeID.ICGEMFINDER,value); }
        }
        public int apLevel
        {
            get { return instant[(int)UpgradeID.ICAUTOPOWER & 127];}
            set { setter(UpgradeID.ICAUTOPOWER,value); }
        }
        public int afLevel
        {
            get { return instant[(int)UpgradeID.ICAUTOSPEED & 127];}
            set { setter(UpgradeID.ICAUTOSPEED,value); }
        }

        public int this[UpgradeID i]
        {
            get
	        {
		        if (i.HasFlag(UpgradeID.BSFLAG)) return blacksmith[(int)i & 127];
                if (i.HasFlag(UpgradeID.ICFLAG)) return instant[(int)i & 127];
                    throw new ArgumentException("i");
	        }
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

        public static UpgradeState operator +(UpgradeState a, ClickUpgradePath c)
        {
            UpgradeState retval = new UpgradeState(a);
            c.UpgradeOrder.ForEach(x => retval.instant[(int)x&127]++);
            return retval;
        }
    }
}
