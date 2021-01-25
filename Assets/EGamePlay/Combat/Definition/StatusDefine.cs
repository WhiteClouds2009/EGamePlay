namespace EGamePlay.Combat
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    
    public enum StatusType
    {
        [LabelText("Buff(增益)")]
        Buff,
        [LabelText("Debuff(减益)")]
        Debuff,
        [LabelText("其他")]
        Other,
    }

    public enum EffectTriggerType
    {
        [LabelText("立即触发")]
        Immediate = 0,
        [LabelText("条件触发")]
        Condition = 1,
        [LabelText("行动点触发")]
        Action = 2,
        [LabelText("间隔触发")]
        Interval = 3,
    }

    public enum ConditionType
    {
        [LabelText("当x秒内没有受伤")]
        WhenInTimeNoDamage = 0,
        [LabelText("当生命值低于x")]
        WhenHPLower = 1,
        [LabelText("当生命值低于百分比x")]
        WhenHPPctLower = 2,
    }
}