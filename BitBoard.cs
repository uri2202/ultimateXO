using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

public class BitBoard
{
    private ushort board = 0;
    
    public static ushort[,] moves = {//i need this array to be public so i can check if the move is legal on the gameXO class
    { 1, 0b10, 0b100 },
    { 0b1000, 0b10000, 0b100000 },
    { 0b1000000, 0b10000000, 0b100000000 }
};

    private static ushort[] wins = {
    (ushort)(moves[0, 0] + moves[0, 1] + moves[0, 2]),
    (ushort)(moves[1, 0] + moves[1, 1] + moves[1, 2]),
    (ushort)(moves[2, 0] + moves[2, 1] + moves[2, 2]),
    (ushort)(moves[0, 0] + moves[1, 0] + moves[2, 0]),
    (ushort)(moves[0, 1] + moves[1, 1] + moves[2, 1]),
    (ushort)(moves[0, 2] + moves[1, 2] + moves[2, 2]),
    (ushort)(moves[0, 0] + moves[1, 1] + moves[2, 2]),
    (ushort)(moves[0, 2] + moves[1, 1] + moves[2, 0])
};
    
    public void MakeMove(int row, int col)
    {
        
        
        board += moves[col, row];
        
    }
    public bool Won()
    {
        for (int i = 0; i < wins.Length; i++)
        {
            if ((ushort)(board & wins[i]) == wins[i])
            {
                return true;
            }
        }
        return false;
    }
    public ushort GetBoard()
    {
        return board;
    }

}


