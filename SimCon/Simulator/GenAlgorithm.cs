using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Simulator
{
    public class GenAlgorithm
    {
        public string name;
        public int[] robots = new int[2];
        public int[] barriers = new int[2];
        public int[] food = new int[2];
        public float[] dim = new float[2];
        public float GridSquareSize;

        public GenAlgorithm(string n, float[] d, int[] r, int[] b, int[] f)
        {
            this.name = n;
            this.dim = d;
            this.robots = r;
            this.barriers = b;
            this.food = f;
            this.GridSquareSize = 1;
        }

        public GenAlgorithm(string n, float[] d, int[] r, int[] b, int[] f, float g)
        {
            this.name = n;
            this.dim = d;
            this.robots = r;
            this.barriers = b;
            this.food = f;
            this.GridSquareSize = g;
        }
    }
}
