﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EGamePlay.Combat.Ability
{
    /// <summary>
    /// 能力实体，存储着某个英雄某个能力的数据和状态
    /// </summary>
    public abstract class AbilityEntity : Entity
    {
        public CombatEntity OwnerEntity { get => GetParent<CombatEntity>(); }
        public object ConfigObject { get; set; }
        public int Level { get; set; } = 1;


        public override void Awake(object initData)
        {
            ConfigObject = initData;
        }

        //尝试激活能力
        public virtual void TryActivateAbility()
        {
            //Log.Debug($"{GetType().Name}->TryActivateAbility");
            ActivateAbility();
        }
        
        //激活能力
        public virtual void ActivateAbility()
        {
            
        }

        //结束能力
        public virtual void EndAbility()
        {
            Destroy(this);
        }

        //创建能力执行体
        public virtual AbilityExecution CreateAbilityExecution()
        {
            return null;
        }
        
        public void ApplyEffectTo(CombatEntity targetEntity, Effect effectItem)
        {
            // 伤害行为
            if (effectItem is DamageEffect damageEffect)
            {
                var action = this.OwnerEntity.CreateCombatAction<DamageAction>();
                action.Target = targetEntity;
                action.DamageSource = DamageSource.Skill;
                action.DamageEffect = damageEffect;
                action.ApplyDamage();
            }
            // 治疗行为
            else if (effectItem is CureEffect cureEffect)
            {
                var action = this.OwnerEntity.CreateCombatAction<CureAction>();
                action.Target = targetEntity;
                action.CureEffect = cureEffect;
                action.ApplyCure();
            }
            // 效果赋予行为
            else
            {
                var action = this.OwnerEntity.CreateCombatAction<AssignEffectAction>();
                action.Target = targetEntity;
                action.SourceAbility = this;
                action.Effect = effectItem;
                if (effectItem is AddStatusEffect addStatusEffect)
                {
                    var statusConfig = addStatusEffect.AddStatus;
                    statusConfig.Duration = addStatusEffect.Duration;
                    if (addStatusEffect.Params != null && statusConfig.Effects != null)
                    {
                        if (statusConfig.EnabledAttributeModify)
                        {
                            foreach (var item3 in addStatusEffect.Params)
                            {
                                var type = (AttributeType)Enum.Parse(typeof(AttributeType), item3.Key);
                                statusConfig.AttributeTypeDatas.Add(type, item3.Value);
                            }
                        }
                        if (statusConfig.EnabledLogicTrigger)
                        {
                            foreach (var item6 in statusConfig.Effects)
                            {
                                foreach (var item3 in addStatusEffect.Params)
                                {
                                    if (!string.IsNullOrEmpty(item6.Interval))
                                    {
                                        item6.Interval = item6.Interval.Replace(item3.Key, item3.Value);
                                    }
                                    if (!string.IsNullOrEmpty(item6.ConditionParam))
                                    {
                                        item6.ConditionParam = item6.ConditionParam.Replace(item3.Key, item3.Value);
                                    }
                                }
                                if (item6 is DamageEffect damage)
                                {
                                    foreach (var item4 in addStatusEffect.Params)
                                    {
                                        if (!string.IsNullOrEmpty(damage.DamageValueFormula))
                                        {
                                            damage.DamageValueFormula = damage.DamageValueFormula.Replace(item4.Key, item4.Value);
                                        }
                                    }
                                }
                                else if (item6 is CureEffect cure)
                                {
                                    foreach (var item5 in addStatusEffect.Params)
                                    {
                                        if (!string.IsNullOrEmpty(cure.CureValueFormula))
                                        {
                                            cure.CureValueFormula = cure.CureValueFormula.Replace(item5.Key, item5.Value);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                action.ApplyAssignEffect();
            }
        }

        //应用能力效果
        public virtual void ApplyAbilityEffectsTo(CombatEntity targetEntity)
        {
            List<Effect> Effects = null;
            if (ConfigObject is SkillConfigObject skillConfigObject)
            {
                Effects = skillConfigObject.Effects;
            }
            if (ConfigObject is StatusConfigObject statusConfigObject)
            {
                if (statusConfigObject.EnabledLogicTrigger)
                {
                    Effects = statusConfigObject.Effects;
                }
            }
            if (Effects == null)
            {
                return;
            }
            foreach (var effectItem in Effects)
            {
                ApplyEffectTo(targetEntity, effectItem);
            }
        }
    }
}