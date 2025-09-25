using UnityEngine;
using DG.Tweening;

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
                case EUnitType.Item:
                    self.u_DataName.SetValue($"Item{itemData.TableId}");
                    break;
            }
        }
        
        #region YIUIEvent开始
        
        private static void OnEventClickAction(this LSCardSelectItemComponent self)
        {
            self.Fiber().UIEvent(new OnCardSelectClickEvent() { Index = self.Index }).Coroutine();
        }
        #endregion YIUIEvent结束
    }
}