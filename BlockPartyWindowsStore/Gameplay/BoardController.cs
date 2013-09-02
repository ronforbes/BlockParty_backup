using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class BoardController
    {
        Board board;
        
        int selectedRow = -1, selectedColumn = -1;

        TimeSpan previousClickTime = TimeSpan.Zero;
        int previousClickX, previousClickY;

        readonly TimeSpan doubleClickDuration = TimeSpan.FromSeconds(1);
        const float doubleClickDistance = 5;

        public BoardController(Board board)
        {
            this.board = board;
        }

        public void HandleInput(GameTime gameTime)
        {
            // If the mouse has been pressed over a valid block, select the block that it's hovering over
            if (board.Screen.ScreenManager.InputManager.LeftButtonPressed)
            {
                // Determine which block the mouse is hovering over
                int row = (board.Screen.ScreenManager.InputManager.WorldY - board.Renderer.Rectangle.Y + (int)(board.Blocks[0, 0].Renderer.Height * (float)(board.RaiseTimeElapsed.TotalMilliseconds / board.RaiseDuration.TotalMilliseconds))) / board.Blocks[0, 0].Renderer.Height;
                int column = (board.Screen.ScreenManager.InputManager.WorldX - board.Renderer.Rectangle.X) / board.Blocks[0, 0].Renderer.Width;

                if (row >= 0 && row < board.Rows && column >= 0 && column < board.Columns)
                {
                    if (board.Blocks[row, column].State == Block.BlockState.Idle)
                    {
                        selectedRow = row;
                        selectedColumn = column;
                        board.Blocks[selectedRow, selectedColumn].Press();
                    }
                }

                // Determine if the mouse was double pressed
                if (gameTime.TotalGameTime - previousClickTime < doubleClickDuration)
                {
                    if (Vector2.Distance(new Vector2(board.Screen.ScreenManager.InputManager.WorldX, board.Screen.ScreenManager.InputManager.WorldY), new Vector2(previousClickX, previousClickY)) < doubleClickDistance)
                    {
                        board.RaiseRate = board.RaiseRateAccelerated;
                        board.StopTimeRemaining = TimeSpan.Zero;
                    }
                }

                previousClickTime = gameTime.TotalGameTime;
                previousClickX = board.Screen.ScreenManager.InputManager.WorldX;
                previousClickY = board.Screen.ScreenManager.InputManager.WorldY;
            }

            // If a block is selected, swap it based on the mouse's position
            if (selectedRow != -1 && selectedColumn != -1)
            {
                // Swap the blocks if the mouse has moved to a different column
                if (board.Screen.ScreenManager.InputManager.WorldX - board.Renderer.Rectangle.X < selectedColumn * board.Blocks[0, 0].Renderer.Width &&
                    selectedColumn - 1 >= 0 &&
                    board.Blocks[selectedRow, selectedColumn].State == Block.BlockState.Idle &&
                    (board.Blocks[selectedRow, selectedColumn - 1].State == Block.BlockState.Idle ||
                    board.Blocks[selectedRow, selectedColumn - 1].State == Block.BlockState.Empty))
                {
                    // Swap the selected block with the one on its left
                    board.Blocks[selectedRow, selectedColumn].SetupSlide(Block.BlockSlideDirection.Left, board.Blocks[selectedRow, selectedColumn - 1]);
                    board.Blocks[selectedRow, selectedColumn - 1].SetupSlide(Block.BlockSlideDirection.Right, board.Blocks[selectedRow, selectedColumn]);

                    board.Blocks[selectedRow, selectedColumn].Slide();
                    board.Blocks[selectedRow, selectedColumn - 1].Slide();
                    board.Screen.ScreenManager.AudioManager.Play("BlockSlide", 1.0f, 0.0f, 0.0f);

                    selectedColumn--;
                }

                if (board.Screen.ScreenManager.InputManager.WorldX - board.Renderer.Rectangle.X > (selectedColumn + 1) * board.Blocks[0, 0].Renderer.Width &&
                    selectedColumn + 1 < board.Columns &&
                    board.Blocks[selectedRow, selectedColumn].State == Block.BlockState.Idle &&
                    (board.Blocks[selectedRow, selectedColumn + 1].State == Block.BlockState.Idle ||
                    board.Blocks[selectedRow, selectedColumn + 1].State == Block.BlockState.Empty))
                {
                    // Swap the selected block with the one on its right
                    board.Blocks[selectedRow, selectedColumn].SetupSlide(Block.BlockSlideDirection.Right, board.Blocks[selectedRow, selectedColumn + 1]);
                    board.Blocks[selectedRow, selectedColumn + 1].SetupSlide(Block.BlockSlideDirection.Left, board.Blocks[selectedRow, selectedColumn]);

                    board.Blocks[selectedRow, selectedColumn].Slide();
                    board.Blocks[selectedRow, selectedColumn + 1].Slide();
                    board.Screen.ScreenManager.AudioManager.Play("BlockSlide", 1.0f, 0.0f, 0.0f);

                    selectedColumn++;
                }
            }

            // Deselect the block if the mouse button is no longer being held
            if (board.Screen.ScreenManager.InputManager.LeftButtonReleased && selectedRow != -1 && selectedColumn != -1)
            {
                board.Blocks[selectedRow, selectedColumn].Release();
                selectedRow = -1;
                selectedColumn = -1;
            }
        }
    }
}
