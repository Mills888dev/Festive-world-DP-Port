using System;
using System.Collections.Generic;
using RWCustom;
using UnityEngine;

namespace FestiveWorld
{
    public class SantaHat : UpdatableAndDeletable, IDrawable
    {
        public SantaHat(GraphicsModule parent, int anchorSprite, float rotation, float headRadius, bool flipFlips)
        {
            Debug.Log("Created a hat");
            this.parent = parent;
            this.anchorSprite = anchorSprite;
            this.rotation = rotation;
            this.headRadius = headRadius;
            this.flipFlips = flipFlips;
            List<SantaHat> list;
            if (!SantaHat.hats.TryGetValue(parent, out list))
            {
                list = new List<SantaHat>();
                SantaHat.hats[parent] = list;
            }
            list.Add(this);
            parent.owner.room.AddObject(this);
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[3];
            TriangleMesh.Triangle[] tris = new TriangleMesh.Triangle[]
            {
                new TriangleMesh.Triangle(0, 1, 2),
                new TriangleMesh.Triangle(1, 2, 3),
                new TriangleMesh.Triangle(2, 3, 4),
                new TriangleMesh.Triangle(3, 4, 5),
                new TriangleMesh.Triangle(4, 5, 6),
                new TriangleMesh.Triangle(5, 6, 7),
                new TriangleMesh.Triangle(6, 7, 8)
            };
            TriangleMesh triangleMesh = new TriangleMesh("Futile_White", tris, false, false);
            sLeaser.sprites[0] = triangleMesh;
            sLeaser.sprites[1] = new FSprite("JetFishEyeA", true);
            sLeaser.sprites[2] = new FSprite("LizardScaleA6", true);
            this.AddToContainer(sLeaser, rCam, null);
        }

