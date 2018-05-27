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
        private int Size { get; set; }
        private int _moveCount = 0;

        public FieldComponent(int size)
        {
            this.Size = size;
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

        public Message setFieldComponentCellWithValue(int x, int y, CellState value) 
        {
            if (x > this.Size || y > this.Size || x < 0 || y < 0)
            {
                return Message.WrongCoordinates;

            } else
            {
                if (this.Cells[x, y] == CellState.Empty)
                {
                    this.Cells[x, y] = value;
                    this._moveCount++;

                    //winCheck in row & column

                    for (int i = 0; i < this.Size; i++)
                    {
                        if (this.Cells[x, i] != value) 
                            break;
                        if (i == this.Size - 1)
                        {
                            this.SelfState = value;
                            return Message.StateChanged;
                        }
                    }

                    for (int i = 0; i < this.Size; i++)
                    {
                        if (this.Cells[i, y] != value)
                            break;
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
                            if (this.Cells[i, i] != value)
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
                            if (this.Cells[i, (this.Size - 1) - i] != value)
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
