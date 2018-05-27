using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syrov.Nsudotnet.TicTacToe
{
    public enum CellState { Empty, X, O, Draw}

    public enum Message { Cool, OccupatedCell, WrongCoordinates, NotAvalibleCell, StateChanged}

    class FieldComponent
    {
        public CellState[,] Cells { get; set; }
        public CellState SelfState { get; set; }
        private int _size { get; set; }
        private int _moveCount = 0;

        public FieldComponent(int size)
        {
            this._size = size;
            this.SelfState = CellState.Empty;
            Cells = new CellState[size, size];
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    Cells[i, j] = CellState.Empty;
                }
            }
        }

        public Message SetFieldComponentCellWithValue(int x, int y, CellState value) 
        {
            if (x > this._size || y > this._size || x < 0 || y < 0)
            {
                return Message.WrongCoordinates;

            } else
            {
                if (this.Cells[x, y] == CellState.Empty)
                {
                    this.Cells[x, y] = value;
                    this._moveCount++;

                    //winCheck in row & column

                    for (int i = 0; i < this._size; i++)
                    {
                        if (this.Cells[x, i] != value) 
                            break;
                        if (i == this._size - 1)
                        {
                            this.SelfState = value;
                            return Message.StateChanged;
                        }
                    }

                    for (int i = 0; i < this._size; i++)
                    {
                        if (this.Cells[i, y] != value)
                            break;
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
                            if (this.Cells[i, i] != value)
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
                            if (this.Cells[i, (this._size - 1) - i] != value)
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
                        this.SelfState = CellState.Draw;

                    return Message.Cool;
                } else
                {
                    return Message.OccupatedCell;
                }
            }
        }
    
    }
}
