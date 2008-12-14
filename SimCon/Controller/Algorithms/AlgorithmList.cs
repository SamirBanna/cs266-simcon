using System;
using System.Collections.Generic;
using System.Text;


namespace CS266.SimCon.Controller
{
 
    ///
    /// <summary>Class used to create algorithm classes based on the name of the class</summary>
    /// 
    class AlgorithmList
    {
        public static Algorithm makeAlgorithm(String name, Robot r)
        {
            Algorithm alg = null;

            switch (name)
            {
                case "RandomWalk":
                    alg = new RandomWalk(r);
                    break;

                case "WalkStraight":
                    alg = new WalkStraight(r);
                    break;

            }

            return alg;

        }
    }
}
