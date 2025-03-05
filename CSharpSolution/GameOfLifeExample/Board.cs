using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeExample
{
    public class Board
    {
        
        public Cell[,] Positions;
        private int size = 0;
        public Board(int Size)
        {
            Positions = new Cell[Size, Size]; //creates the array but does not instantiate the members.
            size = Size;

            // Instantiate the array. we have to iterate through every single one and create each instance seperately
            // A lot of programmers get stuck here as creating the array LOOKS like it does this job.

            for(int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                    Positions[x, y] = new Cell();

            ClearBoard();
        }

        public void ClearBoard()
        {
            foreach (Cell cell in Positions)
                cell.Alive = false;
        }

        public void SetPosition(int x, int y, bool state)
        {
            Positions[x, y].Alive = state;
        }
        public void TogglePosition(int x, int y)
        {
            Positions[x, y].Alive = !Positions[x, y].Alive;
        }

        public void ProcessTick()
        {
            UpdateCells();

            foreach (var cell in Positions)
                cell.DetermineFate();

            foreach (var cell in Positions)
                cell.Tick();
        }

        private void UpdateCells()
        {

            for (int y = 0; y < Positions.GetLength(1); y++) // Get the length of the array
            {
                int Ay = Wrap(y, -1);       //Above
                int By = Wrap(y, 1);        //Below
                
                for (int x = 0; x < Positions.GetLength(0); x++) // width
                {
                    int Lx = Wrap(x, -1);   //Left
                    int Rx = Wrap(x, 1);    //Right

                    // Load the cells neighbors
                    Positions[x, y].Neighbors.Add(Positions[Lx, Ay].Alive);
                    Positions[x, y].Neighbors.Add(Positions[x, Ay].Alive);
                    Positions[x, y].Neighbors.Add(Positions[Rx, Ay].Alive);
                    Positions[x, y].Neighbors.Add(Positions[Lx, y].Alive);
                    Positions[x, y].Neighbors.Add(Positions[Rx, y].Alive);
                    Positions[x, y].Neighbors.Add(Positions[Lx, By].Alive);
                    Positions[x, y].Neighbors.Add(Positions[x, By].Alive);
                    Positions[x, y].Neighbors.Add(Positions[Rx, By].Alive);
                }
            }
        }
        /// <summary>
        /// Causes the numbers to WRAP around at the size defined for the array
        /// </summary>
        /// <param name="i"> the number you want to wrap and have returned with an increment</param>
        /// <param name="direction">-1 for down 1 for up</param>
        /// <returns>new value wrapped to the bounds of size</returns>
        private int Wrap(int i, int direction)
        {
            if (i < 1 && direction < 0)
                i = size - 1;
            else if (i == size - 1 && direction > 0)
                i = 0;
            else
                i = i + direction;
            return i;
        }
    }
}
