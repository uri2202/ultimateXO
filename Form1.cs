
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox;

namespace ultimateXO
{
    public partial class Form1 : Form
    {
        string xOro="";//the text of the current player thats making the move
        private const int BoardSize = 3;
        private const int numOfBoards = 9;
        private Button[, ,] buttons; // The array of buttons representing the board
        TableLayoutPanel[] boards;
        private bool xTurn = true;
        private GameXO game = new GameXO();

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
        }

        // Initialize the Tic Tac Toe board
        private void InitializeBoard()
        {

            buttons = new Button[numOfBoards, BoardSize, BoardSize];
            boards = new TableLayoutPanel[numOfBoards];

            table1.RowCount = BoardSize;
            table1.ColumnCount = BoardSize;
            boards[0] = table1;
            table2.RowCount = BoardSize;
            table2.ColumnCount = BoardSize;
            boards[1] = table2;
            table3.RowCount = BoardSize;
            table3.ColumnCount = BoardSize;
            boards[2] = table3;
            table4.RowCount = BoardSize;
            table4.ColumnCount = BoardSize;
            boards[3] = table4;
            table5.RowCount = BoardSize;
            table5.ColumnCount = BoardSize;
            boards[4] = table5;
            table6.RowCount = BoardSize;
            table6.ColumnCount = BoardSize;
            boards[5] = table6;
            table7.RowCount = BoardSize;
            table7.ColumnCount = BoardSize;
            boards[6] = table7;
            table8.RowCount = BoardSize;
            table8.ColumnCount = BoardSize;
            boards[7] = table8;
            table9.RowCount = BoardSize;
            table9.ColumnCount = BoardSize;
            boards[8] = table9;
            

            for (int boardNum = 0; boardNum < numOfBoards; boardNum++)
            {
                

                for (int row = 0; row < BoardSize; row++)
                {
                    for (int col = 0; col < BoardSize; col++)
                    {
                        Button button = new Button();
                        button.Width = 40;
                        button.Height = 40;
                        button.Font = new Font("Arial", 20);
                        button.AutoSize = false;
                        button.BackColor= Color.LightBlue;
                        button.Click += new EventHandler(Button_Click);
                        boards[boardNum].Controls.Add(button, col, row);
                        buttons[boardNum,row, col] = button;
                    }
                }
               // boards[boardNum].CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            }
           
        }

        // Handle button clicks on the Tic Tac Toe board
        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            
            
            TableLayoutPanel panel = (TableLayoutPanel)button.Parent;
            int panelNum = Array.IndexOf(boards, panel);//getting the row,col,which panel the button is on
            int row = table1.GetRow(button);
            int col = table1.GetColumn(button);
           
            if (xTurn) xOro = "X";
            else xOro = "O";
            if (button.Text == "")//checking if the turn can be played
            {

                button.Text = xOro;
                game.MakeMove(panelNum, row, col, xTurn);

            }
            else return;
            int tempRow = panelNum / BoardSize;
            int tempCol = panelNum % BoardSize;

            if (game.CheckWin(panelNum, xTurn) == 1)//if 0 or X won this board
            {
               
                game.MakeMove(-1, tempRow, tempCol, xTurn);
                if (game.CheckWin(-1, xTurn)==1)
                {
                    EndGame(true);
                }
                else if(game.CheckWin(-1, xTurn) == 2)
                {
                    EndGame(false);
                }
                BoardWon(panelNum,true);
            }
            else if(game.CheckWin(panelNum, xTurn) == 2)
            {
                game.MakeMove(-2,tempRow, tempCol, xTurn);
                BoardWon(panelNum,false);
            }
            DisableBoards(row, col);
            xTurn = !xTurn;




        }
        public void DisableBoards(int row,int col) //this function disables the buttons based on the move that was made
        {
            int panelNum = row*BoardSize + col;//this gets me the correct index in the array of panels
            if(game.CheckWin(panelNum,true)==1|| game.CheckWin(panelNum, false) == 1|| game.CheckWin(panelNum, true) == 2 || game.CheckWin(panelNum, false) == 2)
            {
                for (int bNum = 0; bNum < numOfBoards; bNum++)
                {
                    if (!(game.CheckWin(bNum, true) == 1 || game.CheckWin(bNum, false) == 1|| game.CheckWin(bNum, true) == 2 || game.CheckWin(bNum, false) == 2))
                    {
                        for (int iRow = 0; iRow < BoardSize; iRow++)
                        {
                            for (int iCol = 0; iCol < BoardSize; iCol++)
                            {
                                buttons[bNum, iRow, iCol].Enabled = true;
                                buttons[bNum, iRow, iCol].BackColor = Color.LightBlue;
                            }
                        }
                    }
                
                }
            }
            else
            {
                for (int bNum = 0; bNum < numOfBoards; bNum++)
                {
                    if (bNum != panelNum)
                    {
                        for (int iRow = 0; iRow < BoardSize; iRow++)
                        {
                            for (int iCol = 0; iCol < BoardSize; iCol++)
                            {
                                buttons[bNum, iRow, iCol].Enabled = false;
                                buttons[bNum, iRow, iCol].BackColor = Color.White;
                            }
                        }
                    }
                    else
                    {
                        for (int iRow = 0; iRow < BoardSize; iRow++)
                        {
                            for (int iCol = 0; iCol < BoardSize; iCol++)
                            {
                                buttons[bNum, iRow, iCol].Enabled = true;
                                buttons[bNum, iRow, iCol].BackColor = Color.LightBlue;
                            }
                        }
                    }
                }
            }
           
        }
        public void BoardWon(int boardNum,bool didWin)
        {
            
            Label overlay = new Label();
            if (didWin) overlay.Text = xOro;
            else overlay.Text = "-";
            overlay.Font = new Font(overlay.Font.FontFamily, 80);
            overlay.Width = boards[boardNum].Width;
            overlay.Height = boards[boardNum].Height;
            overlay.Anchor = AnchorStyles.None;
            overlay.TextAlign = ContentAlignment.MiddleCenter;
            overlay.Dock= DockStyle.Fill;
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    boards[boardNum].Controls.Remove(buttons[boardNum, i, j]);
                }
            }
            boards[boardNum].RowCount = 1;
            boards[boardNum].ColumnCount= 1;
            boards[boardNum].AutoSize = true;
            boards[boardNum].Controls.Add(overlay,0,0); 
        }
        public void EndGame(bool isWin)
        {
            if(isWin) {
                MessageBox.Show("Game Over! " + xOro + " Won!!!!!");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Game Over! Draw!!!!" );
                Application.Exit();
            }
            
        }

        
    }
}