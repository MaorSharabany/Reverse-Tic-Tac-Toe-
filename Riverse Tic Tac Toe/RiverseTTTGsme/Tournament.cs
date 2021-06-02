using System;
using System.Collections.Generic;
using System.Text;

namespace RiverseTTTGsme
{
    public class Tournament
    {
        private byte m_Player1Score;
        private byte m_Player2Score;
        private bool m_IsMultiplayer;
        private byte m_BoardSize;
        private Game m_Game;
        private GameUI m_GameUI;

        /* Constructor.
         * Init primitives, get user's size and mode inputs
         * and run the first game. */
        public Tournament()
        {
            m_Player1Score = 0;
            m_Player2Score = 0;

            m_BoardSize = GameUI.InsertBoardSizeMsg();
            m_IsMultiplayer = GameUI.IsMultiplayerMsg();

            gameRun();
        }

        /* Game runner - Init Game and GameUI and iteratively runs new round
         * until a game is ended. */
        private void gameRun()
        {
            m_Game = new Game(m_BoardSize, m_IsMultiplayer);
            m_GameUI = new GameUI(m_Game);

            int roundResult = 0;
            Cell usersChoice = new Cell(255,255);

            while(roundResult == 0)
            {
                usersChoice = m_GameUI.InsertNextPlayerMoveMsg();
                roundResult = m_Game.PlayerMove(usersChoice);
                m_GameUI.PrintBoard(m_Game.Board);
            }

            gameEnd(roundResult);
        }

        /* End a game - Prints the result, asks  */
        private void gameEnd(int i_GameResult)
        {
            if(i_GameResult == 4)
            {
                m_GameUI.QuitMsg();
            }
            else if (i_GameResult == 3)
            {
                m_GameUI.TieGameMsg();
            }
            else if (i_GameResult == 2)
            {
                m_Player1Score++;
                m_GameUI.PlayerWinMsg(1);
            }
            else if (i_GameResult == 1)
            {
                m_Player2Score++;
                m_GameUI.PlayerWinMsg(2);
            }

            m_GameUI.PointStatusMsg(m_Player1Score, m_Player2Score);

            if (m_GameUI.PlayAgainMsg())
            {
                gameRun();
            }
        }
    }
}