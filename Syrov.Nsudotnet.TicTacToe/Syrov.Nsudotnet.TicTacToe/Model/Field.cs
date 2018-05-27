using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syrov.Nsudotnet.TicTacToe
{
    class Field
    {
        public CellState SelfState { get; set; }
        public Tuple<int, int> CurrentAvalibleCell { get; set; }
        private FieldComponent[,] _gameField { get; set; }
        private int _size { get; set; }
        private int _moveCount = 0;
        

        public Field(int size)
        {
            this._size = size;
            this.SelfState = CellState.Empty;
            this.CurrentAvalibleCell = null;
            this._gameField = new FieldComponent[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this._gameField[i, j] = new FieldComponent(size);
                }
            }
        }

        public Message SetFieldCellWithValue(int xField, int yField, int xCell, int yCell, CellState value)
        {
            if (this.IsValidCoordinates(xField, yField))
            {
                if (this._gameField[xField, yField].SelfState != CellState.Draw)
                {
                    if (this.CurrentAvalibleCell != null)
                    {
                        if (xField != this.CurrentAvalibleCell.Item1 || yField != this.CurrentAvalibleCell.Item2)
                            return Message.NotAvalibleCell;
                    }
                    else
                        this.CurrentAvalibleCell = new Tuple<int, int>(xCell, yCell);

                    Message responseFromCell = this._gameField[xField, yField]
                        .SetFieldComponentCellWithValue(xCell, yCell, value);

                    if (responseFromCell == Message.Cool || responseFromCell == Message.StateChanged)
                    {
                        this.CurrentAvalibleCell = new Tuple<int, int>(xCell, yCell);
                        if (responseFromCell == Message.StateChanged)
                        {
                            return this.WinCheck(xField, yField, value);
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

        public CellState[,] GetCellsStates()
        {
            CellState[,] cellsStates = new CellState[this._size, this._size];
            for (int i = 0; i < this._size; i++)
            {
                for (int j = 0; j < this._size; j++)
                {
                    cellsStates[i, j] = this._gameField[i, j].SelfState;
                }
            }

            return cellsStates;
        }

        public CellState[,] GetStatesOfCellAtIndex(int x, int y)
        {
            return this._gameField[x, y].Cells;
        }

        public CellState GetSelfStateOfCellAtindex(int x, int y)
        {
            return this._gameField[x, y].SelfState;
        }

        private Boolean IsValidCoordinates(int x, int y)
        {
            if (x >= 0 && x < this._size && y >= 0 && y < this._size)
                return true;
            else
                return false;
        }

        private Message WinCheck(int x, int y, CellState value)
        {
            for (int i = 0; i < this._size; i++)
            {
                if (this._gameField[x, i].SelfState != value) 
                    break;
                if (i == this._size - 1)
                {
                    this.SelfState = value;
                    return Message.StateChanged;
                }
            }

            for (int i = 0; i < this._size; i++)
            {
                if (this._gameField[i, y].SelfState != value)
                {
                    break;
                }

                if (i == this._size - 1)
                {
                    this.SelfState = value;
                    return Message.StateChanged;
                }
            }

            //winCheck in diagonals

            if (x == y)
            {
                for (int i = 0; i < this._size; i++)
                {
                    if (this._gameField[i, i].SelfState != value)
                        break;
                    if (i == this._size - 1)
                    {
                        this.SelfState = value;
                        return Message.StateChanged;
                    }
                }
            }

            if (x + y == this._size - 1)
            {
                for (int i = 0; i < this._size; i++)
                {
                    if (this._gameField[i, (this._size - 1) - i].SelfState != value)
                        break;
                    if (i == this._size - 1)
                    {
                        this.SelfState = value;
                        return Message.StateChanged;
                    }
                }
            }

            //check for draw situation

            if (this._moveCount == (Math.Pow(this._size, 2) - 1))
            {
                this.SelfState = CellState.Draw;
                return Message.StateChanged;
            }

            return Message.Cool;
        }
    }
}
