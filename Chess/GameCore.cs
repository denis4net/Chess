﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Chess;
using Chess.Figures;

namespace Chess
{

    class GameCore: IGameControl
    {
		private PlayWindow playWindow;
        private InviteWindow inviteWindow;
        private CoreMatrix matrix;
		private Chess.Figures.FigureColor runColor;
		private Position figurePos;
		private PlayersState pState;

		public GameCore ()
		{
            Application.EnableVisualStyles();
            runColor = FigureColor.WHITE;
			pState = new PlayersState();
			pState.Black = PlayerState.NORMAL;
			pState.White = PlayerState.NORMAL;
		}

        public void Initialize()
        {
            matrix = new CoreMatrix();
            playWindow = new PlayWindow(this, "Chess", new GuiMatrix(matrix));
            playWindow.FormClosed += new FormClosedEventHandler(PlayWindowClose);
            inviteWindow = new InviteWindow();
            inviteWindow.FormClosed += new FormClosedEventHandler(InviteWindowClose);
            inviteWindow.Show();
            Application.Run();
        }

        private void ReInitialize()
        {
            runColor = FigureColor.WHITE;
            matrix = new CoreMatrix();
            playWindow = new PlayWindow(this, "Chess", new GuiMatrix(matrix));
            playWindow.FormClosed += new FormClosedEventHandler(PlayWindowClose);

            inviteWindow = new InviteWindow();
            inviteWindow.FormClosed += new FormClosedEventHandler(InviteWindowClose);
            inviteWindow.Show();
        }

		private void CheckForMate ()
		{
				for (int i=0; i<8; i++) { 		//y
					for (int j=0; j<8; j++) { 	//x
					Position currentPos = new Position(j, i);

					if( matrix.HasFigureAt(currentPos) ) {
						Figure currentFigure = matrix.FigureAt(currentPos);
						FigureColor currentColor = currentFigure.Color;
						FigureColor enemyColor =(currentColor == FigureColor.WHITE)?(FigureColor.BLACK):(FigureColor.WHITE);

						Position kingPos = matrix.GetKing (enemyColor);
						Console.WriteLine ("King position is " + kingPos.X +' ' +kingPos.Y);  
						List<Position> atack = currentFigure.GetAvailableAtackPositons(currentPos, matrix);

						if( atack.Contains(kingPos) ) {
							string message;

							Console.Write("Warning:");
							if(runColor == enemyColor )	{
								message = "Shah";
							}
							else
							{
								message = "Checkmate";
							}

							Console.WriteLine(message);
						}
					}
					}
				}
		}

        private void PlayWindowClose(Object o, FormClosedEventArgs e)
        {
            ReInitialize();
        }
        
		private void InviteWindowClose(Object o, FormClosedEventArgs e)
        {
            switch (inviteWindow.returnValue)
            {
                case "Offline":
                {
                    playWindow.Show();
                } break;
                case "Online":
                {
                    Application.Exit();
                } break;
                default:
                {
                    Application.Exit();
                } break;
            }
        }

		private void StartGame ()
		{
            
		}

		//IGameControl
		public void FigureMoved(Position oldPos, Position newPos)
		{

		}

		public void SpotSelected (Position spotPos)
		{
			Console.WriteLine("Square selected: " + spotPos.X +' ' + spotPos.Y);


			//select a figure and highlig
			if (matrix.HasFigureAt (spotPos)) {
				Figure figure = matrix.FigureAt (spotPos);

				if (figure.Color == runColor) {
					figure.ConsolePrintPosition (spotPos);

                    if(playWindow.matrix.SetSelected(spotPos))
                        playWindow.matrix.SetHighlighted(figure.GetAvailableAtackPositons(spotPos, matrix));
					figurePos = spotPos;
				}
			}

			//moving figure to new position
			if (playWindow.matrix.GetSpot (spotPos).Highlighted && figurePos != null ) {
                Console.WriteLine("Moving figure from: " + figurePos.X + ' ' + figurePos.Y + " to " + spotPos.X + ' ' + spotPos.Y);

                if (runColor == FigureColor.WHITE)
                {
                    playWindow.PrintToConsole("System: ", System.Drawing.Color.Red);
                    playWindow.PrintToConsoleLn("Player1 moved " + matrix.FigureAt(figurePos).ToString() + " from " +
                    (char)('A' + figurePos.X) + Convert.ToString(8 - figurePos.Y) + " to " +
                    (char)('A' + spotPos.X) + Convert.ToString(8 - spotPos.Y), System.Drawing.Color.FromArgb(64, 128, 255));
                }
                else
                {
                    playWindow.PrintToConsole("System: ", System.Drawing.Color.Red);
                    playWindow.PrintToConsoleLn("Player2 moved " + matrix.FigureAt(figurePos).ToString() + " from " +
                    (char)('A' + figurePos.X) + Convert.ToString(8 - figurePos.Y) + " to " +
                    (char)('A' + spotPos.X) + Convert.ToString(8 - spotPos.Y), System.Drawing.Color.FromArgb(128, 64, 255));
                }

				matrix.MoveFigure( figurePos, spotPos);
                playWindow.matrix.MoveImage( figurePos, spotPos );
                playWindow.matrix.ResetAllAttribures();

				runColor = (runColor == FigureColor.WHITE)?(FigureColor.BLACK):(FigureColor.WHITE);
				CheckForMate();
				FigureMoved(figurePos, spotPos);
			}
		}
        
		public bool SpotFocused(Position spotPos)
        {				
                if ( (matrix.HasFigureAt(spotPos) && matrix.FigureAt(spotPos).Color == runColor)
			    || playWindow.matrix.GetSpot(spotPos).Highlighted ) 

					return true;
                else 
					return false;
        }

		public void StartButtonClicked()
		{

		}

		public void StopButtonClicked()
		{

		}
    }
}
