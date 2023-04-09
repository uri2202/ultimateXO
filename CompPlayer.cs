using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CompPlayer
{
    public CompPlayer()
    {

    }
    public Move MakeMove(GameXO game, Move move)// the ai is always the O player, game is the current game situation, move is the move that was just made by the real player
    {
        Move finalMove = new Move();
        //first we need to check in which boards the move can be made
        bool[] legalBoards = new bool[9];//true if the board in the index is legal false otherwise
        Array.Fill(legalBoards, false);
        if(!game.XMainBoard.WasMoveMade(move.Row,move.Col)&& !game.OMainBoard.WasMoveMade(move.Row, move.Col) && !game.MainDrawBoard.WasMoveMade(move.Row, move.Col))//this checks if the board we got sent to is full or not
        {
            legalBoards[move.Row*3+move.Col] = true;
        }
        else
        {
            Array.Fill(legalBoards, true);
        }
        for (int board = 0; board < 9; board++)
        {
            if (legalBoards[board]&& !game.XMainBoard.WasMoveMadeInd(board) && !game.OMainBoard.WasMoveMadeInd(board) && !game.MainDrawBoard.WasMoveMadeInd(board))
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (!game.XBoards[board].WasMoveMade(row,col)&& !game.OBoards[board].WasMoveMade(row, col))
                        {
                            finalMove.Row= row;
                            finalMove.Col= col;
                            finalMove.Board= board;
                            return finalMove;
                        }
                    }
                }
            }
        }
        return null;

    }
}

