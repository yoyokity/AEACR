﻿using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Base3_坠喷泉落血雨_高优先 : ISlotResolver
{
    public int Check()
    {
        if (!Data.Spells.坠喷泉.IsUnlock()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.普通4预备) && !Core.Me.HasAura(Data.Buffs.百花4预备)) return -1;
        
        //判断是否即将过期
        if (DncHelper.AuraInGCDs(Data.Buffs.普通4预备, 1))
            return 1;
        if (DncHelper.AuraInGCDs(Data.Buffs.百花4预备, 1))
            return 2;

        return -5;
    }

    private static uint GetSpells()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);

        if (Qt.Instance.GetQt("AOE") && Data.Spells.落血雨.IsUnlock() &&
            enemyCount >= 3)
            return Data.Spells.落血雨;

        return Data.Spells.坠喷泉;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpells().GetSpell());
    }
}