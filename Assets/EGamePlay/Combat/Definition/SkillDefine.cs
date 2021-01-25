namespace EGamePlay.Combat
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    
    [LabelText("释放类型")]
    public enum SkillSpellType
    {
        [LabelText("主动技能")]
        Initiative,

        [LabelText("被动技能")]
        Passive,
    }

    [LabelText("作用对象")]
    public enum AddSkillEffetTargetType
    {
        [LabelText("技能目标")]
        SkillTarget = 0,
        [LabelText("自身")]
        Self = 1
    }

    public enum ActionControlType
    {
        [LabelText("（空）")]
        None = 0,

        [LabelText("移动禁止")]
        MoveForbid = 1 << 1,

        [LabelText("施法禁止")]
        SkillForbid = 1 << 2,

        [LabelText("攻击禁止")]
        AttackForbid = 1 << 3,

        [LabelText("移动控制")]
        MoveControl = 1 << 4,

        [LabelText("攻击控制")]
        AttackControl = 1 << 5
    }
}