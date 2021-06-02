using System;
using System.Collections.Generic;
using System.Text;

namespace RiverseTTTGsme
{
    public class GameUI
    {
        private Game m_Game;

        public GameUI(Game i_Game)
        {
            m_Game = i_Game;
            PrintBoard(m_Game.Board);
        }


        /*Validates the initial input of board size from the user
        Returns the selected input size */ 
        public static byte InsertBoardSizeMsg()
        {
            string boardSize = "";
            bool validInput = false;
          
            string welcomeMsg = "Welcome to `Reverse Tic-Tac-Toe` Game for Console!";
            string insertBoardSizeMsg = "Please insert board size number between 3 to 9";
            string invalidInputMsg = "Invalid input board size. Please insert number between 3 to 9";

            Console.WriteLine(welcomeMsg);
            Console.WriteLine(insertBoardSizeMsg);

            while (!validInput)
            {
                boardSize = Console.ReadLine();
                if(boardSize.Length != 1)
                {
                    Console.WriteLine(invalidInputMsg);
                }

                else if(boardSize.Length == 1)
                {
                     if(byte.Parse(boardSize) < 3 || byte.Parse(boardSize) > 9)
                    {
                        Console.WriteLine(invalidInputMsg);
                    }

                    else
                    {
                        validInput = true;
                        break;
                    }
                }
            }

            return byte.Parse(boardSize);
        }


        /*Set the Game Mode*/ 
        public static bool IsMultiplayerMsg()
        {
            string userChoice = "";
            bool validInput = false;
            bool isMultiplayer = false;
           
            string selectGameModeMsg = "Please select if you would like to play in `Multiplayer Mode` (Y/N)";
            string invalidInputMsg = "Invalid input Game Mode choice. Please insert Y/N";

            Console.WriteLine(selectGameModeMsg);

            while (!validInput)
            {
                userChoice = Console.ReadLine();
                if(userChoice.Length != 1)
                {
                    Console.WriteLine(invalidInputMsg);
                }

                else if(userChoice.Length == 1)
                {
                    if(userChoice.ToLower() == "y")
                    {
                        isMultiplayer = true;
                        validInput = true;
                        break;
                    }

                    else if(userChoice.ToLower() == "n")
                    {
                        validInput = true;
                        break;
                    }

                    else
                    {
                        Console.WriteLine(invalidInputMsg);
                    }
                }
            }

            return isMultiplayer;
        }


        /*Prints the Winner username*/ 
        public void PlayerWinMsg(byte i_PlayerNumber)
        {
            Console.WriteLine(String.Format("Congratulations Player {0} Won the game!", i_PlayerNumber));
        }


        /*Tie Game Message*/
        public void TieGameMsg()
        {
            Console.WriteLine(String.Format("You got a Tie Game!"));
        }


        /*Game points status message*/
        public void PointStatusMsg(byte i_Player1Score, byte i_Player2Score)
        {
            Console.WriteLine("Game score status:");
            Console.WriteLine(string.Format("Player1 Score: {0}, Player2 Score: {1}", i_Player1Score, i_Player2Score));
        }


        /*Checks if the user would like to play another round*/
        public bool PlayAgainMsg()
        {
            bool isValidInput = false;
            bool isPlayAgain = false;
            string userInput = "";

            string isPlayAgainMsg = "The game is over. \n Would you like to play another round? (Y/N)";
            string invalidMsg = "Invalid choice. Would you like to play another round ? (insert Y/N)";

            Console.WriteLine(isPlayAgainMsg);

            while (!isValidInput)
            {
                userInput = Console.ReadLine();

                if (userInput.Length != 1)
                {
                    Console.WriteLine(invalidMsg);
                }

                else if (userInput.Length == 1)
                {
                    if (userInput.ToLower() == "y")
                    {
                        isPlayAgain = true;
                        isValidInput = true;
                        break;
                    }

                    else if (userInput.ToLower() == "n")
                    {
                        isValidInput = true;
                        break;
                    }

                    else
                    {
                        Console.WriteLine(invalidMsg);
                    }
                }
            }

            if(isPlayAgain)
            {
                Ex02.ConsoleUtils.Screen.Clear();
            }

            return isPlayAgain;
        }


        /*Prints a message that the desired cell is already in used*/
        public void CellIsUsedMsg(Cell i_Cell)
        {
            Console.WriteLine(string.Format("The cell ({0}, {1}) has already filled. \nPlease select another cell!", i_Cell.row, i_Cell.column));
        }


