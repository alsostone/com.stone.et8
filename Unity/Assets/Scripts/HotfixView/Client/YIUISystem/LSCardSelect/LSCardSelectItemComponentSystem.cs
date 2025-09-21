using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  alsostone
    /// Date    2025.9.20
    /// Desc
    /// </summary>
    [FriendOf(typeof(LSCardSelectItemComponent))]
    public static partial class LSCardSelectItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LSCardSelectItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LSCardSelectItemComponent self)
        {
        }
        
        public static void SetData(this LSCardSelectItemComponent self, LSRandomDropItem itemData, int index)
        {
            self.ItemData = itemData;
            self.Index = index;
            
            switch (itemData.Type)
            {
                case EUnitType.Block:
                    self.u_DataName.SetValue($"Block{itemData.TableId}");
                    break;
                case EUnitType.Building:
                    self.u_DataName.SetValue($"Building{itemData.TableId}");
                    break;
            }
        }
        
        #region YIUIEvent开始
        
        private static void OnEventClickAction(this LSCardSelectItemComponent self)
        {
            ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.CardSelect, self.Index);
            self.Room().SendCommandMeesage(cmd);
        }
        #endregion YIUIEvent结束
    }
}