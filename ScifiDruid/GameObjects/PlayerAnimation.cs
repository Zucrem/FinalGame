﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Box2DNet.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Box2DNet;

namespace ScifiDruid.GameObjects
{
    public class PlayerAnimation : _GameObject
    {
        private Texture2D texture;
        private Vector2 spriteSize;
        private List <Vector2> spriteVector= new List<Vector2>();
        //time
        private float elapsed;
        private float delay = 300f;

        //every player state sprite size
        //idle
        private int idleSrcWidth, idleSrcHeight;
        //shoot
        private int shootSrcWidth, shootSrcHeight;
        //run
        private int runSrcWidth, runSrcHeight;
        //shoot and run
        private int shootAndRunSrcWidth, shootAndRunSrcHeight;
        //shoot up
        private int shootUpSrcWidth, shootUpSrcHeight;
        //shoot up and run
        private int shootUpAndRunSrcWidth, shootUpAndRunSrcHeight;
        //jump
        private int jumpSrcWidth, jumpSrcHeight;
        //shoot on air
        private int shootOnAirSrcWidth, shootOnAirSrcHeight;
        //falling
        private int fallingSrcWidth, fallingSrcHeight;
        //skill
        private int skillSrcWidth, skillSrcHeight;
        //take damage
        private int takeDamageSrcWidth, takeDamageSrcHeight;
        //dead
        private int deadSrcWidth, deadSrcHeight;

        //all sprite position in spritesheet
        private Rectangle sourceRect;

        private List<Vector2> idleRectVector = new List<Vector2>();
        private List<Vector2> shootRectVector = new List<Vector2>();
        private List<Vector2> runRectVector = new List<Vector2>();
        private List<Vector2> shootAndRunRectVector = new List<Vector2>();
        private List<Vector2> shootUpRectVector = new List<Vector2>();
        private List<Vector2> shootUpAndRunRectVector = new List<Vector2>();
        private List<Vector2> jumpRectVector = new List<Vector2>();
        private List<Vector2> shootOnAirRectVector = new List<Vector2>();
        private List<Vector2> fallingRectVector = new List<Vector2>();
        private List<Vector2> skillRectVector = new List<Vector2>();
        private List<Vector2> takeDamageRectVector = new List<Vector2>();
        private List<Vector2> deadRectVector = new List<Vector2>();

        private int idleFrames, shootFrames, runFrames, shootAndRunFrames, shootUpFrames, shootUpAndRunFrames, jumpFrames, shootOnAirFrames, fallingFrames, skillFrames, takeDamageFrames, deadFrames;

        private int posX, posY;
        private int row = 0;
        private int c = 0;
        private int frames = 0;
        private int allframes;


