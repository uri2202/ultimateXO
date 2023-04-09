using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Move
{
    private int board;
    private int row;
    private int col;

    public Move()
    {
       
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


}

