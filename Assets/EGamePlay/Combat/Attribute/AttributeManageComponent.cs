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

        private readonly Dictionary<AttributeType, FloatModifier> growthModifiers = new Dictionary<AttributeType, FloatModifier>();

        //物理抗性
        public float PhysicalReduction { get; private set; }

        //法术抗性
        public float MagicReduction { get; private set; }

        public float Hp
        {
            get;
            private set;
        }

        public float Mp
        {
            get;
            private set;
        }



        public override void Setup()
        {
            Initialize();
        }

        public void Initialize()
        {
            // 三大属性
            var Strength = AddNumeric(AttributeType.Strength, 10);
            var Agility = AddNumeric(AttributeType.Agility, 10);
            var Intellect = AddNumeric(AttributeType.Intellect, 10);

            Strength.minLimit = 1;
            Agility.minLimit = 1;
            Intellect.minLimit = 1;

            // 生命值
            var HpMax = AddNumeric(AttributeType.HpMax, 200);
            var HpMaxRate = AddNumeric(AttributeType.HpMaxRate, 20);
            var HpRegeneration = AddNumeric(AttributeType.HpRegeneration, 10);
            var HpRegenerationRate = AddNumeric(AttributeType.HpRegenerationRate, 10);

            HpMax.minLimit = 150;

            // 法术值
            var MpMax = AddNumeric(AttributeType.MpMax, 120);
            var MpMaxRate = AddNumeric(AttributeType.MpMaxRate, 20);
            var MpRegeneration = AddNumeric(AttributeType.MpRegeneration, 10);
            var MpRegenerationRate = AddNumeric(AttributeType.MpRegenerationRate, 10);

            MpMax.minLimit = 80;

            // 攻击
            var Attack = AddNumeric(AttributeType.Attack, 20);
            var AttackSpeed = AddNumeric(AttributeType.AttackSpeed, 100);
            var AttackSpeedRate = AddNumeric(AttributeType.AttackSpeedRate, 10);

            Attack.minLimit = 10;
            AttackSpeed.minLimit = 10;

            // 防御
            var Armor = AddNumeric(AttributeType.Armor, 10);
            var ArmorRate = AddNumeric(AttributeType.ArmorRate, 10);
            var Resistance = AddNumeric(AttributeType.Resistance, 10);
            var ResistanceRate = AddNumeric(AttributeType.ResistanceRate, 10);

            // 常规
            var MoveSpeed = AddNumeric(AttributeType.MoveSpeed, 10);
            var MoveSpeedRate = AddNumeric(AttributeType.MoveSpeedRate, 10);
            var SkillStrengthen = AddNumeric(AttributeType.SkillStrengthen, 10);
            var SkillStrengthenRate = AddNumeric(AttributeType.SkillStrengthenRate, 10);

            MoveSpeed.minLimit = 50;
            
            //属性更新的衍生影响
            HpMax.UpdateAction = (Attribute) =>
            {
                //最大值限制
                if (Hp > HpMax.Value)
                {
                    Hp = HpMax.Value;
                }
            };

            MpMax.UpdateAction = (Attribute) =>
            {
                //最大值限制
                if (Mp > MpMax.Value)
                {
                    Mp = MpMax.Value;
                }
            };

            Strength.UpdateAction = (Attribute) =>
            {
                UpdateGrowth(Attribute, growthModifiers[AttributeType.HpMax], HpMax, HpMaxRate);
                UpdateGrowth(Attribute, growthModifiers[AttributeType.HpRegeneration], HpRegeneration, HpRegenerationRate);
            };

            Agility.UpdateAction = (Attribute) =>
            {
                UpdateGrowth(Attribute, growthModifiers[AttributeType.Armor], Armor, ArmorRate);
                UpdateGrowth(Attribute, growthModifiers[AttributeType.AttackSpeed], AttackSpeed, AttackSpeedRate);
                UpdateGrowth(Attribute, growthModifiers[AttributeType.MoveSpeed], MoveSpeed, MoveSpeedRate);
            };

            Intellect.UpdateAction = (Attribute) =>
            {
                UpdateGrowth(Attribute, growthModifiers[AttributeType.MpMax], MpMax, MpMaxRate);
                UpdateGrowth(Attribute, growthModifiers[AttributeType.MpRegeneration], MpRegeneration, MpRegenerationRate);
                UpdateGrowth(Attribute, growthModifiers[AttributeType.SkillStrengthen], SkillStrengthen, SkillStrengthenRate);
            };

            Armor.UpdateAction = (Attribute) =>
            {
                PhysicalReduction = GetPhysicalReduction();
            };

            Resistance.UpdateAction = (Attribute) =>
            {
                MagicReduction = GetMagicReduction();
            };

            Hp = HpMax.Value;
            Mp = MpMax.Value;
        }

        private FloatNumeric AddNumeric(AttributeType type, float baseValue)
        {
            var key = type.ToString();

            var numeric = new FloatNumeric();
            numeric.SetBase(baseValue);
            attributeNumerics.Add(key, numeric);

            var modifiers = new FloatModifier();
            growthModifiers.Add(type, modifiers);

            return numeric;
        }

        public FloatNumeric GetNumeric(AttributeType type)
        {
            return attributeNumerics[type.ToString()];
        }

        /// <summary>
        /// 更新成长数值
        /// </summary>
        /// <param name="modifier">数值修饰器</param>
        /// <param name="element">主属性</param>
        /// <param name="target">影响属性</param>
        /// <param name="rate">影响属性比率</param>
        private void UpdateGrowth(FloatNumeric element, FloatModifier modifier, FloatNumeric target, FloatNumeric rate)
        {
            var value = element.Value * rate.Value;
            if (modifier == null)
            {
                modifier = new FloatModifier();
            }
            else
            {
                target.RemoveAddModifier(modifier);
            }

            modifier.Value = value;
            target.AddAddModifier(modifier);
        }

        private float GetPhysicalReduction()
        {
            var armor = GetNumeric(AttributeType.Armor);

            //护甲的物理伤害减免
            float armorReduction = 0.06f * armor.Value / (1 + 0.06f * armor.Value);

            return armorReduction;
        }

        private float GetMagicReduction()
        {
            var resistance = GetNumeric(AttributeType.Resistance);

            //抗性的法术伤害减免
            float magicArmorReduction = 0.06f * resistance.Value / (1 + 0.06f * resistance.Value);

            return magicArmorReduction;
        }

        public bool AddHp(float value)
        {
            var HpMax = attributeNumerics[nameof(AttributeType.HpMax)];
            //预期结果
            var presentValue = Hp + value;
            //不超过最大值
            Hp = (presentValue < HpMax.Value ? presentValue : HpMax.Value);

            return true;
        }

        public bool MinusHp(float value)
        {
            var HpMax = attributeNumerics[nameof(AttributeType.HpMax)];
            //预期结果
            var presentValue = Hp - value;
            if (presentValue <= 0)
            {
                //触发死亡
                Hp = 0;
                return false;
            }
            else
            {
                Hp = presentValue;
                return true;
            }
        }
    }
}
