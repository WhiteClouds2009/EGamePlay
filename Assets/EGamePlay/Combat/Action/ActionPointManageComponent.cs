﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 行动点，一次战斗行动<see cref="CombatAction"/>会触发战斗实体一系列的行动点
    /// </summary>
    public sealed class ActionPoint
    {
        public List<Action<CombatAction>> Listeners { get; set; } = new List<Action<CombatAction>>();
    }

    /// <summary>
    /// 行动点管理器，在这里管理一个战斗实体所有行动点的添加监听、移除监听、触发流程
    /// </summary>
    public sealed class ActionPointManageComponent : Component
    {
        private Dictionary<ActionPointType, ActionPoint> ActionPoints { get; set; } = new Dictionary<ActionPointType, ActionPoint>();


        public override void Setup()
        {
            base.Setup();
        }

        public void AddListener(ActionPointType actionPointType, Action<CombatAction> action)
        {
            if (!ActionPoints.ContainsKey(actionPointType))
            {
                ActionPoints.Add(actionPointType, new ActionPoint());
            }
            ActionPoints[actionPointType].Listeners.Add(action);
        }

        public void RemoveListener(ActionPointType actionPointType, Action<CombatAction> action)
        {
            if (ActionPoints.ContainsKey(actionPointType))
            {
                ActionPoints[actionPointType].Listeners.Remove(action);
            }
        }

        public void TriggerActionPoint(ActionPointType actionPointType, CombatAction action)
        {
            if (ActionPoints.ContainsKey(actionPointType) && ActionPoints[actionPointType].Listeners.Count > 0)
            {
                for (int i = ActionPoints[actionPointType].Listeners.Count - 1; i >= 0; i--)
                {
                    var item = ActionPoints[actionPointType].Listeners[i];
                    item.Invoke(action);
                }
            }
        }
    }
}