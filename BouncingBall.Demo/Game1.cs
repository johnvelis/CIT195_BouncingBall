using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BouncingBall.Demo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //
        // Use persistance in later version of the game
        //
        #region GLOBAL VARIABLES

        private SpriteBatch _spriteBatch;

        private Texture2D _ball;
        private Texture2D _wall;

        private int _ballXSpeed;
        private int _ballYSpeed;

        private int _gridWidth;
        private int _gridHeight;

        private int _mapGridRowCount;
        private int _mapGridColCount;

        private Vector2 _ballPos;

        private int _score;

        private MouseState _mouseState;

        #endregion

        #region GAME CONSTRUCTOR

        public Game1()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);

            _gridWidth = 64;
            _gridHeight = 64;
            _mapGridRowCount = 8;
            _mapGridColCount = 12;

            graphics.PreferredBackBufferWidth = _mapGridColCount * _gridWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = (_mapGridRowCount + 3) * _gridHeight;  // set this value to the desired height of your window
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

        }

        #endregion

        #region GAME Initialize METHOD

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _ballXSpeed = 4;
            _ballYSpeed = 4;
            _ballPos.X = 100;
            _ballPos.Y = 200;
            _score = 0;

            IsMouseVisible = true;

            base.Initialize();
        }
        #endregion

        #region GAME LoadContent METHOD

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _ball = Content.Load<Texture2D>("ball");
            _wall = Content.Load<Texture2D>("wall");

        }

        #endregion

        #region GAME UnloadContent METHOD

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region GAME Update METHOD

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            _mouseState = Mouse.GetState();

            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (MouseOnBall())
                {
                    _score = _score + 1;

                    Random ballXPosRandom = new Random();
                    Random ballYPosRandom = new Random();

                    _ballPos.X = ballXPosRandom.Next(100,400);
                    _ballPos.Y = ballYPosRandom.Next(100, 400);
                }
            }

            MoveBall();

            base.Update(gameTime);
        }
        #endregion

        #region GAME Draw METHOD

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_ball, _ballPos, Color.White);
            _spriteBatch.End();

            BuildMap();

            base.Draw(gameTime);
        }

        #endregion

        #region HELPER METHODS
        private void BuildMap()
        {
            _spriteBatch.Begin();

            // Draw top and bottom walls
            for (int col = 0; col < _mapGridColCount; col++)
            {
                int wallXPos = col * _gridWidth;
                int topWallYPos = 0;
                int bottomWallYPos = (_mapGridRowCount - 1) * _gridHeight;

                _spriteBatch.Draw(_wall, new Vector2(wallXPos, topWallYPos), Color.White);
                _spriteBatch.Draw(_wall, new Vector2(wallXPos, bottomWallYPos), Color.White);
            }

            // Draw side walls
            for (int row = 0; row < _mapGridRowCount - 2; row++)
            {
                int wallYPos = (row + 1) * _gridHeight;
                int leftWallXPos = 0;
                int rightWallXPos = (_mapGridColCount - 1) * _gridWidth;

                _spriteBatch.Draw(_wall, new Vector2(leftWallXPos, wallYPos), Color.White);
                _spriteBatch.Draw(_wall, new Vector2(rightWallXPos, wallYPos), Color.White);
            }

            _spriteBatch.End();
        }
        private void MoveBall()
        {
            if (_ballPos.X <= _gridWidth || _ballPos.X >= (_mapGridColCount - 2) * _gridWidth)
            {
                _ballXSpeed = -_ballXSpeed;
            }

            if (_ballPos.Y <= _gridHeight || _ballPos.Y >= (_mapGridRowCount - 2) * _gridHeight)
            {
                _ballYSpeed = -_ballYSpeed;
            }

            _ballPos.X = _ballPos.X + _ballXSpeed;
            _ballPos.Y = _ballPos.Y + _ballYSpeed;
        }

        private bool MouseOnBall()
        {
            _mouseState = Mouse.GetState();

            if (_mouseState.X < _ballPos.X) return false;
            if (_mouseState.X > (_ballPos.X + _ball.Width)) return false;
            if (_mouseState.Y < _ballPos.Y) return false;
            if (_mouseState.Y > (_ballPos.Y + _ball.Height)) return false;
            return true;
        }

        #endregion
    }
}
