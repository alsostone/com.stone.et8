using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.9
    /// Desc
    /// </summary>
    [FriendOf(typeof(GMToolsStageItemComponent))]
    public static partial class GMToolsStageItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMToolsStageItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this GMToolsStageItemComponent self)
        {
        }
        
        public static void ResetItem(this GMToolsStageItemComponent self, GMToolsItemExtraData extraData, GMToolsStageInfo info)
        {
            self.ExtraData = extraData;
            self.Info = info;
            self.u_DataText.SetValue(string.IsNullOrEmpty(info.Desc) ? $"{info.Id}" : $"{info.Id}:{info.Desc}");
            self.u_DataSelected.SetValue(info.Selected);
        }
        
        #region YIUIEvent开始
        private static void OnEventClickAction(this GMToolsStageItemComponent self)
        {
            self.Info.Selected = !self.Info.Selected;
            self.u_DataSelected.SetValue(self.Info.Selected);
            self.ExtraData.ChangeCallBack?.Invoke();
        }
        #endregion YIUIEvent结束
    }
}