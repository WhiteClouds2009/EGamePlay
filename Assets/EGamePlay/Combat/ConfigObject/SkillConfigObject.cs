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
    [CreateAssetMenu(fileName = "技能配置", menuName = "技能|状态/技能配置")]
    [LabelText("技能配置")]
    public class SkillConfigObject : SerializedScriptableObject
    {
        // 是否显示范围半径
        public bool ShowAoeaRadius
        {
            get
            {
                var targetSelectTypeFlag = (TargetSelectType == TargetSelectType.Circle || TargetSelectType == TargetSelectType.Line);
                return (AffectTargetType != AffectTargetType.Self) && targetSelectTypeFlag;
            }
        }

        public bool ShowDistance
        {
            get
            {
                return AffectTargetType != AffectTargetType.Self;
            }
        }

        [Title("常规设置")]
        [LabelText("技能ID"), DelayedProperty]
        public uint ID;

        [LabelText("技能名称"), DelayedProperty]
        public string Name = "技能1";

        [LabelText("技能图片"), DelayedProperty]
        public Sprite Image;

        [Space(20)]
        [Title("释放设置")]
        public SkillSpellType SkillSpellType;

        [ShowIf("SkillSpellType", SkillSpellType.Initiative)]
        public AffectTargetType AffectTargetType;

        [HideIf("AffectTargetType", AffectTargetType.Self)]
        public TargetSelectType TargetSelectType;

        [LabelText("范围半径"), DelayedProperty]
        [ShowIf("ShowAoeaRadius")]
        public float Radius = 1;

        [LabelText("释放距离"), DelayedProperty]
        [ShowIf("ShowDistance")]
        public float Distance = 1;

        [Title("消耗设置"), Space(20)]
        [LabelText("冷却时间"), DelayedProperty, SuffixLabel("秒", true)]
        public float Cooldown = 3;

        [LabelText("蓝量消耗"), DelayedProperty]
        public float MagicCost = 20;

        // //[LabelText("区域场引导配置"), ShowIf("TargetSelectType", SkillTargetSelectType.AreaSelect)]
        // //public GameObject AreaGuideObj;
        // [LabelText("区域场配置"), ShowIf("TargetSelectType", SkillTargetSelectType.AreaSelect)]
        // public GameObject AreaCollider;


        [Title("效果设置"), Space(20)]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowItemCount = false, HideAddButton = true)]
        [HideReferenceObjectPicker]
        [LabelText("行动列表")]
        public List<Effect> Effects = new List<Effect>();

        [HorizontalGroup]
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

        [Space(20)]
        [Title("技能表现")]
        [LabelText("技能动作")]
        public AnimationClip SkillAnimationClip;

        [LabelText("技能特效")]
        public GameObject SkillEffectObject;

        [LabelText("技能音效")]
        public AudioClip SkillAudio;

        [Space(20)]
        [Title("文本描述")]
        [TextArea, LabelText("技能描述")]
        public string SkillDescription;
    }
}