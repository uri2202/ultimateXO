﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


public class GameXO : ICloneable
{
   
    private BitBoard xMainBoard = new BitBoard();
    private BitBoard oMainBoard = new BitBoard();
    private BitBoard[] xBoards = { new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard()};
    private BitBoard[] oBoards = { new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard() };
    private static ushort fullBoard = 0b0111111111;
    private BitBoard mainDrawBoard = new BitBoard();//we need a board to keep track of the draws so we can detect a tie, the bits that are on are small bords that ended on a tie

    /// <summary>
    /// checks if a specific board has won or drawn
    /// </summary>
    /// <param name="boardNum">-1 if we want to check the main boards otherwise index </param>
    /// <param name="isX"> which player we want to check X or O</param>
    /// <returns> 0 - nothing 1 - win 2 - draw</returns>
    public int CheckWin(int boardNum,bool isX)
    {
        
        
        
        if (isX) {
            if (boardNum == -1)//if its the main board
            {
                if (xMainBoard.Won()) return 1;
               
                
            }
           else if(xBoards[boardNum].Won()) return 1;
           
        }
        else
        {
            if (boardNum == -1)//if its the main board
            {
                if (oMainBoard.Won()) return 1;


            }
            else if (oBoards[boardNum].Won()) return 1;
        }
        if (boardNum == -1)
        {
            if (xMainBoard.Board + oMainBoard.Board + mainDrawBoard.Board == fullBoard) return 2;//check for draw
        }
        else if (oBoards[boardNum].Board + xBoards[boardNum].Board == fullBoard) return 2;
        return 0;
        
    }
    public object Clone()
    {
        var clone = new GameXO();

        // Deep copy of BitBoard objects
        clone.xMainBoard = this.xMainBoard.Clone() as BitBoard;
        clone.oMainBoard = this.oMainBoard.Clone() as BitBoard;
        clone.xBoards = this.xBoards.Select(bb => bb.Clone() as BitBoard).ToArray();
        clone.oBoards = this.oBoards.Select(bb => bb.Clone() as BitBoard).ToArray();
        clone.mainDrawBoard = this.mainDrawBoard.Clone() as BitBoard;

       

        return clone;
    }
    /// <summary>
    /// saves the move and checks if it was legal
    /// </summary>
    /// <param name="boardNum">-1 if we want to make the move on the main boards otherwise index</param>
    /// <param name="row">row of the move</param>
    /// <param name="col">column if the move</param>
    /// <param name="isX">which player makes the move</param>
    /// <returns>true if the move was made and legel, false otherwise</returns>
    public bool MakeMove(int boardNum, int row,int col,bool isX)
    {
        if (boardNum == -2)
        {
            mainDrawBoard.MakeMove(row, col);//if we need to update draw board
            return true;
        }
        if(boardNum == -1)//if main board
        {
            if(((xMainBoard.Board + oMainBoard.Board) & BitBoard.moves[row, col]) == 0)
            {
                if (isX)
                {
                    xMainBoard.MakeMove(row, col);

                }
                else
                {
                    oMainBoard.MakeMove(row, col);
                }
            }
            else
            {
                return false;
            }
           
        }
        else 
        {
            if (((xBoards[boardNum].Board + oBoards[boardNum].Board) & BitBoard.moves[row, col]) != 0) return false;//checks if the move that we want to make hasnt already been made by x or o
            if (isX)
            {
                xBoards[boardNum].MakeMove(row, col);
            }
            else
            {
                oBoards[boardNum].MakeMove(row, col);
            }
            
        }
        return true;

    }

    public BitBoard XMainBoard
    {
        get { return xMainBoard; }
        set { xMainBoard = value; }
    }

    public BitBoard OMainBoard
    {
        get { return oMainBoard; }
        set { oMainBoard = value; }
    }

    public BitBoard MainDrawBoard
    {
        get { return mainDrawBoard; }
        set { mainDrawBoard = value; }
    }
    public BitBoard[] XBoards
    {
        get { return xBoards; }
        set { xBoards = value; }
    }

    public BitBoard[] OBoards
    {
        get { return oBoards; }
        set { oBoards = value; }
    }


}






