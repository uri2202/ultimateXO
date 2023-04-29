using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using static System.Formats.Asn1.AsnWriter;

public class BitBoard : ICloneable
{
    private ushort board = 0;
    
    public static ushort[,] moves = {//i need this array to be public so i can check if the move is legal on the gameXO class
    { 1, 0b10, 0b100 },
    { 0b1000, 0b10000, 0b100000 },
    { 0b1000000, 0b10000000, 0b100000000 }
};

    public static ushort[] wins = {
    (ushort)(moves[0, 0] + moves[0, 1] + moves[0, 2]),
    (ushort)(moves[1, 0] + moves[1, 1] + moves[1, 2]),
    (ushort)(moves[2, 0] + moves[2, 1] + moves[2, 2]),
    (ushort)(moves[0, 0] + moves[1, 0] + moves[2, 0]),
    (ushort)(moves[0, 1] + moves[1, 1] + moves[2, 1]),
    (ushort)(moves[0, 2] + moves[1, 2] + moves[2, 2]),
    (ushort)(moves[0, 0] + moves[1, 1] + moves[2, 2]),
    (ushort)(moves[0, 2] + moves[1, 1] + moves[2, 0])
};
    public BitBoard(ushort board)
    {
        this.board = board;
    }
    public BitBoard()
    {
        
    }

    public void MakeMove(int row, int col)
    {
        
        
        board += moves[row, col];
        
    }
    public void MakeMoveInd(int square)
    {

        int row = square / 3;//get the row and col of the panel number
        int col = square % 3;
        board += moves[row, col];

    }

    public object Clone()
    {
        BitBoard clone = new BitBoard();
        clone.board = this.board;
        return clone;
    }
    /// <summary>
    /// checks all of the winnig patterns on the bitboard
    /// </summary>
    /// <returns>wether this bitboard won</returns>
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
    
    public ushort Board
    {
        get { return board; }
        set { board = value; }
    }
    /// <summary>
    /// checks if a move was made on this bitboard
    /// </summary>
    /// <param name="row"> row of the move we want to check</param>
    /// <param name="col"> column of the move we want to check</param>
    /// <returns>true if that bit is '1' otherwise false</returns>
    public bool WasMoveMade(int row,int col)
    {
        int squareNum = row * 3 + col;


        if ((board & (1 << squareNum)) != 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// checks if a move was made on this bitboard
    /// </summary>
    /// <param name="squareNum"> the index of the bit we want to check</param>
    /// <returns>true if that bit is '1' otherwise false</returns>
    public bool WasMoveMadeInd(int squareNum)
    {
       
        if ((board & (1 << squareNum)) != 0)
        {
            return true;
        }
        return false;
    }

}


