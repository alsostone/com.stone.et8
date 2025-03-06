namespace ET.Client
{
	[Event(SceneType.Demo | SceneType.LockStep)]
	public class LoginFinish_RemoveLoginUI: AEvent<Scene, LoginFinish>
	{
		protected override async ETTask Run(Scene scene, LoginFinish args)
		{
			await YIUIMgrComponent.Inst.ClosePanelAsync<LoginPanelComponent>(false,true);
		}
	}
}
