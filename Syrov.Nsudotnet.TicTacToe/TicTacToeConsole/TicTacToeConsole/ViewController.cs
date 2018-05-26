using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//x - red, O - blue

namespace TicTacToeConsole
{
    class ViewController
    {
        private Field _field;
        private int _fieldSize = 3;
        private CellState _activePlayer;

        public ViewController(int size)
        {
            this._field = new Field(this._fieldSize);
            this._activePlayer = CellState.X;
            this.drawField();
            Console.WriteLine("To make a move, enter 4 numbers: coordinates of cell in the big field with format [x] [y]" +
                ", x and y have values from 0 to 2, then coordinates of cell in a small field, specified with previous " +
                "entered values, in the same way.");
            
            while (true)
            {
                var coordinates = Console.In.ReadLine().Split(' ');
                if(coordinates.Length < 4)
                {
                    this.showMessage(Message.WrongCoordinates);
                    continue;
                }
                Message message = Message.Cool;
                try
                {
                        message = this._field.setFieldCellWithValue(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1]),
                        Int32.Parse(coordinates[2]), Int32.Parse(coordinates[3]), this._activePlayer);
                } catch (FormatException e)
                {
                    this.showMessage(Message.WrongCoordinates);
                    continue;
                }
                    if (message == Message.Cool)
                {
                    Console.Clear();
                    this.drawField();
                    this.changeCurrentPlayer();
                    this.showTurnMessage();
                } else
                {
                    if(message == Message.StateChanged)
                    {
                        Console.Clear();
                        this.drawField();
                        this.showEndOfGameMessage();
                        Console.Read();
                    }
                    showMessage(message);
                    continue;
                }
            }
        }

        private void drawField()
        {
            for (int i = 0; i < this._fieldSize; i++)
            {
                this.drawFieldRow(i);
            }
        }

        private void drawFieldRow(int rowField)
        {
            ConsoleColor colorForComponent;

            for (int rowComponent = 0; rowComponent < this._fieldSize; rowComponent++)
            {
                //cycle on rows for each component in specified row of main field

                for (int columnField = 0; columnField < this._fieldSize; columnField++)
                {
                    //cycle on columns of main field

                    colorForComponent = this.getCellColor(rowField, columnField);
                    Console.ForegroundColor = colorForComponent;
                    var componentCells = this._field.getStatesOfCellAtIndex(rowField, columnField);

                    for (int columnComponent = 0; columnComponent < this._fieldSize; columnComponent++)
                    {
                        //cycle on columns for each component
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (componentCells[rowComponent, columnComponent] == CellState.X)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ForegroundColor = ConsoleColor.Blue;

                        Console.Write(this.getStringRepresentationOfState(componentCells[rowComponent, columnComponent]));
                        Console.ForegroundColor = colorForComponent;
                        Console.Write("]");
                    }
                    Console.Write("  ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        private String getStringRepresentationOfState(CellState state)
        {
            switch(state)
            {
                case CellState.X:
                    return "X";
                case CellState.O:
                    return "O";
                case CellState.Empty:
                    return " ";
                default:
                    return "";
            }
        }

        private void showMessage(Message message)
        {
            switch (message)
            {
                case Message.NotAvalibleCell:
                    Console.WriteLine("You can't make a move in this cell.");
                    break;
                case Message.OccupatedCell:
                    Console.WriteLine("Selected cell is already occupied.");
                    break;
                case Message.WrongCoordinates:
                    Console.WriteLine("You entered wrong coordinates.");
                    break;
            }
        }

        private ConsoleColor getCellColor(int x, int y)
        {
            switch (this._field.getSelfStateOfCellAtindex(x, y))
            {
                case CellState.Draw:
                    return ConsoleColor.Green;
                    
                case CellState.X:
                    return ConsoleColor.Red;
                    
                case CellState.O:
                    return ConsoleColor.Blue;
                    
                case CellState.Empty:
                    return ConsoleColor.White;
                default:
                    return ConsoleColor.White;
            }
        }

        private void showTurnMessage()
        {
            int xField = this._field.CurrentAvalibleCell.Item1;
            int yField = this._field.CurrentAvalibleCell.Item2;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("You can make a move only in small field with coordinates: " +
                xField.ToString() + " " + yField.ToString());

            if(this._activePlayer == CellState.O)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("O ");
            } else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("X ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("turn:");
        }

        private void changeCurrentPlayer()
        {
            if (this._activePlayer == CellState.X)
            {
                this._activePlayer = CellState.O;
            } else
            {
                this._activePlayer = CellState.X;
            }
        }

        private void showEndOfGameMessage()
        {
            Console.ForegroundColor = ConsoleColor.White;
            switch(this._field.SelfState)
            {
                case CellState.Draw:
                    Console.WriteLine("Draw! No Winners and no losers today!");
                    break;
                case CellState.O:
                    Console.WriteLine("O won the game! Congratitulations!");
                    break;
                case CellState.X:
                    Console.WriteLine("X won the game! Congratitulations!");
                    break;
            }
        }
    }
}
