using System;
using System.Collections.Generic;
using System.Text;


namespace CS266.SimCon.Controller
{
    /**
     * 
     * Class used to create algorithm classes based on the name of the class
     * 
     * */
    class AlgorithmList
    {
        public static Algorithm makeAlgorithm(String name, Robot r)
        {
            Algorithm alg = null;

            switch (name)
            {
                case "randWalk":
                    alg = new randWalk(r);
                    break;

                case "WalkStraight":
                    alg = new WalkStraight(r);
                    break;

            }

            return alg;

        }
    }
}
