using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeExample
{
    public class Cell
    {
        public bool Alive = false;
        public List<bool> Neighbors;
        private bool NewState = false;
        public Cell()
        {
            Neighbors = new List<bool>();
        }
        public void DetermineFate()
        {
            //int NeighborsAlive = Neighbors.Where(x => x.Equals(true)).Count();  // This Lambda counts how many are true
            int NeighborsAlive = 0;
            foreach (bool state in Neighbors)
            {
                if (state == true)
                    NeighborsAlive++;
            }

            
            NewState = false;

            // Test our rules
            if (NeighborsAlive == 2 && Alive)
            {
                NewState = true;
            }
            else if (NeighborsAlive == 3)
            {
                NewState = true;
            }
            // All other rules state we die so a default of false works here to simplify
            Neighbors.Clear(); // Empty the neighbors list as we do not need this anymore
        }

        public void Tick()
        {
            Alive = NewState;  // Update our new state of life

        }
    }
}
