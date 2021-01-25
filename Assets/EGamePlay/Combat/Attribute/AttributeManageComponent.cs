using System.Collections.Generic;
using System.ComponentModel;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 战斗属性数值管理组件，在这里管理所有角色战斗属性数值的存储、变更、刷新等
    /// </summary>
    public class AttributeManageComponent : Component
    {
        public readonly Dictionary<string, FloatNumeric> attributeNumerics = new Dictionary<string, FloatNumeric>();
        // public FloatNumeric HealthPoint { get { return attributeNumerics[nameof(AttributeType.HealthPoint)]; } }
        // public FloatNumeric AttackPower { get { return attributeNumerics[nameof(AttributeType.AttackPower)]; } }
        // public FloatNumeric AttackDefense { get { return attributeNumerics[nameof(AttributeType.AttackDefense)]; } }
        // public FloatNumeric CriticalProbability { get { return attributeNumerics[nameof(AttributeType.CriticalProbability)]; } }
        [Description("力量")]
        public FloatNumeric Strength { get; set; }

        [Description("敏捷")]
        public FloatNumeric Agility { get; set; }

        [Description("智力")]
        public FloatNumeric Intellect { get; set; }

        [Description("最大生命值")]
        public FloatNumeric HpMax { get; set; }

        [Description("[每点力量]最大生命值")]
        public FloatNumeric HpMaxGrowth { get; set; }

        [Description("最大法术值")]
        public FloatNumeric MpMax { get; set; }

        [Description("[每点智力]最大法术值")]
        public FloatNumeric MpMaxGrowth { get; set; }

        [Description("生命恢复")]
        public FloatNumeric HpRegeneration { get; set; }

        [Description("[每点力量]生命恢复")]
        public FloatNumeric HpRegenerationGrowth { get; set; }

        [Description("法力恢复")]
        public FloatNumeric MpRegeneration { get; set; }

        [Description("[每点智力]法力恢复")]
        public FloatNumeric MpRegenerationGrowth { get; set; }

        [Description("护甲")]
        public FloatNumeric Armor { get; set; }

        [Description("[每点敏捷]护甲")]
        public FloatNumeric ArmorGrowth { get; set; }

        [Description("抗性")]
        public FloatNumeric Resistance { get; set; }

        [Description("[每点力量]抗性")]
        public FloatNumeric ResistanceGrowth { get; set; }

        [Description("移动速度")]
        public FloatNumeric MoveSpeed { get; set; }

        [Description("[每点敏捷]移动速度")]
        public FloatNumeric MoveSpeedGrowth { get; set; }

        [Description("攻击力")]
        public FloatNumeric Attack { get; set; }

        [Description("攻击速度")]
        public FloatNumeric AttackSpeed { get; set; }

        [Description("[每点敏捷]攻击速度")]
        public FloatNumeric AttackSpeedGrowth { get; set; }

        [Description("攻击距离")]
        public FloatNumeric AttackDistance { get; set; }

        [Description("命中率")]
        public FloatNumeric HitRate { get; set; }

        [Description("闪避率")]
        public FloatNumeric DodgeRate { get; set; }

        [Description("暴击率")]
        public FloatNumeric CrticalRate { get; set; }

        [Description("技能增强")]
        public FloatNumeric SkillDamageRate { get; set; }

        [Description("[每点智力]技能增强")]
        public FloatNumeric SkillDamageRateGrowth { get; set; }

        public override void Setup()
        {
            Initialize();
        }

        public void Initialize()
        {
            // AddNumeric(nameof(AttributeType.HealthPoint), 99_999);
            // AddNumeric(nameof(AttributeType.AttackPower), 1000);
            // AddNumeric(nameof(AttributeType.AttackDefense), 300);
            // AddNumeric(nameof(AttributeType.CriticalProbability), 0.5f);
        }

        public FloatNumeric AddNumeric(string type, float baseValue)
        {
            var numeric = new FloatNumeric();
            numeric.SetBase(baseValue);
            attributeNumerics.Add(type, numeric);
            return numeric;
        }
    }
}