        public void ParentDrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            if (sLeaser.sprites.Length > this.anchorSprite)
            {
                this.basePos.Set(sLeaser.sprites[this.anchorSprite].x, sLeaser.sprites[this.anchorSprite].y);
                this.baseRot = sLeaser.sprites[this.anchorSprite].rotation;
                if (this.flipFlips)
                {
                    this.flipX = (sLeaser.sprites[this.anchorSprite].scaleY > 0f);
                    this.flipY = (sLeaser.sprites[this.anchorSprite].scaleX < 0f);
                }
                else
                {
                    this.flipX = (sLeaser.sprites[this.anchorSprite].scaleX > 0f);
                    this.flipY = (sLeaser.sprites[this.anchorSprite].scaleY < 0f);
                }
                MirosBirdGraphics mirosBirdGraphics;
                NeedleWormGraphics needleWormGraphics;
                EggBugGraphics eggBugGraphics;
                if ((mirosBirdGraphics = (this.parent as MirosBirdGraphics)) != null)
                {
                    if (((MirosBird)mirosBirdGraphics.owner).Head.pos.x > ((MirosBird)mirosBirdGraphics.owner).mainBodyChunk.pos.x)
                    {
                        this.flipY = !this.flipY;
                        return;
                    }
                }
                else if ((needleWormGraphics = (this.parent as NeedleWormGraphics)) != null)
                {
                    if (((NeedleWorm)needleWormGraphics.owner).lookDir.x < 0f)
                    {
                        this.flipY = !this.flipY;
                        return;
                    }
                }
                else if ((eggBugGraphics = (this.parent as EggBugGraphics)) != null && eggBugGraphics.bug.bodyChunks[0].pos.x > eggBugGraphics.bug.bodyChunks[1].pos.x)
                {
                    this.flipY = !this.flipY;
                }
            }
        }

        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            Vector2 vector = this.basePos;
            Vector2 vector2 = new Vector2(Mathf.Cos((this.rotation + this.baseRot) * -0.017453292f), Mathf.Sin((this.rotation + this.baseRot) * -0.017453292f));
            Vector2 a = -Custom.PerpendicularVector(vector2);
            if (this.flipY)
            {
                vector2 *= -1f;
            }
            if (this.flipX)
            {
                a *= -1f;
            }
            vector += vector2 * this.headRadius;
            Vector2 vector3 = vector + vector2 * 20f;
            sLeaser.sprites[2].SetPosition(vector);
            sLeaser.sprites[2].rotation = this.rotation + this.baseRot;
            sLeaser.sprites[2].scaleY = (this.flipX ? -1f : 1f);
            Vector2 vector4 = this.tuftPos;
            if (!Custom.DistLess(this.tuftPos, vector3, 20f))
            {
                this.tuftPos = vector3 + (this.tuftPos - vector3).normalized * 20f;
                if (!Custom.DistLess(this.lastTuftPos, this.tuftPos, 20f))
                {
                    this.lastTuftPos = this.tuftPos + (this.lastTuftPos - this.tuftPos).normalized * 20f;
                }
            }
            sLeaser.sprites[1].SetPosition(Vector2.Lerp(this.lastTuftPos, this.tuftPos, timeStacker));
            TriangleMesh triangleMesh = (TriangleMesh)sLeaser.sprites[0];
            Vector2 vector5 = vector - a * 7f;
            Vector2 vector6 = vector + a * 7f;
            Vector2 vector7 = Vector2.Lerp(vector5, vector3, 0.5f);
            Vector2 vector8 = Vector2.Lerp(vector6, vector3, 0.5f);
            int i = 0;
            int num = triangleMesh.vertices.Length;
            while (i < num)
            {
                bool flag = i % 2 == 1;
                float t = (float)(i / 2) / (float)(num - 1) * 2f;
                Vector2 pos = Vector2.Lerp(Vector2.Lerp(flag ? vector6 : vector5, flag ? vector8 : vector7, t), Vector2.Lerp(flag ? vector8 : vector7, Vector2.Lerp(this.lastTuftPos, this.tuftPos, timeStacker), t), t);
                triangleMesh.MoveVertice(i, pos);
                i++;
            }
            if (this.parent.culled && !this.parent.lastCulled)
            {
                for (int j = 0; j < 3; j++)
                {
                    sLeaser.sprites[0].isVisible = !this.parent.culled;
                }
            }
            if (base.slatedForDeletetion || rCam.room != this.room || this.room != this.parent.owner.room)
            {
                sLeaser.CleanSpritesAndRemove();
            }
        }

        public override void Update(bool eu)
        {
            base.Update(eu);
            this.lastTuftPos = this.tuftPos;
            List<SantaHat> list;
            if (!SantaHat.hats.TryGetValue(this.parent, out list) || !list.Contains(this))
            {
                this.Destroy();
            }
            GraphicsModule graphicsModule = this.parent;
            if (((graphicsModule != null) ? graphicsModule.owner : null) == null || this.parent.owner.slatedForDeletetion || base.slatedForDeletetion)
            {
                this.Destroy();
            }
            else if (this.parent.owner.room != null)
            {
                Vector2 vector = this.basePos;
                Vector2 vector2 = new Vector2(Mathf.Cos((this.rotation + this.baseRot) * -0.017453292f), Mathf.Sin((this.rotation + this.baseRot) * -0.017453292f));
                Vector2 vector3 = -Custom.PerpendicularVector(vector2);
                if (this.flipY)
                {
                    vector2 *= -1f;
                }
                if (this.flipX)
                {
                    vector3 *= -1f;
                }
                vector += vector2 * 20f;
                this.tuftVel.y = this.tuftVel.y - this.parent.owner.gravity;
                this.tuftVel += vector3 * ((Vector2.Dot(vector3, this.tuftPos - vector) > 0f) ? 1.5f : -1.5f);
                this.tuftVel += (vector - this.tuftPos) * 0.2f;
                this.tuftVel *= 0.6f;
                this.tuftPos += this.tuftVel;
                if (!Custom.DistLess(this.tuftPos, vector, 13f))
                {
                    this.tuftPos = vector + (this.tuftPos - vector).normalized * 13f;
                }
            }
            if (base.slatedForDeletetion)
            {
                Debug.Log("Destroying hat");
                base.RemoveFromRoom();
                List<SantaHat> list2;
                if (SantaHat.hats.TryGetValue(this.parent, out list2))
                {
                    list2.Remove(this);
                    if (list2.Count == 0)
                    {
                        SantaHat.hats.Remove(this.parent);
                    }
                }
            }
        }

        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            if (newContatiner == null)
            {
                newContatiner = rCam.ReturnFContainer("Items");
            }
            for (int i = 0; i < sLeaser.sprites.Length; i++)
            {
                sLeaser.sprites[i].RemoveFromContainer();
            }
            for (int j = 0; j < sLeaser.sprites.Length; j++)
            {
                newContatiner.AddChild(sLeaser.sprites[j]);
            }
        }

        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            sLeaser.sprites[0].color = Color.red;
            sLeaser.sprites[1].color = Color.white;
            sLeaser.sprites[2].color = Color.white;
        }

        public static Dictionary<GraphicsModule, List<SantaHat>> hats = new Dictionary<GraphicsModule, List<SantaHat>>();

        public GraphicsModule parent;

        public int anchorSprite;

        public float rotation;

        public float headRadius;

        public bool flipX;

        public bool flipY;

        public bool flipFlips;

        public Vector2 basePos = Vector2.zero;

        public float baseRot;

        public Vector2 tuftPos = Vector2.zero;

        public Vector2 lastTuftPos = Vector2.zero;

        public Vector2 tuftVel = Vector2.zero;
    }
}
