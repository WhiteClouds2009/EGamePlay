using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using System.IO;

using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.OdinInspector.Editor;

namespace EGamePlay.Combat
{
    [CreateAssetMenu(fileName = "状态配置", menuName = "技能|状态/状态配置")]
    //[LabelText("状态配置")]
    public class StatusConfigObject : SerializedScriptableObject
    {
        [Title("常规配置")]
        [LabelText("状态ID"), DelayedProperty]
        public string ID = "0";

        [LabelText("状态名称"), DelayedProperty]
        public string Name = "状态1";

        [LabelText("状态类型")]
        public StatusType StatusType;

        [HideInInspector]
        public uint Duration;

        [LabelText("是否在状态栏显示")]
        public bool ShowInStatusIconList;

        [LabelText("是否可叠加")]
        public bool CanStack;

        [LabelText("最高叠加层数"), ShowIf("CanStack"), Range(0, 99)]
        public int MaxStack = 0;

        [Title("逻辑配置"), Space(20)]
        [ToggleGroup("EnabledStateModify", "行为禁制")]
        public bool EnabledStateModify;

        [ToggleGroup("EnabledStateModify")]
        public ActionControlType ActionControlType;

        [ToggleGroup("EnabledAttributeModify", "属性修饰")]
        public bool EnabledAttributeModify;

        [ToggleGroup("EnabledAttributeModify")]
        [LabelText("属性列表")]
        public Dictionary<AttributeType, string> AttributeTypeDatas = new Dictionary<AttributeType, string>();

        [ToggleGroup("EnabledLogicTrigger", "逻辑触发")]
        public bool EnabledLogicTrigger;

        [ToggleGroup("EnabledLogicTrigger")]
        [LabelText("效果列表")]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowItemCount = false, HideAddButton = true)]
        [HideReferenceObjectPicker]
        public List<Effect> Effects = new List<Effect>();
        
        [HorizontalGroup("EnabledLogicTrigger/Hor2")]
        [HideLabel]
        [OnValueChanged("AddEffect")]
        [ValueDropdown("EffectTypeSelect")]
        public string EffectTypeName = "(添加效果)";

        public IEnumerable<string> EffectTypeSelect()
        {
            var types = typeof(Effect).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(Effect).IsAssignableFrom(x))
                .Where(x => x.GetCustomAttribute<EffectAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<EffectAttribute>().Order)
                .Select(x => x.GetCustomAttribute<EffectAttribute>().EffectType);

            var results = types.ToList();
            results.Insert(0, "(添加效果)");
            return results;
        }

        private void AddEffect()
        {
            if (EffectTypeName != "(添加效果)")
            {
                var effectType = typeof(Effect).Assembly.GetTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => typeof(Effect).IsAssignableFrom(x))
                    .Where(x => x.GetCustomAttribute<EffectAttribute>() != null)
                    .Where(x => x.GetCustomAttribute<EffectAttribute>().EffectType == EffectTypeName)
                    .First();
                var effect = Activator.CreateInstance(effectType) as Effect;
                effect.Enabled = true;
                Effects.Add(effect);

                EffectTypeName = "(添加效果)";
            }
        }

        [Title("子状态配置"), Space(20)]
        [LabelText("子状态效果列表")]
        [ListDrawerSettings(DraggableItems = false, ShowItemCount = false)]
        public List<StatusConfigObject> ChildrenStatuses;

        [Title("状态表现"), Space(20)]
        [LabelText("状态特效")]
        public GameObject ParticleEffect;

        [LabelText("状态音效")]
        public AudioClip Audio;

        [Title("文本描述"), Space(20)]
        [TextArea, LabelText("状态描述")]
        public string StatusDescription;
    }
}