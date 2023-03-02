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
   
    

    public bool CheckWin(int boardNum,bool isX)// returns -1 if noone won, 0 if O won, 1 if X won
    {
        if (isX) {
            if (boardNum == -1)//if its the main board
            {
                return xMainBoard.Won();
               
                
            }
            return xBoards[boardNum].Won();
           
        }
        else
        {
            if (boardNum == -1)//if its the main board
            {
                return oMainBoard.Won();


            }
            return oBoards[boardNum].Won();
        }
        
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
    
        


    

