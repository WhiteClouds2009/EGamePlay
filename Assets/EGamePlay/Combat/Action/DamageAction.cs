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
        //伤害数值
        public int DamageValue { get; set; }
        //是否是暴击
        public bool IsCritical { get; set; }


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
            var reduction = GetDamageReduction(Target, DamageEffect.DamageType);
            var range = 1;
            var addedRate = 0;
            var addedValue = 0;
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

            //攻击伤害 = 攻击力 * 伤害倍率 * 伤害波动 * 额外倍率 + 额外伤害
            DamageValue = (int)Mathf.Max(1, damageValue * (1 - reduction) * range * (1 + addedRate) + addedValue);
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