        /*Gets the desired input cell from the player
        Validates the input and return the new cell*/
        public Cell InsertNextPlayerMoveMsg()
        {
            bool isValidInput = false;
            bool isPlayerQuit = false;
            string userInputCell = "";
            int playerNumber = m_Game.CurrentPlaying;

            string[] inputAfterSplit = new string[2];
            byte x, y;
            Cell cellToReturn = new Cell(254, 254); //Initialize cell value to 'quit'

            string insertValueMsg = "Player {0}, insert desired cell values in range 1 : {1} (format: number space number. Example: `1 3`)";
            string invalidMsg = "Invalid input. Insert desired cell values in range 1 : {0} format: `number space number`. Example: `1 3`";

            Console.WriteLine(string.Format(insertValueMsg, playerNumber, m_Game.BoardSize));

            while (!isValidInput)
            {
                userInputCell = Console.ReadLine();

                //Checks if the user cheese to quit
                isPlayerQuit = isQuit(userInputCell);
                if (isPlayerQuit)
                {
                    isValidInput = true;
                    break;
                }
              
                if(userInputCell.Length != 3)
                {
                    Console.WriteLine(string.Format(invalidMsg, m_Game.BoardSize));
                }

                else if(userInputCell.Length == 3)
                {
                    if (userInputCell[1] != ' ')
                    {
                        Console.WriteLine(string.Format(invalidMsg, m_Game.BoardSize));
                    }

                    else
                    {
                        inputAfterSplit = userInputCell.Split(" ");

                        bool parseSuccessX = false;
                        bool parseSuccessY = false;

                        parseSuccessX = Byte.TryParse(inputAfterSplit[0], out x);
                        parseSuccessY = Byte.TryParse(inputAfterSplit[1], out y);

                        if(!parseSuccessX || !parseSuccessY)
                        {
                            Console.WriteLine(string.Format(invalidMsg, m_Game.BoardSize));
                        }

                        else if (x < 0 || x > m_Game.BoardSize)
                        {
                            Console.WriteLine(string.Format(invalidMsg, m_Game.BoardSize));
                        }

                        else if (y < 0 || y > m_Game.BoardSize)
                        {
                            Console.WriteLine(string.Format(invalidMsg, m_Game.BoardSize));
                        }

                        else
                        {
                            cellToReturn = new Cell(x, y);

                            if (m_Game.IsEmptyCell(new Cell((byte)(cellToReturn.row - 1), (byte)(cellToReturn.column - 1))))
                            {
                                isValidInput = true;
                                break;
                            }

                            else
                            {
                                CellIsUsedMsg(cellToReturn);
                                continue;
                            }
                        }
                    }
                }
            }

            Ex02.ConsoleUtils.Screen.Clear();

            /*If the user cheese to quit - return (254, 254)
            * else, return the user choice */
            if (!isPlayerQuit)
            {
                cellToReturn = new Cell((byte)(cellToReturn.row - 1), (byte)(cellToReturn.column - 1));
            }
            
            return cellToReturn;
        }


        //Checks if the user chose to quit.
        private bool isQuit(string i_QuitChar)
        {
            bool isPlayerQuit = false;

            if (i_QuitChar == "Q" || i_QuitChar == "q")
            {
                Ex02.ConsoleUtils.Screen.Clear();
                isPlayerQuit = true;
            }

            return isPlayerQuit;  
        }

        
        //Quit game message
        public void QuitMsg()
        {
            string quitStr = "Player {0} Quit the game.";
            Console.WriteLine(string.Format(quitStr, m_Game.CurrentPlaying));
        }


        /*Prints the Board*/
        public void PrintBoard(char[,] m_GameBord)
        {
            byte drowingBoardSize = (byte)(m_Game.BoardSize + 1);
            char[,] drowingBoard = new char[drowingBoardSize, drowingBoardSize];
            int countCol = 1;
            int countRow = 1;

            //Initialize drawing board
            for (int row = 0; row < drowingBoardSize; row++)
            {
                for (int col = 0; col < drowingBoardSize; col++)
                {
                    drowingBoard[row, col] = ' ';
                }
            }

            //Printing drawing board
            for (int row = 0; row < drowingBoardSize; row++)
            {
                if (row > 0)
                {
                    Console.Write(countRow + " | ");
                    countRow++;
                }

                for (int col = 0; col < drowingBoardSize; col++)
                {
                    if (row == 0 && col > 0)
                    {
                        Console.Write("   " + countCol + "    ");
                        countCol++;
                    }

                    else if (row > 0 && col == 0)
                    {
                        Console.Write("  ");
                    }

                    else if (row > 0)
                    {

                        Console.Write(m_Game.Board[row-1, col-1]);
                        Console.Write("   |   ");
                    }
                }

                if (row == 0)
                {
                    Console.WriteLine();
                }

                else
                {
                    Console.WriteLine("\n");
                    Console.Write("  ");

                    for (int i = 0; i < drowingBoardSize; i++)
                    {
                        Console.Write("=======");
                    }

                    Console.WriteLine("\n");
                }
            }
        }     
    }
}
