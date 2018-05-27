using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syrov.Nsudotnet.TicTacToe
{
    class Field
    {
        private FieldComponent[,] GameField { get; set; }
        private int Size { get; set; }
        public CellState SelfState { get; set; }
        private int _moveCount = 0;
        public Tuple<int, int> CurrentAvalibleCell { get; set; }

        public Field(int size)
        {
            this.Size = size;
            this.SelfState = CellState.Empty;
            this.CurrentAvalibleCell = null;
            this.GameField = new FieldComponent[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.GameField[i, j] = new FieldComponent(size);
                }
            }
        }

        public Message setFieldCellWithValue(int xField, int yField, int xCell, int yCell, CellState value)
        {
            if (this.isValidCoordinates(xField, yField))
            {
                if (this.GameField[xField, yField].SelfState != CellState.Draw)
                {
                    if (this.CurrentAvalibleCell != null)
                    {
                        if (xField != this.CurrentAvalibleCell.Item1 || yField != this.CurrentAvalibleCell.Item2)
                            return Message.NotAvalibleCell;
                    }
                    else
                        this.CurrentAvalibleCell = new Tuple<int, int>(xCell, yCell);

                    Message responseFromCell = this.GameField[xField, yField]
                        .setFieldComponentCellWithValue(xCell, yCell, value);

                    if (responseFromCell == Message.Cool || responseFromCell == Message.StateChanged)
                    {
                        this.CurrentAvalibleCell = new Tuple<int, int>(xCell, yCell);
                        if (responseFromCell == Message.StateChanged)
                        {
                            return this.winCheck(xField, yField, value);
                        }
                      
                    }
                    return responseFromCell;
                }
                else
                {
                    return Message.OccupatedCell;
                }
            } else
            {
                return Message.WrongCoordinates;
            }
        }

        public CellState[,] getCellsStates()
        {
            CellState[,] cellsStates = new CellState[this.Size, this.Size];
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    cellsStates[i, j] = this.GameField[i, j].SelfState;
                }
            }

            return cellsStates;
        }

        public CellState[,] getStatesOfCellAtIndex(int x, int y)
        {
            return this.GameField[x, y].Cells;
        }

        public CellState getSelfStateOfCellAtindex(int x, int y)
        {
            return this.GameField[x, y].SelfState;
        }

        private Boolean isValidCoordinates(int x, int y)
        {
            if (x >= 0 && x < this.Size && y >= 0 && y < this.Size)
                return true;
            else
                return false;
        }

        private Message winCheck(int x, int y, CellState value)
        {
            for (int i = 0; i < this.Size; i++)
            {
                if (this.GameField[x, i].SelfState != value) 
                    break;
                if (i == this.Size - 1)
                {
                    this.SelfState = value;
                    return Message.StateChanged;
                }
            }

            for (int i = 0; i < this.Size; i++)
            {
                if (this.GameField[i, y].SelfState != value)
                {
                    break;
                }

                if (i == this.Size - 1)
                {
                    this.SelfState = value;
                    return Message.StateChanged;
                }
            }

            //winCheck in diagonals

            if (x == y)
            {
                for (int i = 0; i < this.Size; i++)
                {
                    if (this.GameField[i, i].SelfState != value)
                        break;
                    if (i == this.Size - 1)
                    {
                        this.SelfState = value;
                        return Message.StateChanged;
                    }
                }
            }

            if (x + y == this.Size - 1)
            {
                for (int i = 0; i < this.Size; i++)
                {
                    if (this.GameField[i, (this.Size - 1) - i].SelfState != value)
                        break;
                    if (i == this.Size - 1)
                    {
                        this.SelfState = value;
                        return Message.StateChanged;
                    }
                }
            }

            //check for draw situation

            if (this._moveCount == (Math.Pow(this.Size, 2) - 1))
            {
                this.SelfState = CellState.Draw;
                return Message.StateChanged;
            }

            return Message.Cool;
        }
    }
}
