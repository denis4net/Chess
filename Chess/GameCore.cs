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

		public GameCore ()
		{
            Application.EnableVisualStyles();
            runColor = FigureColor.WHITE;
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
            matrix = new CoreMatrix();
            playWindow = new PlayWindow(this, "Chess", new GuiMatrix(matrix));
            playWindow.FormClosed += new FormClosedEventHandler(PlayWindowClose);

            inviteWindow = new InviteWindow();
            inviteWindow.FormClosed += new FormClosedEventHandler(InviteWindowClose);
            inviteWindow.Show();
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
			if (matrix.HasFigureAt (spotPos)) {
				Figure figure = matrix.FigureAt (spotPos);

				if (figure.Color == runColor) {
					figure.ConsolePrintPosition (spotPos);

					playWindow.matrix.SetHighlighted (figure.GetAvailableMovePossitons (spotPos));
					figurePos = spotPos;
				}
			}

			if (playWindow.matrix.GetSpot (spotPos).Highlighted && figurePos != null ) {
				Console.WriteLine( "Moving figure from: " +figurePos.X +' ' +figurePos.Y
				                  + " to " + spotPos.X +' ' +spotPos.Y);
				matrix.MoveFigure( figurePos, spotPos);
				playWindow.matrix.MoveImage( figurePos, spotPos );
			}
		}
        public bool SpotFocused(Position spotPos)
        {
				//зачем тут ловить ошибки, я без понятия
				
                if (matrix.HasFigureAt(spotPos) && matrix.FigureAt(spotPos).Color == runColor) 
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
