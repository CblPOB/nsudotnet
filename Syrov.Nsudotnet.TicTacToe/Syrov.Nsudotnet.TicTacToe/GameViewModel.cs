using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Syrov.Nsudotnet.TicTacToe
{
    public enum Player { PlayerX, PlayerO}

    class GameViewModel
    {
        public Field Field;
        public int FieldSize = 3;
        public Player ActivePlayer;

        public GameViewModel(int sizeOfField, Player activePlayer)
        {
            this.Field = new Field(this.FieldSize);
            this.ActivePlayer = activePlayer;
        }

        public Message MakeMove(int xField, int yField, int xComponent, int yComponent)
        {
            CellState valueForCell = this.ActivePlayer == Player.PlayerX ? CellState.X : CellState.O; 
            return this.Field.SetFieldCellWithValue(xField, yField, xComponent, yComponent, valueForCell);
        }

        public void ChangeActivePlayer()
        {
            this.ActivePlayer = this.ActivePlayer == Player.PlayerX ? Player.PlayerO : Player.PlayerX;
        }
    }
}
