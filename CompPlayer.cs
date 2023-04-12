using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


class CompPlayer
{
    private const int gameWin = 10000;
    private const int boardWin = 5;
    private const int centerBoardWin = 11;
    private const int cornerBoardWin = 8;
    private const int centerSquare = 2;
    public CompPlayer()
    {

    }
    public Move MakeMove(GameXO game, Move move)// the ai is always the O player, game is the current game situation, move is the move that was just made by the real player
    {
        Move finalMove = new Move();
        finalMove.Score = -100000;
        Move tempMove = new Move();
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
                           
                            tempMove.Row= row;
                            tempMove.Col= col;
                            tempMove.Board= board;
                            tempMove.Score = Evaluate(tempMove, game, false );
                            if(tempMove.Score>finalMove.Score) finalMove = tempMove.Clone() as Move;
                        }
                    }
                }
            }
        }
        return finalMove;

    }
    private int Evaluate(Move move,GameXO game, bool isX)
    {
        int finalScore = 0;
        GameXO tempGame = game.Clone() as GameXO;
        //here we make sure we make the move for the correct player and if the move led to a small board win we make sure to add it to the main boards
        if (isX)
        {
            tempGame.XBoards[move.Board].MakeMove(move.Row, move.Col);
            if (tempGame.XBoards[move.Board].Won())
            {
                tempGame.XMainBoard.MakeMoveInd(move.Board);
            }
        }

       else
        {
            tempGame.OBoards[move.Board].MakeMove(move.Row,move.Col);
            if (tempGame.OBoards[move.Board].Won())
            {
                tempGame.OMainBoard.MakeMoveInd(move.Board);
            }
        }
        finalScore += GameWon(tempGame, isX);
        finalScore += BoardWon(tempGame, isX);


        return finalScore;
    }
    private int GameWon(GameXO game,bool isX)//heuristic that gives score based on winning the whole game
    {
        int score = 0;
        if (game.OMainBoard.Won()) score += gameWin;
        else if (game.XMainBoard.Won()) score -= gameWin;
        if (isX) score *= -1;
        return score;
        
    }
    private int BoardWon(GameXO game,bool isX)//heurstic that gives score based on small board wins
    {
        int score = 0;
        for (int i = 0; i < 9; i++)
        {
            if (game.OBoards[i].Won())
            {
                if (i == 4) score += centerBoardWin;
                else if (1 == 0 || i == 2 || i == 8 || i == 8) score += cornerBoardWin;
                else score += boardWin;
            }
            else if (game.XBoards[i].Won())
            {
                if (i == 4) score -= centerBoardWin;
                else if (1 == 0 || i == 2 || i == 8 || i == 8) score -= cornerBoardWin;
                else score -= boardWin;
            }
        }
        if(isX) score *= -1;
        return score;
    }
}

