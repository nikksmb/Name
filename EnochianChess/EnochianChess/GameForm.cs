using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnochianChess
{
    public partial class GameForm : Form
    {

        static int arrangement = 0;

        //This is convenient for resize
        static Label[] labelGroup;
        static Button[] buttonGroup;
        static Bitmap buttonBackground;

        Game game;
        static ChessmenInfo chessmenInfo;
       // Chessboard chessboard;

        public struct ChessmenParameters
        {
            public string ruNameNominative;
            public string engName;
            public string ruNameAccusative;
            public string ruMove;
            public string ruAttack;
            public Bitmap chessmanBitmap;
            public int horizontal;
            public int vertical;
            public Players player;
            public ChessmenNames kindOfChessman;
        }

        public struct InterfaceElements
        {
            public PictureBox chessboardPic;
            public PictureBox takenChessmenPic;
            public PictureBox lostChessmenPic;
            public Label statusBar;
            public Label turnBar;
            public RichTextBox logBar;
            public Button surrenderButton;
            public Button prisonersChangeButton;
        }

        public enum Players { Yellow = 0, Red = 1, Blue = 2, Black = 3 };
        public enum ChessmenNames
        {
            King = 0, Queen = 1, Bishop = 2, Knight = 3, Tower = 4, PawnOfQueen = 5, PawnOfBishop = 6,
            PawnOfKnight = 7, PawnOfTower = 8
        };

        //CHESSMEN INFO
        private class ChessmenInfo
        {
            /* Numbers of chessmen are: 0 - King, 1 - Queen, 2 - Bishop, 3 - Knight, 4 - Tower,
             * 5 - Pawn of Queen, 6 - Pawn of Bishop, 7 - Pawn of Knight, 8 - Pawn of Tower.
             * Numbers of players are: 0 - Yellow, 1 - Red, 2 - Blue, 3 - Black.
             * Numbers of arrangements shown near.*/

            //Bitmaps of each chessman
            //    private Bitmap[,] chessmanBitmap {get;}

            private Bitmap[,] chessmanBitmap = new Bitmap[4, 9] { 

                //Yellow player
                {Properties.Resources.YellowKing, Properties.Resources.YellowQueen,
                Properties.Resources.YellowBishop, Properties.Resources.YellowKnight, Properties.Resources.YellowTower,
                Properties.Resources.YellowPawnQueen, Properties.Resources.YellowPawnBishop,Properties.Resources.YellowPawnKnight,
                Properties.Resources.YellowPawnTower},

                //Red player
                {Properties.Resources.RedKing, Properties.Resources.RedQueen,
                Properties.Resources.RedBishop, Properties.Resources.RedKnight, Properties.Resources.RedTower,
                Properties.Resources.RedPawnQueen, Properties.Resources.RedPawnBishop,Properties.Resources.RedPawnKnight,
                Properties.Resources.RedPawnTower},

                //Blue player
                {Properties.Resources.BlueKing, Properties.Resources.BlueQueen,
                Properties.Resources.BlueBishop, Properties.Resources.BlueKnight, Properties.Resources.BlueTower,
                Properties.Resources.BluePawnQueen, Properties.Resources.BluePawnBishop,Properties.Resources.BluePawnKnight,
                Properties.Resources.BluePawnTower},

                //Black player
                {Properties.Resources.BlackKing, Properties.Resources.BlackQueen,
                Properties.Resources.BlackBishop, Properties.Resources.BlackKnight, Properties.Resources.BlackTower,
                Properties.Resources.BlackPawnQueen, Properties.Resources.BlackPawnBishop,Properties.Resources.BlackPawnKnight,
                Properties.Resources.BlackPawnTower}
        
            };

            /* Numbers of chessmen are: 0 - King, 1 - Queen, 2 - Bishop, 3 - Knight, 4 - Tower,
             * 5 - Pawn of Queen, 6 - Pawn of Bishop, 7 - Pawn of Knight, 8 - Pawn of Tower.
             * Numbers of players are: 0 - Yellow, 1 - Red, 2 - Blue, 3 - Black.*/


            //russian language with different cases
            private string[] ruNamePlayerColorNominativeMasculine = new string[4] {
                "Жёлтый", "Красный", "Синий", "Чёрный"
            };

            private string[] ruNamePlayerColorNominativeFeminine = new string[4] {
                "Жёлтая", "Красная", "Синяя", "Чёрная"
            };

            private string[] ruNamePlayerColorAccusativeMasculine = new string[4] {
                "Жёлтого", "Красного", "Синего", "Чёрного"
            };

            private string[] ruNamePlayerColorAccusativeFeminine = new string[4] {
                "Жёлтую", "Красную", "Синюю", "Чёрную"
            };

            private string[] ruNameChessmanNominative = new string[9] {
                "Король", "Королева", "Слон", "Конь", "Ладья", 
                "Пешка королевы", "Пешка слона", "Пешка коня", "Пешка ладьи"
            };

            private string[] ruNameChessmanAccusative = new string[9] {
                "Короля", "Королеву", "Слона", "Коня", "Ладью", 
                "Пешку королевы", "Пешку слона", "Пешку коня", "Пешку ладьи"
            };

            //Positions of any chessman, any player, any arrangement
            //      private int[, , ,] positionOfChessmen { get; }

            private int[, , ,] positionOfChessmen = new int[8, 4, 9, 2] 
            {
                /* 1. Air of Fire and of Earth*/
                {
                    //Yellow player
                {{1,0},{1,4},{1,1},{1,3},{1,2},{2,4},{2,1},{2,3},{2,2}},
                    //Red player
                {{8,9},{8,5},{8,8},{8,6},{8,7},{7,5},{7,8},{7,6},{7,7}},
                    //Blue player
                {{0,8},{4,8},{1,8},{3,8},{2,8},{4,7},{1,7},{3,7},{2,7}},
                    //Black player
                {{9,1},{5,1},{8,1},{6,1},{7,1},{5,2},{8,2},{6,2},{7,2}}
                },

                /* 2. Air of Air and of Water*/
                {
                    //Yellow player
                {{1,0},{1,2},{1,1},{1,3},{1,4},{2,2},{2,1},{2,3},{2,4}},
                    //Red player
                {{8,9},{8,7},{8,8},{8,6},{8,5},{7,7},{7,8},{7,6},{7,5}},
                    //Blue player
                {{0,8},{2,8},{1,8},{3,8},{4,8},{2,7},{1,7},{3,7},{4,7}},
                    //Black player
                {{9,1},{7,1},{8,1},{6,1},{5,1},{7,2},{8,2},{6,2},{5,2}}
                },

                /* 3. Fire of Air and of Water*/
                {
                    //Yellow player
                {{1,0},{1,4},{1,3},{1,1},{1,2},{2,4},{2,3},{2,1},{2,2}},
                    //Red player
                {{8,9},{8,5},{8,6},{8,8},{8,7},{7,5},{7,6},{7,8},{7,7}},
                    //Blue player
                {{0,8},{4,8},{3,8},{1,8},{2,8},{4,7},{3,7},{1,7},{2,7}},
                    //Black player
                {{9,1},{5,1},{6,1},{8,1},{7,1},{5,2},{6,2},{8,2},{7,2}}
                },
        
                /* 4. Water of Air and of Water*/
                {
                    //Yellow player
                {{1,0},{1,1},{1,2},{1,4},{1,3},{2,1},{2,2},{2,4},{2,3}},
                    //Red player
                {{8,9},{8,8},{8,7},{8,5},{8,6},{7,8},{7,7},{7,5},{7,6}},
                    //Blue player
                {{0,8},{1,8},{2,8},{4,8},{3,8},{1,7},{2,7},{4,7},{3,7}},
                    //Black player
                {{9,1},{8,1},{7,1},{5,1},{6,1},{8,2},{7,2},{5,2},{6,2}}
                },

                /* 5. Earth of Fire and of Earth*/
                {
                    //Yellow player
                {{1,0},{1,3},{1,2},{1,4},{1,1},{2,3},{2,2},{2,4},{2,1}},
                    //Red player
                {{8,9},{8,6},{8,7},{8,5},{8,8},{7,6},{7,7},{7,5},{7,8}},
                    //Blue player
                {{0,8},{3,8},{2,8},{4,8},{1,8},{3,7},{2,7},{4,7},{1,7}},
                    //Black player
                {{9,1},{6,1},{7,1},{5,1},{8,1},{6,2},{7,2},{5,2},{8,2}}
                },

                /* 6. Earth of Air and of Water*/
                {
                    //Yellow player
                {{1,0},{1,3},{1,4},{1,2},{1,1},{2,3},{2,4},{2,2},{2,1}},
                    //Red player
                {{8,9},{8,6},{8,5},{8,7},{8,8},{7,6},{7,5},{7,7},{7,8}},
                    //Blue player
                {{0,8},{3,8},{4,8},{2,8},{1,8},{3,7},{4,7},{2,7},{1,7}},
                    //Black player
                {{9,1},{6,1},{5,1},{7,1},{8,1},{6,2},{5,2},{7,2},{8,2}}
                },

                /* 7. Fire of Fire and of Earth*/
                {
                    //Yellow player
                {{1,0},{1,2},{1,3},{1,1},{1,4},{2,2},{2,3},{2,1},{2,4}},
                    //Red player
                {{8,9},{8,7},{8,6},{8,8},{8,5},{7,7},{7,6},{7,8},{7,5}},
                    //Blue player
                {{0,8},{2,8},{3,8},{1,8},{4,8},{2,7},{3,7},{1,7},{4,7}},
                    //Black player
                {{9,1},{7,1},{6,1},{8,1},{5,1},{7,2},{6,2},{8,2},{5,2}}
                },

                /* 8. Water of Fire and of Earth*/
                {
                    //Yellow player
                {{1,0},{1,1},{1,4},{1,2},{1,3},{2,1},{2,4},{2,2},{2,3}},
                    //Red player
                {{8,9},{8,8},{8,5},{8,7},{8,6},{7,8},{7,5},{7,7},{7,6}},
                    //Blue player
                {{0,8},{1,8},{4,8},{2,8},{3,8},{1,7},{4,7},{2,7},{3,7}},
                    //Black player
                {{9,1},{8,1},{5,1},{7,1},{6,1},{8,2},{5,2},{7,2},{6,2}}
                },
            };

            private int numberOfArrangement;

            public ChessmenInfo(int numberOfArrangement)
            {
                this.numberOfArrangement = numberOfArrangement;
            }

            private int GetGender(int indexOfChessman)
            {
                int gender = -1;
                switch (indexOfChessman)
                {
                    case 0: gender = 0;
                        break;
                    case 1: gender = 1;
                        break;
                    case 2: gender = 0;
                        break;
                    case 3: gender = 0;
                        break;
                    case 4: gender = 1;
                        break;
                    case 5: gender = 1;
                        break;
                    case 6: gender = 1;
                        break;
                    case 7: gender = 1;
                        break;
                    case 8: gender = 1;
                        break;
                    default:
                        {
                            MessageBox.Show("Внутренняя ошибка, невозможно выполнить действие");
                            break;
                        }
                }
                return gender;
            }

            private string GetNominativeRu(int indexOfPlayer, int indexOfChessman)
            {
                int gender = GetGender(indexOfChessman);
                string result = " ";
                if (gender == 0)
                {
                    result = ruNamePlayerColorNominativeMasculine[indexOfPlayer] + ' ' +
                        ruNameChessmanNominative[indexOfChessman];
                }
                if (gender == 1)
                {
                    result = ruNamePlayerColorNominativeFeminine[indexOfPlayer] + ' ' +
                        ruNameChessmanNominative[indexOfChessman];
                }
                return result;
            }

            private string GetAccusativeRu(int indexOfPlayer, int indexOfChessman)
            {
                int gender = GetGender(indexOfChessman);
                string result = " ";
                if (gender == 0)
                {
                    result = ruNamePlayerColorAccusativeMasculine[indexOfPlayer] + ' ' +
                        ruNameChessmanAccusative[indexOfChessman];
                }
                if (gender == 1)
                {
                    result = ruNamePlayerColorAccusativeFeminine[indexOfPlayer] + ' ' +
                        ruNameChessmanAccusative[indexOfChessman];
                }
                return result;
            }

            private string GetMoveRu(int indexOfChessman)
            {
                int gender = GetGender(indexOfChessman);
                string result = " ";
                if (gender == 0)
                {
                    result = "переместился на";
                }
                if (gender == 1)
                {
                    result = "переместилась на";
                }
                return result;
            }

            private string GetAttackRu(int indexOfChessman)
            {
                int gender = GetGender(indexOfChessman);
                string result = " ";
                if (gender == 0)
                {
                    result = "атаковал";
                }
                if (gender == 1)
                {
                    result = "атаковала";
                }
                return result;
            }


            private Chessman GetNewCorrectPawn(ChessmenParameters chessmanParameters, Chessboard chessboard)
            {
                Chessman result;
                switch ((int)chessmanParameters.player)
                {
                    case 0: result = new YellowPawn(chessmanParameters, chessboard);
                        break;
                    case 1: result = new RedPawn(chessmanParameters, chessboard);
                        break;
                    case 2: result = new BluePawn(chessmanParameters, chessboard);
                        break;
                    case 3: result = new BlackPawn(chessmanParameters, chessboard);
                        break;
                    default:
                        {
                            result = null;
                            break;
                        }
                }
                return result;
            }

            /* Numbers of chessmen are: 0 - King, 1 - Queen, 2 - Bishop, 3 - Knight, 4 - Tower,
             * 5 - Pawn of Queen, 6 - Pawn of Bishop, 7 - Pawn of Knight, 8 - Pawn of Tower.
             * Numbers of players are: 0 - Yellow, 1 - Red, 2 - Blue, 3 - Black.
             * Number of arrangement - numberOfArrangement. */
            public Chessman CreateNewChessman(string playersColor, string chessmanName, Chessboard chessboard)
            {
                int indexOfPlayer = -1;
                int indexOfChessman = -1;
                ChessmenParameters chessmanParam = new ChessmenParameters();
                Chessman result;
                //gets number of player
                switch (playersColor)
                {
                    case "Yellow": indexOfPlayer = 0;
                        break;
                    case "Red": indexOfPlayer = 1;
                        break;
                    case "Blue": indexOfPlayer = 2;
                        break;
                    case "Black": indexOfPlayer = 3;
                        break;
                    default:
                        {
                            MessageBox.Show("Внутренняя ошибка, невозможно выполнить действие");
                            return null;
                        }
                }

                //gets number of chessman
                switch (chessmanName)
                {
                    case "King": indexOfChessman = 0;
                        break;
                    case "Queen": indexOfChessman = 1;
                        break;
                    case "Bishop": indexOfChessman = 2;
                        break;
                    case "Knight": indexOfChessman = 3;
                        break;
                    case "Tower": indexOfChessman = 4;
                        break;
                    case "PawnOfQueen": indexOfChessman = 5;
                        chessmanName = "Pawn of Queen";
                        break;
                    case "PawnOfBishop": indexOfChessman = 6;
                        chessmanName = "Pawn of Bishop";
                        break;
                    case "PawnOfKnight": indexOfChessman = 7;
                        chessmanName = "Pawn of Knight";
                        break;
                    case "PawnOfTower": indexOfChessman = 8;
                        chessmanName = "Pawn of Tower";
                        break;
                    default:
                        {
                            MessageBox.Show("Внутренняя ошибка, невозможно выполнить действие");
                            return null;
                        }
                }

                //set parameters
                chessmanParam.horizontal = positionOfChessmen[numberOfArrangement, indexOfPlayer, indexOfChessman, 1];
                chessmanParam.vertical = positionOfChessmen[numberOfArrangement, indexOfPlayer, indexOfChessman, 0];
                chessmanParam.ruNameNominative = GetNominativeRu(indexOfPlayer, indexOfChessman);
                chessmanParam.engName = playersColor + ' ' + chessmanName;
                chessmanParam.chessmanBitmap = chessmanBitmap[indexOfPlayer, indexOfChessman];
                chessmanParam.ruNameAccusative = GetAccusativeRu(indexOfPlayer, indexOfChessman);
                chessmanParam.kindOfChessman = (ChessmenNames)indexOfChessman;
                chessmanParam.player = (Players)indexOfPlayer;
                chessmanParam.ruAttack = GetAttackRu(indexOfChessman);
                chessmanParam.ruMove = GetMoveRu(indexOfChessman);
                switch (indexOfChessman)
                {
                    case 0: result = new King(chessmanParam, chessboard);
                        break;
                    case 1: result = new Queen(chessmanParam, chessboard);
                        break;
                    case 2: result = new Bishop(chessmanParam, chessboard);
                        break;
                    case 3: result = new Knight(chessmanParam, chessboard);
                        break;
                    case 4: result = new Tower(chessmanParam, chessboard);
                        break;
                    case 5: result = GetNewCorrectPawn(chessmanParam, chessboard);
                        break;
                    case 6: result = GetNewCorrectPawn(chessmanParam, chessboard);
                        break;
                    case 7: result = GetNewCorrectPawn(chessmanParam, chessboard);
                        break;
                    case 8: result = GetNewCorrectPawn(chessmanParam, chessboard);
                        break;
                    default:
                        {
                            result = null;
                            break;
                        }
                }
                return result;
            }


        }
        //dopilit'
        //PLAYER
        public class Player
        {
            private Game game;
            private string playersName;
            private Players player;
            private Players controller;
            private Player ally;
            private List<Chessman> listOfChessmen;
            private List<Chessman> listOfLostChessmen;
            private List<Chessman> listOfTakenChessmen;
            private PictureBox takenChessmenPic;
            private PictureBox lostChessmenPic;
            public bool IsInGame { get; private set; }
            public bool IsSurrender { private set; get; }
            private bool check;
            private bool canKingMove;
            private bool lastKing;
            private bool enableTurningPawns;
            private bool IsController { get; set; }

            public Player(Game game, Players player, PictureBox takenPic, PictureBox lostPic, string name)
            {
                listOfChessmen = new List<Chessman>();
                listOfLostChessmen = new List<Chessman>();
                listOfTakenChessmen = new List<Chessman>();
                this.player = player;
                controller = player;
                takenChessmenPic = takenPic;
                lostChessmenPic = lostPic;
                IsInGame = true;
                check = false;
                canKingMove = true;
                lastKing = false;
                enableTurningPawns = false;
                playersName = name;
                this.game = game;
            }

            //this is almost useless method in game logic
            public void SetAlly(Player allyLogical)
            {
                ally = allyLogical;
            }

            public Player GetAlly()
            {
                return ally;
            }

            public void SetController(Players player)
            {
                controller = player;
                //also unfreeze, if required or not
                foreach (Chessman chessman in listOfChessmen)
                {
                    chessman.SetFrozenState(false);
                }
                if (this.player != player)
                {
                    IsSurrender = true;
                }
                else
                {
                    IsSurrender = false;
                }
            }

            public Players GetPlayersIdentifier()
            {
                return player;
            }

            public string GetName()
            {
                return playersName;
            }

            public void SetLastKingState(bool isThatLastKing)
            {
                lastKing = isThatLastKing;
            }

            public bool IsThatLastKing()
            {
                return lastKing;
            }

            public void AddChessman(Chessman chessman)
            {
                listOfChessmen.Add(chessman);
            }

            public void DeleteChessman(Chessman chessman)
            {
                listOfChessmen.Remove(chessman);
            }

            public void KingIsDead()
            {
                if (player == controller)
                {
                    foreach (Chessman chessman in listOfChessmen)
                    {
                        chessman.SetFrozenState(true);
                    }
                }
                IsInGame = false;
                ally.SetLastKingState(true);
                if (lastKing)
                {
                    //so allys king is dead too
                    //need to give possibility to write last turn to log
                    //so Chessboard will call forward line

                    //game.FinishGame((int)this.GetPlayersIdentifier() + (int)ally.GetPlayersIdentifier());

                }
                else
                {
                    //or allys king is still alive
                    if (player == controller)
                    {
                        MessageBox.Show(playersName + " потерял короля и вышел из игры. Вернуться в игру он сможет только в случае обмена пленными.", "Внимание");
                        game.AddStringToLogBuffer(playersName + " выбывает из игры.\n", false);
                    }
                    else
                    {
                        MessageBox.Show(playersName + " потерял короля. Так как контролем над игроком обладает другой игрок, фигуры не замораживаются.", "Внимание");
                    }
                    if (IsController)
                    {
                        MessageBox.Show(playersName + " потерял короля, который владел контролем над союзником. Теперь контроль возвращается обратно.", "Внимание");
                        ally.SetController(ally.GetPlayersIdentifier());
                    }
                }
            }

            public bool PriorityPawn()
            {
                //dopilit'
                int countOfQueens = 0;
                int countOfPawns = 0;
                int countOfBishops = 0;
                Chessman lastPawn = null;
                foreach (Chessman chessman in listOfChessmen)
                {
                    //frozen - mark as turned pawn
                    if ((chessman.IsInGame()) && (chessman.GetFrozenState()))
                    {
                        switch (chessman.GetChessmanName())
                        {
                            case ChessmenNames.Bishop:
                                countOfBishops++;
                                break;
                            case ChessmenNames.Queen:
                                countOfQueens++;
                                break;
                            case ChessmenNames.PawnOfBishop:
                                countOfPawns++;
                                lastPawn = chessman;
                                break;
                            case ChessmenNames.PawnOfKnight:
                                countOfPawns++;
                                lastPawn = chessman;
                                break;
                            case ChessmenNames.PawnOfQueen:
                                countOfPawns++;
                                lastPawn = chessman;
                                break;
                            case ChessmenNames.PawnOfTower:
                                countOfPawns++;
                                lastPawn = chessman;
                                break;
                            default: 
                                break;
                        }
                    }
                }
                //true if priority pawn
                if ((countOfPawns == 1) &&
                    (((countOfBishops <= 1) && (countOfQueens == 0)) ||
                    ((countOfBishops == 0) && (countOfQueens <= 1))))
                {
                    if (lastPawn != null)
                        lastPawn.SetPriorityState(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void SetCheckState(bool state)
            {
                check = state;
            }

            public bool GetCheckState()
            {
                return check;
            }

            public void SetKingMovementState(bool state)
            {
                canKingMove = state;
            }

            public bool CanKingMove()
            {
                return canKingMove;
            }

            public void SetSurrendingState(bool surrending)
            {
                IsSurrender = surrending;
            }

            public void PawnIsDead()
            {
                enableTurningPawns = true;
            }

            public bool CanPawnTurn()
            {
                return enableTurningPawns;
            }

            public void AddLostChessman(Chessman chessman)
            {
                listOfLostChessmen.Add(chessman);
            }

            public void AddTakenChessman(Chessman chessman)
            {
                listOfTakenChessmen.Add(chessman);
            }

            public void ShowMessages()
            {
                if (check)
                {
                    MessageBox.Show("Вам объявлен шах.","Внимание");
                }
                if (IsSurrender)
                {
                    MessageBox.Show("Данный игрок вышел из игры. Его фигурами управляет товарищ по команде", "Внимание");
                }
            }

            //graphics
            public void DrawLostChessman()
            {
                if (lostChessmenPic.Image != null)
                {
                    lostChessmenPic.Image.Dispose();
                }
                int height = 2;
                int width = 1 + (listOfLostChessmen.Count / height);
                lostChessmenPic.Image = new Bitmap(lostChessmenPic.Width, lostChessmenPic.Height);
                int sizeOfCell = Math.Min(lostChessmenPic.Width / width, lostChessmenPic.Height / height);
                for (int i = 0; i < listOfLostChessmen.Count; i++)
                {
                    listOfLostChessmen[i].ShowChessman(lostChessmenPic, (i % width) * sizeOfCell, (i / width) * sizeOfCell, sizeOfCell, sizeOfCell);
                }
                lostChessmenPic.Refresh();
            }

            public void DrawTakenChessman()
            {
                if (takenChessmenPic.Image != null)
                {
                    takenChessmenPic.Image.Dispose();
                }
                int height = 2;
                int width = 1 + (listOfTakenChessmen.Count / height);
                takenChessmenPic.Image = new Bitmap(takenChessmenPic.Width, takenChessmenPic.Height);
                int sizeOfCell = Math.Min(takenChessmenPic.Width / width, takenChessmenPic.Height / height);
                for (int i = 0; i < listOfTakenChessmen.Count; i++)
                {
                    listOfTakenChessmen[i].ShowChessman(takenChessmenPic, (i % width) * sizeOfCell, (i / width) * sizeOfCell, sizeOfCell, sizeOfCell);
                }
                takenChessmenPic.Refresh();
            }
        }

        //dopilit'
        //GAME
        public class Game
        {
            //possible game endings
            const int NO_WINNERS = 0;
            //these constants mean that sum of codes of losing players will be parameter of FinishGame method
            const int WON_RED_AND_YELLOW_TEAM = (int)Players.Black + (int)Players.Blue;
            const int WON_BLACK_AND_BLUE_TEAM = (int)Players.Red + (int)Players.Yellow;



            private string logTurn, logBuffer;  //could be colorful
            private Chessboard chessboard;
            private Players turn; //current turn
            private Player yellowPlayer;
            private Player redPlayer;
            private Player bluePlayer;
            private Player blackPlayer;
            private PictureBox chessboardPic;
            private PictureBox takenChessmenPic;
            private PictureBox lostChessmenPic;
            private Label statusBar;
            private Label turnBar;
            private RichTextBox log;
            private Button surrenderButton;
            private Button prisonersChangeButton;
            private int numberOfTurn;

            public Game(InterfaceElements interfaceElements)
            {
                chessboardPic = interfaceElements.chessboardPic;
                takenChessmenPic = interfaceElements.takenChessmenPic;
                lostChessmenPic = interfaceElements.lostChessmenPic;
                log = interfaceElements.logBar;
                statusBar = interfaceElements.statusBar;
                turnBar = interfaceElements.turnBar;
                surrenderButton = interfaceElements.surrenderButton;
                prisonersChangeButton = interfaceElements.prisonersChangeButton;
            }

            public void StartNewGame()
            {
                redPlayer = new Player(this, Players.Red, takenChessmenPic, lostChessmenPic, "Красный игрок");
                yellowPlayer = new Player(this, Players.Yellow, takenChessmenPic, lostChessmenPic, "Желтый игрок");
                redPlayer.SetAlly(yellowPlayer);
                yellowPlayer.SetAlly(redPlayer);
                blackPlayer = new Player(this, Players.Black, takenChessmenPic, lostChessmenPic, "Черный игрок");
                bluePlayer = new Player(this, Players.Blue, takenChessmenPic, lostChessmenPic, "Синий игрок");
                blackPlayer.SetAlly(bluePlayer);
                bluePlayer.SetAlly(blackPlayer);
                //dopilit'
                turn = Players.Black;
                ToggleTurn();

                logTurn = "";
                logBuffer = "";
                chessboard = new Chessboard(this, 0, 0, chessboardPic.Width, chessboardPic.Height, arrangement);
                chessboard.SetStatusBar(statusBar);
                chessboardPic.MouseClick -= chessboardPic_MouseClick;
                chessboardPic.MouseClick += chessboardPic_MouseClick;
                chessboardPic.MouseMove -= chessboardPic_MouseMove;
                chessboardPic.MouseMove += chessboardPic_MouseMove;
                surrenderButton.Click -= surrenderButton_Click;
                surrenderButton.Click += surrenderButton_Click;
                prisonersChangeButton.Click -= prisonersChangeButton_Click;
                prisonersChangeButton.Click += prisonersChangeButton_Click;
                chessboard.ShowChessboard(chessboardPic);
                numberOfTurn = 1;
                //dopilit'
            }

            private void prisonersChangeButton_Click(object sender, EventArgs e)
            {
                if (DialogResult.Yes == MessageBox.Show("Обмен пленными позволит вернуть в игру потерявших королей игроков." +
                        " Согласие на это должны дать оба оставшихся игрока.\n Оба игрока согласны на обмен пленными?", "Обмен пленными", MessageBoxButtons.YesNo))
                {
                    //dopilit'
                    prisonersChangeButton.Enabled = false;
                }
            }

            private void surrenderButton_Click(object sender, EventArgs e)
            {
                if (!GetPlayerObject(turn).IsSurrender)
                {
                    if (DialogResult.Yes == MessageBox.Show("Вы уверены, что хотите сдаться и прекратить свою игру?" +
                        " Ваши фигуры перейдут под контроль союзника, но он сможет ходить ими только во время хода соответствующего цвета.", "Сдаться", MessageBoxButtons.YesNo))
                    {
                        //here is surrender
                        //surrending gives control to ally, without freezing. Exception - only king left.
                        Player surrendingPlayer = GetPlayerObject(turn);
                        surrendingPlayer.SetSurrendingState(true);
                        logTurn += surrendingPlayer.GetName() + " сдался.\n";
                        //his ally now controlling his chessmen, but infact almost nothing has changed
                        surrendingPlayer.SetController(surrendingPlayer.GetAlly().GetPlayersIdentifier());
                        if ((surrendingPlayer.GetAlly().IsSurrender) || (!surrendingPlayer.GetAlly().IsInGame))
                        {
                            FinishGame((int)surrendingPlayer.GetPlayersIdentifier() + (int)surrendingPlayer.GetAlly().GetPlayersIdentifier());
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Данный игрок уже сдался. Если вы хотите сдаться, сдавайтесь на своем ходе.","Внимание");
                }
            }

            private void chessboardPic_MouseClick(object sender, MouseEventArgs e)
            {
                int x = e.X;
                int y = e.Y;
                chessboard.ClickOnChessboard(x, y, turn);
            }

            private void chessboardPic_MouseMove(object sender, MouseEventArgs e)
            {
                int x = e.X;
                int y = e.Y;
                chessboard.WriteInformationInStatusBar(x, y);
            }

            public void EnablePrisonersChange()
            {
                prisonersChangeButton.Enabled = true;
            }


            public void SaveGame()
            {
            }

            public void LoadGame()
            {
            }

            public void ToggleTurn()
            {
                switch (turn)
                {
                    case Players.Red:
                        if (blackPlayer.IsInGame)
                        {
                            turn = Players.Black;
                            blackPlayer.DrawLostChessman();
                            blackPlayer.DrawTakenChessman();
                        }
                        else
                        {
                            turn = Players.Black;
                            ToggleTurn();
                            return;
                        }
                        break;
                    case Players.Black:
                        if (yellowPlayer.IsInGame)
                        {
                            turn = Players.Yellow;
                            yellowPlayer.DrawLostChessman();
                            yellowPlayer.DrawTakenChessman();
                        }
                        else
                        {
                            turn = Players.Yellow;
                            ToggleTurn();
                            return;
                        }
                        break;
                    case Players.Yellow:
                        if (bluePlayer.IsInGame)
                        {
                            turn = Players.Blue;
                            bluePlayer.DrawLostChessman();
                            bluePlayer.DrawTakenChessman();
                        }
                        else
                        {
                            turn = Players.Blue;
                            ToggleTurn();
                            return;
                        }
                        break;
                    case Players.Blue:
                        if (redPlayer.IsInGame)
                        {
                            turn = Players.Red;
                            redPlayer.DrawLostChessman();
                            redPlayer.DrawTakenChessman();
                        }
                        else
                        {
                            turn = Players.Red;
                            ToggleTurn();
                            return;
                        }
                        break;
                }
                InterfaceChangeWhileTurn();
                GetPlayerObject(turn).ShowMessages();
                numberOfTurn++;
            }

            

            private void InterfaceChangeWhileTurn()
            {
                switch (turn)
                {
                    case Players.Red:
                        
                        turnBar.Text = "Ходит красный игрок";
                        turnBar.ForeColor = Color.FromArgb(192, 0, 0);
                        statusBar.ForeColor = Color.FromArgb(192, 0, 0);
                        surrenderButton.BackColor = Color.FromArgb(192, 0, 0);
                        prisonersChangeButton.BackColor = Color.FromArgb(192, 0, 0);
                        buttonBackground = Properties.Resources.ButtonRed;
                        break;
                    case Players.Black:
                        turnBar.Text = "Ходит черный игрок";
                        turnBar.ForeColor = Color.FromArgb(157, 157, 158);
                        statusBar.ForeColor = Color.FromArgb(157, 157, 158);
                        surrenderButton.BackColor = Color.FromArgb(157, 157, 158);
                        prisonersChangeButton.BackColor = Color.FromArgb(157, 157, 158);
                        buttonBackground.Dispose();
                        buttonBackground = Properties.Resources.ButtonGray;
                        break;
                    case Players.Yellow:
                        turnBar.Text = "Ходит желтый игрок";
                        turnBar.ForeColor = Color.FromArgb(255, 237, 0);
                        statusBar.ForeColor = Color.FromArgb(255, 237, 0);
                        surrenderButton.BackColor = Color.FromArgb(255, 237, 0);
                        prisonersChangeButton.BackColor = Color.FromArgb(255, 237, 0);
                        buttonBackground.Dispose();
                        buttonBackground = Properties.Resources.ButtonYellow;
                        break;
                    case Players.Blue:
                        turnBar.Text = "Ходит синий игрок";
                        turnBar.ForeColor = Color.FromArgb(0, 160, 227);
                        statusBar.ForeColor = Color.FromArgb(0, 160, 227);
                        surrenderButton.BackColor = Color.FromArgb(0, 160, 227);
                        prisonersChangeButton.BackColor = Color.FromArgb(0, 160, 227);
                        buttonBackground.Dispose();
                        buttonBackground = Properties.Resources.ButtonBlue;
                        break;
                }
            }

            public void AddStringToLog(string newLog, bool withNumber)
            {
                if (withNumber)
                {
                    logTurn += numberOfTurn.ToString() + ". " + newLog;
                }
                else
                {
                    logTurn += newLog;
                }
                logTurn += logBuffer;
                logBuffer = "";
                log.Text = logTurn;
            }

            public void AddStringToLogBuffer(string newBuffer, bool withNumber)
            {
                if (withNumber)
                {
                    logBuffer += numberOfTurn.ToString() + ". " + newBuffer;
                }
                else
                {
                    logBuffer += newBuffer;
                }
            }            

            public void ResizeChessboard()
            {
                chessboard.ResizeChessboard(0, 0, chessboardPic.Width, chessboardPic.Height);
                chessboard.ShowChessboard(chessboardPic);
                GetPlayerObject(turn).DrawLostChessman();
                GetPlayerObject(turn).DrawTakenChessman();
            }

            public bool IsGameStarted()
            {
                return (chessboard != null);
            }

            public Player GetPlayerObject(Players player)
            {
                
                switch (player)
                {
                    case Players.Yellow:
                        return yellowPlayer;
                    case Players.Red:
                        return redPlayer;
                    case Players.Black:
                        return blackPlayer;
                    case Players.Blue:
                        return bluePlayer;
                    default:
                        return null;
                }
            }

            
            public void FinishGame(int finishCode /*Code of losing team required; team which call finish game*/) 
            {
                chessboardPic.MouseClick -= chessboardPic_MouseClick;
                chessboardPic.MouseMove -= chessboardPic_MouseMove;
                surrenderButton.Click -= surrenderButton_Click;
                prisonersChangeButton.Click -= prisonersChangeButton_Click;
                switch (finishCode)
                {
                    case NO_WINNERS:
                        MessageBox.Show("Ничейная ситуация. Игра завершена.","Конец игры");
                        logTurn += "Ничейная ситуация. Конец игры.";
                        break;
                    case WON_RED_AND_YELLOW_TEAM:
                        MessageBox.Show("Победу одержала команда Желтого и Красного игрока. Игра завершена.", "Конец игры");
                        logTurn += "Победу одержала команда Желтого и Красного игрока. Конец игры.";
                        break;
                    case WON_BLACK_AND_BLUE_TEAM:
                        MessageBox.Show("Победу одержала команда Синего и Черного игрока. Игра завершена.", "Конец игры");
                        logTurn += "Победу одержала команда Синего и Черного игрока. Конец игры.";
                        break;
                    default:
                        break;
                }
                log.Text = logTurn;
            }
        }

        //CHESSMAN
        public abstract class Chessman
        {
            protected ChessmenParameters creatingState;
            protected Bitmap imageOfChessman;
            //position
            protected int horizontal, vertical;

            protected Players player;
            protected Player ownerObject;

            //chessman is not taken
            protected bool isInGame;

            protected bool isPossibleToMove;

            protected bool isPriority;

            //chessman is frozen after surrending of player or losing king
            protected bool isFrozen;

            //current chessboard
            protected Chessboard chessboard;

            protected ChessmenNames nameOfChessman;

            //string information
            protected string engName;
            protected string ruNameNominative;
            protected string ruNameAccusative;
            protected string engMove = "moved";
            protected string ruMove;
            protected string engAttack = "attacked";
            protected string ruAttack;


            public Chessman(ChessmenParameters chessmanParameter, Chessboard chessboard)
            {
                creatingState = chessmanParameter;
                this.engName = chessmanParameter.engName;
                this.horizontal = chessmanParameter.horizontal;
                this.vertical = chessmanParameter.vertical;
                this.ruNameNominative = chessmanParameter.ruNameNominative;
                this.ruNameAccusative = chessmanParameter.ruNameAccusative;
                this.player = chessmanParameter.player;
                this.imageOfChessman = chessmanParameter.chessmanBitmap;
                this.imageOfChessman.MakeTransparent(Color.White);
                this.chessboard = chessboard;
                this.nameOfChessman = chessmanParameter.kindOfChessman;
                this.isInGame = true;
                this.isFrozen = false;
                this.ruAttack = chessmanParameter.ruAttack;
                this.ruMove = chessmanParameter.ruMove;
                isPriority = false;
                ownerObject = chessboard.GetPlayerObject(player);
                ownerObject.AddChessman(this);
            }

            public ChessmenParameters GetChessmanParametres()
            {
                creatingState.horizontal = this.horizontal;
                creatingState.vertical = this.vertical;
                return creatingState;
            }

            //graphics

            public virtual void DrawChessmanOnChessboard(ref Bitmap chessboardBitmap, int x, int y, int cellSize)
            {
                if (isInGame)
                {
                    Graphics chessboardGraphics = Graphics.FromImage(chessboardBitmap);
                    //coefficients in cell between sides
                    /* ----1/10---
                     * ----4/5----
                     * 1/6-2/3-1/6 = 1
                     * -----------
                     * ----1/10--- = 1
                     * */
                    chessboardGraphics.DrawImage(imageOfChessman, x + cellSize * horizontal + cellSize / 6, y + cellSize * vertical + cellSize / 10, 2 * cellSize / 3, 4 * cellSize / 5);
                    chessboardGraphics.Dispose();
                }
            }

            public virtual void DrawChessmanOnBitmap(ref Bitmap chessmanBitmap, int x, int y, int width, int height)
            {
                if (isInGame)
                {
                    Graphics chessmanGraphics = Graphics.FromImage(chessmanBitmap);
                    chessmanGraphics.DrawImage(imageOfChessman, x, y, width, height);
                    chessmanGraphics.Dispose();
                }
            }

            //useful for help menu
            public void ShowChessman(PictureBox picturebox, int x, int y, int width, int height)
            {
                int cellSize = Math.Max(width, height);
                Graphics chessmanGraphics = Graphics.FromImage(picturebox.Image);
                chessmanGraphics.DrawImage(imageOfChessman, x + cellSize / 6, y + cellSize / 10, 2 * width / 3, 4 * height / 5);
                chessmanGraphics.Dispose();
            }

            //string methods

            public string GetNameOfChessmanEnglish()
            {
                return engName;
            }

            public string GetNameOfChessmanRussianNominative()
            {
                return ruNameNominative;
            }

            public string GetNameOfChessmanRussianAccusative()
            {
                return ruNameAccusative;
            }

            //ingame methods

            public ChessmenNames GetChessmanName()
            {
                return nameOfChessman;
            }

            public Players GetChessmanOwner()
            {
                return player;
            }

            public bool IsInGame()
            {
                return isInGame;
            }

            public abstract void ShowPossibleMovements(bool deselect);

            public abstract void ShowPossibleAttacks(bool deselect);

            public abstract bool IsThatPossibleMove(int horizontal, int vertical);

            public virtual void MoveChessman(int horizontal, int vertical, bool logged)
            {
                string turnList = "";
                if (logged)
                {
                    turnList = GetNameOfChessmanRussianNominative();
                    turnList += " " + chessboard.GetHorizontalChar(9 - this.horizontal) + " : " + (9 - this.vertical).ToString();
                    turnList += " " + ruMove + ' ';
                    turnList += " " + chessboard.GetHorizontalChar(9 - horizontal) + " : " + (9 - vertical).ToString();
                }
                chessboard.MakeThisCellUsual(this.horizontal, this.vertical);
                this.horizontal = horizontal;
                this.vertical = vertical;
                if (logged)
                {
                    turnList += '\n';
                    chessboard.SendLogToGame(turnList, true);
                }
                chessboard.DrawChessman(this);
            }

            public abstract bool IsThatPossibleAttack(int horizontal, int vertical);

            public virtual void AttackChessmanOnCoordinates(int horizontal, int vertical)
            {
                //dopilit'
                //string of state
                string turnList = GetNameOfChessmanRussianNominative();
                turnList += " " + chessboard.GetHorizontalChar(9 - this.horizontal) + " : " + (9 - this.vertical).ToString();
                turnList += ' ' + ruAttack + ' ';
                Chessman attackedChessman = chessboard.GetChessmanOnCoordinates(horizontal, vertical);
                while (attackedChessman != null)
                {
                    turnList += ' ' + attackedChessman.GetNameOfChessmanRussianAccusative();
                    turnList += " " + chessboard.GetHorizontalChar(9 - horizontal) + " : " + (9 - vertical).ToString();
                    attackedChessman.Dying();
                    ownerObject.AddTakenChessman(attackedChessman);
                    attackedChessman = chessboard.GetChessmanOnCoordinates(horizontal, vertical);
                }
                turnList += '\n';
                chessboard.SendLogToGame(turnList, true);
                chessboard.MakeThisCellUsual(this.horizontal, this.vertical);
                this.horizontal = horizontal;
                this.vertical = vertical;
                chessboard.DrawChessman(this);
            }

            public bool IsThatEnemyChessman(Players player)
            {
                bool result = true;
                if (this.player == player)
                    return false;
                int sum = (int)this.player + (int)player;
                result = ((sum != 1) && (sum != 5) && (!isFrozen));
                return result;
            }

            public virtual void Dying()
            {
                chessboard.MakeThisCellUsual(horizontal, vertical);
                horizontal = -1;
                vertical = -1;
                isInGame = false;
                ownerObject.AddLostChessman(this);
            }

            public virtual Point GetCoordinates()
            {
                Point result = new Point();
                result.X = horizontal;
                result.Y = vertical;
                return result;
            }

            public void SetFrozenState(bool frozen)
            {
                isFrozen = frozen;
            }
            
            public bool GetFrozenState()
            {
                return isFrozen;
            }

            public bool CanMoveOrAttack()
            {
                bool canAttack, canMove;
                ShowPossibleMovements(true);
                canAttack = isPossibleToMove;
                ShowPossibleAttacks(true);
                canMove = isPossibleToMove;
                return ((canMove) || (canAttack));
            }

            //only for pawns
            public void SetPriorityState(bool state)
            {
                isPriority = state;
            }

            public bool IsPriority()
            {
                return isPriority;
            }

            public virtual bool IsReadyToTurn()
            {
                return false;
            }
        }

        //KING
        public class King : Chessman
        {
            //specific fields are not exist
            protected int graphHorizontal;
            protected int graphVertical;

            public King(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {
                if (horizontal == 9)
                {
                    horizontal = 8;
                    graphHorizontal = 9;
                }
                else
                {
                    if (horizontal == 0)
                    {
                        horizontal = 1;
                        graphHorizontal = 0;
                    }
                    else
                    {
                        graphHorizontal = horizontal;
                    }
                }

                if (vertical == 9)
                {
                    vertical = 8;
                    graphVertical = 9;
                }
                else
                {
                    if (vertical == 0)
                    {
                        vertical = 1;
                        graphVertical = 0;
                    }
                    else
                    {
                        graphVertical = vertical;
                    }
                }
            }

            //graphics spec
            public override void DrawChessmanOnChessboard(ref Bitmap chessboardBitmap, int x, int y, int cellSize)
            {
                if (isInGame)
                {
                    Graphics chessboardGraphics = Graphics.FromImage(chessboardBitmap);
                    chessboardGraphics.DrawImage(imageOfChessman, x + cellSize * graphHorizontal + cellSize / 6, y + cellSize * graphVertical + cellSize / 10, 2 * cellSize / 3, 4 * cellSize / 5);
                    chessboardGraphics.Dispose();
                }
            }

            public override Point GetCoordinates()
            {
                Point result = new Point();
                result.X = graphHorizontal;
                result.Y = graphVertical;
                return result;
            }

            public Point GetRealCoordinates()
            {
                Point result = new Point();
                result.X = horizontal;
                result.Y = vertical;
                return result;
            }

            public override void MoveChessman(int horizontal, int vertical, bool logged)
            {
                string turnList = "";
                if (logged)
                {
                    turnList = GetNameOfChessmanRussianNominative();
                    turnList += " " + chessboard.GetHorizontalChar(9 - this.horizontal) + " : " + (9 - this.vertical).ToString();
                    turnList += " " + ruMove + ' ';
                    turnList += " " + chessboard.GetHorizontalChar(9 - horizontal) + " : " + (9 - vertical).ToString();
                }
                chessboard.MakeThisCellUsual(this.graphHorizontal, this.graphVertical);
                this.horizontal = horizontal;
                this.vertical = vertical;
                graphHorizontal = horizontal;
                graphVertical = vertical;
                if (logged)
                {
                    turnList += '\n';
                    chessboard.SendLogToGame(turnList, true);
                }
                chessboard.DrawChessman(this);
            }

            public override void AttackChessmanOnCoordinates(int horizontal, int vertical)
            {
                Chessman attackedChessman = chessboard.GetChessmanOnCoordinates(horizontal, vertical);
                while (attackedChessman != null)
                {
                    attackedChessman.Dying();
                    attackedChessman = chessboard.GetChessmanOnCoordinates(horizontal, vertical);
                }
                chessboard.MakeThisCellUsual(this.graphHorizontal, this.graphVertical);
                this.horizontal = horizontal;
                this.vertical = vertical;
                graphHorizontal = horizontal;
                graphVertical = vertical;
                chessboard.DrawChessman(this);
            }

            public override void Dying()
            {
                horizontal = -1;
                vertical = -1;
                isInGame = false;
                chessboard.MakeThisCellUsual(graphHorizontal, graphVertical);
                graphHorizontal = horizontal;
                graphVertical = vertical;
                ownerObject.AddLostChessman(this);
                ownerObject.KingIsDead();/**/
                //dopilit'
                //class "Player" maybe

            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(graphHorizontal, graphVertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(graphHorizontal, graphVertical);
                }
                chessboard.DrawChessman(this);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        newHorizontal = horizontal + i;
                        newVertical = vertical + j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            isPossibleToMove = true;
                            if (deselect)
                            {
                                chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                            }
                            else
                            {
                                chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                            }

                        }
                    }
                }

            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        newHorizontal = this.horizontal + i;
                        newVertical = this.vertical + j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            if ((newHorizontal == horizontal) && (newVertical == vertical))
                            {
                                result = true;

                                return result;
                            }
                        }
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                        if (!((i == 0) && (j == 0)))
                        {
                            newHorizontal = this.horizontal + i;
                            newVertical = this.vertical + j;
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                            {
                                isPossibleToMove = true;
                                if (deselect)
                                {
                                    chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                }
                                else
                                {
                                    chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                                }
                                //refresh
                                chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                            }
                        }
                }

            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        newHorizontal = this.horizontal + i;
                        newVertical = this.vertical + j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                        {
                            if ((newHorizontal == horizontal) && (newVertical == vertical))
                            {
                                result = true;

                                return result;
                            }
                        }
                    }
                }
                return result;
            }

        }

        //QUEEN
        public class Queen : Chessman
        {
            public Queen(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {

            }

            public override void ShowPossibleMovements(bool deselect)
            {
                int newHorizontal, newVertical;
                isPossibleToMove = false;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                for (int i = -2; i <= 2; i += 2)
                {
                    for (int j = -2; j <= 2; j += 2)
                    {
                        newHorizontal = horizontal + i;
                        newVertical = vertical + j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            isPossibleToMove = true;
                            if (deselect)
                            {
                                chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                            }
                            else
                            {
                                chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                            }

                        }
                    }
                }

            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int i = -2; i <= 2; i += 2)
                {
                    for (int j = -2; j <= 2; j += 2)
                    {
                        newHorizontal = this.horizontal + i;
                        newVertical = this.vertical + j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            if ((newHorizontal == horizontal) && (newVertical == vertical))
                            {
                                result = true;

                                return result;
                            }
                        }
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                int newHorizontal, newVertical;
                isPossibleToMove = false;
                for (int i = -2; i <= 2; i += 2)
                {
                    for (int j = -2; j <= 2; j += 2)
                        if (!((i == 0) && (j == 0)))
                        {
                            newHorizontal = this.horizontal + i;
                            newVertical = this.vertical + j;
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                            {
                                isPossibleToMove = true;
                                if (deselect)
                                {
                                    chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                }
                                else
                                {
                                    chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                                }
                                //refresh
                                chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                            }
                        }
                }

            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int i = -2; i <= 2; i += 2)
                {
                    for (int j = -2; j <= 2; j += 2)
                    {
                        newHorizontal = this.horizontal + i;
                        newVertical = this.vertical + j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                        {
                            if ((newHorizontal == horizontal) && (newVertical == vertical))
                            {
                                result = true;

                                return result;
                            }
                        }
                    }
                }
                return result;
            }
        }

        //BISHOP
        public class Bishop : Chessman
        {
            public Bishop(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {

            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                for (int k = -1; k <= 1; k += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            newHorizontal = horizontal + i * j * k;
                            newVertical = vertical + i * j;
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                   (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                            {
                                isPossibleToMove = true;
                                if (deselect)
                                {
                                    chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                }
                                else
                                {
                                    chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int k = -1; k <= 1; k += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            newHorizontal = this.horizontal + i * j * k;
                            newVertical = this.vertical + i * j;
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                            {
                                if ((newHorizontal == horizontal) && (newVertical == vertical))
                                {
                                    result = true;

                                    return result;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                for (int k = -1; k <= 1; k += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            newHorizontal = horizontal + i * j * k;
                            newVertical = vertical + i * j;
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                   (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                            {

                            }
                            else
                            {
                                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                 (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                                 (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                                {
                                    isPossibleToMove = true;
                                    if (deselect)
                                    {
                                        chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                    }
                                    else
                                    {
                                        chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                                    }
                                    //refresh
                                    chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                                }
                                break;
                            }
                        }
                    }
                }
            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int k = -1; k <= 1; k += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            newHorizontal = this.horizontal + i * j * k;
                            newVertical = this.vertical + i * j;
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                            {

                            }
                            else
                            {
                                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                                {
                                    if ((newHorizontal == horizontal) && (newVertical == vertical))
                                    {
                                        result = true;

                                        return result;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                return result;
            }
        }

        //KNIGHT
        public class Knight : Chessman
        {
            public Knight(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {

            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                for (int k = 0; k <= 1; k++)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        for (int j = -2; j <= 2; j += 4)
                        {
                            if (k == 0)
                            {
                                newHorizontal = horizontal + i;
                                newVertical = vertical + j;
                            }
                            else
                            {
                                newHorizontal = horizontal + j;
                                newVertical = vertical + i;
                            }
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                            {
                                isPossibleToMove = true;
                                if (deselect)
                                {
                                    chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                }
                                else
                                {
                                    chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                                }

                            }
                        }
                    }
                }
            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int k = 0; k <= 1; k++)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        for (int j = -2; j <= 2; j += 4)
                        {
                            if (k == 0)
                            {
                                newHorizontal = this.horizontal + i;
                                newVertical = this.vertical + j;
                            }
                            else
                            {
                                newHorizontal = this.horizontal + j;
                                newVertical = this.vertical + i;
                            }
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                            {
                                if ((newHorizontal == horizontal) && (newVertical == vertical))
                                {
                                    result = true;

                                    return result;
                                }
                            }
                        }
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                for (int k = 0; k <= 1; k++)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        for (int j = -2; j <= 2; j += 4)
                        {
                            if (!((i == 0) && (j == 0)))
                            {
                                if (k == 0)
                                {
                                    newHorizontal = this.horizontal + i;
                                    newVertical = this.vertical + j;
                                }
                                else
                                {
                                    newHorizontal = this.horizontal + j;
                                    newVertical = this.vertical + i;
                                }
                                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                    (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                                    (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                                {
                                    isPossibleToMove = true;
                                    if (deselect)
                                    {
                                        chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                    }
                                    else
                                    {
                                        chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                                    }
                                    //refresh
                                    chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                                }
                            }
                        }
                    }

                }
            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int k = 0; k <= 1; k++)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        for (int j = -2; j <= 2; j += 4)
                        {
                            if (k == 0)
                            {
                                newHorizontal = this.horizontal + i;
                                newVertical = this.vertical + j;
                            }
                            else
                            {
                                newHorizontal = this.horizontal + j;
                                newVertical = this.vertical + i;
                            }
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                                (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                            {
                                if ((newHorizontal == horizontal) && (newVertical == vertical))
                                {
                                    result = true;

                                    return result;
                                }
                            }
                        }
                    }
                }
                return result;
            }
        }

        //TOWER
        public class Tower : Chessman
        {
            public Tower(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {

            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = horizontal + i * j;
                        newVertical = vertical;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                               (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            isPossibleToMove = true;
                            if (deselect)
                            {
                                chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                            }
                            else
                            {
                                chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = horizontal;
                        newVertical = vertical + i * j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                               (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            isPossibleToMove = true;
                            if (deselect)
                            {
                                chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                            }
                            else
                            {
                                chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = this.horizontal + i * j;
                        newVertical = this.vertical;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            if ((newHorizontal == horizontal) && (newVertical == vertical))
                            {
                                result = true;

                                return result;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = this.horizontal;
                        newVertical = this.vertical + i * j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                            if ((newHorizontal == horizontal) && (newVertical == vertical))
                            {
                                result = true;

                                return result;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = horizontal + i * j;
                        newVertical = vertical;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                               (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {

                        }
                        else
                        {
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                             (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                             (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                            {
                                isPossibleToMove = true;
                                if (deselect)
                                {
                                    chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                }
                                else
                                {
                                    chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                                }
                                //refresh
                                chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                            }
                            break;
                        }
                    }
                }
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = horizontal;
                        newVertical = vertical + i * j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {

                        }
                        else
                        {
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                             (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                             (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                            {
                                isPossibleToMove = true;
                                if (deselect)
                                {
                                    chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                                }
                                else
                                {
                                    chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                                }
                                //refresh
                                chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                            }
                            break;
                        }
                    }
                }
            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {/**/
                bool result = false;
                int newHorizontal, newVertical;
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = this.horizontal + i * j;
                        newVertical = this.vertical;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {

                        }
                        else
                        {
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                            {
                                if ((newHorizontal == horizontal) && (newVertical == vertical))
                                {
                                    result = true;

                                    return result;
                                }
                            }
                            break;
                        }
                    }
                }
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        newHorizontal = this.horizontal;
                        newVertical = this.vertical + i * j;
                        if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                        {
                        }
                        else
                        {
                            if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                            (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player)))
                            {
                                if ((newHorizontal == horizontal) && (newVertical == vertical))
                                {
                                    result = true;

                                    return result;
                                }
                                
                            }
                            break;
                        }
                    }
                }
                return result;
            }
        }

        //PAWNS

        //Each player's pawn has different direction, so each player must have his own pawn
        //YELLOW PAWN
        public class YellowPawn : Chessman
        {
            public YellowPawn(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {
            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                newHorizontal = horizontal;
                newVertical = vertical + 1;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                    (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    isPossibleToMove = true;
                    if (deselect)
                    {
                        chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                    }
                    else
                    {
                        chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                    }
                }
            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal;
                newVertical = this.vertical + 1;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                   (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    if ((newHorizontal == horizontal) && (newVertical == vertical))
                    {
                        result = true;
                        return result;
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                newVertical = this.vertical + 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newHorizontal = this.horizontal + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        isPossibleToMove = true;
                        if (deselect)
                        {
                            chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                        }
                        else
                        {
                            chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                        }
                        chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                    }
                }
            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newVertical = this.vertical + 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newHorizontal = this.horizontal + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        if ((newHorizontal == horizontal) && (newVertical == vertical))
                        {
                            result = true;
                            return result;
                        }
                    }
                }
                return result;
            }

            public override bool IsReadyToTurn()
            {
                return (vertical == 8);
            }

            public override void Dying()
            {
                base.Dying();
                ownerObject.PawnIsDead();
            }

        }

        //RED PAWN
        public class RedPawn : Chessman
        {
            public RedPawn(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {
            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                newHorizontal = horizontal;
                newVertical = vertical - 1;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                    (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    isPossibleToMove = true;
                    if (deselect)
                    {
                        chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                    }
                    else
                    {
                        chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                    }
                }
            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal;
                newVertical = this.vertical - 1;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                   (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    if ((newHorizontal == horizontal) && (newVertical == vertical))
                    {
                        result = true;
                        return result;
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                newVertical = this.vertical - 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newHorizontal = this.horizontal + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        isPossibleToMove = true;
                        if (deselect)
                        {
                            chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                        }
                        else
                        {
                            chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                        }
                        chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                    }
                }
            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newVertical = this.vertical - 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newHorizontal = this.horizontal + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        if ((newHorizontal == horizontal) && (newVertical == vertical))
                        {
                            result = true;
                            return result;
                        }
                    }
                }
                return result;
            }

            public override bool IsReadyToTurn()
            {
                return (vertical == 1);
            }

            public override void Dying()
            {
                base.Dying();
                ownerObject.PawnIsDead();
            }
        }

        //BLUE PAWN
        public class BluePawn : Chessman
        {
            public BluePawn(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {

            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                newHorizontal = horizontal - 1;
                newVertical = vertical;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                    (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    isPossibleToMove = true;
                    if (deselect)
                    {
                        chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                    }
                    else
                    {
                        chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                    }
                }
            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal - 1;
                newVertical = this.vertical;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                   (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    if ((newHorizontal == horizontal) && (newVertical == vertical))
                    {
                        result = true;
                        return result;
                    }
                }
                return result;
            }
            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal - 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newVertical = this.vertical + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        isPossibleToMove = true;
                        if (deselect)
                        {
                            chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                        }
                        else
                        {
                            chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                        }
                        chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                    }
                }
            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal - 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newVertical = this.vertical + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        if ((newHorizontal == horizontal) && (newVertical == vertical))
                        {
                            result = true;
                            return result;
                        }
                    }
                }
                return result;
            }

            public override bool IsReadyToTurn()
            {
                return (horizontal == 1);
            }

            public override void Dying()
            {
                base.Dying();
                ownerObject.PawnIsDead();
            }
        }

        //BLACK PAWN
        public class BlackPawn : Chessman
        {
            public BlackPawn(ChessmenParameters chessmanParameters, Chessboard chessboard)
                : base(chessmanParameters, chessboard)
            {

            }

            public override void ShowPossibleMovements(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                if (deselect)
                {
                    chessboard.MakeThisCellUsual(horizontal, vertical);
                }
                else
                {
                    chessboard.MakeThisCellSelected(horizontal, vertical);
                }
                chessboard.DrawChessman(this);
                newHorizontal = horizontal + 1;
                newVertical = vertical;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                    (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    isPossibleToMove = true;
                    if (deselect)
                    {
                        chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                    }
                    else
                    {
                        chessboard.MakeThisCellSelected(newHorizontal, newVertical);
                    }
                }
            }

            public override bool IsThatPossibleMove(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal + 1;
                newVertical = this.vertical;
                if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                   (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) == null))
                {
                    if ((newHorizontal == horizontal) && (newVertical == vertical))
                    {
                        result = true;
                        return result;
                    }
                }
                return result;
            }

            public override void ShowPossibleAttacks(bool deselect)
            {
                isPossibleToMove = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal + 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newVertical = this.vertical + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        isPossibleToMove = true;
                        if (deselect)
                        {
                            chessboard.MakeThisCellUsual(newHorizontal, newVertical);
                        }
                        else
                        {
                            chessboard.MakeThisCellAttacked(newHorizontal, newVertical);
                        }
                        chessboard.DrawChessman(chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical));
                    }
                }
            }

            public override bool IsThatPossibleAttack(int horizontal, int vertical)
            {
                bool result = false;
                int newHorizontal, newVertical;
                newHorizontal = this.horizontal + 1;
                for (int i = -1; i <= 1; i += 2)
                {
                    newVertical = this.vertical + i;
                    if (chessboard.IsThisCellExists(newHorizontal, newVertical) &&
                       (chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical) != null) &&
                       ((chessboard.GetChessmanOnCoordinates(newHorizontal, newVertical).IsThatEnemyChessman(this.player))))
                    {
                        if ((newHorizontal == horizontal) && (newVertical == vertical))
                        {
                            result = true;
                            return result;
                        }
                    }
                }
                return result;
            }

            public override bool IsReadyToTurn()
            {
                return (horizontal == 8);
            }

            public override void Dying()
            {
                base.Dying();
                ownerObject.PawnIsDead();
            }
        }

        //CHESSBOARD
        public class Chessboard
        {
            //Game constants
            const int COUNT_OF_PLAYERS = 4;
            const int COUNT_OF_PLAYER_CHESSMEN = 9;
            const int COUNT_OF_CHESSMEN = COUNT_OF_PLAYERS * COUNT_OF_PLAYER_CHESSMEN;
            const int WIDTH_OF_CHESSBOARD = 8;

            private Game game;

            private enum CellState { Usual, Selected, Attacked };
            private CellState[,] cellState = new CellState[10, 10];

            private static Bitmap chessboardBitmap;
            private PictureBox chessboardPicturebox;

            private int x, y, width, height;
            private int sizeOfCell, sizeOfChessboard;
            private Color colorOfLightCell, colorOfDarkCell, colorOfSelectedCell, colorOfAttackedCell;

            private Chessman[] currentState;
            private King[] kings;
            private int numberOfArrangement;

            private Label statusBar;
            private string selectedChessmanStr = "";
            private Chessman selectedChessman;

            private char[] indexToChar = { 'H', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A', 'A' };

            public Chessboard(Game game, int X, int Y, int Width, int Height, int arrangementNumber)
            {
                x = X;
                y = Y;
                width = Width;
                height = Height;
                sizeOfCell = Math.Min(Width, Height) / 10;
                sizeOfChessboard = Math.Min(width - x, height - y);
                numberOfArrangement = arrangementNumber;
                this.game = game;
                OnceMakeFirstArrangement();
                //dopilit'
                colorOfDarkCell = Color.FromArgb(0, 152, 70);
                colorOfLightCell = Color.FromArgb(255, 255, 255);
                colorOfSelectedCell = Color.FromArgb(150, 100, 255);
                colorOfAttackedCell = Color.FromArgb(255, 159, 0);

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        cellState[i, j] = CellState.Usual;
                    }
                }
            }

            //Ingame methods

            private void OnceMakeFirstArrangement()
            //There are 8 variants of different first positions of chessmen. Possibly this class should
            //get users options from record
            {
                int k = 0, index = 0;
                kings = new King[4];
                ChessmenInfo chessmanInfo = new ChessmenInfo(numberOfArrangement);
                currentState = new Chessman[COUNT_OF_CHESSMEN];
                for (Players i = Players.Yellow; i <= Players.Black; i++)
                {
                    for (ChessmenNames j = ChessmenNames.King; j <= ChessmenNames.PawnOfTower; j++)
                    {
                        currentState[k] = chessmanInfo.CreateNewChessman(i.ToString(), j.ToString(), this);
                        if (currentState[k].GetChessmanName() == ChessmenNames.King)
                        {
                            kings[index] = (King)currentState[k];
                            index++;
                        }
                        k++;
                    }
                }
            }

            public Chessman GetChessmanOnCoordinates(int horizontal, int vertical)
            {
                Chessman result = null;
                for (int i = 0; i < COUNT_OF_CHESSMEN; i++)
                {    
                    if ((currentState[i].GetCoordinates().X == horizontal) &&
                        (currentState[i].GetCoordinates().Y == vertical))
                    {
                        result = currentState[i];
                        return result;
                    }
                }
                for (int i = 0; i < COUNT_OF_CHESSMEN; i++)
                {
                    if (currentState[i].GetChessmanName() == ChessmenNames.King)
                    {
                        if ((((King)currentState[i]).GetRealCoordinates().X == horizontal) &&
                        (((King)currentState[i]).GetRealCoordinates().Y == vertical))
                        {
                            result = currentState[i];
                            return result;
                        }
                    }
                }
                return result;
            }

            public bool IsThisCellExists(int horizontal, int vertical)
            {
                bool result = true;
                //spesial corners are ignored, because infact they aren't exist
                if ((horizontal > 0) && (horizontal < 9) && (vertical > 0) && (vertical < 9))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }

            private void TurnPawn(ChessmenNames typeOfChessman, int numberInCurrentState)
            {
                string log = currentState[numberInCurrentState].GetNameOfChessmanRussianNominative() + " превращается в ";
                Point oldCoordinates = currentState[numberInCurrentState].GetCoordinates();
                Point redrawCoordinates = new Point();
                Chessman turningChessman = chessmenInfo.CreateNewChessman(currentState[numberInCurrentState].GetChessmanOwner().ToString(), typeOfChessman.ToString(), this);
                redrawCoordinates = turningChessman.GetCoordinates();
                game.GetPlayerObject(turningChessman.GetChessmanOwner()).DeleteChessman(currentState[numberInCurrentState]);
                currentState[numberInCurrentState] = turningChessman;
                currentState[numberInCurrentState].MoveChessman(oldCoordinates.X, oldCoordinates.Y, false);
                log += currentState[numberInCurrentState].GetNameOfChessmanRussianAccusative();
                ShowChessboard(chessboardPicturebox);
                game.AddStringToLog(log + "\n",false);
            }

            //Analyse after each turn
            public void TurnAnalyse()
            {
                //kings analyse
                //thrones analyse
                //pawns analyse
                //end of game analys (not win maybe)
                //jujube (pat) for player (wtf that word)

                AnalysePawns();
                AnalyseKings();
            }

            private const Point redThrone = new Point(8, 8);
            private const Point yellowThrone = new Point(1, 1);
            private const Point blueThrone = new Point(1, 8);
            private const Point blackThrone = new Point(8, 1);

            private void AnalyseThrones()
            {
                if ((GetChessmanOnCoordinates(yellowThrone.X, yellowThrone.Y).GetChessmanName() == ChessmenNames.King) &&
                    (GetChessmanOnCoordinates(yellowThrone.X, yellowThrone.Y).GetChessmanOwner() == Players.Red))
                {
                    game.GetPlayerObject(Players.Yellow).SetController(Players.Red);
                    //dopilit'
                    //if on yellow throne sit red king
                }
                if ((GetChessmanOnCoordinates(redThrone.X, redThrone.Y).GetChessmanName() == ChessmenNames.King) &&
                    (GetChessmanOnCoordinates(redThrone.X, redThrone.Y).GetChessmanOwner() == Players.Yellow))
                {
                    //if on red throne sit yellow king
                }
                if ((GetChessmanOnCoordinates(blueThrone.X, blueThrone.Y).GetChessmanName() == ChessmenNames.King) &&
                    (GetChessmanOnCoordinates(blueThrone.X, blueThrone.Y).GetChessmanOwner() == Players.Black))
                {
                    //if on blue throne sit black king
                }
                if ((GetChessmanOnCoordinates(blackThrone.X, blackThrone.Y).GetChessmanName() == ChessmenNames.King) &&
                    (GetChessmanOnCoordinates(blackThrone.X, blackThrone.Y).GetChessmanOwner() == Players.Blue))
                {
                    //if on black throne sit blue king
                }
            }

            private void AnalysePawns()
            {
                bool redPriority = game.GetPlayerObject(Players.Red).PriorityPawn();
                bool yellowPriority = game.GetPlayerObject(Players.Yellow).PriorityPawn();
                bool bluePriority = game.GetPlayerObject(Players.Blue).PriorityPawn();
                bool blackPriority = game.GetPlayerObject(Players.Black).PriorityPawn();
                for (int i = 0; i < COUNT_OF_CHESSMEN; i++)
                {
                    if (!currentState[i].GetFrozenState())
                    {
                        ChessmenParameters param = currentState[i].GetChessmanParametres();
                        switch (param.kindOfChessman)
                        {
                            case ChessmenNames.PawnOfBishop:
                                if (currentState[i].IsReadyToTurn())
                                {
                                    if (currentState[i].IsPriority())
                                    {
                                        //dopilit'
                                        /**/
                                    }
                                    else
                                    {
                                        TurnPawn(ChessmenNames.Bishop, i);
                                    }
                                }
                                break;
                            case ChessmenNames.PawnOfKnight:
                                if (currentState[i].IsReadyToTurn())
                                {
                                    if (currentState[i].IsPriority())
                                    {
                                    }
                                    else
                                    {
                                        TurnPawn(ChessmenNames.Knight, i);
                                    }
                                }
                                break;
                            case ChessmenNames.PawnOfQueen:
                                if (currentState[i].IsReadyToTurn())
                                {
                                    if (currentState[i].IsPriority())
                                    {
                                    }
                                    else
                                    {
                                        TurnPawn(ChessmenNames.Queen, i);
                                    }
                                }
                                break;
                            case ChessmenNames.PawnOfTower:
                                if (currentState[i].IsReadyToTurn())
                                {
                                    if (currentState[i].IsPriority())
                                    {
                                    }
                                    else
                                    {
                                        TurnPawn(ChessmenNames.Tower, i);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            //Analyse for check
            private void AnalyseKings()
            {
                Point coordinates;
                bool withoutCheck;
                int lostKings = 0;
                int checksumOfLoose = 0;
                for (int j = 0; j < 4; j++)
                {
                    withoutCheck = true;
                    coordinates = kings[j].GetRealCoordinates();
                    for (int i = 0; i < COUNT_OF_CHESSMEN; i++)
                    {
                        if ((currentState[i].IsInGame()) && !(currentState[i].GetFrozenState()))
                        {
                            if (currentState[i].IsThatPossibleAttack(coordinates.X, coordinates.Y))
                            {
                                game.GetPlayerObject(kings[j].GetChessmanOwner()).SetCheckState(true);
                                game.AddStringToLog(kings[j].GetNameOfChessmanRussianNominative() + " под шахом.\n", false);
                                withoutCheck = false;
                                //vypilit'
                                //kings[j].ShowPossibleMovements(true);
                                //kings[j].ShowPossibleAttacks(true);
                                if (!kings[j].CanMoveOrAttack())
                                {
                                    game.GetPlayerObject(kings[j].GetChessmanOwner()).SetKingMovementState(false);
                                }
                                else
                                {
                                    game.GetPlayerObject(kings[j].GetChessmanOwner()).SetKingMovementState(true);
                                }
                            }
                        }
                    }
                    if (withoutCheck)
                    {
                        game.GetPlayerObject(kings[j].GetChessmanOwner()).SetCheckState(false);
                        game.GetPlayerObject(kings[j].GetChessmanOwner()).SetKingMovementState(true);
                    }
                    if (!kings[j].IsInGame())
                    {
                        lostKings ++;
                        checksumOfLoose += (int)(kings[j].GetChessmanOwner());
                    }
                }
                if (lostKings == 2)
                {
                    if ((checksumOfLoose != (int)Players.Red + (int)Players.Yellow) &&
                        (checksumOfLoose != (int)Players.Black + (int)Players.Blue))
                    {
                        //enable of prisoners change
                        game.EnablePrisonersChange();
                    }
                    else
                    {
                        //if 2 kings from same team are dead
                        game.FinishGame(checksumOfLoose);
                    }
                }
            }



            //Graphics for chessboard

            public void ResizeChessboard(int X, int Y, int Width, int Height)
            {
                x = X;
                x = X;
                y = Y;
                width = Width;
                height = Height;
                sizeOfCell = Math.Min(Width, Height) / 10;
                sizeOfChessboard = Math.Min(width - x, height - y);
            }

            public void DrawChessmen()
            {
                for (int i = 0; i < COUNT_OF_CHESSMEN; i++)
                {
                    currentState[i].DrawChessmanOnChessboard(ref chessboardBitmap, x, y, sizeOfCell);
                }
            }

            public void DrawChessman(Chessman chessman)
            {
                chessman.DrawChessmanOnChessboard(ref chessboardBitmap, x, y, sizeOfCell);
            }

            public void MakeThisCellSelected(int horizontal, int vertical)
            {
                cellState[horizontal, vertical] = CellState.Selected;

                Graphics graphics = Graphics.FromImage(chessboardBitmap);
                Brush chessboardSelectedBrush = new SolidBrush(colorOfSelectedCell);
                Pen chessboardPen = new Pen(Color.Black, 2);
                graphics.FillRectangle(chessboardSelectedBrush, x + horizontal * sizeOfCell, y + vertical * sizeOfCell, sizeOfCell, sizeOfCell);
                graphics.DrawRectangle(chessboardPen, x + horizontal * sizeOfCell, y + vertical * sizeOfCell, sizeOfCell, sizeOfCell);
                graphics.Dispose();
            }

            public void MakeThisCellAttacked(int horizontal, int vertical)
            {
                cellState[horizontal, vertical] = CellState.Attacked;

                Graphics graphics = Graphics.FromImage(chessboardBitmap);
                Brush chessboardSelectedBrush = new SolidBrush(colorOfAttackedCell);
                Pen chessboardPen = new Pen(Color.Black, 2);
                graphics.FillRectangle(chessboardSelectedBrush, x + horizontal * sizeOfCell, y + vertical * sizeOfCell, sizeOfCell, sizeOfCell);
                graphics.DrawRectangle(chessboardPen, x + horizontal * sizeOfCell, y + vertical * sizeOfCell, sizeOfCell, sizeOfCell);
                graphics.Dispose();
            }

            public void MakeThisCellUsual(int horizontal, int vertical)
            {
                cellState[horizontal, vertical] = CellState.Usual;
                int graphHorizontal, graphVertical;
                if (horizontal == 9)
                {
                    horizontal = 8;
                    graphHorizontal = 9;
                }
                else
                {
                    if (horizontal == 0)
                    {
                        horizontal = 1;
                        graphHorizontal = 0;
                    }
                    else
                    {
                        graphHorizontal = horizontal;
                    }
                }
                if (vertical == 9)
                {
                    vertical = 8;
                    graphVertical = 9;
                }
                else
                {
                    if (vertical == 0)
                    {
                        vertical = 1;
                        graphVertical = 0;
                    }
                    else
                    {
                        graphVertical = vertical;
                    }
                }

                Graphics graphics = Graphics.FromImage(chessboardBitmap);
                Brush chessboardSelectedBrush;
                if ((horizontal + vertical) % 2 == 0)
                {
                    chessboardSelectedBrush = new SolidBrush(colorOfLightCell);
                }
                else
                {
                    chessboardSelectedBrush = new SolidBrush(colorOfDarkCell);
                }
                Pen chessboardPen = new Pen(Color.Black, 2);
                graphics.FillRectangle(chessboardSelectedBrush, x + graphHorizontal * sizeOfCell, y + graphVertical * sizeOfCell, sizeOfCell, sizeOfCell);
                graphics.DrawRectangle(chessboardPen, x + graphHorizontal * sizeOfCell, y + graphVertical * sizeOfCell, sizeOfCell, sizeOfCell);
                graphics.Dispose();
            }

            public Bitmap GetChessboardBitmap()
            {
                DrawChessboard();
                DrawChessmen();
                return chessboardBitmap;
            }

            public void ShowChessboard(PictureBox picturebox)
            {
                DrawChessboard();
                DrawChessmen();
                chessboardPicturebox = picturebox;
                picturebox.Image = chessboardBitmap;
                picturebox.Refresh();
            }


            private void DrawChessboard()
            {

                if (chessboardBitmap != null)
                {
                    chessboardBitmap.Dispose();
                }

                chessboardBitmap = new Bitmap(width, height);
                Graphics chessboardGraphics = Graphics.FromImage(chessboardBitmap);

                //creating pen
                Pen chessboardPen = new Pen(Color.Black, 2);

                //creating brushes
                Brush chessboardBackground = new SolidBrush(Color.LightSkyBlue);
                Brush chessboardLightBrush = new SolidBrush(colorOfLightCell);
                Brush chessboardDarkBrush = new SolidBrush(colorOfDarkCell);
                Brush chessboardSelectedBrush = new SolidBrush(colorOfSelectedCell);
                Brush chessboardAttackedBrush = new SolidBrush(colorOfAttackedCell);

                chessboardGraphics.FillRectangle(chessboardBackground, x, y, width, height);

                //drawing chessboard
                for (int i = 1; i <= WIDTH_OF_CHESSBOARD; i++)
                {
                    for (int j = 1; j <= WIDTH_OF_CHESSBOARD; j++)
                    {
                        //Corner cells have special shape, they can't pass here
                        if (!(((i == 1) || (i == WIDTH_OF_CHESSBOARD)) && ((j == 1) || (j == WIDTH_OF_CHESSBOARD))))
                        {
                            switch (cellState[i, j])
                            {
                                case CellState.Usual:
                                    if ((i + j) % 2 == 0)
                                    {
                                        chessboardGraphics.FillRectangle(chessboardLightBrush, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                                    }
                                    else
                                    {
                                        chessboardGraphics.FillRectangle(chessboardDarkBrush, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                                    }
                                    break;
                                case CellState.Attacked:
                                    chessboardGraphics.FillRectangle(chessboardAttackedBrush, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                                    break;
                                case CellState.Selected:
                                    chessboardGraphics.FillRectangle(chessboardSelectedBrush, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                                    break;
                            }
                        }
                        chessboardGraphics.DrawRectangle(chessboardPen, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                    }
                }

                //Drawing special corners, they are doubled, so some rectangle parametrs are doubled
                chessboardGraphics.FillRectangle(chessboardLightBrush, x + 0 * sizeOfCell, y + 1 * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.DrawRectangle(chessboardPen, x + 0 * sizeOfCell, y + 1 * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.FillRectangle(chessboardLightBrush, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.DrawRectangle(chessboardPen, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.FillRectangle(chessboardDarkBrush, x + 1 * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.DrawRectangle(chessboardPen, x + 1 * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.FillRectangle(chessboardDarkBrush, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + 0 * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.DrawRectangle(chessboardPen, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + 0 * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.Dispose();
            }

            //Methods for status bar

            public void SetStatusBar(Label statusLabel)
            {
                statusBar = statusLabel;
            }

            public Label GetStatusBar()
            {
                return statusBar;
            }

            public char GetHorizontalChar(int horizontal)
            {
                return indexToChar[horizontal];
            }

            public void SendLogToGame(string newLog, bool withNumber)
            {
                game.AddStringToLog(newLog, withNumber);
            }

            public Player GetPlayerObject(Players player)
            {
                return game.GetPlayerObject(player);
            }

            public void WriteInformationInStatusBar(int mouseX, int mouseY)
            {
                int horizontal, vertical;
                char horizontalChar;
                horizontal = mouseX / sizeOfCell;
                vertical = mouseY / sizeOfCell;
                //if cell exists
                if (((horizontal > 0) && (horizontal < 9) && (vertical > 0) && (vertical < 9)) ||
                    //special corners
                    (((horizontal == 9) && (vertical == 8)) ||
                    ((horizontal == 0) && (vertical == 1)) ||
                    ((horizontal == 8) && (vertical == 0)) ||
                    ((horizontal == 1) && (vertical == 9))))
                {
                    //special corners should have usual number
                    if (horizontal == 0)
                        horizontal = 1;
                    if (horizontal == 9)
                        horizontal = 8;
                    if (vertical == 0)
                        vertical = 1;
                    if (vertical == 9)
                        vertical = 8;
                    vertical = 9 - vertical;
                    horizontal = 9 - horizontal;
                    horizontalChar = GetHorizontalChar(horizontal);
                    statusBar.Text = selectedChessmanStr + " " + horizontalChar + " : " + vertical.ToString();
                }
                else
                {
                    ClearStatusBar();
                }
            }

            public void ClearStatusBar()
            {
                statusBar.Text = selectedChessmanStr;
            }

            //for selecting/clicking

            public void ClickOnChessboard(int mouseX, int mouseY, Players player)
            {
                int horizontal, vertical;
                horizontal = mouseX / sizeOfCell;
                vertical = mouseY / sizeOfCell;
                //if cell exists
                if (((horizontal > 0) && (horizontal < 9) && (vertical > 0) && (vertical < 9)) ||
                    //special corners
                    (((horizontal == 9) && (vertical == 8)) ||
                    ((horizontal == 0) && (vertical == 1)) ||
                    ((horizontal == 8) && (vertical == 0)) ||
                    ((horizontal == 1) && (vertical == 9))))
                {
                    bool wasAttack = false;
                    if (GetChessmanOnCoordinates(horizontal, vertical) != null)
                    {

                        if (GetChessmanOnCoordinates(horizontal, vertical) != selectedChessman)
                        {
                            //attack method if enemy
                            if (selectedChessman != null)
                            {
                                selectedChessman.ShowPossibleMovements(true);
                                selectedChessman.ShowPossibleAttacks(true);
                                if (selectedChessman.IsThatPossibleAttack(horizontal, vertical))
                                {
                                    selectedChessman.AttackChessmanOnCoordinates(horizontal, vertical);
                                    selectedChessmanStr = "";
                                    selectedChessman = null;
                                    wasAttack = true;
                                    TurnAnalyse();
                                    //turn happened
                                    game.ToggleTurn();
                                }
                            }
                            //is rightfully select
                            if ((!wasAttack) && 
                                (player == GetChessmanOnCoordinates(horizontal, vertical).GetChessmanOwner()))
                            {
                                //is check and could king move
                                if (game.GetPlayerObject(player).GetCheckState())
                                {
                                    if (!(game.GetPlayerObject(player).CanKingMove()))
                                    {
                                        if (GetChessmanOnCoordinates(horizontal, vertical).GetChessmanName() != ChessmenNames.King)
                                        {
                                            selectedChessman = GetChessmanOnCoordinates(horizontal, vertical);
                                            selectedChessmanStr = selectedChessman.GetNameOfChessmanRussianNominative() + ' ';
                                            selectedChessman.ShowPossibleMovements(false);
                                            selectedChessman.ShowPossibleAttacks(false);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Вам объявлен шах, но король не может двигаться.", "Внимание");
                                        }
                                    }
                                    else
                                    {
                                        if (GetChessmanOnCoordinates(horizontal, vertical).GetChessmanName() != ChessmenNames.King)
                                        {
                                            MessageBox.Show("Вам объявлен шах. Ходить может только король.", "Внимание");
                                        }
                                        else
                                        {
                                            selectedChessman = GetChessmanOnCoordinates(horizontal, vertical);
                                            selectedChessmanStr = selectedChessman.GetNameOfChessmanRussianNominative() + ' ';
                                            selectedChessman.ShowPossibleMovements(false);
                                            selectedChessman.ShowPossibleAttacks(false);
                                        }
                                    }
                                }
                                else
                                {
                                    //if (GetChessmanOnCoordinates(horizontal, vertical).CanMoveOrAttack())
                                    //{
                                        selectedChessman = GetChessmanOnCoordinates(horizontal, vertical);
                                        selectedChessmanStr = selectedChessman.GetNameOfChessmanRussianNominative() + ' ';
                                        selectedChessman.ShowPossibleMovements(false);
                                        selectedChessman.ShowPossibleAttacks(false);
                                    //}
                                }                                
                            }
                        }
                        else
                        {
                            //if repeat click then deselect chessman
                            selectedChessman.ShowPossibleMovements(true);
                            selectedChessman.ShowPossibleAttacks(true);
                            selectedChessmanStr = "";
                            selectedChessman = null;
                        }
                    }
                    else
                    {
                        //move method if possible move
                        if (selectedChessman != null)
                        {
                            selectedChessmanStr = "";
                            selectedChessman.ShowPossibleMovements(true);
                            selectedChessman.ShowPossibleAttacks(true);
                            if (selectedChessman.IsThatPossibleMove(horizontal, vertical))
                            {
                                selectedChessman.MoveChessman(horizontal, vertical, true);
                                TurnAnalyse();
                                //turn happened
                                game.ToggleTurn();
                            }
                            selectedChessman = null;
                        }
                    }
                    chessboardPicturebox.Refresh();
                }
            }

        }

        public GameForm()
        {
            InitializeComponent();
            //creating array of objects for resizing
            labelGroup = new Label[5] { label1, label2, label3, label4, label5 };
            buttonGroup = new Button[2] { button1, button2 };
            //creating game
            InterfaceElements gameParametres = new InterfaceElements();
            gameParametres.chessboardPic = pictureBox1;
            gameParametres.logBar = richTextBox1;
            gameParametres.lostChessmenPic = pictureBox3;
            gameParametres.takenChessmenPic = pictureBox2;
            gameParametres.prisonersChangeButton = button2;
            gameParametres.surrenderButton = button1;
            gameParametres.statusBar = label4;
            gameParametres.turnBar = label1;
            game = new Game(gameParametres);

            //activate resize for chessboard
            tableLayoutPanel1.SizeChanged += GameForm_ResizeEnd;
            chessmenInfo = new ChessmenInfo(arrangement);
            buttonBackground = Properties.Resources.ButtonBlue;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = buttonBackground;
            (sender as Button).BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = null;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void новаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.StartNewGame();
        }

        //form resize
        private void GameForm_ResizeEnd(object sender, EventArgs e)
        {
            foreach (Label label in labelGroup)
            {
                label.Font = new Font(label.Font.Name, Math.Min(label.Height / 3, label.Width / 3), FontStyle.Regular);
            }
            foreach (Button button in buttonGroup)
            {
                button.Font = new Font(button.Font.Name, Math.Min(button.Height / 6, button.Width / 6), FontStyle.Regular);
            }
            tableLayoutPanel1.Width = 10 * GameForm.ActiveForm.Width / 10 - menuStrip1.Height;
            tableLayoutPanel1.Height = 10 * GameForm.ActiveForm.Height / 10 - menuStrip1.Height * 3;
            if (game.IsGameStarted())
            {
                game.ResizeChessboard();
            }
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Show();
        }
    }
}
