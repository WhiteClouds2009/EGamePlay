using EGamePlay.Combat.Ability;
using System.Collections.Generic;

namespace EGamePlay.Combat.Status
{
    public class StatusAbility : AbilityEntity
    {
        //投放者、施术者
        public CombatEntity Caster { get; set; }
        public StatusConfigObject StatusConfigObject { get; set; }
        public Dictionary<AttributeType, FloatModifier> AttributeTypeModifiers { get; set; }
        private List<StatusAbility> ChildrenStatuses { get; set; } = new List<StatusAbility>();


        public override void Awake(object initData)
        {
            base.Awake(initData);
            StatusConfigObject = initData as StatusConfigObject;
        }

        //激活
        public override void ActivateAbility()
        {
            base.ActivateAbility();
            if (StatusConfigObject.EnabledStateModify)
            {

            }

            // 如果开启了属性修改
            if (StatusConfigObject.EnabledAttributeModify)
            {
                AttributeTypeModifiers = new Dictionary<AttributeType, FloatModifier>();
                var attributeComponent = GetParent<CombatEntity>().AttributeComponent;
                foreach (var item in StatusConfigObject.AttributeTypeDatas)
                {
                    var value = int.Parse(item.Value);
                    var numericModifier = new FloatModifier() { Value = value };
                    var attribute = attributeComponent.attributeNumerics[nameof(item.Value)];
                    attribute.AddAddModifier(numericModifier);
                    AttributeTypeModifiers.Add(item.Key, numericModifier);
                }
            }


            if (StatusConfigObject.EnabledLogicTrigger)
            {
                foreach (var item in StatusConfigObject.Effects)
                {
                    var logicEntity = EntityFactory.CreateWithParent<LogicEntity>(this, item);
                    if (item.EffectTriggerType == EffectTriggerType.Interval)
                    {
                        logicEntity.AddComponent<LogicIntervalTriggerComponent>();
                    }
                    if (item.EffectTriggerType == EffectTriggerType.Condition)
                    {
                        logicEntity.AddComponent<LogicConditionTriggerComponent>();
                    }
                    if (item.EffectTriggerType == EffectTriggerType.Action)
                    {
                        logicEntity.AddComponent<LogicActionTriggerComponent>();
                    }
                }
            }
            foreach (var item in StatusConfigObject.ChildrenStatuses)
            {
                var status = OwnerEntity.AttachStatus<StatusAbility>(item);
                status.Caster = OwnerEntity;
                status.TryActivateAbility();
            }
        }

        //结束
        public override void EndAbility()
        {
            if (StatusConfigObject.EnabledStateModify)
            {

            }
            if (StatusConfigObject.EnabledAttributeModify)
            {
                var attributeComponent = GetParent<CombatEntity>().AttributeComponent;
                foreach (var item in StatusConfigObject.AttributeTypeDatas)
                {
                    var numericModifier = AttributeTypeModifiers[item.Key];
                    var attribute = attributeComponent.attributeNumerics[nameof(item.Value)];
                    attribute.RemoveAddModifier(numericModifier);
                }
            }
            if (StatusConfigObject.EnabledLogicTrigger)
            {

            }
            foreach (var item in ChildrenStatuses)
            {
                item.EndAbility();
            }
            ChildrenStatuses.Clear();
            AttributeTypeModifiers = null;
            GetParent<CombatEntity>().OnStatusRemove(this);
            base.EndAbility();
        }

        //应用能力效果
        public override void ApplyAbilityEffectsTo(CombatEntity targetEntity)
        {
            base.ApplyAbilityEffectsTo(targetEntity);
        }
    }
}