        public PlayerAnimation(Texture2D texture, Vector2 position) : base(texture)
        {
            this.texture = texture;

            //get size of sprite
            //idle
            idleSrcWidth = 46;
            idleSrcHeight = 96;
            //shoot
            shootSrcWidth = 80;
            shootSrcHeight = 94;
            //run
            runSrcWidth = 68;
            runSrcHeight = 94;
            //shoot and run
            shootAndRunSrcWidth = 96;
            shootAndRunSrcHeight = 94;
            //shoot up
            shootUpSrcWidth = 40;
            shootUpSrcHeight = 110;
            //shoot up and run
            shootUpAndRunSrcWidth = 62;
            shootUpAndRunSrcHeight = 110;
            //jump
            jumpSrcWidth = 44;
            jumpSrcHeight = 96;
            //shoot on air
            shootOnAirSrcWidth = 62;
            shootOnAirSrcHeight = 98;
            //falling
            fallingSrcWidth = 74;
            fallingSrcHeight = 106;
            //skill
            skillSrcWidth = 64;
            skillSrcHeight = 94;
            //take damage
            takeDamageSrcWidth = 54;
            takeDamageSrcHeight = 92;
            //dead
            deadSrcWidth = 100;
            deadSrcHeight = 92;

            //position of spritesheet
            //idle vector to list
            idleRectVector.Add(new Vector2(0, 16));
            idleRectVector.Add(new Vector2(46, 16));
            idleRectVector.Add(new Vector2(92, 16));

            idleFrames = idleRectVector.Count();

            //shoot vector to list
            shootRectVector.Add(new Vector2(0, 292));

            shootFrames = shootRectVector.Count();

            //run vector to list
            runRectVector.Add(new Vector2(0, 148));
            runRectVector.Add(new Vector2(68, 148));
            runRectVector.Add(new Vector2(216, 148));
            runRectVector.Add(new Vector2(272, 148));
            runRectVector.Add(new Vector2(340, 148));
            runRectVector.Add(new Vector2(408, 148));

            runFrames = runRectVector.Count();

            //shoot and run vector to list
            shootAndRunRectVector.Add(new Vector2(81, 292));
            shootAndRunRectVector.Add(new Vector2(177, 292));
            shootAndRunRectVector.Add(new Vector2(273, 292));
            shootAndRunRectVector.Add(new Vector2(369, 292));
            shootAndRunRectVector.Add(new Vector2(465, 292));
            shootAndRunRectVector.Add(new Vector2(561, 292));
            shootAndRunRectVector.Add(new Vector2(657, 292));

            shootAndRunFrames = shootAndRunRectVector.Count();

            //shoot up vector to list
            shootUpRectVector.Add(new Vector2(260, 0));

            shootUpFrames = shootUpRectVector.Count();

            //shoot up and run vector to list
            shootUpAndRunRectVector.Add(new Vector2(404, 0));
            shootUpAndRunRectVector.Add(new Vector2(466, 0));
            shootUpAndRunRectVector.Add(new Vector2(528, 0));
            shootUpAndRunRectVector.Add(new Vector2(590, 0));
            shootUpAndRunRectVector.Add(new Vector2(652, 0));
            shootUpAndRunRectVector.Add(new Vector2(714, 0));
            shootUpAndRunRectVector.Add(new Vector2(776, 0));

            shootUpAndRunFrames = shootUpAndRunRectVector.Count();

            //jump vector to list
            jumpRectVector.Add(new Vector2(0, 420));
            jumpRectVector.Add(new Vector2(44, 420));

            jumpFrames = jumpRectVector.Count();

            //shoot on air vector to list
            shootOnAirRectVector.Add(new Vector2(52, 418));
            shootOnAirRectVector.Add(new Vector2(234, 418));

            shootOnAirFrames = shootOnAirRectVector.Count();

            //falling vector to list
            fallingRectVector.Add(new Vector2(432, 416));
            fallingRectVector.Add(new Vector2(504, 416));

            fallingFrames = fallingRectVector.Count();

            //skill vector to list
            skillRectVector.Add(new Vector2(672, 422));
            skillRectVector.Add(new Vector2(736, 422));

            skillFrames = skillRectVector.Count();

            //take damage vector to list
            takeDamageRectVector.Add(new Vector2(32, 570));

            takeDamageFrames = takeDamageRectVector.Count();

            //dead vector to list
            deadRectVector.Add(new Vector2(0, 570));
            deadRectVector.Add(new Vector2(100, 570));
            deadRectVector.Add(new Vector2(200, 570));
            deadRectVector.Add(new Vector2(300, 570));
            deadRectVector.Add(new Vector2(400, 570));
            deadRectVector.Add(new Vector2(500, 570));
            deadRectVector.Add(new Vector2(600, 570));
            deadRectVector.Add(new Vector2(700, 570));
            deadRectVector.Add(new Vector2(800, 570));

            deadFrames = deadRectVector.Count();

            //get start position
            this.posX = (int)position.X;
            this.posY = (int)position.Y;

        }

        public void Initialize()
        {
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            spriteVector = idleRectVector;
            spriteSize = new Vector2(idleSrcWidth, idleSrcHeight);
            allframes = idleFrames;
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
        public void Draw(SpriteBatch spriteBatch, Vector2 playerOrigin, SpriteEffects charDirection, Vector2 position)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, playerOrigin, 1f, charDirection, 0f);
        }
    }
}
