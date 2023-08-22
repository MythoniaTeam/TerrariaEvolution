



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

        }

        Vector2 velTemp;
        Vector2 posTemp;
        bool hasTemp = false;

        Color color;
        float c;

        bool update;
        bool updated;

        public override bool PreAI(NPC npc)
        {
            //DEBUG
            //timeScale = 0.1f;
            //return false;//true;

            MDebug.Print($"PreAI", Color.Violet);
            if (npc.type == NPCID.BaldZombie) return true;

            
            timeScale = 1f;
            //timeScale *= 0.99f;
            c = (Main.GameUpdateCount - myUpdateCount >= 1 / timeScale) ? 1 : 0.5f;


            //MDebug.Print($"{npc.TypeName}: {myUpdateCount}, {Main.GameUpdateCount}", Color.White * c);
            MDebug.Print($"Vel: {npc.velocity.X / 16 , 6:0.00}, {npc.velocity.Y / 16 , 6:0.00} ({velTemp.X / 16 , 6:0.00}, {velTemp.Y / 16 , 6:0.00})", Color.White * c);
            MDebug.Print($"Pos: {npc.position.X / 16 , 6:0.00}, {npc.position.Y / 16 , 6:0.00} ({posTemp.X / 16 , 6:0.00}, {posTemp.Y / 16 , 6:0.00}), HasTemp: {hasTemp}", Color.White * c);
            //return true;

            //如果达到一定间隔，开始更新
            if (Main.GameUpdateCount - myUpdateCount >= 1 / timeScale)
            {
                //MDebug.Print($"{npc.TypeName}: Vel = {velTemp}", Color.Green * c);
                //npc.velocity = velTemp;
                //velTemp = Vector2.Zero;

                //如果之前没有更新
                if(update == false)
                {
                    //如果有 位置 速度 记录，将记录调出
                    if(hasTemp)
                    {
                        MDebug.Print($"Vel = ({velTemp.X / 16, 6:0.00}, {velTemp.Y / 16, 6:0.00})", Color.SkyBlue * c);
                        MDebug.Print($"Pos = ({posTemp.X / 16, 6:0.00}, {posTemp.Y / 16, 6:0.00})", Color.SkyBlue * c);

                        npc.position = posTemp;
                        npc.velocity = velTemp;
                        hasTemp = false;
                    }
                    update = true;
                }
                
                myUpdateCount += MathF.Ceiling((Main.GameUpdateCount - myUpdateCount) * timeScale) / timeScale;

                return true;
            }
            //否则不更新
            else
            {
                //MDebug.Print($"{npc.TypeName}: Vel = 0", Color.Turquoise * c);
                //npc.velocity = Vector2.Zero;
                if(update == true)
                {
                    MDebug.Print($"Vel = ({npc.velocity.X / 16 , 6:0.00}, {npc.velocity.Y / 16 , 6:0.00})", Color.Pink * c);
                    MDebug.Print($"Pos = ({npc.position.X / 16 , 6:0.00}, {npc.position.Y / 16 , 6:0.00})", Color.Pink * c);
                    posTemp = npc.position;
                    velTemp = npc.velocity;
                    hasTemp = true;
                    update = false;
                }

                //将速度设为0，将位置固定在记录的位置
                if (hasTemp)
                {
                    npc.position = posTemp;
                }
                npc.velocity = Vector2.Zero;

                return false;
            } 
        }

        public override void PostAI(NPC npc)
        {
            MDebug.Print($"PostAI", Color.Violet);
            MDebug.Print($"Vel: {npc.velocity.X / 16 , 6:0.00}, {npc.velocity.Y / 16 , 6:0.00} ({velTemp.X / 16 , 6:0.00}, {velTemp.Y / 16 , 6:0.00})", Color.White * c);
            MDebug.Print($"Pos: {npc.position.X / 16 , 6:0.00}, {npc.position.Y / 16 , 6:0.00} ({posTemp.X / 16 , 6:0.00}, {posTemp.Y / 16 , 6:0.00}), HasTemp: {hasTemp}", Color.White * c);

            //DEBUG
            //npc.position -= npc.velocity;
            //npc.velocity = Vector2.Zero;
            return;

            npc.position -= npc.velocity * (1 - timeScale);



            if(update)
            {
                npc.position -= npc.velocity * (1 - timeScale);
                
            }
            else
            {
                posTemp += velTemp * timeScale;
            }

            //npc.position -= npc.velocity * (1 - timeScale);


            if (npc.type == NPCID.BaldZombie) return;


            updated = update;
        }

    }
}
