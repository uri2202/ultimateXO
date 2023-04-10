using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Move : ICloneable
{
    private int board;
    private int row;
    private int col;
    private int score;

    public Move()
    {
       
    }

    public object Clone()
    {
        Move clone  = new Move();
        clone.board = board;
        clone.row = row;
        clone.col = col;
        clone.score = score;
        return clone;
    }

    public Move(int board, int row, int col)
    {
        this.board = board;
        this.row = row;
        this.col = col;
    }
    public int Board
    {
        get { return board; }
        set { board = value; }
    }

    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    public int Col
    {
        get { return col; }
        set { col = value; }
    }
    public int Score
    {
        get { return score; }
        set { score = value; }
    }


}

