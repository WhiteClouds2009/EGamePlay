using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay;
using System;
using B83.ExpressionParser;
using GameUtils;


namespace EGamePlay.Combat
{
    /// <summary>
    /// 伤害行动
    /// </summary>
    public class DamageAction : CombatAction
    {
        public DamageEffect DamageEffect { get; set; }

        //伤害来源
        public DamageSource DamageSource { get; set; }

        //伤害类型
        public DamageType DamageType { get; set; }

        //伤害数值
        public int DamageValue { get; set; }

        //是否是暴击
        public bool IsCritical { get; set; }

        //波动范围
        public float ComputeMinRange = 0.8f;
        public float ComputeMaxRange = 1.2f;

        //伤害的额外比例
        public float ComputeAddedRate = 0;

        //固定伤害
        public float ComputeFixedValue = 0;

        //技能和状态的伤害计算
        private int ParseDamage()
        {
            var expression = ExpressionHelper.ExpressionParser.EvaluateExpression(DamageEffect.DamageValueFormula);

            foreach (var item in Creator.AttributeComponent.attributeNumerics)
            {
                if (expression.Parameters.ContainsKey(item.Key))
                {
                    expression.Parameters[item.Key].Value = Creator.AttributeComponent.attributeNumerics[item.Key].Value;
                }
            }

            return (int)expression.Value;
        }

        //前置处理
        private void PreProcess()
        {
            if(DamageEffect != null)
            {
                DamageType = DamageEffect.DamageType;
            }

            //触发 造成伤害前 行动点
            Creator.TriggerActionPoint(ActionPointType.PreCauseDamage, this);
            //触发 承受伤害前 行动点
            Target.TriggerActionPoint(ActionPointType.PreReceiveDamage, this);

            var reduction = GetDamageReduction(Target, DamageType);
            var damageValue = 0f;
            // 普通攻击
            if (DamageSource == DamageSource.Attack)
            {
                var attack = Creator.AttributeComponent.GetNumeric(AttributeType.Attack);
                var crtical = Creator.AttributeComponent.GetNumeric(AttributeType.Crtical);
                damageValue = attack.Value;

                IsCritical = (RandomHelper.RandomRate() / 100f) < crtical.Value;
                if (IsCritical)
                {
                    damageValue = (int)(DamageValue * 1.5f);
                }
            }
            // 技能攻击
            else if (DamageSource == DamageSource.Skill)
            {
                damageValue = ParseDamage();
            }
            // 状态伤害
            else if (DamageSource == DamageSource.Buff)
            {
                damageValue = ParseDamage();
            }

            var range = UnityEngine.Random.Range(ComputeMinRange, ComputeMaxRange);

            //攻击伤害 = 攻击力 * 伤害倍率 * 伤害波动 * 额外倍率 + 固定伤害
            DamageValue = (int)Mathf.Max(1, damageValue * (1 - reduction) * range * (1 + ComputeAddedRate) + ComputeFixedValue);
        }

        //应用伤害
        public void ApplyDamage()
        {
            PreProcess();
            Target.ReceiveDamage(this);
            PostProcess();
        }

        //后置处理
        private void PostProcess()
        {
            //触发 造成伤害后 行动点
            Creator.TriggerActionPoint(ActionPointType.PostCauseDamage, this);
            //触发 承受伤害后 行动点
            Target.TriggerActionPoint(ActionPointType.PostReceiveDamage, this);
        }

        private float GetDamageReduction(CombatEntity Target, DamageType damageType)
        {
            float damageReduction = 0f;

            switch (damageType)
            {
                case DamageType.physics:
                    {
                        //物理抗性
                        damageReduction = Target.AttributeComponent.PhysicalReduction;
                    }
                    break;
                case DamageType.magic:
                    {
                        //法术抗性
                        damageReduction = Target.AttributeComponent.MagicReduction;
                    }
                    break;
            }

            return damageReduction;
        }
    }

    public enum DamageSource
    {
        Attack,//普攻
        Skill,//技能
        Buff,//Buff
    }
}