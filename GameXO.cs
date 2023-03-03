using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


public class GameXO
{
   
    private BitBoard xMainBoard = new BitBoard();
    private BitBoard oMainBoard = new BitBoard();
    private BitBoard[] xBoards = { new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard()};
    private BitBoard[] oBoards = { new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard(), new BitBoard() };
    private ushort fullBoard = 0b0111111111;
    

    public int CheckWin(int boardNum,bool isX)//0 - nothing 1 - win 2 - draw
    {
        
        if(boardNum == -1)
        {
            if (xMainBoard.GetBoard() + oMainBoard.GetBoard() == fullBoard) return 2;//check for draw
        }
        else if (oBoards[boardNum].GetBoard() + xBoards[boardNum].GetBoard() == fullBoard) return 2;
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
        return 0;
        
    }
    public bool MakeMove(int boardNum, int row,int col,bool isX)//returns true if the move was made and legel, false otherwise
    {
        
        if(boardNum == -1)
        {
            if(((xMainBoard.GetBoard() + oMainBoard.GetBoard()) & BitBoard.moves[row, col]) == 0)
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
            if (((xBoards[boardNum].GetBoard() + oBoards[boardNum].GetBoard()) & BitBoard.moves[row, col]) != 0) return false;//checks if the move that we want to make hasnt already been made by x or o
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
}
    
        


    

