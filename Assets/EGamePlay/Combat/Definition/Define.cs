using System.ComponentModel;

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace EGamePlay.Combat
{
    [LabelText("作用对象")]
    public enum AffectTargetType
    {
        [LabelText("自身")]
        Self = 0,

        [LabelText("己方")]
        SelfTeam = 1,

        [LabelText("敌方")]
        EnemyTeam = 2
    }

    [LabelText("作用对象检测方式")]
    public enum TargetSelectType
    {
        [LabelText("指向性")]
        Point,

        [LabelText("直线")]
        Line,

        [LabelText("范围")]
        Circle
    }

    public enum ActionPointType
    {
        [LabelText("技能命中前")]
        PreCauseSkill,

        [LabelText("技能命中后")]
        PostCauseSkill,

        [LabelText("受到技能作用前")]
        PreReceiveSkill,

        [LabelText("受到技能作用后")]
        PostReceiveSkill,

        [LabelText("造成伤害前")]
        PreCauseDamage,

        [LabelText("承受伤害前")]
        PreReceiveDamage,

        [LabelText("造成伤害后")]
        PostCauseDamage,

        [LabelText("承受伤害后")]
        PostReceiveDamage,

        [LabelText("给予治疗前")]
        PreGiveCure,

        [LabelText("接受治疗前")]
        PreReceiveCure,

        [LabelText("给予治疗后")]
        PostGiveCure,

        [LabelText("接受治疗后")]
        PostReceiveCure,

        [LabelText("赋给效果")]
        AssignEffect,

        [LabelText("接受效果")]
        ReceiveEffect,

        [LabelText("赋加状态后")]
        PostGiveStatus,

        [LabelText("承受状态后")]
        PostReceiveStatus
    }
}

[LabelText("伤害类型")]
    public enum DamageType
    {
        [Description("物理")]
        [LabelText("物理")]
        physics,

        [Description("法术")]
        [LabelText("法术")]
        magic,

        [Description("纯粹")]
        [LabelText("纯粹")]
        pure
    }