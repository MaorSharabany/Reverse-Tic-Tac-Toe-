using System;
using System.Collections.Generic;
using System.Text;

namespace RiverseTTTGsme
{
    public class Game
    {
        private char[,] m_Board;
        private byte m_FilledCells;
        private byte m_BoardSize;
        private bool m_IsMultiplayer;

        /* Constructor */
        public Game(byte i_BoardSize, bool i_IsMultiplayer)
        {
            m_BoardSize = i_BoardSize;
            m_IsMultiplayer = i_IsMultiplayer;
            m_FilledCells = 0;
            boardInitializer(); 
        }

        /* Intializes all of the board's cells with the 'space' character. */
        private void boardInitializer()
        {
            m_Board = new char[m_BoardSize, m_BoardSize];

            for(int i=0; i< m_BoardSize; i++)
            {
                for(int j=0; j<m_BoardSize; j++)
                {
                    m_Board[i, j] = ' ';
                }
            }
        }

        /* Plays a move by a player request. If the game mode is not Multiplayer, preforms a computer move too.
         * Returns result values as the following:
         * -1 - invalid cell values
         * 0 - game is not over, keep playing
         * 1 - player 1 lose
         * 2 - player 2 lose
         * 3 - tie
         * 4 - quit
         */
        public int PlayerMove(Cell i_userChoice)
        {
            int result = 0;

            if(i_userChoice.row == 254)
            {
                // the user has quit the game
                result = 4;
            }
            else
            {
                // play the user's move
                result = makeMove(i_userChoice);
            }

            // if the game mode is not multiplayer, play a computer move
            if(result == 0 && !m_IsMultiplayer)
            {
                Cell randomFreeCell = pickRandomFreeCell();
                if (randomFreeCell.row != 255)
                {
                    result = makeMove(randomFreeCell);
                }
            }

            return result;
        }

        /* Plays a move by the input cell's values.
         * Returns result values as the following:
         * -1 - invalid cell values
         * 0 - game is not over, keep playing
         * 1 - player 1 lose
         * 2 - player 2 lose
         * 3 - tie
         */
        private int makeMove(Cell i_Cell)
        {
            bool validCellIndexes = i_Cell.row < m_BoardSize && i_Cell.column < m_BoardSize;
            int result = 0;

            if (!validCellIndexes)      //invalid cell
            {
                result = -1;
            }
            else if(isBoardFull())      //tie
            {
                result = 3;
            }
            else                        //play
            {
                updateCell(i_Cell);

                if (didLose(i_Cell))    //player lose
                {
                    result = PlayedLast;
                }
                else if (isBoardFull()) //tie
                {
                    result = 3;
                }
            }

            return result;
        }

        /* Updates a cell value in the board matrix and increments 'FilledCells' value by 1. */
        private void updateCell(Cell i_Cell)
        {
            m_Board[i_Cell.row, i_Cell.column] = currentPlayerSign();
            m_FilledCells++;
        }

        /* Returns true if the input cell is free, and false otherwise */
        public bool IsEmptyCell(Cell i_Cell)
        {
            return m_Board[i_Cell.row, i_Cell.column] == ' ';
        }

        /* Returns true if the last played player had lost the game, and false otherwise.
         * Losing is where there is a complete sequence of the player's sign on the board. */
        private bool didLose(Cell i_Cell)
        {
            // gets the sign of the last player in order to search for a sequence
            char lastPlayerSign = this.lastPlayerSign();

            bool isOnMainDiagonal = i_Cell.row == i_Cell.column;
            bool isOnAntiDiagonal = i_Cell.row == m_BoardSize - i_Cell.column - 1;

            // initializes all of the sequences queries to 'true'
            bool columnSequence = true;
            bool rowSequence = true;
            bool mainDiagonalSequence = true;
            bool antiDiagonalSequence = true;

            // searches the board for sequence BRAKERS
            for(int i=0; i<m_BoardSize; i++)
            {

                if(m_Board[i, i_Cell.column] != lastPlayerSign)    
                {
                    //a column sequence braker was found
                    columnSequence = false;
                }
                if(m_Board[i_Cell.row, i] != lastPlayerSign)       
                {
                    //a row sequence braker was found
                    rowSequence = false;
                }
                if(!isOnMainDiagonal || (isOnMainDiagonal && m_Board[i, i] != lastPlayerSign))    
                {
                    /* either the cell is not on the main diagonal
                     * or a main diagonal sequence braker was found */
                    mainDiagonalSequence = false;
                }
                if(!isOnAntiDiagonal || (isOnAntiDiagonal && m_Board[i, m_BoardSize - i - 1] != lastPlayerSign))
                {
                    /* either the cell is not on the anti-diagonal
                     * or an anti-diagonal sequence braker was found */
                    antiDiagonalSequence = false;
                }
            }

            return columnSequence || rowSequence || mainDiagonalSequence || antiDiagonalSequence;
        }

        /* Returns true if the board is full, and false otherwise. */
        private bool isBoardFull()
        {
            return Math.Pow(m_BoardSize, 2) == m_FilledCells;
        }

        /* Picks a random cell that is free. 
         Returns the picked cell if, or a cell with the values (255,255) in case of failure. */
        private Cell pickRandomFreeCell()
        {
            int numOfFreeCells = (byte)(Math.Pow(m_BoardSize, 2) - m_FilledCells);

            // randomize an integer between (0 - num of free cells)
            Random random = new Random();
            byte randomCellOrder = (byte)random.Next(0, numOfFreeCells);

            // adjust the random integer to cell's values (by order)
            for(byte i=0; i<m_BoardSize; i++)
            {
                for(byte j=0; j<m_BoardSize; j++)
                {
                    Cell randomCell = new Cell(i, j);

                    if(IsEmptyCell(randomCell) && randomCellOrder == 0)
                    {
                        return randomCell;
                    }
                    else if(IsEmptyCell(randomCell))
                    {
                        randomCellOrder--;
                    }
                }
            }

            return new Cell(255, 255);
        }

        /* Board's matrix getter */
        public char[,] Board
        {
            get
            {
                return m_Board;
            }
        }

        /* Board size value getter */
        public byte BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }

        /* Played last getter.
         * Returns 1 if player 1 had played last, and 2 if player 2 played last. */
        public byte PlayedLast
        {
            get
            {
                byte playerNum = 2;

                if(m_FilledCells == 0)
                {
                    playerNum = 0;
                }

                else if (m_FilledCells % 2 == 1)
                {
                    playerNum = 1;
                }

                return playerNum;
            }
        }

        /* Currently playing getter.
         * Returns 1 if it's the 1st player's turn, 
         * and 2 if it's the 2nd player's turn. */
        public byte CurrentPlaying
        {
            get
            {
                byte playerNum = 1;

                if (m_FilledCells % 2 == 1)
                {
                    playerNum = 2;
                }

                return playerNum;
            }
        }

        /* Returns the sign (X/O) of the player that had played the last move. */
        private char lastPlayerSign()
        {
            char sign;

            if (this.PlayedLast == 2)
            {
                sign = 'O';
            }
            else
            {
                sign = 'X';
            }

            return sign;
        }

        /* Returns the sign (X/O) of the player that is about to play the next move. */
        private char currentPlayerSign()
        {
            char sign;

            if (this.CurrentPlaying == 1)
            {
                sign = 'X';
            }
            else
            {
                sign = 'O';
            }

            return sign;
        }
    }
}
