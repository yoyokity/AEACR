﻿using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.Opener;

public class Opener80 : IOpener
{
    public int StartCheck()
    {
        if (!SgeHelper.均衡中) return -1;
        return 0;
    }

    public int StopCheck(int index)
    {
        return -1;
    }

    public List<Action<Slot>> Sequence { get; } =
    [
        Step1
    ];

    private static SpellTargetType 单盾目标(int 倒数第几 = 1) =>
        PartyHelper.Party.Count switch
        {
            8 when 倒数第几 == 1 => SpellTargetType.Pm8,
            8 when 倒数第几 == 2 => SpellTargetType.Pm7,
            4 when 倒数第几 == 1 => SpellTargetType.Pm4,
            4 when 倒数第几 == 2 => SpellTargetType.Pm3,
            _ => SpellTargetType.Self
        };

    public void InitCountDown(CountDownHandler countDownHandler)
    {
        Qt.Reset();

        const int startTime = 15000;
        if (!SgeHelper.学者搭档)
        {
            countDownHandler.AddAction(startTime, Data.Spells.均衡);
            countDownHandler.AddAction(startTime - 1000, Data.Spells.均衡诊断, 单盾目标());
            countDownHandler.AddAction(startTime - 2500, Data.Spells.均衡);
            countDownHandler.AddAction(startTime - 3500, Data.Spells.均衡诊断, 单盾目标(2));
            countDownHandler.AddAction(startTime - 5000, Data.Spells.活化);

            countDownHandler.AddAction(6000, Data.Spells.均衡);
            countDownHandler.AddAction(5000, Data.Spells.均衡预后adaptive);
        }
        else
        {
            var target = Helper.GetMt();
            if (target != null && Data.Spells.混合.IsUnlock())
                countDownHandler.AddAction(startTime, () => Data.Spells.混合.GetSpell(target));
        }

        countDownHandler.AddAction(3500, Data.Spells.均衡);
        if (Qt.Instance.GetQt("爆发药"))
            countDownHandler.AddPotionAction(1000);
    }

    private static void Step1(Slot slot)
    {
        slot.Add(Data.Spells.均衡注药adaptive.GetSpell());
    }
}