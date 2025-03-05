using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.UI;         	// For Generic Device Support
using System;

namespace GameOfLifeExample

{
    public class ControlSystem : CrestronControlSystem
    {
        public Board myBoard;

        public XpanelForSmartGraphics myXpanel;

        public ControlSystem()
            : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }

            // Get in and out of here.
        public override void InitializeSystem()
        {
            try
            {
                

                myXpanel = new XpanelForSmartGraphics(0x03, this);
                myXpanel.Register();
                myXpanel.SigChange += MyXpanel_SigChange;

                CrestronConsole.PrintLine("Xpanel started");

                myBoard = new Board(15);  // Create our new board

                CrestronConsole.PrintLine("Board Created");

            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
        }


        // Contrary to popular belief,  it's 100% OK to have code here in ControlSystem.
        // You have to manage things properly.  Adhere to the rules in the constructor AND InitilizeSystem
        // 
        // Just dont dump everything in here.  Leverage classes where it makes sense.

        // Example: I have a single event handler for touchpanel joins with minimal code. If this becomes complex, it makes sense
        // to break this out into it's own class to manage it better and to not clutter controlsystem with code.
        // Why is my TP join handler here?  it simplifies context and access. the classes I need are all right here.
        // If I decided to put the toucpanel in it's own class, I would simply pass the board class to the touchpanel's class
        // Beware the OO traps:  some people believe everything needs to be Object Oriented, and loosely coupled to the extreme.
        //  Avoid wasting time optimizing and making reuseable code you will never actually re-use.  "someday" will never come.
        private void MyXpanel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            
            if (args.Sig.Type == eSigType.Bool && args.Sig.BoolValue == true)  // We only care about button pushes
            {
                // Yes Tim is using Join Numbers and not contracts
                // Join Map: 230 = Next   231 = Clear.  1 to 225 is the playfield

                if (args.Sig.Number > 0 && args.Sig.Number <= 225)  // Playfield joins
                {
                    int Choice = (int)(args.Sig.Number - 1);       // Yes just like I did in S+
                    int X = Choice % 15;
                    int Y = Choice / 15;

                    myBoard.TogglePosition(X, Y);
                    UpdatePanel();
                }
                else if (args.Sig.Number == 230)             // Next Button
                {
                    myBoard.ProcessTick();
                    UpdatePanel();
                }
                else if (args.Sig.Number == 231)            //Clear button
                {
                    myBoard.ClearBoard();
                    UpdatePanel();
                }
            }
        }

        private void UpdatePanel()
        {
            ushort index = 1;
            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    myXpanel.BooleanInput[index].BoolValue = myBoard.Positions[x, y].Alive;
                    index++;
                
                }
            }
        }
    }
}