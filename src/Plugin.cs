using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using IL;
using On;
using UnityEngine;

namespace FestiveWorld
{
    [BepInPlugin("FestiveWorld", "Festive world", "0.1.0")]

    public class FestiveWorldMod : BaseUnityPlugin
	{

		public void OnEnable()
		{
            On.GraphicsModule.InitiateSprites += initiateHat;
            On.GraphicsModule.DrawSprites += DrawHat;
        }



        private void initiateHat(On.GraphicsModule.orig_InitiateSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            orig.Invoke(self, sLeaser, rCam);
            List<SantaHat> list;    
            if (SantaHat.hats.TryGetValue(self, out list) && list.Count > 0)
            {
                using (List<SantaHat>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        SantaHat obj = enumerator.Current;
                        rCam.room.AddObject(obj);
                    }
                    return;
                }
            }
            if (self != null)
            {
                if (self is PlayerGraphics)
                {
                    new SantaHat(self, 3, -90f, 5f, false);
                    return;
                }
                if (self is LizardGraphics)
                {
                    new SantaHat(self, 14, 0f, 10f, true);
                    return;
                }
                DaddyGraphics daddyGraphics;
                if ((daddyGraphics = (self as DaddyGraphics)) != null)
                {
                    DaddyGraphics daddyGraphics2 = daddyGraphics;
                    int num = daddyGraphics2.daddy.bodyChunks.Length;
                    int num2 = (int)typeof(DaddyGraphics).GetMethod("BodySprite", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Invoke(daddyGraphics2, new object[]
                    {
                        0
                    });
                    for (int i = 0; i < num; i++)
                    {
                        new SantaHat(self, num2 + i, UnityEngine.Random.value * 360f, daddyGraphics2.daddy.bodyChunks[i].rad, false);
                    }
                    return;
                }
                VultureGraphics vultureGraphics;
                if ((vultureGraphics = (self as VultureGraphics)) != null)
                {
                    VultureGraphics obj2 = vultureGraphics;
                    new SantaHat(self, (int)typeof(VultureGraphics).GetProperty("HeadSprite", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj2, new object[0]), -90f, 10f, false);
                    return;
                }
                MouseGraphics mouseGraphics;
                if ((mouseGraphics = (self as MouseGraphics)) != null)
                {
                    MouseGraphics mouseGraphics2 = mouseGraphics;
                    new SantaHat(self, mouseGraphics2.HeadSprite, -90f, 6f, false);
                    return;
                }
                SpiderGraphics spiderGraphics;
                if ((spiderGraphics = (self as SpiderGraphics)) != null)
                {
                    SpiderGraphics spiderGraphics2 = spiderGraphics;
                    new SantaHat(self, 0, -90f, spiderGraphics2.owner.firstChunk.rad, false);
                    return;
                }
                if (self is BigSpiderGraphics)
                {
                    new SantaHat(self, 1, 0f, 5f, false);
                    return;
                }
                CicadaGraphics cicadaGraphics;
                if ((cicadaGraphics = (self as CicadaGraphics)) != null)
                {
                    CicadaGraphics cicadaGraphics2 = cicadaGraphics;
                    new SantaHat(self, cicadaGraphics2.BodySprite, -90f, 0f, false);
                    return;
                }
                NeedleWormGraphics needleWormGraphics;
                if ((needleWormGraphics = (self as NeedleWormGraphics)) != null)
                {
                    NeedleWormGraphics needleWormGraphics2 = needleWormGraphics;
                    new SantaHat(self, needleWormGraphics2.EyeSprite(0), 180f, 7f, false);
                    return;
                }
                MirosBirdGraphics mirosBirdGraphics;
                if ((mirosBirdGraphics = (self as MirosBirdGraphics)) != null)
                {
                    MirosBirdGraphics obj3 = mirosBirdGraphics;
                    new SantaHat(self, (int)typeof(MirosBirdGraphics).GetProperty("HeadSprite", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj3, new object[0]), 0f, 8f, false);
                    return;
                }
                OracleGraphics oracleGraphics;
                if ((oracleGraphics = (self as OracleGraphics)) != null)
                {
                    OracleGraphics oracleGraphics2 = oracleGraphics;
                    new SantaHat(self, oracleGraphics2.HeadSprite, 90f, 4f, false);
                    return;
                }
                if (self is SnailGraphics)
                {
                    new SantaHat(self, 6, -90f, 11f, false);
                    return;
                }
                DeerGraphics deerGraphics;
                if ((deerGraphics = (self as DeerGraphics)) != null)
                {
                    DeerGraphics obj4 = deerGraphics;
                    new SantaHat(self, (int)typeof(DeerGraphics).GetMethod("BodySprite", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Invoke(obj4, new object[]
                    {
                        0
                    }), 90f, 28f, false);
                    return;
                }
                if (self is JetFishGraphics)
                {
                    new SantaHat(self, 1, -90f, 8f, false);
                    return;
                }
                EggBugGraphics eggBugGraphics;
                if ((eggBugGraphics = (self as EggBugGraphics)) != null)
                {
                    EggBugGraphics eggBugGraphics2 = eggBugGraphics;
                    new SantaHat(self, eggBugGraphics2.HeadSprite, 0f, 6f, false);
                    return;
                }
                ScavengerGraphics scavengerGraphics;
                if ((scavengerGraphics = (self as ScavengerGraphics)) != null)
                {
                    ScavengerGraphics scavengerGraphics2 = scavengerGraphics;
                    new SantaHat(self, scavengerGraphics2.HeadSprite, 90f, 7f, false);
                    return;
                }
                CentipedeGraphics centipedeGraphics;
                if ((centipedeGraphics = (self as CentipedeGraphics)) != null)
                {
                    CentipedeGraphics centipedeGraphics2 = centipedeGraphics;
                    for (int j = 0; j < 2; j++)
                    {
                        new SantaHat(self, centipedeGraphics2.SegmentSprite(UnityEngine.Random.Range(0, centipedeGraphics2.owner.bodyChunks.Length)), 180f * (float)j, 4f, false);
                    }
                    return;
                }
                DropBugGraphics dropBugGraphics;
                if ((dropBugGraphics = (self as DropBugGraphics)) != null)
                {
                    DropBugGraphics dropBugGraphics2 = dropBugGraphics;
                    new SantaHat(self, dropBugGraphics2.HeadSprite, -90f, 7f, false);
                }
            }
        }

        private void DrawHat(On.GraphicsModule.orig_DrawSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            orig.Invoke(self, sLeaser, rCam, timeStacker, camPos);
            List<SantaHat> list;
            if (SantaHat.hats.TryGetValue(self, out list))
            {
                foreach (SantaHat santaHat in list)
                {
                    santaHat.ParentDrawSprites(sLeaser, rCam, timeStacker, camPos);
                }
            }
        }
	}
}



