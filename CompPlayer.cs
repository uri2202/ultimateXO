using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


class CompPlayer
{
    private const int gameWin = 10000;
    private const int boardWin = 7;
    private const int centerBoardWin = 12;
    private const int cornerBoardWin = 9;
    private const int centerSquare = 2;
    private const int centerBoardSquare = 2;
    private const int twoBoardInARow = 4;
    private const int twoSquaresInARow = 3;
    private const int anyBoardMove = -3;
    private const int moveToWinBoard = -5;
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
        finalScore += CenterSquare(tempGame, isX);
        finalScore += CenterBoardSquare(tempGame, isX);
        finalScore += BoardsInARow(tempGame, isX);
        finalScore += SquaresInARow(tempGame, isX);
        finalScore += MoveToAnyBoard(tempGame, move);
        finalScore += MoveToBoardWin(tempGame, move, isX);



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
    private int CenterSquare(GameXO game, bool isX)
    {
        ushort centerBit = BitBoard.moves[1, 1];//the bit that represents the center square in the board
        int score = 0;
        for (int i = 0; i < 9; i++)
        {
            if ((game.OBoards[i].Board & centerBit) != 0)
            {
                score += centerSquare;
            }
            if ((game.XBoards[i].Board & centerBit) != 0)
            {
                score -= centerSquare;
            }

        }
        if(isX)score*= -1;
        return score;
    }
    private int CenterBoardSquare(GameXO game, bool isX)
    {
        int score = 0;
        ushort tempBoard = game.OBoards[4].Board;
        int count = BitCount(tempBoard);
        
        score += count * centerBoardSquare;
         tempBoard = game.XBoards[4].Board;
        count = BitCount(tempBoard);
        
        score-= count * centerBoardSquare;
        if(isX) score*= -1;
        return score;
    }
    private int BitCount(ushort num)
    {
        int count = 0;
        while(num>0)
        {
            count += num & 1;
            num >>= 1;
        }
        return count;
    }

    private bool CanWinInOne(BitBoard winBoard,BitBoard secondBoard,BitBoard drawBoard)//winBoard is the board that we want to check if it can win in 1 move and the secondBoard is the second one, both of then could be x or o
    {
        bool canWin = false;
        for (int i = 0; i < BitBoard.wins.Length&&!canWin; i++)
        {
            ushort emptySquares = (ushort)~(winBoard.Board|secondBoard.Board|drawBoard.Board);
            if (BitCount((ushort)(winBoard.Board & BitBoard.wins[i])) == 2)//we need to check that the player has alredy placed 2 moves that can lead to a win
            {
                ushort emptySquareMask = (ushort)(BitBoard.wins[i]&emptySquares);//this has the empty square that is a part of this win pattern
                if ( emptySquareMask != 0)//we need to check that there is a empty sqaure this a part of the win pattern
                {
                    canWin = true;
                }
            }
        }
        return canWin;
    }
    private int BoardsInARow(GameXO game, bool isX)//this heuristic is for two boards wins that are in a row/column/diaginal that can lead to a game win
    {
        int score = 0;
        if (CanWinInOne(game.OMainBoard, game.XMainBoard,game.MainDrawBoard)) score += twoBoardInARow;
        if (CanWinInOne(game.XMainBoard, game.OMainBoard,game.MainDrawBoard)) score -= twoBoardInARow;
        if(isX) score*=-1;
        return score;
    }
    private int SquaresInARow(GameXO game, bool isX)
    {
        int score = 0;
        for (int i = 0; i < 9; i++)
        {
            if (CanWinInOne(game.OBoards[i], game.XBoards[i],new BitBoard(0))) score += twoBoardInARow;
            if (CanWinInOne(game.XBoards[i], game.OBoards[i], new BitBoard(0))) score -= twoBoardInARow;
        }
        if (isX) score *= -1;
        return score;
    }
    private int MoveToAnyBoard(GameXO game, Move move)//heuristc that checks if the move sent the enemy to play in any board
    {
        ushort fullBoard = (ushort)(game.OMainBoard.Board|game.MainDrawBoard.Board|game.XMainBoard.Board);
        if ((BitBoard.moves[move.Row, move.Col] & fullBoard) != 0) return anyBoardMove;
        return 0;
    }
    private int MoveToBoardWin(GameXO game, Move move, bool isX)
    {
        ushort fullBoard = (ushort)(game.OMainBoard.Board | game.MainDrawBoard.Board | game.XMainBoard.Board);
        int score = 0;
        ushort mask = 1;
        bool canWin = false;
        if (MoveToAnyBoard(game, move) == anyBoardMove)//if the player can play in any board he wants we need to check all of them
        {
            if (isX)
            {
                for (int i = 0; i < 9&&!canWin; i++)
                {
                    if ((mask & fullBoard) != 0)//this check that the board we are cheking hasnt already won
                    {
                        if (CanWinInOne(game.OBoards[i], game.XBoards[i],new BitBoard(0)))
                        {
                            score += moveToWinBoard;
                            canWin = true;
                        }
                       
                    }
                    mask <<= 1;
                }
            }
            else
            {
                for (int i = 0; i < 9 && !canWin; i++)
                {
                    if ((mask & fullBoard) != 0)//this check that the board we are cheking hasnt already won
                    {
                        if (CanWinInOne(game.XBoards[i], game.OBoards[i], new BitBoard(0)))
                        {
                            score += moveToWinBoard;
                            canWin = true;
                        }
                    }
                    mask <<= 1;
                }
            }
        }
        else
        {
            int boardNum = move.Row * 3 + move.Col;
            if (isX)
            {
                if (CanWinInOne(game.OBoards[boardNum], game.XBoards[boardNum],new BitBoard(0)))  score += moveToWinBoard;
            }
            else
            {
                if (CanWinInOne(game.XBoards[boardNum], game.OBoards[boardNum], new BitBoard(0))) score += moveToWinBoard;
            }
        }
        return score;
    }
}

