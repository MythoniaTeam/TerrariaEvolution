



using System.Diagnostics;
using Terraria;
using TerrariaRebirth.Utilities;
using Terraria.ID;

namespace Evolution.Entities
{
    class player: ModPlayer
    {
        public override void PreUpdate()
        {
            MDebug.Print($"=== Frame: {Main.GameUpdateCount} ===", color:Color.Red);
        }

        
    }

    internal class MGlobalNPC : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        static float timeScale = 0.5f;

        float myUpdateCount;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            base.OnSpawn(npc, source);
            myUpdateCount = Main.GameUpdateCount;

            MDebug.Print($"{npc.TypeName}: PosTemp = {npc.position}", Color.Yellow);

            rotationTempAfterAI = npc.rotation;

        }

        Vector2 velTemp;
        Vector2 posTemp;
        double frameTemp;
        float rotationTempPreAI;
        float rotationTempAfterAI;
        float rotationDeltaTemp;
        bool hasTemp = false;

        Color color;
        float c;

        bool update;
        bool updated;

        float rot;
        int dir;

        public override bool PreAI(NPC npc)
        {
            //if (npc.spriteDirection != 1)

            MDebug.Print($"({npc.frameCounter})", Color.Yellow);
            //return true;


            npc.position -= npc.velocity * (1 - timeScale);
            
            if (update) velTemp = npc.velocity;
            else npc.velocity = velTemp;

            if(npc.frameCounter != 0) npc.frameCounter = (npc.frameCounter - frameTemp) * timeScale;

            //DEBUG
            //timeScale = 0.1f;
            //return false;//true;



            timeScale = 0.1f;


            c = (Main.GameUpdateCount - myUpdateCount >= 1 / timeScale) ? 1 : 0.5f;

            //npc.rotation += 10;
            //if(npc.directionY != -1) 
                //MDebug.Print($"Rotation: {npc.directionY}", Color.Yellow * c);
            //MDebug.Print($"Pre: {npc.rotation / MathF.PI * 180, 8:0.0}, ({rotationTemp / MathF.PI * 180,8:0.0}), dr {rotationDeltaTemp / MathF.PI * 180,8:0.0}", Color.White * c);
            

            //如果达到一定间隔，开始更新
            if (Main.GameUpdateCount - myUpdateCount >= 1 / timeScale)
            {
                rotationTempPreAI = npc.rotation;
                npc.rotation = rotationTempAfterAI;

                myUpdateCount += MathF.Ceiling((Main.GameUpdateCount - myUpdateCount) * timeScale) / timeScale;
                update = true;
                return true;
            }
            //否则不更新
            else
            {
                update = false;
                return false;
            } 
        }

        Vector2 posTemp2;


        public override void AI(NPC npc)
        {
            //rotationDeltaTemp = npc.rotation - rotationTemp;
            //npc.rotation = rotationTemp;
            rotationTempAfterAI = npc.rotation;
        }

        public override void PostAI(NPC npc)
        {
            MDebug.Print($"({npc.frameCounter})", Color.Yellow);

            //npc.ai[1] = 0;

            //return;

            //npc.directionY = 1;
            //npc.spriteDirection = 1;

            //return;

            //MDebug.Print($"PostStart: {npc.rotation / MathF.PI * 180,8:0.0}, ({rotationTemp / MathF.PI * 180,8:0.0}), dr {rotationDeltaTemp / MathF.PI * 180,8:0.0}", Color.White * c);

            if (update)
            {
                rotationDeltaTemp = npc.rotation - rotationTempPreAI;
                rotationDeltaTemp %= 2 * MathF.PI;
                MDebug.Print($"  = {rotationDeltaTemp / MathF.PI * 180,8:0.0}", Color.Purple * c);

                if (MathF.Abs(rotationDeltaTemp) > MathF.PI)
                {
                    MDebug.Print($"  = {rotationDeltaTemp / MathF.PI * 180,8:0.0}", Color.Purple * c);

                    rotationDeltaTemp -= MathF.Sign(rotationDeltaTemp) * MathF.PI * 2;
                    MDebug.Print($"  = {rotationDeltaTemp / MathF.PI * 180,8:0.0}", Color.Red * c);

                }
                npc.rotation = rotationTempPreAI;
            }
            //MDebug.Print($"PostMid: {npc.rotation / MathF.PI * 180,8:0.0}, ({rotationTemp / MathF.PI * 180,8:0.0}), dr {rotationDeltaTemp / MathF.PI * 180,8:0.0}", Color.White * c);
            npc.rotation += rotationDeltaTemp * timeScale;

            frameTemp = npc.frameCounter;

            //posTemp2 = npc.position;
            //if(!updated) npc.position = posTemp2;

            //if (npc.spriteDirection != dir)
            //{
            //    //MDebug.Print($"PostEnd: {npc.direction}, {npc.directionY} ({npc.spriteDirection})", Color.Yellow * c);
            //    //MDebug.Print($"PostEnd: {npc.rotation / MathF.PI * 180 % 360,8:0.0}, ({rotationTemp / MathF.PI * 180 %360,8:0.0}), dr {rotationDeltaTemp / MathF.PI * 180,8:0.0}", Color.White * c);
            //    updated = false;
            //    //npc.velocity = Vector2.Zero;
            //    posTemp2 = npc.position;

            //}
            //dir = npc.spriteDirection;


            //npc.rotation = rot;
            //rot += 1 * MathF.PI / 180;

            //npc.spriteDirection = -1;

            
        }

    }
}
