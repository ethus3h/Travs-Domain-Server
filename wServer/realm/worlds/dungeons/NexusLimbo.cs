﻿namespace wServer.realm.worlds
{
    public class NexusLimbo : World
    {
        public NexusLimbo()
        {
            Id = NEXUS_LIMBO;
            Name = "Nexus Tutorial";
            Background = 0;
            Difficulty = 0;
            SetMusic("world/Nexus", "world/Nexus2", "world/Nexus3");
        }

        protected override void Init()
        {
            base.FromWorldMap(
                typeof (RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.dungeons.nexusLimbo.wmap"));
        }
    }
}