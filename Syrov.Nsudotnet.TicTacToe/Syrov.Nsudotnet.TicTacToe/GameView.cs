using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//x - red, O - blue

namespace Syrov.Nsudotnet.TicTacToe
{
    class GameView
    {
        private GameViewModel _viewModel { get; set; }

        public GameView(int sizeOfField)
        {
            this._viewModel = new GameViewModel(sizeOfField, Player.PlayerX);

            this.DrawField();
            this.ShowTurnMessage();

            while (true)
            {
                
                var coordinates = Console.In.ReadLine().Split(' ');
                if (coordinates.Length < 4)
                {
                    this.ShowErrorMessage(Message.WrongCoordinates);
                    continue;
                }
                Message message = Message.Cool;
                try
                {
                        message = this._viewModel.MakeMove(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1]),
                        Int32.Parse(coordinates[2]), Int32.Parse(coordinates[3]));
                } catch (FormatException)
                {
                    this.ShowErrorMessage(Message.WrongCoordinates);
                    continue;
                }
                if (message == Message.Cool)
                {
                    Console.Clear();
                    this._viewModel.ChangeActivePlayer();
                    this.DrawField();
                    this.ShowTurnMessage();
                } else
                {
                    if (message == Message.StateChanged)
                    {
                        Console.Clear();
                        this.DrawField();
                        this.ShowEndOfGameMessage();
                        Console.Read(); 
                        break;
                    } else
                    {
                        this.ShowErrorMessage(message);
                        continue;
                    }
                }

            }
        }

        private void ShowTurnMessage()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("To make a move, enter 4 numbers: coordinates of cell in the big field with format [x] [y]" +
                ", x and y have values from 0 to 2, then coordinates of cell in a small field, specified with previous " +
                "entered values, in the same way.");
            if (this._viewModel.Field.CurrentAvalibleCell != null)
            {
                int xField = this._viewModel.Field.CurrentAvalibleCell.Item1;
                int yField = this._viewModel.Field.CurrentAvalibleCell.Item2;
                Console.WriteLine("You can make a move only in small field with coordinates: " +
                    xField.ToString() + " " + yField.ToString());
            }

            if (this._viewModel.ActivePlayer == Player.PlayerO)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("O ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("X ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("turn:");
        }

        private void ShowErrorMessage(Message message)
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

        private void ShowEndOfGameMessage()
        {
            Console.ForegroundColor = ConsoleColor.White;
            switch (this._viewModel.Field.SelfState)
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

        private void DrawField()
        {
            for (int i = 0; i < this._viewModel.FieldSize; i++)
            {
                this.DrawFieldRow(i);
            }
        }

        private void DrawFieldRow(int rowField)
        {
            ConsoleColor colorForComponent;

            for (int rowComponent = 0; rowComponent < this._viewModel.FieldSize; rowComponent++)
            {
                //cycle on rows for each component in specified row of main field

                for (int columnField = 0; columnField < this._viewModel.FieldSize; columnField++)
                {
                    //cycle on columns of main field

                    colorForComponent = this.GetCellColor(rowField, columnField);
                    Console.ForegroundColor = colorForComponent;
                    var componentCells = this._viewModel.Field.GetStatesOfCellAtIndex(rowField, columnField);

                    for (int columnComponent = 0; columnComponent < this._viewModel.FieldSize; columnComponent++)
                    {
                        //cycle on columns for each component
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (componentCells[rowComponent, columnComponent] == CellState.X)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ForegroundColor = ConsoleColor.Blue;

                        Console.Write(this.GetStringRepresentationOfState(componentCells[rowComponent, columnComponent]));
                        Console.ForegroundColor = colorForComponent;
                        Console.Write("]");
                    }
                    Console.Write("  ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        private ConsoleColor GetCellColor(int x, int y)
        {
            switch (this._viewModel.Field.GetSelfStateOfCellAtindex(x, y))
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

        private String GetStringRepresentationOfState(CellState state)
        {
            switch (state)
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


    }
}
