using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 治疗行动
    /// </summary>
    public class CureAction : CombatAction
    {
        public CureEffect CureEffect { get; set; }
        //治疗数值
        public int CureValue { get; set; }


        //前置处理
        private void PreProcess()
        {
            //触发 给予治疗前 行动点
            Creator.TriggerActionPoint(ActionPointType.PreGiveCure, this);
            //触发 接受治疗前 行动点
            Target.TriggerActionPoint(ActionPointType.PreReceiveCure, this);

            if (CureEffect != null)
            {
                var expression = ExpressionHelper.ExpressionParser.EvaluateExpression(CureEffect.CureValueFormula);

                foreach (var item in Creator.AttributeComponent.GetNumericList())
                {
                    var key = item.Key.ToString();
                    if (expression.Parameters.ContainsKey(key))
                    {
                        expression.Parameters[key].Value = Creator.AttributeComponent.GetNumeric(item.Key).Value;
                    }
                }

                CureValue = (int)expression.Value;
            }
        }

        public void ApplyCure()
        {
            PreProcess();
            Target.ReceiveCure(this);
            PostProcess();
        }

        //后置处理
        private void PostProcess()
        {
            Creator.TriggerActionPoint(ActionPointType.PostGiveCure, this);
            Target.TriggerActionPoint(ActionPointType.PostReceiveCure, this);
        }
    }
}