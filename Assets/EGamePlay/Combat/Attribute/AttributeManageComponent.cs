using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 战斗属性数值管理组件，在这里管理所有角色战斗属性数值的存储、变更、刷新等
    /// </summary>
    public class AttributeManageComponent : Component
    {
        private readonly Dictionary<AttributeType, FloatNumeric> attributeNumerics = new Dictionary<AttributeType, FloatNumeric>();

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

            // 生命值
            var HpMax = AddNumeric(AttributeType.HpMax, 200);
            var HpRegeneration = AddNumeric(AttributeType.HpRegeneration, 10);

            // 法术值
            var MpMax = AddNumeric(AttributeType.MpMax, 120);
            var MpRegeneration = AddNumeric(AttributeType.MpRegeneration, 0);

            // 攻击
            var Attack = AddNumeric(AttributeType.Attack, 20);
            var AttackSpeed = AddNumeric(AttributeType.AttackSpeed, 100);

            // 防御
            var Armor = AddNumeric(AttributeType.Armor, 10);
            var Resistance = AddNumeric(AttributeType.Resistance, 10);

            // 常规
            var MoveSpeed = AddNumeric(AttributeType.MoveSpeed, 300);
            var SkillStrengthen = AddNumeric(AttributeType.SkillStrengthen, 10);
            var Crtical = AddNumeric(AttributeType.Crtical, 0);

            //设置上下限
            Strength.minLimit = 1;
            Strength.maxLimit = 9999;
            Agility.minLimit = 1;
            Agility.maxLimit = 9999;
            Intellect.minLimit = 1;
            Intellect.maxLimit = 9999;

            HpMax.minLimit = 200;
            HpMax.maxLimit = 99999;
            MpMax.minLimit = 100;
            MpMax.maxLimit = 99999;

            Attack.minLimit = 10;
            Attack.maxLimit = 99999;
            AttackSpeed.minLimit = 10;
            AttackSpeed.maxLimit = 600;

            MoveSpeed.minLimit = 50;
            MoveSpeed.maxLimit = 550;

            SkillStrengthen.minLimit = 0.01f;
            SkillStrengthen.maxLimit = 9f;

            Crtical.minLimit = 0;
            Crtical.maxLimit = 1;

            Hp = HpMax.Value;
            Mp = MpMax.Value;
        }

        private FloatNumeric AddNumeric(AttributeType type, float baseValue)
        {
            var numeric = new FloatNumeric();
            numeric.SetBase(baseValue);
            attributeNumerics.Add(type, numeric);

            return numeric;
        }

        public FloatNumeric GetNumeric(AttributeType type)
        {
            return attributeNumerics[type];
        }

        public Dictionary<AttributeType, FloatNumeric> GetNumericList()
        {
            return attributeNumerics;
        }

        public void UpdateAllNumeric()
        {
            foreach (var item in attributeNumerics)
            {
                item.Value.Update();
            }
        }

        public float UpdatePhysicalReduction()
        {
            var armor = GetNumeric(AttributeType.Armor);

            //护甲的物理伤害减免
            PhysicalReduction = 0.06f * armor.Value / (1 + 0.06f * armor.Value);

            return PhysicalReduction;
        }

        public float UpdateMagicReduction()
        {
            var resistance = GetNumeric(AttributeType.Resistance);

            //抗性的法术伤害减免
            MagicReduction = 0.06f * resistance.Value / (1 + 0.06f * resistance.Value);

            return MagicReduction;
        }

        public void SetHp(float value)
        {
            var HpMax = attributeNumerics[AttributeType.HpMax];
            //预期结果
            var presentValue = value;
            //不超过最大值
            Hp = (presentValue < HpMax.Value ? presentValue : HpMax.Value);
        }

        public bool AddHp(float value)
        {
            var HpMax = attributeNumerics[AttributeType.HpMax];
            //预期结果
            var presentValue = Hp + value;
            //不超过最大值
            Hp = (presentValue < HpMax.Value ? presentValue : HpMax.Value);

            return true;
        }

        public bool MinusHp(float value)
        {
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

        public void SetMp(float value)
        {
            var MpMax = attributeNumerics[AttributeType.MpMax];
            //预期结果
            var presentValue = value;
            //不超过最大值
            Mp = (presentValue < MpMax.Value ? presentValue : MpMax.Value);
        }

        public bool AddMp(float value)
        {
            var MpMax = attributeNumerics[AttributeType.MpMax];
            //预期结果
            var presentValue = Mp + value;
            //不超过最大值
            Mp = (presentValue < MpMax.Value ? presentValue : MpMax.Value);

            return true;
        }

        public bool MinusMp(float value)
        {
            //预期结果
            var presentValue = Mp - value;
            if (presentValue <= 0)
            {
                Mp = 0;
                return false;
            }
            else
            {
                Mp = presentValue;
                return true;
            }
        }
    }
}
