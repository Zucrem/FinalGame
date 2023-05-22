﻿using Box2DNet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScifiDruid.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScifiDruid.GameObjects.JaneBomb;

namespace Verdantia.GameObjects
{
    public class Guardian : _GameObject
    {
        private Texture2D texture;
        private GameTime gameTime;

        //animation
        private int guardianSizeX;
        private int guardianSizeY;
        private Vector2 guardianOrigin;

        private Vector2 guardianSize;

        private List<Vector2> guardianRectVector;

        private int guardianCount;
        //frames 
        private int frames = 0;
        private int allframes;

        //all sprite position in spritesheet
        private Rectangle sourceRect;

        //srpite to run
        private Vector2 spriteSize;
        private List<Vector2> spriteVector;

        //time
        private float elapsed;
        private float delay = 200f;
        public Guardian(Texture2D texture, Vector2 position, Vector2 guardianSpriteSize, List<Vector2> guardianSpriteList) : base(texture)
        {
            this.texture = texture;
            this.position = position;

            //animation
            guardianSize = guardianSpriteSize;

            guardianRectVector = guardianSpriteList;

            guardianSizeX = (int)guardianSize.X;
            guardianSizeY = (int)guardianSize.Y;

            guardianOrigin = new Vector2(guardianSizeX / 2, guardianSizeY / 2);

            spriteSize = guardianSize;
            spriteVector = guardianRectVector;
            allframes = spriteVector.Count();
            Debug.WriteLine(spriteVector[0]);
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime; 
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                if (frames >= allframes - 1)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }
            sourceRect = new Rectangle((int)spriteVector[frames].X, (int)spriteVector[frames].Y, (int)spriteSize.X, (int)spriteSize.Y);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(position), sourceRect, Color.White, 0, guardianOrigin, 1f, charDirection, 0f);
        }
    }
}
