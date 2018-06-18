using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    class StableSummer
    {
        List<double> partials;
        public double Sum()
        {
            return partials.Sum();
        }

        public StableSummer(StableSummer cloneFrom)
            : this()
        {
            if (cloneFrom != null)
            {
                partials.AddRange(cloneFrom.partials);
            }
        }

        public StableSummer(IList<double> sequence)
            : this()
        {
            if (sequence != null)
            {
                AddRange(sequence);
            }
        }

        public StableSummer()
        {
            partials = new List<double>();
        }

        public void AddRange(IList<double> sequence)
        {
            // there's probably a parallel implementation based on partitioning sequence
            // doing multiple StableSummers and then combining them
            foreach (double term in sequence)
            {
                Add(term);
            }
        }

        public void Add(double term)
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
            int i = 0;

            double smallerSummand = term;
            foreach (double yp in partials)
            {
                double largerSummand;
                if (Math.Abs(term) < Math.Abs(yp))
                {
                    smallerSummand = yp;
                    largerSummand = term;
                }
                else
                {
                    largerSummand = yp;
                }
                double roundedSum = smallerSummand + largerSummand;
                double roundoffError = largerSummand - (roundedSum - smallerSummand);
                if (roundoffError != 0.0)
                {
                    if (i == partials.Count)
                    {
                        partials.Add(roundoffError);
                    }
                    else
                    {
                        partials[i] = roundoffError;
                    }
                    i++;
                }
                term = roundedSum;
            }
            partials.RemoveRange(i, partials.Count - i);
            partials.Add(smallerSummand);       
        }
    }
}
