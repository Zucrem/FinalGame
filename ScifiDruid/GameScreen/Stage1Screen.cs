﻿using ScifiDruid.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using System.Diagnostics;
using Box2DNet.Factories;
using Box2DNet;
using Box2DNet.Dynamics;
using ScifiDruid.Managers;
using Box2DNet.Content;
using Box2DNet.Common;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Input;

namespace ScifiDruid.GameScreen
{
    class Stage1Screen : PlayScreen
    {
        //create enemy
        protected List<Enemy> allEnemies;

        protected Enemy flameMech;
        protected List<Enemy> flameMechEnemies;
        protected int flameMechPositionList;
        protected int flameMechCount;

        protected Enemy chainsawMech;
        protected List<Enemy> chainsawMechEnemies;
        protected int chainsawMechPositionList;
        protected int chainsawMechCount;
        public override void Initial()
        {
            base.Initial();

            startmaptileX = 10f;
            endmaptileX = 170f;

            Player.health = 5;
            Player.mana = 100;
            Player.maxHealth = 5;
            Player.maxMana = 100;

            //create tileset for map1
            //map = new TmxMap("Content/stage1test.tmx");
            map = new TmxMap("Content/Stage1.tmx");
            tilesetStage1 = content.Load<Texture2D>("Pictures/Play/StageScreen/Stage1Tileset/" + map.Tilesets[0].Name.ToString());

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;
            tilesetTileWidth = tilesetStage1.Width / tileWidth;

            tilemapManager = new TileMapManager(map, tilesetStage1, tilesetTileWidth, tileWidth, tileHeight);

            //all object lists
            deadBlockRects = new List<Rectangle>();
            blockRects = new List<Rectangle>();
            playerRects = new List<Rectangle>();
            mechanicRects = new List<Rectangle>();
            ground1MonsterRects = new List<Rectangle>();
            ground2MonsterRects = new List<Rectangle>();
            flyMonsterRects = new List<Rectangle>();
            bossRects = new List<Rectangle>();

            polygon = new Dictionary<Polygon, Vector2>();
            //add list rectangle
            foreach (var o in map.ObjectGroups["Blocks"].Objects)
            {
                if (o.Name == "")
                {
                    blockRects.Add(new Rectangle((int)o.X + ((int)o.Width / 2), (int)o.Y + ((int)o.Height / 2), (int)o.Width, (int)o.Height));
                }
                if (o.Name == "triangle")
                {
                    Vertices vertices = new Vertices();
                    foreach (var item in o.Points)
                    {
                        vertices.Add(ConvertUnits.ToSimUnits(new Vector2((float)item.X, (float)item.Y)));
                    }

                    Vector2 position = new Vector2((float)o.X, (float)o.Y);
                    Polygon poly = new Polygon(vertices, true);
                    polygon.Add(poly, position);
                }
            }
            foreach (var o in map.ObjectGroups["Player"].Objects)
            {
                if (o.Name == "startRect")
                {
                    startRect = new Rectangle((int)o.X + ((int)o.Width / 2), (int)o.Y + ((int)o.Height / 2), (int)o.Width, (int)o.Height);
                }
                if (o.Name == "end")
                {
                    endRect = new Rectangle((int)o.X + ((int)o.Width / 2), (int)o.Y + ((int)o.Height / 2), (int)o.Width, (int)o.Height);
                    //playerRects.Add(new Rectangle((int)o.X + ((int)o.Width / 2), (int)o.Y + ((int)o.Height / 2), (int)o.Width, (int)o.Height));
                }
            }
            foreach (var o in map.ObjectGroups["SpecialBlocks"].Objects)
            {
                if (o.Name == "water")
                {
                    deadBlockRects.Add(new Rectangle((int)o.X + ((int)o.Width / 2), (int)o.Y + ((int)o.Height / 2), (int)o.Width, (int)o.Height));
                }
            }
            foreach (var o in map.ObjectGroups["SpecialProps"].Objects)
            {
            }
            foreach (var o in map.ObjectGroups["GroundMonster"].Objects)
            {
                if (o.Name == "ground_mon_1")
                {
                    ground1MonsterRects.Add(new Rectangle((int)o.X + ((int)o.Width / 2), (int)o.Y + ((int)o.Height / 2), (int)o.Width, (int)o.Height));
                }
                if (o.Name == "ground_mon_2")
                {
                    ground2MonsterRects.Add(new Rectangle((int)o.X + ((int)o.Width / 2), (int)o.Y + ((int)o.Height / 2), (int)o.Width, (int)o.Height));
                }
            }
            foreach (var o in map.ObjectGroups["FlyingMonster"].Objects)
            {
            }
            foreach (var o in map.ObjectGroups["Boss"].Objects)
            {
            }

            //create collision for block in the world
            foreach (Rectangle rect in blockRects)
            {
                Vector2 collisionPosition = ConvertUnits.ToSimUnits(new Vector2(rect.X, rect.Y));
                //Singleton.Instance.world.Step(0.001f);

                Body body = BodyFactory.CreateRectangle(Singleton.Instance.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1f, collisionPosition);
                body.UserData = "ground";
                body.Restitution = 0.0f;
                body.Friction = 0.3f;
            }

            foreach (var poly in polygon)
            {
                Vector2 collisionPosition = ConvertUnits.ToSimUnits(poly.Value);
                //Singleton.Instance.world.Step(0.001f);

                Body body = BodyFactory.CreatePolygon(Singleton.Instance.world, poly.Key.Vertices, 1f, collisionPosition);
                body.UserData = "ground";
                body.Restitution = 0.0f;
                body.Friction = 0.0f;
            }
            //create dead block for block in the world
            foreach (Rectangle rect in deadBlockRects)
            {
                Vector2 deadBlockPosition = ConvertUnits.ToSimUnits(new Vector2(rect.X, rect.Y));
                //Singleton.Instance.world.Step(0.001f);

                Body body = BodyFactory.CreateRectangle(Singleton.Instance.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1f, deadBlockPosition);
                body.UserData = "dead";
                body.Restitution = 0.0f;
                body.Friction = 0.3f;
            }


            //create player on position
            player.Initial(startRect);

            //create enemy on position
            allEnemies = new List<Enemy>();

            //ground1
            flameMechEnemies = new List<Enemy>();
            flameMechPositionList = ground1MonsterRects.Count();
            for (int i = 0; i < flameMechPositionList; i++)
            {
                flameMech = new Enemy(flameMechTex)
                {
                    size = new Vector2(112, 86),
                    health = 3,
                    speed = 0.22f,
                    walkSize = new Vector2(112, 86),
                    runSize = new Vector2(0, 0),
                    deadSize = new Vector2(112, 86),
                    walkList = new List<Vector2>() { new Vector2(0, 0), new Vector2(112, 0) },
                    runList = new List<Vector2>(),
                    deadList = new List<Vector2>() { new Vector2(0, 108), new Vector2(112, 108) },
                };
                flameMechEnemies.Add(flameMech);
                allEnemies.Add(flameMech);
            }

            //create enemy position
            flameMechCount = 0;
            foreach (Enemy chainsawBot in flameMechEnemies)
            {
                chainsawBot.Initial(ground1MonsterRects[flameMechCount]);
                flameMechCount++;
            }
            //ground2
            chainsawMechEnemies = new List<Enemy>();
            chainsawMechPositionList = ground2MonsterRects.Count();
            for (int i = 0; i < chainsawMechPositionList; i++)
            {
                chainsawMech = new Enemy(chainsawMechTex)
                {
                    size = new Vector2(118, 100),
                    health = 4,
                    speed = 0.22f,
                    walkSize = new Vector2(118, 100),
                    runSize = new Vector2(136, 100),
                    deadSize = new Vector2(118, 100),
                    walkList = new List<Vector2>() { new Vector2(0, 0), new Vector2(144, 0) },
                    runList = new List<Vector2>() { new Vector2(0, 136), new Vector2(136, 136) },
                    deadList = new List<Vector2>() { new Vector2(0, 254), new Vector2(142, 254) },
                };
                chainsawMechEnemies.Add(chainsawMech);
                allEnemies.Add(chainsawMech);
            }

            //create enemy position
            chainsawMechCount = 0;
            foreach (Enemy chainsawBot in chainsawMechEnemies)
            {
                chainsawBot.Initial(ground2MonsterRects[chainsawMechCount]);
                chainsawMechCount++;
            }
        }
        public override void LoadContent()
        {
            base.LoadContent();

            Initial();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (play)
            {
                if (gamestate == GameState.PLAY)
                {
                    if (!Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        foreach (Enemy flamewBot in flameMechEnemies)
                        {
                            flamewBot.Update(gameTime);
                            flamewBot.EnemyAction();
                        }
                        foreach (Enemy chainsawBot in chainsawMechEnemies)
                        {
                            chainsawBot.Update(gameTime);
                            chainsawBot.EnemyAction();
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //draw tileset for map1
            if (play)
            {
                if (gamestate == GameState.START || gamestate == GameState.PLAY)
                {
                    tilemapManager.Draw(spriteBatch);

                    //draw enemy animation
                    foreach (Enemy flameBot in flameMechEnemies)
                    {
                        flameBot.Draw(spriteBatch);
                    }
                    foreach (Enemy chainsawBot in chainsawMechEnemies)
                    {
                        chainsawBot.Draw(spriteBatch);
                    }

                    if (!fadeFinish)
                    {
                        spriteBatch.Draw(blackTex, Vector2.Zero, colorStart);
                    }
                }
            }
        }
    }
}
