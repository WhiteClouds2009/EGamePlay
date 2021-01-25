namespace EGamePlay.Combat
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using System.ComponentModel;

    [LabelText("属性类型")]
    public enum AttributeType
    {
        [LabelText("无")]
        [Description("无")]
        None,

        [LabelText("力量")]
        [Description("力量")]
        Strength,

        [LabelText("敏捷")]
        [Description("敏捷")]
        Agility,

        [LabelText("智力")]
        [Description("智力")]
        Intellect,

        [LabelText("最大生命值")]
        [Description("最大生命值")]
        HpMax,

        [LabelText("[每点力量]最大生命值")]
        [Description("[每点力量]最大生命值")]
        HpMaxGrowth,

        [LabelText("最大法术值")]
        [Description("最大法术值")]
        MpMax,

        [LabelText("[每点智力]最大法术值")]
        [Description("[每点智力]最大法术值")]
        MpMaxGrowth,

        [LabelText("生命恢复")]
        [Description("生命恢复")]
        HpRegeneration,

        [LabelText("[每点力量]生命恢复")]
        [Description("[每点力量]生命恢复")]
        HpRegenerationGrowth,

        [LabelText("法力恢复")]
        [Description("法力恢复")]
        MpRegeneration,

        [LabelText("[每点智力]法力恢复")]
        [Description("[每点智力]法力恢复")]
        MpRegenerationGrowth,

        [LabelText("护甲")]
        [Description("护甲")]
        Armor,

        [LabelText("[每点敏捷]护甲")]
        [Description("[每点敏捷]护甲")]
        ArmorGrowth,

        [LabelText("抗性")]
        [Description("抗性")]
        Resistance,

        [LabelText("[每点力量]抗性")]
        [Description("[每点力量]抗性")]
        ResistanceGrowth,

        [LabelText("移动速度")]
        [Description("移动速度")]
        MoveSpeed,

        [LabelText("[每点敏捷]移动速度")]
        [Description("[每点敏捷]移动速度")]
        MoveSpeedGrowth,

        [LabelText("攻击力")]
        [Description("攻击力")]
        Attack,

        [LabelText("攻击速度")]
        [Description("攻击速度")]
        AttackSpeed,

        [LabelText("[每点敏捷]攻击速度")]
        [Description("[每点敏捷]攻击速度")]
        AttackSpeedGrowth,

        [LabelText("攻击距离")]
        [Description("攻击距离")]
        AttackDistance,

        [LabelText("命中率")]
        [Description("命中率")]
        HitRate,

        [LabelText("闪避率")]
        [Description("闪避率")]
        DodgeRate,

        [LabelText("暴击率")]
        [Description("暴击率")]
        CrticalRate,

        [LabelText("技能增强")]
        [Description("技能增强")]
        SkillDamageRate,

        [LabelText("[每点智力]技能增强")]
        [Description("[每点智力]技能增强")]
        SkillDamageRateGrowth,
    }
}