namespace ET.Client
{
	[Event(SceneType.Demo | SceneType.LockStep)]
	public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, AppStartInitFinish>
	{
		protected override async ETTask Run(Scene root, AppStartInitFinish args)
		{
			await YIUIMgrComponent.Inst.Root.OpenPanelAsync<LoginPanelComponent>();
#if ENABLE_DEBUG
			await YIUIMgrComponent.Inst.Root.OpenPanelAsync<GMToolsPanelComponent>();
#endif
		}
	}
}
