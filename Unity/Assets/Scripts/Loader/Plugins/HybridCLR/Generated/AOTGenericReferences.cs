using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"DOTween.dll",
		"MemoryPack.dll",
		"MongoDB.Bson.dll",
		"System.Core.dll",
		"System.Runtime.CompilerServices.Unsafe.dll",
		"System.dll",
		"Unity.Core.dll",
		"Unity.Loader.dll",
		"Unity.ThirdParty.dll",
		"UnityEngine.CoreModule.dll",
		"YIUIFramework.dll",
		"YooAsset.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// DG.Tweening.Core.DOGetter<float>
	// DG.Tweening.Core.DOSetter<float>
	// ET.AEvent<object,ET.ChangePosition>
	// ET.AEvent<object,ET.ChangeRotation>
	// ET.AEvent<object,ET.Client.AfterCreateClientScene>
	// ET.AEvent<object,ET.Client.AfterCreateCurrentScene>
	// ET.AEvent<object,ET.Client.AfterUnitCreate>
	// ET.AEvent<object,ET.Client.AppStartInitFinish>
	// ET.AEvent<object,ET.Client.EnterMapFinish>
	// ET.AEvent<object,ET.Client.LSSceneChangeStart>
	// ET.AEvent<object,ET.Client.LSSceneInitFinish>
	// ET.AEvent<object,ET.Client.LoginFinish>
	// ET.AEvent<object,ET.Client.SceneChangeFinish>
	// ET.AEvent<object,ET.Client.SceneChangeStart>
	// ET.AEvent<object,ET.EntryEvent1>
	// ET.AEvent<object,ET.EntryEvent3>
	// ET.AEvent<object,ET.LSCardBagAdd>
	// ET.AEvent<object,ET.LSCardBagRemove>
	// ET.AEvent<object,ET.LSCardSelectAdd>
	// ET.AEvent<object,ET.LSCardSelectDone>
	// ET.AEvent<object,ET.LSEscape>
	// ET.AEvent<object,ET.LSGridDataReset>
	// ET.AEvent<object,ET.LSPlacementDrag>
	// ET.AEvent<object,ET.LSPlacementNew>
	// ET.AEvent<object,ET.LSPlacementRotate>
	// ET.AEvent<object,ET.LSSelectionChanged>
	// ET.AEvent<object,ET.LSStageGameOver>
	// ET.AEvent<object,ET.LSTouchDrag>
	// ET.AEvent<object,ET.LSTouchDragCancel>
	// ET.AEvent<object,ET.LSTouchDragEnd>
	// ET.AEvent<object,ET.LSTouchDragStart>
	// ET.AEvent<object,ET.LSUnitCasting>
	// ET.AEvent<object,ET.LSUnitCreate>
	// ET.AEvent<object,ET.LSUnitFloating>
	// ET.AEvent<object,ET.LSUnitFx>
	// ET.AEvent<object,ET.LSUnitMoving>
	// ET.AEvent<object,ET.LSUnitPlaced>
	// ET.AEvent<object,ET.LSUnitRemove>
	// ET.AEvent<object,ET.MoveStart>
	// ET.AEvent<object,ET.MoveStop>
	// ET.AEvent<object,ET.NumbericChange>
	// ET.AEvent<object,ET.PropChange>
	// ET.AInvokeHandler<ET.FiberInit,object>
	// ET.AInvokeHandler<ET.MailBoxInvoker>
	// ET.AInvokeHandler<ET.NetComponentOnRead>
	// ET.AInvokeHandler<ET.TimerCallback>
	// ET.AInvokeHandler<ET.YIUIInvokeCoroutineLock,object>
	// ET.AInvokeHandler<ET.YIUIInvokeWaitAsync,object>
	// ET.AInvokeHandler<ET.YIUIInvokeWaitFrameAsync,object>
	// ET.ATimer<object>
	// ET.AwakeSystem<object,TrueSync.TSVector,TrueSync.TSQuaternion>
	// ET.AwakeSystem<object,UnityEngine.Vector3,float,float>
	// ET.AwakeSystem<object,byte>
	// ET.AwakeSystem<object,int,byte>
	// ET.AwakeSystem<object,int,int>
	// ET.AwakeSystem<object,int,object,TrueSync.TSVector>
	// ET.AwakeSystem<object,int,object,object>
	// ET.AwakeSystem<object,int,object>
	// ET.AwakeSystem<object,int>
	// ET.AwakeSystem<object,long>
	// ET.AwakeSystem<object,object,int>
	// ET.AwakeSystem<object,object,object,byte>
	// ET.AwakeSystem<object,object,object,object>
	// ET.AwakeSystem<object,object,object>
	// ET.AwakeSystem<object,object>
	// ET.AwakeSystem<object>
	// ET.Client.IYIUIEvent<ET.Client.EventPutTipsView>
	// ET.Client.IYIUIEvent<ET.Client.OnCardItemHighlightEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnCardSelectClickEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnCardSelectResetEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnCardViewResetEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnClickChildListEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnClickItemEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnClickParentListEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnGMEventClose>
	// ET.Client.IYIUIEvent<ET.Client.UIArrowDragEndEvent>
	// ET.Client.IYIUIEvent<ET.Client.UIArrowDragEvent>
	// ET.Client.IYIUIEvent<ET.Client.UIArrowDragStartEvent>
	// ET.Client.IYIUIEvent<ET.Client.UICardDragEndEvent>
	// ET.Client.IYIUIEvent<ET.Client.UICardDragStartEvent>
	// ET.Client.IYIUIEvent<ET.Client.UISelectDragEndEvent>
	// ET.Client.IYIUIEvent<ET.Client.UISelectDragEvent>
	// ET.Client.IYIUIEvent<ET.Client.UISelectDragStartEvent>
	// ET.Client.IYIUIOpen<int>
	// ET.Client.IYIUIOpen<object,object,object>
	// ET.Client.IYIUIOpen<object>
	// ET.Client.YIUIBindSystem<object>
	// ET.Client.YIUICloseTweenSystem<object>
	// ET.Client.YIUIEventSystem<object,ET.Client.EventPutTipsView>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnCardItemHighlightEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnCardSelectClickEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnCardSelectResetEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnCardViewResetEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnClickChildListEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnClickItemEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnClickParentListEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnGMEventClose>
	// ET.Client.YIUIEventSystem<object,ET.Client.UIArrowDragEndEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.UIArrowDragEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.UIArrowDragStartEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.UICardDragEndEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.UICardDragStartEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.UISelectDragEndEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.UISelectDragEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.UISelectDragStartEvent>
	// ET.Client.YIUIInitializeSystem<object>
	// ET.Client.YIUIOpenSystem<object,int>
	// ET.Client.YIUIOpenSystem<object,object,object,object>
	// ET.Client.YIUIOpenSystem<object,object>
	// ET.Client.YIUIOpenSystem<object>
	// ET.Client.YIUIOpenTweenSystem<object>
	// ET.ConfigSystem<object>
	// ET.DeserializeSystem<object>
	// ET.DestroySystem<object>
	// ET.DoubleMap<object,long>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.WaitType.Wait_Room2C_Start>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>
	// ET.ETAsyncTaskMethodBuilder<ET.EntityRef<object>>
	// ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>
	// ET.ETAsyncTaskMethodBuilder<byte>
	// ET.ETAsyncTaskMethodBuilder<int>
	// ET.ETAsyncTaskMethodBuilder<long>
	// ET.ETAsyncTaskMethodBuilder<object>
	// ET.ETAsyncTaskMethodBuilder<uint>
	// ET.ETTask<ET.Client.WaitType.Wait_Room2C_Start>
	// ET.ETTask<ET.Client.Wait_CreateMyUnit>
	// ET.ETTask<ET.Client.Wait_SceneChangeFinish>
	// ET.ETTask<ET.Client.Wait_UnitStop>
	// ET.ETTask<ET.EntityRef<object>>
	// ET.ETTask<System.ValueTuple<uint,object>>
	// ET.ETTask<byte>
	// ET.ETTask<int>
	// ET.ETTask<long>
	// ET.ETTask<object>
	// ET.ETTask<uint>
	// ET.EntityRef<object>
	// ET.IAwake<TrueSync.TSVector,TrueSync.TSQuaternion>
	// ET.IAwake<UnityEngine.Vector3,float,float>
	// ET.IAwake<byte>
	// ET.IAwake<int,byte>
	// ET.IAwake<int,int>
	// ET.IAwake<int,object,TrueSync.TSVector>
	// ET.IAwake<int,object,object>
	// ET.IAwake<int,object>
	// ET.IAwake<int>
	// ET.IAwake<long>
	// ET.IAwake<object,int>
	// ET.IAwake<object,object,byte>
	// ET.IAwake<object,object,object>
	// ET.IAwake<object,object>
	// ET.IAwake<object>
	// ET.IAwakeSystem<TrueSync.TSVector,TrueSync.TSQuaternion>
	// ET.IAwakeSystem<UnityEngine.Vector3,float,float>
	// ET.IAwakeSystem<byte>
	// ET.IAwakeSystem<int,byte>
	// ET.IAwakeSystem<int,int>
	// ET.IAwakeSystem<int,object,TrueSync.TSVector>
	// ET.IAwakeSystem<int,object,object>
	// ET.IAwakeSystem<int,object>
	// ET.IAwakeSystem<int>
	// ET.IAwakeSystem<long>
	// ET.IAwakeSystem<object,int>
	// ET.IAwakeSystem<object,object,byte>
	// ET.IAwakeSystem<object,object,object>
	// ET.IAwakeSystem<object,object>
	// ET.IAwakeSystem<object>
	// ET.LateUpdateSystem<object>
	// ET.ListComponent<Unity.Mathematics.float3>
	// ET.Singleton<object>
	// ET.StateMachineWrap<ET.Client.A2NetClient_MessageHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.A2NetClient_RequestHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AI_Attack.<Execute>d__1>
	// ET.StateMachineWrap<ET.Client.AI_XunLuo.<Execute>d__1>
	// ET.StateMachineWrap<ET.Client.AfterCreateClientScene_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AfterCreateClientScene_LSAddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<Call>d__6>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<DisposeAsync>d__3>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2>
	// ET.StateMachineWrap<ET.Client.CommonPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.EffectViewComponentSystem.<PlayFx>d__3>
	// ET.StateMachineWrap<ET.Client.EffectViewComponentSystem.<PlayFx>d__4>
	// ET.StateMachineWrap<ET.Client.EnterMapHelper.<EnterMapAsync>d__0>
	// ET.StateMachineWrap<ET.Client.EnterMapHelper.<Match>d__1>
	// ET.StateMachineWrap<ET.Client.EntryEvent3_InitClient.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.FiberInit_NetClient.<Handle>d__0>
	// ET.StateMachineWrap<ET.Client.G2C_ReconnectHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.GMCommandComponentSystem.<Run>d__5>
	// ET.StateMachineWrap<ET.Client.GMCommandItemComponentSystem.<WaitRefresh>d__6>
	// ET.StateMachineWrap<ET.Client.GMPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.GMToolsBatchViewComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.GMToolsPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.GMToolsStageViewComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.GMViewComponentSystem.<YIUIEvent>d__5>
	// ET.StateMachineWrap<ET.Client.GMViewComponentSystem.<YIUIOpen>d__6>
	// ET.StateMachineWrap<ET.Client.GM_OpenReddotPanel.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_Performance.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_Test.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_TipsTest1.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_TipsTest2.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_TipsTest3.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_TipsTest4.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_TipsTest5.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.GM_UploadFile.<Run>d__1>
	// ET.StateMachineWrap<ET.Client.HttpClientHelper.<Get>d__0>
	// ET.StateMachineWrap<ET.Client.LSCardSelectPanelComponentSystem.<YIUICloseTween>d__7>
	// ET.StateMachineWrap<ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__10>
	// ET.StateMachineWrap<ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__8>
	// ET.StateMachineWrap<ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpenTween>d__6>
	// ET.StateMachineWrap<ET.Client.LSEscapeEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSGridDataResetEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSLobbyPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.LSOperaDragComponentSystem.<ScanTouchLongPress>d__4>
	// ET.StateMachineWrap<ET.Client.LSPlacementDragEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSPlacementNewEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSPlacementRotateEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSRoomPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.LSSceneChangeHelper.<SceneChangeTo>d__0>
	// ET.StateMachineWrap<ET.Client.LSSceneChangeHelper.<SceneChangeToReconnect>d__2>
	// ET.StateMachineWrap<ET.Client.LSSceneChangeHelper.<SceneChangeToReplay>d__1>
	// ET.StateMachineWrap<ET.Client.LSSceneChangeStart_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSSceneInitFinish_Finish.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSSelectionChangedEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSStageGameOverEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSTouchDragCancelEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSTouchDragEndEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSTouchDragEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSTouchDragStartEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitCardBagAddEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitCardBagRemoveEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitCardSelectAddEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitCardSelectDoneEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitCastingEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitCreateEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitFloatingEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitFxEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitMovingEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitPlacedEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitPropEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LSUnitRemoveEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LobbyPanelComponentSystem.<OnEventEnterAction>d__6>
	// ET.StateMachineWrap<ET.Client.LobbyPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.LoginFinish_CreateLobbyUI.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LoginFinish_CreateUILSLobby.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LoginFinish_RemoveLoginUI.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LoginHelper.<Login>d__0>
	// ET.StateMachineWrap<ET.Client.LoginPanelComponentSystem.<OnEventLoginAction>d__8>
	// ET.StateMachineWrap<ET.Client.LoginPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.M2C_CreateMyUnitHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_CreateUnitsHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_PathfindingResultHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_RemoveUnitsHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_StartSceneChangeHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_StopHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.Main2NetClient_LoginHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.MainPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.Match2G_MatchSuccessHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.MessageTipsViewComponentSystem.<YIUICloseTween>d__7>
	// ET.StateMachineWrap<ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__8>
	// ET.StateMachineWrap<ET.Client.MessageTipsViewComponentSystem.<YIUIOpenTween>d__6>
	// ET.StateMachineWrap<ET.Client.MoveHelper.<MoveToAsync>d__0>
	// ET.StateMachineWrap<ET.Client.MoveHelper.<MoveToAsync>d__1>
	// ET.StateMachineWrap<ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.OperaComponentSystem.<Test1>d__2>
	// ET.StateMachineWrap<ET.Client.OperaComponentSystem.<Test2>d__3>
	// ET.StateMachineWrap<ET.Client.PingComponentSystem.<PingAsync>d__2>
	// ET.StateMachineWrap<ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__5>
	// ET.StateMachineWrap<ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__6>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__10>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__11>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__12>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__13>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__14>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__15>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__16>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__7>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__8>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__9>
	// ET.StateMachineWrap<ET.Client.PlayViewComponentSystem.<YIUIOpen>d__6>
	// ET.StateMachineWrap<ET.Client.PrefabPoolComponentSystem.<FetchAsync>d__5>
	// ET.StateMachineWrap<ET.Client.PrefabPoolComponentSystem.<PreloadAsync>d__9>
	// ET.StateMachineWrap<ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__6>
	// ET.StateMachineWrap<ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__7>
	// ET.StateMachineWrap<ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__8>
	// ET.StateMachineWrap<ET.Client.RedDotPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.ReplayViewComponentSystem.<YIUIOpen>d__6>
	// ET.StateMachineWrap<ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__5<object>>
	// ET.StateMachineWrap<ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__4<object>>
	// ET.StateMachineWrap<ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__6>
	// ET.StateMachineWrap<ET.Client.Room2C_CheckHashFailHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.Room2C_CheckHashFailHandler.<SaveToFile>d__3>
	// ET.StateMachineWrap<ET.Client.Room2C_EnterHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.Room2C_FrameMessageHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.Room2C_StartHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.Room2C_TimeAdjustHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2>
	// ET.StateMachineWrap<ET.Client.RouterAddressComponentSystem.<Init>d__1>
	// ET.StateMachineWrap<ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3>
	// ET.StateMachineWrap<ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1>
	// ET.StateMachineWrap<ET.Client.RouterHelper.<Connect>d__2>
	// ET.StateMachineWrap<ET.Client.RouterHelper.<CreateRouterSession>d__0>
	// ET.StateMachineWrap<ET.Client.RouterHelper.<GetRouterAddress>d__1>
	// ET.StateMachineWrap<ET.Client.SceneChangeFinishEvent_CreateUIHelp.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.SceneChangeHelper.<SceneChangeTo>d__0>
	// ET.StateMachineWrap<ET.Client.SceneChangeStart_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.SettlePanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.SettleStatViewComponentSystem.<OnEventBackLastAction>d__6>
	// ET.StateMachineWrap<ET.Client.SettleStatViewComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.TextTipsViewComponentSystem.<PlayAnimation>d__6>
	// ET.StateMachineWrap<ET.Client.TextTipsViewComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.TipsHelper.<CloseTipsView>d__7>
	// ET.StateMachineWrap<ET.Client.TipsHelper.<Open>d__0<object>>
	// ET.StateMachineWrap<ET.Client.TipsHelper.<OpenToParent2NewVo>d__6<object>>
	// ET.StateMachineWrap<ET.Client.TipsHelper.<OpenToParent>d__2<object>>
	// ET.StateMachineWrap<ET.Client.TipsHelper.<OpenToParent>d__4<object>>
	// ET.StateMachineWrap<ET.Client.TipsPanelComponentSystem.<>c__DisplayClass8_0.<<OpenTips>g__Create|0>d>
	// ET.StateMachineWrap<ET.Client.TipsPanelComponentSystem.<OpenTips>d__8>
	// ET.StateMachineWrap<ET.Client.TipsPanelComponentSystem.<PutTips>d__9>
	// ET.StateMachineWrap<ET.Client.TipsPanelComponentSystem.<YIUIEvent>d__6>
	// ET.StateMachineWrap<ET.Client.TipsPanelComponentSystem.<YIUIOpen>d__5>
	// ET.StateMachineWrap<ET.Client.UploadFileAddressComponentSystem.<UploadFile>d__1>
	// ET.StateMachineWrap<ET.Client.UploadFileAddressComponentSystem.<UploadFileAsync>d__2>
	// ET.StateMachineWrap<ET.Client.YIUIInvokeCoroutineLockHandler.<Handle>d__0>
	// ET.StateMachineWrap<ET.Client.YIUIInvokeTimerComponentWaitAsyncHandler.<Handle>d__0>
	// ET.StateMachineWrap<ET.Client.YIUIInvokeTimerComponentWaitFrameAsyncHandler.<Handle>d__0>
	// ET.StateMachineWrap<ET.ConsoleComponentSystem.<Start>d__1>
	// ET.StateMachineWrap<ET.Entry.<StartAsync>d__2>
	// ET.StateMachineWrap<ET.EntryEvent1_InitShare.<Run>d__0>
	// ET.StateMachineWrap<ET.FiberInit_Main.<Handle>d__0>
	// ET.StateMachineWrap<ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1>
	// ET.StateMachineWrap<ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1>
	// ET.StateMachineWrap<ET.MessageHandler.<Handle>d__1<object,object,object>>
	// ET.StateMachineWrap<ET.MessageHandler.<Handle>d__1<object,object>>
	// ET.StateMachineWrap<ET.MessageSessionHandler.<HandleAsync>d__2<object,object>>
	// ET.StateMachineWrap<ET.MessageSessionHandler.<HandleAsync>d__2<object>>
	// ET.StateMachineWrap<ET.MoveComponentSystem.<MoveToAsync>d__5>
	// ET.StateMachineWrap<ET.NumericChangeEvent_NotifyWatcher.<Run>d__0>
	// ET.StateMachineWrap<ET.ObjectWaitSystem.<>c__DisplayClass5_0.<<Wait>g__WaitTimeout|0>d<object>>
	// ET.StateMachineWrap<ET.ObjectWaitSystem.<Wait>d__4<object>>
	// ET.StateMachineWrap<ET.ObjectWaitSystem.<Wait>d__5<object>>
	// ET.StateMachineWrap<ET.ReloadConfigConsoleHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.ReloadDllConsoleHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.RpcInfo.<Wait>d__7>
	// ET.StateMachineWrap<ET.SessionSystem.<>c__DisplayClass4_0.<<Call>g__Timeout|0>d>
	// ET.StateMachineWrap<ET.SessionSystem.<Call>d__3>
	// ET.StateMachineWrap<ET.SessionSystem.<Call>d__4>
	// ET.StructBsonSerialize<TrueSync.FP>
	// ET.StructBsonSerialize<TrueSync.TSQuaternion>
	// ET.StructBsonSerialize<TrueSync.TSVector2>
	// ET.StructBsonSerialize<TrueSync.TSVector4>
	// ET.StructBsonSerialize<TrueSync.TSVector>
	// ET.StructBsonSerialize<Unity.Mathematics.float2>
	// ET.StructBsonSerialize<Unity.Mathematics.float3>
	// ET.StructBsonSerialize<Unity.Mathematics.float4>
	// ET.StructBsonSerialize<Unity.Mathematics.quaternion>
	// ET.StructBsonSerialize<object>
	// ET.UnOrderMultiMap<object,object>
	// ET.UpdateSystem<object>
	// MemoryPack.Formatters.ArrayFormatter<ET.SearchUnitPackable>
	// MemoryPack.Formatters.ArrayFormatter<ST.GridBuilder.FlowFieldNode>
	// MemoryPack.Formatters.ArrayFormatter<byte>
	// MemoryPack.Formatters.ArrayFormatter<object>
	// MemoryPack.Formatters.DictionaryFormatter<int,TrueSync.FP>
	// MemoryPack.Formatters.DictionaryFormatter<int,int>
	// MemoryPack.Formatters.DictionaryFormatter<int,long>
	// MemoryPack.Formatters.DictionaryFormatter<int,object>
	// MemoryPack.Formatters.DictionaryFormatter<long,long>
	// MemoryPack.Formatters.ListFormatter<ET.LSCommandData>
	// MemoryPack.Formatters.ListFormatter<ET.LSRandomDropItem>
	// MemoryPack.Formatters.ListFormatter<ET.SearchUnitPackable>
	// MemoryPack.Formatters.ListFormatter<TrueSync.TSVector>
	// MemoryPack.Formatters.ListFormatter<Unity.Mathematics.float3>
	// MemoryPack.Formatters.ListFormatter<long>
	// MemoryPack.Formatters.ListFormatter<object>
	// MemoryPack.Formatters.SortedDictionaryFormatter<long,long>
	// MemoryPack.Formatters.StackFormatter<int>
	// MemoryPack.Formatters.UnmanagedFormatter<int>
	// MemoryPack.IMemoryPackFormatter<ET.LSCommandData>
	// MemoryPack.IMemoryPackFormatter<ET.LSRandomDropItem>
	// MemoryPack.IMemoryPackFormatter<ET.SearchUnitPackable>
	// MemoryPack.IMemoryPackFormatter<TrueSync.TSVector>
	// MemoryPack.IMemoryPackFormatter<Unity.Mathematics.float3>
	// MemoryPack.IMemoryPackFormatter<byte>
	// MemoryPack.IMemoryPackFormatter<int>
	// MemoryPack.IMemoryPackFormatter<long>
	// MemoryPack.IMemoryPackFormatter<object>
	// MemoryPack.IMemoryPackable<ET.SearchUnitPackable>
	// MemoryPack.IMemoryPackable<object>
	// MemoryPack.MemoryPackFormatter<ET.SearchUnitPackable>
	// MemoryPack.MemoryPackFormatter<System.UIntPtr>
	// MemoryPack.MemoryPackFormatter<int>
	// MemoryPack.MemoryPackFormatter<object>
	// MongoDB.Bson.Serialization.IBsonSerializer<object>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<TrueSync.FP>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<TrueSync.TSQuaternion>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<TrueSync.TSVector2>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<TrueSync.TSVector4>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<TrueSync.TSVector>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.float2>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.float3>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.float4>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.quaternion>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<object>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<TrueSync.FP>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<TrueSync.TSQuaternion>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<TrueSync.TSVector2>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<TrueSync.TSVector4>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<TrueSync.TSVector>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.float2>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.float3>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.float4>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.quaternion>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<object>
	// RVO.PoolLinkedList.Enumerator<long,object>
	// RVO.PoolLinkedList<long,object>
	// RVO.PoolLinkedNode<object>
	// System.Action<DotRecast.Detour.StraightPathItem>
	// System.Action<ET.EntityRef<object>>
	// System.Action<ET.LSCommandData>
	// System.Action<ET.LSRandomDropItem>
	// System.Action<ET.MessageSessionDispatcherInfo>
	// System.Action<ET.NumericWatcherInfo>
	// System.Action<ET.SearchUnit>
	// System.Action<ET.SearchUnitPackable>
	// System.Action<ST.GridBuilder.FieldV2>
	// System.Action<ST.GridBuilder.IndexV2>
	// System.Action<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Action<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Action<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Action<System.UIntPtr>
	// System.Action<TrueSync.TSVector2>
	// System.Action<TrueSync.TSVector>
	// System.Action<Unity.Mathematics.float3>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte,byte>
	// System.Action<byte>
	// System.Action<int,int>
	// System.Action<int>
	// System.Action<long,int>
	// System.Action<long,object>
	// System.Action<long>
	// System.Action<object,long>
	// System.Action<object,object>
	// System.Action<object>
	// System.ArraySegment.Enumerator<UnityEngine.Vector3>
	// System.ArraySegment.Enumerator<byte>
	// System.ArraySegment<UnityEngine.Vector3>
	// System.ArraySegment<byte>
	// System.Buffers.ArrayPool<byte>
	// System.Buffers.ConfigurableArrayPool.Bucket<byte>
	// System.Buffers.ConfigurableArrayPool<byte>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.LockedStack<byte>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.PerCoreLockedStacks<byte>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool<byte>
	// System.ByReference<UnityEngine.Vector3>
	// System.ByReference<byte>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__35<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary<object,object>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__28<object>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<object>
	// System.Collections.Concurrent.ConcurrentQueue<object>
	// System.Collections.Generic.ArraySortHelper<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ArraySortHelper<ET.EntityRef<object>>
	// System.Collections.Generic.ArraySortHelper<ET.LSCommandData>
	// System.Collections.Generic.ArraySortHelper<ET.LSRandomDropItem>
	// System.Collections.Generic.ArraySortHelper<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.ArraySortHelper<ET.NumericWatcherInfo>
	// System.Collections.Generic.ArraySortHelper<ET.SearchUnit>
	// System.Collections.Generic.ArraySortHelper<ET.SearchUnitPackable>
	// System.Collections.Generic.ArraySortHelper<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.ArraySortHelper<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ArraySortHelper<System.UIntPtr>
	// System.Collections.Generic.ArraySortHelper<TrueSync.TSVector2>
	// System.Collections.Generic.ArraySortHelper<TrueSync.TSVector>
	// System.Collections.Generic.ArraySortHelper<Unity.Mathematics.float3>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.Comparer<ET.ActorId>
	// System.Collections.Generic.Comparer<ET.EntityRef<object>>
	// System.Collections.Generic.Comparer<ET.LSCommandData>
	// System.Collections.Generic.Comparer<ET.LSRandomDropItem>
	// System.Collections.Generic.Comparer<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.Comparer<ET.NumericWatcherInfo>
	// System.Collections.Generic.Comparer<ET.SearchUnit>
	// System.Collections.Generic.Comparer<ET.SearchUnitPackable>
	// System.Collections.Generic.Comparer<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.Comparer<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Comparer<System.UIntPtr>
	// System.Collections.Generic.Comparer<TrueSync.TSVector2>
	// System.Collections.Generic.Comparer<TrueSync.TSVector>
	// System.Collections.Generic.Comparer<Unity.Mathematics.float3>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<uint>
	// System.Collections.Generic.Comparer<ushort>
	// System.Collections.Generic.ComparisonComparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ComparisonComparer<ET.ActorId>
	// System.Collections.Generic.ComparisonComparer<ET.EntityRef<object>>
	// System.Collections.Generic.ComparisonComparer<ET.LSCommandData>
	// System.Collections.Generic.ComparisonComparer<ET.LSRandomDropItem>
	// System.Collections.Generic.ComparisonComparer<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.ComparisonComparer<ET.NumericWatcherInfo>
	// System.Collections.Generic.ComparisonComparer<ET.SearchUnit>
	// System.Collections.Generic.ComparisonComparer<ET.SearchUnitPackable>
	// System.Collections.Generic.ComparisonComparer<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.ComparisonComparer<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ComparisonComparer<System.UIntPtr>
	// System.Collections.Generic.ComparisonComparer<TrueSync.TSVector2>
	// System.Collections.Generic.ComparisonComparer<TrueSync.TSVector>
	// System.Collections.Generic.ComparisonComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ComparisonComparer<byte>
	// System.Collections.Generic.ComparisonComparer<int>
	// System.Collections.Generic.ComparisonComparer<long>
	// System.Collections.Generic.ComparisonComparer<object>
	// System.Collections.Generic.ComparisonComparer<uint>
	// System.Collections.Generic.ComparisonComparer<ushort>
	// System.Collections.Generic.Dictionary.Enumerator<int,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.Enumerator<int,TrueSync.FP>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.Enumerator<object,YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,TrueSync.FP>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.KeyCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection<int,TrueSync.FP>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.KeyCollection<long,long>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.KeyCollection<object,YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<uint,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,TrueSync.FP>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.ValueCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection<int,TrueSync.FP>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.ValueCollection<long,long>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary.ValueCollection<object,YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<uint,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ushort,object>
	// System.Collections.Generic.Dictionary<int,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary<int,TrueSync.FP>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,long>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary<long,long>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,ET.EntityRef<object>>
	// System.Collections.Generic.Dictionary<object,YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,long>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<uint,object>
	// System.Collections.Generic.Dictionary<ushort,object>
	// System.Collections.Generic.EqualityComparer<ET.ActorId>
	// System.Collections.Generic.EqualityComparer<ET.EntityRef<object>>
	// System.Collections.Generic.EqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.EqualityComparer<TrueSync.FP>
	// System.Collections.Generic.EqualityComparer<TrueSync.TSVector2>
	// System.Collections.Generic.EqualityComparer<YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<uint>
	// System.Collections.Generic.EqualityComparer<ushort>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<long>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet.Enumerator<ushort>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<long>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSet<ushort>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<long>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.HashSetEqualityComparer<ushort>
	// System.Collections.Generic.ICollection<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ICollection<ET.EntityRef<object>>
	// System.Collections.Generic.ICollection<ET.LSCommandData>
	// System.Collections.Generic.ICollection<ET.LSRandomDropItem>
	// System.Collections.Generic.ICollection<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.ICollection<ET.NumericWatcherInfo>
	// System.Collections.Generic.ICollection<ET.RpcInfo>
	// System.Collections.Generic.ICollection<ET.SearchUnit>
	// System.Collections.Generic.ICollection<ET.SearchUnitPackable>
	// System.Collections.Generic.ICollection<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.ICollection<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,ET.EntityRef<object>>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,TrueSync.FP>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,ET.EntityRef<object>>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,YIUIFramework.YIUIBindVo>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.ICollection<System.UIntPtr>
	// System.Collections.Generic.ICollection<TrueSync.TSVector2>
	// System.Collections.Generic.ICollection<TrueSync.TSVector>
	// System.Collections.Generic.ICollection<Unity.Mathematics.float3>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IComparer<ET.EntityRef<object>>
	// System.Collections.Generic.IComparer<ET.LSCommandData>
	// System.Collections.Generic.IComparer<ET.LSRandomDropItem>
	// System.Collections.Generic.IComparer<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.IComparer<ET.NumericWatcherInfo>
	// System.Collections.Generic.IComparer<ET.SearchUnit>
	// System.Collections.Generic.IComparer<ET.SearchUnitPackable>
	// System.Collections.Generic.IComparer<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.IComparer<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IComparer<System.UIntPtr>
	// System.Collections.Generic.IComparer<TrueSync.TSVector2>
	// System.Collections.Generic.IComparer<TrueSync.TSVector>
	// System.Collections.Generic.IComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IEnumerable<ET.EntityRef<object>>
	// System.Collections.Generic.IEnumerable<ET.LSCommandData>
	// System.Collections.Generic.IEnumerable<ET.LSRandomDropItem>
	// System.Collections.Generic.IEnumerable<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.IEnumerable<ET.NumericWatcherInfo>
	// System.Collections.Generic.IEnumerable<ET.RpcInfo>
	// System.Collections.Generic.IEnumerable<ET.SearchUnit>
	// System.Collections.Generic.IEnumerable<ET.SearchUnitPackable>
	// System.Collections.Generic.IEnumerable<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.IEnumerable<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,ET.EntityRef<object>>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,TrueSync.FP>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,ET.EntityRef<object>>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,YIUIFramework.YIUIBindVo>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerable<System.UIntPtr>
	// System.Collections.Generic.IEnumerable<TrueSync.TSVector2>
	// System.Collections.Generic.IEnumerable<TrueSync.TSVector>
	// System.Collections.Generic.IEnumerable<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IEnumerator<ET.EntityRef<object>>
	// System.Collections.Generic.IEnumerator<ET.LSCommandData>
	// System.Collections.Generic.IEnumerator<ET.LSRandomDropItem>
	// System.Collections.Generic.IEnumerator<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.IEnumerator<ET.NumericWatcherInfo>
	// System.Collections.Generic.IEnumerator<ET.RpcInfo>
	// System.Collections.Generic.IEnumerator<ET.SearchUnit>
	// System.Collections.Generic.IEnumerator<ET.SearchUnitPackable>
	// System.Collections.Generic.IEnumerator<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.IEnumerator<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,ET.EntityRef<object>>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,TrueSync.FP>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,ET.EntityRef<object>>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,YIUIFramework.YIUIBindVo>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerator<System.UIntPtr>
	// System.Collections.Generic.IEnumerator<TrueSync.TSVector2>
	// System.Collections.Generic.IEnumerator<TrueSync.TSVector>
	// System.Collections.Generic.IEnumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<uint>
	// System.Collections.Generic.IEqualityComparer<ushort>
	// System.Collections.Generic.IList<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IList<ET.EntityRef<object>>
	// System.Collections.Generic.IList<ET.LSCommandData>
	// System.Collections.Generic.IList<ET.LSRandomDropItem>
	// System.Collections.Generic.IList<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.IList<ET.NumericWatcherInfo>
	// System.Collections.Generic.IList<ET.SearchUnit>
	// System.Collections.Generic.IList<ET.SearchUnitPackable>
	// System.Collections.Generic.IList<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.IList<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IList<System.UIntPtr>
	// System.Collections.Generic.IList<TrueSync.TSVector2>
	// System.Collections.Generic.IList<TrueSync.TSVector>
	// System.Collections.Generic.IList<Unity.Mathematics.float3>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IReadOnlyDictionary<int,object>
	// System.Collections.Generic.KeyValuePair<System.UIntPtr,object>
	// System.Collections.Generic.KeyValuePair<int,ET.EntityRef<object>>
	// System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>
	// System.Collections.Generic.KeyValuePair<int,TrueSync.FP>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,long>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>
	// System.Collections.Generic.KeyValuePair<long,long>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,ET.EntityRef<object>>
	// System.Collections.Generic.KeyValuePair<object,YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,long>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<uint,object>
	// System.Collections.Generic.KeyValuePair<ushort,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.List.Enumerator<ET.EntityRef<object>>
	// System.Collections.Generic.List.Enumerator<ET.LSCommandData>
	// System.Collections.Generic.List.Enumerator<ET.LSRandomDropItem>
	// System.Collections.Generic.List.Enumerator<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.List.Enumerator<ET.NumericWatcherInfo>
	// System.Collections.Generic.List.Enumerator<ET.SearchUnit>
	// System.Collections.Generic.List.Enumerator<ET.SearchUnitPackable>
	// System.Collections.Generic.List.Enumerator<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.List.Enumerator<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List.Enumerator<System.UIntPtr>
	// System.Collections.Generic.List.Enumerator<TrueSync.TSVector2>
	// System.Collections.Generic.List.Enumerator<TrueSync.TSVector>
	// System.Collections.Generic.List.Enumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.List<ET.EntityRef<object>>
	// System.Collections.Generic.List<ET.LSCommandData>
	// System.Collections.Generic.List<ET.LSRandomDropItem>
	// System.Collections.Generic.List<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.List<ET.NumericWatcherInfo>
	// System.Collections.Generic.List<ET.SearchUnit>
	// System.Collections.Generic.List<ET.SearchUnitPackable>
	// System.Collections.Generic.List<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.List<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List<System.UIntPtr>
	// System.Collections.Generic.List<TrueSync.TSVector2>
	// System.Collections.Generic.List<TrueSync.TSVector>
	// System.Collections.Generic.List<Unity.Mathematics.float3>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ObjectComparer<ET.ActorId>
	// System.Collections.Generic.ObjectComparer<ET.EntityRef<object>>
	// System.Collections.Generic.ObjectComparer<ET.LSCommandData>
	// System.Collections.Generic.ObjectComparer<ET.LSRandomDropItem>
	// System.Collections.Generic.ObjectComparer<ET.MessageSessionDispatcherInfo>
	// System.Collections.Generic.ObjectComparer<ET.NumericWatcherInfo>
	// System.Collections.Generic.ObjectComparer<ET.SearchUnit>
	// System.Collections.Generic.ObjectComparer<ET.SearchUnitPackable>
	// System.Collections.Generic.ObjectComparer<ST.GridBuilder.FieldV2>
	// System.Collections.Generic.ObjectComparer<ST.GridBuilder.IndexV2>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectComparer<System.UIntPtr>
	// System.Collections.Generic.ObjectComparer<TrueSync.TSVector2>
	// System.Collections.Generic.ObjectComparer<TrueSync.TSVector>
	// System.Collections.Generic.ObjectComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<uint>
	// System.Collections.Generic.ObjectComparer<ushort>
	// System.Collections.Generic.ObjectEqualityComparer<ET.ActorId>
	// System.Collections.Generic.ObjectEqualityComparer<ET.EntityRef<object>>
	// System.Collections.Generic.ObjectEqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectEqualityComparer<TrueSync.FP>
	// System.Collections.Generic.ObjectEqualityComparer<TrueSync.TSVector2>
	// System.Collections.Generic.ObjectEqualityComparer<YIUIFramework.YIUIBindVo>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<uint>
	// System.Collections.Generic.ObjectEqualityComparer<ushort>
	// System.Collections.Generic.Queue.Enumerator<ET.LSCommandData>
	// System.Collections.Generic.Queue.Enumerator<int>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<ET.LSCommandData>
	// System.Collections.Generic.Queue<int>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<int,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<long,long>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<long,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<object,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<int,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<long,long>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<long,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<object,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.Enumerator<long,long>
	// System.Collections.Generic.SortedDictionary.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<long,long>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<long,long>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<long,long>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.KeyCollection<long,long>
	// System.Collections.Generic.SortedDictionary.KeyCollection<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<object,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<int,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<long,long>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<long,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<long,long>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<long,long>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<long,long>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary.ValueCollection<long,long>
	// System.Collections.Generic.SortedDictionary.ValueCollection<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<object,object>
	// System.Collections.Generic.SortedDictionary<int,object>
	// System.Collections.Generic.SortedDictionary<long,ET.EntityRef<object>>
	// System.Collections.Generic.SortedDictionary<long,long>
	// System.Collections.Generic.SortedDictionary<long,object>
	// System.Collections.Generic.SortedDictionary<object,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Stack.Enumerator<ET.EntityRef<object>>
	// System.Collections.Generic.Stack.Enumerator<int>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<ET.EntityRef<object>>
	// System.Collections.Generic.Stack<int>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<DotRecast.Detour.StraightPathItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.EntityRef<object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.LSCommandData>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.LSRandomDropItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.MessageSessionDispatcherInfo>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.NumericWatcherInfo>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.SearchUnit>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.SearchUnitPackable>
	// System.Collections.ObjectModel.ReadOnlyCollection<ST.GridBuilder.FieldV2>
	// System.Collections.ObjectModel.ReadOnlyCollection<ST.GridBuilder.IndexV2>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.UIntPtr>
	// System.Collections.ObjectModel.ReadOnlyCollection<TrueSync.TSVector2>
	// System.Collections.ObjectModel.ReadOnlyCollection<TrueSync.TSVector>
	// System.Collections.ObjectModel.ReadOnlyCollection<Unity.Mathematics.float3>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<DotRecast.Detour.StraightPathItem>
	// System.Comparison<ET.ActorId>
	// System.Comparison<ET.EntityRef<object>>
	// System.Comparison<ET.LSCommandData>
	// System.Comparison<ET.LSRandomDropItem>
	// System.Comparison<ET.MessageSessionDispatcherInfo>
	// System.Comparison<ET.NumericWatcherInfo>
	// System.Comparison<ET.SearchUnit>
	// System.Comparison<ET.SearchUnitPackable>
	// System.Comparison<ST.GridBuilder.FieldV2>
	// System.Comparison<ST.GridBuilder.IndexV2>
	// System.Comparison<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Comparison<System.UIntPtr>
	// System.Comparison<TrueSync.TSVector2>
	// System.Comparison<TrueSync.TSVector>
	// System.Comparison<Unity.Mathematics.float3>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<uint>
	// System.Comparison<ushort>
	// System.Func<object,byte>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Linq.Buffer<ET.RpcInfo>
	// System.Nullable<TrueSync.TSVector>
	// System.Nullable<YIUIFramework.YIUIBindVo>
	// System.Predicate<DotRecast.Detour.StraightPathItem>
	// System.Predicate<ET.EntityRef<object>>
	// System.Predicate<ET.LSCommandData>
	// System.Predicate<ET.LSRandomDropItem>
	// System.Predicate<ET.MessageSessionDispatcherInfo>
	// System.Predicate<ET.NumericWatcherInfo>
	// System.Predicate<ET.SearchUnit>
	// System.Predicate<ET.SearchUnitPackable>
	// System.Predicate<ST.GridBuilder.FieldV2>
	// System.Predicate<ST.GridBuilder.IndexV2>
	// System.Predicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<long,ET.EntityRef<object>>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Predicate<System.UIntPtr>
	// System.Predicate<TrueSync.TSVector2>
	// System.Predicate<TrueSync.TSVector>
	// System.Predicate<Unity.Mathematics.float3>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ushort>
	// System.ReadOnlySpan.Enumerator<UnityEngine.Vector3>
	// System.ReadOnlySpan.Enumerator<byte>
	// System.ReadOnlySpan<UnityEngine.Vector3>
	// System.ReadOnlySpan<byte>
	// System.Runtime.CompilerServices.ConditionalWeakTable.<>c<System.UIntPtr,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.<>c<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<System.UIntPtr,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<System.UIntPtr,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<System.UIntPtr,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span.Enumerator<UnityEngine.Vector3>
	// System.Span.Enumerator<byte>
	// System.Span<UnityEngine.Vector3>
	// System.Span<byte>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory.<>c<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass32_0<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<object>
	// System.ValueTuple<ET.ActorId,object>
	// System.ValueTuple<byte,long>
	// System.ValueTuple<int,TrueSync.TSVector2>
	// System.ValueTuple<int,int>
	// System.ValueTuple<uint,object>
	// System.ValueTuple<uint,uint>
	// System.ValueTuple<ushort,object>
	// Unity.Collections.NativeArray.Enumerator<UnityEngine.Vector3>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<UnityEngine.Vector3>
	// Unity.Collections.NativeArray.ReadOnly<UnityEngine.Vector3>
	// Unity.Collections.NativeArray<UnityEngine.Vector3>
	// UnityEngine.Events.UnityAction<object>
	// YIUIFramework.ArrPrefs<int>
	// YIUIFramework.LinkedListPool.<>c<object>
	// YIUIFramework.LinkedListPool<object>
	// YIUIFramework.ObjAsyncCache<ET.EntityRef<object>>
	// YIUIFramework.ObjCache<object>
	// YIUIFramework.ObjectPool<object>
	// YIUIFramework.Prefs.IPrefsAccessor<object>
	// YIUIFramework.Prefs<object>
	// YIUIFramework.Singleton<object>
	// YIUIFramework.UIDataValueBase<byte>
	// YIUIFramework.UIDataValueBase<int>
	// YIUIFramework.UIDataValueBase<object>
	// YIUIFramework.UIEventDelegate<byte>
	// YIUIFramework.UIEventDelegate<int>
	// YIUIFramework.UIEventDelegate<object>
	// YIUIFramework.UIEventHandleP1<byte>
	// YIUIFramework.UIEventHandleP1<int>
	// YIUIFramework.UIEventHandleP1<object>
	// YIUIFramework.UIEventP1<byte>
	// YIUIFramework.UIEventP1<int>
	// YIUIFramework.UIEventP1<object>
	// YIUIFramework.YIUIListView<object>
	// YIUIFramework.YIUILoopScroll.<>c__DisplayClass83_0<int,object>
	// YIUIFramework.YIUILoopScroll.<>c__DisplayClass83_0<object,object>
	// YIUIFramework.YIUILoopScroll.ListItemRenderer<int,object>
	// YIUIFramework.YIUILoopScroll.ListItemRenderer<object,object>
	// YIUIFramework.YIUILoopScroll.OnClickItemEvent<int,object>
	// YIUIFramework.YIUILoopScroll.OnClickItemEvent<object,object>
	// YIUIFramework.YIUILoopScroll<int,object>
	// YIUIFramework.YIUILoopScroll<object,object>
	// }}

	public void RefMethods()
	{
		// object DG.Tweening.TweenSettingsExtensions.SetTarget<object>(object,object)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.EventPutTipsView>(ET.Fiber,ET.Client.EventPutTipsView)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnCardItemHighlightEvent>(ET.Fiber,ET.Client.OnCardItemHighlightEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnCardSelectClickEvent>(ET.Fiber,ET.Client.OnCardSelectClickEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnCardSelectResetEvent>(ET.Fiber,ET.Client.OnCardSelectResetEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnCardViewResetEvent>(ET.Fiber,ET.Client.OnCardViewResetEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnClickChildListEvent>(ET.Fiber,ET.Client.OnClickChildListEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnClickItemEvent>(ET.Fiber,ET.Client.OnClickItemEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnClickParentListEvent>(ET.Fiber,ET.Client.OnClickParentListEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.OnGMEventClose>(ET.Fiber,ET.Client.OnGMEventClose)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UIArrowDragEndEvent>(ET.Fiber,ET.Client.UIArrowDragEndEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UIArrowDragEvent>(ET.Fiber,ET.Client.UIArrowDragEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UIArrowDragStartEvent>(ET.Fiber,ET.Client.UIArrowDragStartEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UICardDragEndEvent>(ET.Fiber,ET.Client.UICardDragEndEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UICardDragStartEvent>(ET.Fiber,ET.Client.UICardDragStartEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UISelectDragEndEvent>(ET.Fiber,ET.Client.UISelectDragEndEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UISelectDragEvent>(ET.Fiber,ET.Client.UISelectDragEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.UIEvent<ET.Client.UISelectDragStartEvent>(ET.Fiber,ET.Client.UISelectDragStartEvent)
		// YIUIFramework.PanelInfo ET.Client.YIUIMgrComponent.GetPanelInfo<object>()
		// ET.ETTask<bool> ET.Client.YIUIMgrComponentSystem.ClosePanelAsync<object>(ET.Client.YIUIMgrComponent,bool,bool)
		// object ET.Client.YIUIMgrComponentSystem.GetPanel<object>(ET.Client.YIUIMgrComponent)
		// ET.ETTask ET.Client.YIUIMgrComponentSystem.HomePanel<object>(ET.Client.YIUIMgrComponent,bool,ET.Client.YIUIRootComponent)
		// ET.ETTask<object> ET.Client.YIUIPanelComponentSystem.OpenViewAsync<object>(ET.Client.YIUIPanelComponent)
		// ET.ETTask<object> ET.Client.YIUIRootComponentSystem.OpenPanelAsync<object,int>(ET.Client.YIUIRootComponent,int)
		// ET.ETTask<object> ET.Client.YIUIRootComponentSystem.OpenPanelAsync<object,object,object,object>(ET.Client.YIUIRootComponent,object,object,object)
		// ET.ETTask<object> ET.Client.YIUIRootComponentSystem.OpenPanelAsync<object>(ET.Client.YIUIRootComponent)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.A2NetClient_MessageHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.A2NetClient_MessageHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.AfterCreateClientScene_AddComponent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.AfterCreateClientScene_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.AfterCreateClientScene_LSAddComponent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.AfterCreateClientScene_LSAddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.FiberInit_NetClient.<Handle>d__0>(ET.ETTaskCompleted&,ET.Client.FiberInit_NetClient.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.G2C_ReconnectHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.G2C_ReconnectHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__8>(ET.ETTaskCompleted&,ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSEscapeEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSEscapeEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSGridDataResetEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSGridDataResetEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSPlacementDragEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSPlacementDragEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSPlacementNewEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSPlacementNewEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSPlacementRotateEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSPlacementRotateEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSSceneInitFinish_Finish.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSSceneInitFinish_Finish.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSSelectionChangedEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSSelectionChangedEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSStageGameOverEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSStageGameOverEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSTouchDragCancelEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSTouchDragCancelEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSTouchDragEndEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSTouchDragEndEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSTouchDragEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSTouchDragEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSTouchDragStartEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSTouchDragStartEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitCardBagAddEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitCardBagAddEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitCardBagRemoveEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitCardBagRemoveEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitCardSelectAddEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitCardSelectAddEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitCardSelectDoneEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitCardSelectDoneEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitCastingEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitCastingEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitCreateEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitCreateEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitFloatingEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitFloatingEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitMovingEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitMovingEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitPlacedEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitPlacedEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitPropEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitPropEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSUnitRemoveEvent.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.LSUnitRemoveEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.M2C_CreateMyUnitHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.M2C_CreateMyUnitHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.M2C_CreateUnitsHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.M2C_CreateUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.M2C_RemoveUnitsHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.M2C_RemoveUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.M2C_StopHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.M2C_StopHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.Match2G_MatchSuccessHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.Match2G_MatchSuccessHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__5>(ET.ETTaskCompleted&,ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__6>(ET.ETTaskCompleted&,ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__10>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__10&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__11>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__11&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__12>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__12&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__13>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__13&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__14>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__14&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__15>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__15&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__16>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__16&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__7>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__8>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__9>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIEvent>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__6>(ET.ETTaskCompleted&,ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__7>(ET.ETTaskCompleted&,ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__8>(ET.ETTaskCompleted&,ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.Room2C_CheckHashFailHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.Room2C_CheckHashFailHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.Room2C_CheckHashFailHandler.<SaveToFile>d__3>(ET.ETTaskCompleted&,ET.Client.Room2C_CheckHashFailHandler.<SaveToFile>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.Room2C_FrameMessageHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.Room2C_FrameMessageHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.Room2C_StartHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.Room2C_StartHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.Room2C_TimeAdjustHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.Client.Room2C_TimeAdjustHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.SettleStatViewComponentSystem.<OnEventBackLastAction>d__6>(ET.ETTaskCompleted&,ET.Client.SettleStatViewComponentSystem.<OnEventBackLastAction>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.EntryEvent1_InitShare.<Run>d__0>(ET.ETTaskCompleted&,ET.EntryEvent1_InitShare.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.NumericChangeEvent_NotifyWatcher.<Run>d__0>(ET.ETTaskCompleted&,ET.NumericChangeEvent_NotifyWatcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.ReloadConfigConsoleHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.ReloadConfigConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.ReloadDllConsoleHandler.<Run>d__0>(ET.ETTaskCompleted&,ET.ReloadDllConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.Room2C_CheckHashFailHandler.<SaveToFile>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.Room2C_CheckHashFailHandler.<SaveToFile>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ET.ConsoleComponentSystem.<Start>d__1>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ET.ConsoleComponentSystem.<Start>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.A2NetClient_RequestHandler.<Run>d__0>(object&,ET.Client.A2NetClient_RequestHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AI_Attack.<Execute>d__1>(object&,ET.Client.AI_Attack.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AI_XunLuo.<Execute>d__1>(object&,ET.Client.AI_XunLuo.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0>(object&,ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0>(object&,ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<DisposeAsync>d__3>(object&,ET.Client.ClientSenderComponentSystem.<DisposeAsync>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2>(object&,ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EffectViewComponentSystem.<PlayFx>d__3>(object&,ET.Client.EffectViewComponentSystem.<PlayFx>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EffectViewComponentSystem.<PlayFx>d__4>(object&,ET.Client.EffectViewComponentSystem.<PlayFx>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EnterMapHelper.<EnterMapAsync>d__0>(object&,ET.Client.EnterMapHelper.<EnterMapAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EnterMapHelper.<Match>d__1>(object&,ET.Client.EnterMapHelper.<Match>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EntryEvent3_InitClient.<Run>d__0>(object&,ET.Client.EntryEvent3_InitClient.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.G2C_ReconnectHandler.<Run>d__0>(object&,ET.Client.G2C_ReconnectHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.GMCommandComponentSystem.<Run>d__5>(object&,ET.Client.GMCommandComponentSystem.<Run>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.GMCommandItemComponentSystem.<WaitRefresh>d__6>(object&,ET.Client.GMCommandItemComponentSystem.<WaitRefresh>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.GMViewComponentSystem.<YIUIEvent>d__5>(object&,ET.Client.GMViewComponentSystem.<YIUIEvent>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSCardSelectPanelComponentSystem.<YIUICloseTween>d__7>(object&,ET.Client.LSCardSelectPanelComponentSystem.<YIUICloseTween>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__10>(object&,ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__10&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpenTween>d__6>(object&,ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpenTween>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSOperaDragComponentSystem.<ScanTouchLongPress>d__4>(object&,ET.Client.LSOperaDragComponentSystem.<ScanTouchLongPress>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSSceneChangeHelper.<SceneChangeTo>d__0>(object&,ET.Client.LSSceneChangeHelper.<SceneChangeTo>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSSceneChangeHelper.<SceneChangeToReconnect>d__2>(object&,ET.Client.LSSceneChangeHelper.<SceneChangeToReconnect>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSSceneChangeHelper.<SceneChangeToReplay>d__1>(object&,ET.Client.LSSceneChangeHelper.<SceneChangeToReplay>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSSceneChangeStart_AddComponent.<Run>d__0>(object&,ET.Client.LSSceneChangeStart_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSSceneInitFinish_Finish.<Run>d__0>(object&,ET.Client.LSSceneInitFinish_Finish.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSStageGameOverEvent.<Run>d__0>(object&,ET.Client.LSStageGameOverEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LSUnitFxEvent.<Run>d__0>(object&,ET.Client.LSUnitFxEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LobbyPanelComponentSystem.<OnEventEnterAction>d__6>(object&,ET.Client.LobbyPanelComponentSystem.<OnEventEnterAction>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LoginFinish_CreateLobbyUI.<Run>d__0>(object&,ET.Client.LoginFinish_CreateLobbyUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LoginFinish_CreateUILSLobby.<Run>d__0>(object&,ET.Client.LoginFinish_CreateUILSLobby.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LoginFinish_RemoveLoginUI.<Run>d__0>(object&,ET.Client.LoginFinish_RemoveLoginUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LoginHelper.<Login>d__0>(object&,ET.Client.LoginHelper.<Login>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LoginPanelComponentSystem.<OnEventLoginAction>d__8>(object&,ET.Client.LoginPanelComponentSystem.<OnEventLoginAction>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_PathfindingResultHandler.<Run>d__0>(object&,ET.Client.M2C_PathfindingResultHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_StartSceneChangeHandler.<Run>d__0>(object&,ET.Client.M2C_StartSceneChangeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.Main2NetClient_LoginHandler.<Run>d__0>(object&,ET.Client.Main2NetClient_LoginHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MessageTipsViewComponentSystem.<YIUICloseTween>d__7>(object&,ET.Client.MessageTipsViewComponentSystem.<YIUICloseTween>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MessageTipsViewComponentSystem.<YIUIOpenTween>d__6>(object&,ET.Client.MessageTipsViewComponentSystem.<YIUIOpenTween>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MoveHelper.<MoveToAsync>d__1>(object&,ET.Client.MoveHelper.<MoveToAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.OperaComponentSystem.<Test1>d__2>(object&,ET.Client.OperaComponentSystem.<Test1>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.OperaComponentSystem.<Test2>d__3>(object&,ET.Client.OperaComponentSystem.<Test2>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.PingComponentSystem.<PingAsync>d__2>(object&,ET.Client.PingComponentSystem.<PingAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.PrefabPoolComponentSystem.<PreloadAsync>d__9>(object&,ET.Client.PrefabPoolComponentSystem.<PreloadAsync>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__6>(object&,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.Room2C_CheckHashFailHandler.<Run>d__0>(object&,ET.Client.Room2C_CheckHashFailHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.Room2C_EnterHandler.<Run>d__0>(object&,ET.Client.Room2C_EnterHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2>(object&,ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterAddressComponentSystem.<Init>d__1>(object&,ET.Client.RouterAddressComponentSystem.<Init>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3>(object&,ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1>(object&,ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.SceneChangeFinishEvent_CreateUIHelp.<Run>d__0>(object&,ET.Client.SceneChangeFinishEvent_CreateUIHelp.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.SceneChangeHelper.<SceneChangeTo>d__0>(object&,ET.Client.SceneChangeHelper.<SceneChangeTo>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.SceneChangeStart_AddComponent.<Run>d__0>(object&,ET.Client.SceneChangeStart_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.SettleStatViewComponentSystem.<OnEventBackLastAction>d__6>(object&,ET.Client.SettleStatViewComponentSystem.<OnEventBackLastAction>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.TextTipsViewComponentSystem.<PlayAnimation>d__6>(object&,ET.Client.TextTipsViewComponentSystem.<PlayAnimation>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.TipsHelper.<CloseTipsView>d__7>(object&,ET.Client.TipsHelper.<CloseTipsView>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.TipsHelper.<Open>d__0<object>>(object&,ET.Client.TipsHelper.<Open>d__0<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.TipsHelper.<OpenToParent2NewVo>d__6<object>>(object&,ET.Client.TipsHelper.<OpenToParent2NewVo>d__6<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.TipsHelper.<OpenToParent>d__2<object>>(object&,ET.Client.TipsHelper.<OpenToParent>d__2<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.TipsHelper.<OpenToParent>d__4<object>>(object&,ET.Client.TipsHelper.<OpenToParent>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.TipsPanelComponentSystem.<YIUIEvent>d__6>(object&,ET.Client.TipsPanelComponentSystem.<YIUIEvent>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UploadFileAddressComponentSystem.<UploadFile>d__1>(object&,ET.Client.UploadFileAddressComponentSystem.<UploadFile>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.YIUIInvokeTimerComponentWaitAsyncHandler.<Handle>d__0>(object&,ET.Client.YIUIInvokeTimerComponentWaitAsyncHandler.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.YIUIInvokeTimerComponentWaitFrameAsyncHandler.<Handle>d__0>(object&,ET.Client.YIUIInvokeTimerComponentWaitFrameAsyncHandler.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.ConsoleComponentSystem.<Start>d__1>(object&,ET.ConsoleComponentSystem.<Start>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Entry.<StartAsync>d__2>(object&,ET.Entry.<StartAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.FiberInit_Main.<Handle>d__0>(object&,ET.FiberInit_Main.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1>(object&,ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1>(object&,ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageHandler.<Handle>d__1<object,object,object>>(object&,ET.MessageHandler.<Handle>d__1<object,object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageHandler.<Handle>d__1<object,object>>(object&,ET.MessageHandler.<Handle>d__1<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageSessionHandler.<HandleAsync>d__2<object,object>>(object&,ET.MessageSessionHandler.<HandleAsync>d__2<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageSessionHandler.<HandleAsync>d__2<object>>(object&,ET.MessageSessionHandler.<HandleAsync>d__2<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.ObjectWaitSystem.<>c__DisplayClass5_0.<<Wait>g__WaitTimeout|0>d<object>>(object&,ET.ObjectWaitSystem.<>c__DisplayClass5_0.<<Wait>g__WaitTimeout|0>d<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.ReloadConfigConsoleHandler.<Run>d__0>(object&,ET.ReloadConfigConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.SessionSystem.<>c__DisplayClass4_0.<<Call>g__Timeout|0>d>(object&,ET.SessionSystem.<>c__DisplayClass4_0.<<Call>g__Timeout|0>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.EntityRef<object>>.AwaitUnsafeOnCompleted<object,ET.Client.TipsPanelComponentSystem.<>c__DisplayClass8_0.<<OpenTips>g__Create|0>d>(object&,ET.Client.TipsPanelComponentSystem.<>c__DisplayClass8_0.<<OpenTips>g__Create|0>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.AwaitUnsafeOnCompleted<object,ET.Client.RouterHelper.<GetRouterAddress>d__1>(object&,ET.Client.RouterHelper.<GetRouterAddress>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.CommonPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.CommonPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GMPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.GMPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GMToolsBatchViewComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.GMToolsBatchViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GMToolsPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.GMToolsPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GMToolsStageViewComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.GMToolsStageViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GMViewComponentSystem.<YIUIOpen>d__6>(ET.ETTaskCompleted&,ET.Client.GMViewComponentSystem.<YIUIOpen>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GM_Performance.<Run>d__1>(ET.ETTaskCompleted&,ET.Client.GM_Performance.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GM_Test.<Run>d__1>(ET.ETTaskCompleted&,ET.Client.GM_Test.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GM_TipsTest1.<Run>d__1>(ET.ETTaskCompleted&,ET.Client.GM_TipsTest1.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GM_TipsTest2.<Run>d__1>(ET.ETTaskCompleted&,ET.Client.GM_TipsTest2.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GM_TipsTest3.<Run>d__1>(ET.ETTaskCompleted&,ET.Client.GM_TipsTest3.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GM_TipsTest4.<Run>d__1>(ET.ETTaskCompleted&,ET.Client.GM_TipsTest4.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.GM_TipsTest5.<Run>d__1>(ET.ETTaskCompleted&,ET.Client.GM_TipsTest5.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LSLobbyPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.LSLobbyPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LobbyPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.LobbyPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.LoginPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.LoginPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.MainPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.MainPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__8>(ET.ETTaskCompleted&,ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.PlayViewComponentSystem.<YIUIOpen>d__6>(ET.ETTaskCompleted&,ET.Client.PlayViewComponentSystem.<YIUIOpen>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.RedDotPanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.RedDotPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.ReplayViewComponentSystem.<YIUIOpen>d__6>(ET.ETTaskCompleted&,ET.Client.ReplayViewComponentSystem.<YIUIOpen>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.SettlePanelComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.SettlePanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.SettleStatViewComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.SettleStatViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,ET.Client.TextTipsViewComponentSystem.<YIUIOpen>d__5>(ET.ETTaskCompleted&,ET.Client.TextTipsViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.GM_OpenReddotPanel.<Run>d__1>(object&,ET.Client.GM_OpenReddotPanel.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.GM_UploadFile.<Run>d__1>(object&,ET.Client.GM_UploadFile.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.LSRoomPanelComponentSystem.<YIUIOpen>d__5>(object&,ET.Client.LSRoomPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.TipsPanelComponentSystem.<OpenTips>d__8>(object&,ET.Client.TipsPanelComponentSystem.<OpenTips>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.TipsPanelComponentSystem.<PutTips>d__9>(object&,ET.Client.TipsPanelComponentSystem.<PutTips>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.TipsPanelComponentSystem.<YIUIOpen>d__5>(object&,ET.Client.TipsPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.MoveComponentSystem.<MoveToAsync>d__5>(object&,ET.MoveComponentSystem.<MoveToAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.MoveHelper.<MoveToAsync>d__0>(object&,ET.Client.MoveHelper.<MoveToAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<long>.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4>(object&,ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__5<object>>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__5<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__4<object>>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ET.Client.HttpClientHelper.<Get>d__0>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ET.Client.HttpClientHelper.<Get>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ET.Client.UploadFileAddressComponentSystem.<UploadFileAsync>d__2>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ET.Client.UploadFileAddressComponentSystem.<UploadFileAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<Call>d__6>(object&,ET.Client.ClientSenderComponentSystem.<Call>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.PrefabPoolComponentSystem.<FetchAsync>d__5>(object&,ET.Client.PrefabPoolComponentSystem.<FetchAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__5<object>>(object&,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__5<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__4<object>>(object&,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.RouterHelper.<CreateRouterSession>d__0>(object&,ET.Client.RouterHelper.<CreateRouterSession>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.YIUIInvokeCoroutineLockHandler.<Handle>d__0>(object&,ET.Client.YIUIInvokeCoroutineLockHandler.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.ObjectWaitSystem.<Wait>d__4<object>>(object&,ET.ObjectWaitSystem.<Wait>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.ObjectWaitSystem.<Wait>d__5<object>>(object&,ET.ObjectWaitSystem.<Wait>d__5<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.RpcInfo.<Wait>d__7>(object&,ET.RpcInfo.<Wait>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.SessionSystem.<Call>d__3>(object&,ET.SessionSystem.<Call>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.SessionSystem.<Call>d__4>(object&,ET.SessionSystem.<Call>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.AwaitUnsafeOnCompleted<object,ET.Client.RouterHelper.<Connect>d__2>(object&,ET.Client.RouterHelper.<Connect>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.A2NetClient_MessageHandler.<Run>d__0>(ET.Client.A2NetClient_MessageHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.A2NetClient_RequestHandler.<Run>d__0>(ET.Client.A2NetClient_RequestHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AI_Attack.<Execute>d__1>(ET.Client.AI_Attack.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AI_XunLuo.<Execute>d__1>(ET.Client.AI_XunLuo.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AfterCreateClientScene_AddComponent.<Run>d__0>(ET.Client.AfterCreateClientScene_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AfterCreateClientScene_LSAddComponent.<Run>d__0>(ET.Client.AfterCreateClientScene_LSAddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0>(ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0>(ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0>(ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0>(ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0>(ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ClientSenderComponentSystem.<DisposeAsync>d__3>(ET.Client.ClientSenderComponentSystem.<DisposeAsync>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2>(ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EffectViewComponentSystem.<PlayFx>d__3>(ET.Client.EffectViewComponentSystem.<PlayFx>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EffectViewComponentSystem.<PlayFx>d__4>(ET.Client.EffectViewComponentSystem.<PlayFx>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EnterMapHelper.<EnterMapAsync>d__0>(ET.Client.EnterMapHelper.<EnterMapAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EnterMapHelper.<Match>d__1>(ET.Client.EnterMapHelper.<Match>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EntryEvent3_InitClient.<Run>d__0>(ET.Client.EntryEvent3_InitClient.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.FiberInit_NetClient.<Handle>d__0>(ET.Client.FiberInit_NetClient.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.G2C_ReconnectHandler.<Run>d__0>(ET.Client.G2C_ReconnectHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.GMCommandComponentSystem.<Run>d__5>(ET.Client.GMCommandComponentSystem.<Run>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.GMCommandItemComponentSystem.<WaitRefresh>d__6>(ET.Client.GMCommandItemComponentSystem.<WaitRefresh>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.GMViewComponentSystem.<YIUIEvent>d__5>(ET.Client.GMViewComponentSystem.<YIUIEvent>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSCardSelectPanelComponentSystem.<YIUICloseTween>d__7>(ET.Client.LSCardSelectPanelComponentSystem.<YIUICloseTween>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__10>(ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__10&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__8>(ET.Client.LSCardSelectPanelComponentSystem.<YIUIEvent>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpenTween>d__6>(ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpenTween>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSEscapeEvent.<Run>d__0>(ET.Client.LSEscapeEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSGridDataResetEvent.<Run>d__0>(ET.Client.LSGridDataResetEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSOperaDragComponentSystem.<ScanTouchLongPress>d__4>(ET.Client.LSOperaDragComponentSystem.<ScanTouchLongPress>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSPlacementDragEvent.<Run>d__0>(ET.Client.LSPlacementDragEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSPlacementNewEvent.<Run>d__0>(ET.Client.LSPlacementNewEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSPlacementRotateEvent.<Run>d__0>(ET.Client.LSPlacementRotateEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSSceneChangeHelper.<SceneChangeTo>d__0>(ET.Client.LSSceneChangeHelper.<SceneChangeTo>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSSceneChangeHelper.<SceneChangeToReconnect>d__2>(ET.Client.LSSceneChangeHelper.<SceneChangeToReconnect>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSSceneChangeHelper.<SceneChangeToReplay>d__1>(ET.Client.LSSceneChangeHelper.<SceneChangeToReplay>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSSceneChangeStart_AddComponent.<Run>d__0>(ET.Client.LSSceneChangeStart_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSSceneInitFinish_Finish.<Run>d__0>(ET.Client.LSSceneInitFinish_Finish.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSSelectionChangedEvent.<Run>d__0>(ET.Client.LSSelectionChangedEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSStageGameOverEvent.<Run>d__0>(ET.Client.LSStageGameOverEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSTouchDragCancelEvent.<Run>d__0>(ET.Client.LSTouchDragCancelEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSTouchDragEndEvent.<Run>d__0>(ET.Client.LSTouchDragEndEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSTouchDragEvent.<Run>d__0>(ET.Client.LSTouchDragEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSTouchDragStartEvent.<Run>d__0>(ET.Client.LSTouchDragStartEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitCardBagAddEvent.<Run>d__0>(ET.Client.LSUnitCardBagAddEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitCardBagRemoveEvent.<Run>d__0>(ET.Client.LSUnitCardBagRemoveEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitCardSelectAddEvent.<Run>d__0>(ET.Client.LSUnitCardSelectAddEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitCardSelectDoneEvent.<Run>d__0>(ET.Client.LSUnitCardSelectDoneEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitCastingEvent.<Run>d__0>(ET.Client.LSUnitCastingEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitCreateEvent.<Run>d__0>(ET.Client.LSUnitCreateEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitFloatingEvent.<Run>d__0>(ET.Client.LSUnitFloatingEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitFxEvent.<Run>d__0>(ET.Client.LSUnitFxEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitMovingEvent.<Run>d__0>(ET.Client.LSUnitMovingEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitPlacedEvent.<Run>d__0>(ET.Client.LSUnitPlacedEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitPropEvent.<Run>d__0>(ET.Client.LSUnitPropEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LSUnitRemoveEvent.<Run>d__0>(ET.Client.LSUnitRemoveEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LobbyPanelComponentSystem.<OnEventEnterAction>d__6>(ET.Client.LobbyPanelComponentSystem.<OnEventEnterAction>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LoginFinish_CreateLobbyUI.<Run>d__0>(ET.Client.LoginFinish_CreateLobbyUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LoginFinish_CreateUILSLobby.<Run>d__0>(ET.Client.LoginFinish_CreateUILSLobby.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LoginFinish_RemoveLoginUI.<Run>d__0>(ET.Client.LoginFinish_RemoveLoginUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LoginHelper.<Login>d__0>(ET.Client.LoginHelper.<Login>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LoginPanelComponentSystem.<OnEventLoginAction>d__8>(ET.Client.LoginPanelComponentSystem.<OnEventLoginAction>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_CreateMyUnitHandler.<Run>d__0>(ET.Client.M2C_CreateMyUnitHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_CreateUnitsHandler.<Run>d__0>(ET.Client.M2C_CreateUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_PathfindingResultHandler.<Run>d__0>(ET.Client.M2C_PathfindingResultHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_RemoveUnitsHandler.<Run>d__0>(ET.Client.M2C_RemoveUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_StartSceneChangeHandler.<Run>d__0>(ET.Client.M2C_StartSceneChangeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_StopHandler.<Run>d__0>(ET.Client.M2C_StopHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Main2NetClient_LoginHandler.<Run>d__0>(ET.Client.Main2NetClient_LoginHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Match2G_MatchSuccessHandler.<Run>d__0>(ET.Client.Match2G_MatchSuccessHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MessageTipsViewComponentSystem.<YIUICloseTween>d__7>(ET.Client.MessageTipsViewComponentSystem.<YIUICloseTween>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MessageTipsViewComponentSystem.<YIUIOpenTween>d__6>(ET.Client.MessageTipsViewComponentSystem.<YIUIOpenTween>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MoveHelper.<MoveToAsync>d__1>(ET.Client.MoveHelper.<MoveToAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0>(ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.OperaComponentSystem.<Test1>d__2>(ET.Client.OperaComponentSystem.<Test1>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.OperaComponentSystem.<Test2>d__3>(ET.Client.OperaComponentSystem.<Test2>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PingComponentSystem.<PingAsync>d__2>(ET.Client.PingComponentSystem.<PingAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__5>(ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__6>(ET.Client.PlayCardItemComponentSystem.<YIUIEvent>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__10>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__10&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__11>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__11&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__12>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__12&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__13>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__13&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__14>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__14&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__15>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__15&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__16>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__16&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__7>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__8>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayViewComponentSystem.<YIUIEvent>d__9>(ET.Client.PlayViewComponentSystem.<YIUIEvent>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PrefabPoolComponentSystem.<PreloadAsync>d__9>(ET.Client.PrefabPoolComponentSystem.<PreloadAsync>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__6>(ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__7>(ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__8>(ET.Client.RedDotPanelComponentSystem.<YIUIEvent>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__6>(ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Room2C_CheckHashFailHandler.<Run>d__0>(ET.Client.Room2C_CheckHashFailHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Room2C_CheckHashFailHandler.<SaveToFile>d__3>(ET.Client.Room2C_CheckHashFailHandler.<SaveToFile>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Room2C_EnterHandler.<Run>d__0>(ET.Client.Room2C_EnterHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Room2C_FrameMessageHandler.<Run>d__0>(ET.Client.Room2C_FrameMessageHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Room2C_StartHandler.<Run>d__0>(ET.Client.Room2C_StartHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Room2C_TimeAdjustHandler.<Run>d__0>(ET.Client.Room2C_TimeAdjustHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2>(ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterAddressComponentSystem.<Init>d__1>(ET.Client.RouterAddressComponentSystem.<Init>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3>(ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1>(ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.SceneChangeFinishEvent_CreateUIHelp.<Run>d__0>(ET.Client.SceneChangeFinishEvent_CreateUIHelp.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.SceneChangeHelper.<SceneChangeTo>d__0>(ET.Client.SceneChangeHelper.<SceneChangeTo>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.SceneChangeStart_AddComponent.<Run>d__0>(ET.Client.SceneChangeStart_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.SettleStatViewComponentSystem.<OnEventBackLastAction>d__6>(ET.Client.SettleStatViewComponentSystem.<OnEventBackLastAction>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.TextTipsViewComponentSystem.<PlayAnimation>d__6>(ET.Client.TextTipsViewComponentSystem.<PlayAnimation>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.TipsHelper.<CloseTipsView>d__7>(ET.Client.TipsHelper.<CloseTipsView>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.TipsHelper.<Open>d__0<object>>(ET.Client.TipsHelper.<Open>d__0<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.TipsHelper.<OpenToParent2NewVo>d__6<object>>(ET.Client.TipsHelper.<OpenToParent2NewVo>d__6<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.TipsHelper.<OpenToParent>d__2<object>>(ET.Client.TipsHelper.<OpenToParent>d__2<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.TipsHelper.<OpenToParent>d__4<object>>(ET.Client.TipsHelper.<OpenToParent>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.TipsPanelComponentSystem.<YIUIEvent>d__6>(ET.Client.TipsPanelComponentSystem.<YIUIEvent>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UploadFileAddressComponentSystem.<UploadFile>d__1>(ET.Client.UploadFileAddressComponentSystem.<UploadFile>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.EventPutTipsView>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.EventPutTipsView>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardItemHighlightEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardItemHighlightEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardSelectClickEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardSelectClickEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardSelectResetEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardSelectResetEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardViewResetEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnCardViewResetEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnClickChildListEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnClickChildListEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnClickItemEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnClickItemEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnClickParentListEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnClickParentListEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnGMEventClose>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.OnGMEventClose>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UIArrowDragEndEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UIArrowDragEndEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UIArrowDragEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UIArrowDragEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UIArrowDragStartEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UIArrowDragStartEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UICardDragEndEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UICardDragEndEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UICardDragStartEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UICardDragStartEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UISelectDragEndEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UISelectDragEndEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UISelectDragEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UISelectDragEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UISelectDragStartEvent>>(ET.Client.YIUIEventSystem.<UIEvent>d__20<ET.Client.UISelectDragStartEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIInvokeTimerComponentWaitAsyncHandler.<Handle>d__0>(ET.Client.YIUIInvokeTimerComponentWaitAsyncHandler.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIInvokeTimerComponentWaitFrameAsyncHandler.<Handle>d__0>(ET.Client.YIUIInvokeTimerComponentWaitFrameAsyncHandler.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIMgrComponentSystem.<HomePanel>d__19<object>>(ET.Client.YIUIMgrComponentSystem.<HomePanel>d__19<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.ConsoleComponentSystem.<Start>d__1>(ET.ConsoleComponentSystem.<Start>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Entry.<StartAsync>d__2>(ET.Entry.<StartAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EntryEvent1_InitShare.<Run>d__0>(ET.EntryEvent1_InitShare.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.Client.AppStartInitFinish>>(ET.EventSystem.<PublishAsync>d__4<object,ET.Client.AppStartInitFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.Client.LSSceneChangeStart>>(ET.EventSystem.<PublishAsync>d__4<object,ET.Client.LSSceneChangeStart>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.Client.LoginFinish>>(ET.EventSystem.<PublishAsync>d__4<object,ET.Client.LoginFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent1>>(ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent1>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent2>>(ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent2>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent3>>(ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent3>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.FiberInit_Main.<Handle>d__0>(ET.FiberInit_Main.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1>(ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1>(ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageHandler.<Handle>d__1<object,object,object>>(ET.MessageHandler.<Handle>d__1<object,object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageHandler.<Handle>d__1<object,object>>(ET.MessageHandler.<Handle>d__1<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageSessionHandler.<HandleAsync>d__2<object,object>>(ET.MessageSessionHandler.<HandleAsync>d__2<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageSessionHandler.<HandleAsync>d__2<object>>(ET.MessageSessionHandler.<HandleAsync>d__2<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.NumericChangeEvent_NotifyWatcher.<Run>d__0>(ET.NumericChangeEvent_NotifyWatcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.ObjectWaitSystem.<>c__DisplayClass5_0.<<Wait>g__WaitTimeout|0>d<object>>(ET.ObjectWaitSystem.<>c__DisplayClass5_0.<<Wait>g__WaitTimeout|0>d<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.ReloadConfigConsoleHandler.<Run>d__0>(ET.ReloadConfigConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.ReloadDllConsoleHandler.<Run>d__0>(ET.ReloadDllConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.SessionSystem.<>c__DisplayClass4_0.<<Call>g__Timeout|0>d>(ET.SessionSystem.<>c__DisplayClass4_0.<<Call>g__Timeout|0>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.WaitType.Wait_Room2C_Start>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.WaitType.Wait_Room2C_Start>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.WaitType.Wait_Room2C_Start>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_CreateMyUnit>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_CreateMyUnit>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_SceneChangeFinish>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_SceneChangeFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_UnitStop>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_UnitStop>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.EntityRef<object>>.Start<ET.Client.TipsPanelComponentSystem.<>c__DisplayClass8_0.<<OpenTips>g__Create|0>d>(ET.Client.TipsPanelComponentSystem.<>c__DisplayClass8_0.<<OpenTips>g__Create|0>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.Start<ET.Client.RouterHelper.<GetRouterAddress>d__1>(ET.Client.RouterHelper.<GetRouterAddress>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.CommonPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.CommonPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GMPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.GMPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GMToolsBatchViewComponentSystem.<YIUIOpen>d__5>(ET.Client.GMToolsBatchViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GMToolsPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.GMToolsPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GMToolsStageViewComponentSystem.<YIUIOpen>d__5>(ET.Client.GMToolsStageViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GMViewComponentSystem.<YIUIOpen>d__6>(ET.Client.GMViewComponentSystem.<YIUIOpen>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_OpenReddotPanel.<Run>d__1>(ET.Client.GM_OpenReddotPanel.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_Performance.<Run>d__1>(ET.Client.GM_Performance.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_Test.<Run>d__1>(ET.Client.GM_Test.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_TipsTest1.<Run>d__1>(ET.Client.GM_TipsTest1.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_TipsTest2.<Run>d__1>(ET.Client.GM_TipsTest2.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_TipsTest3.<Run>d__1>(ET.Client.GM_TipsTest3.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_TipsTest4.<Run>d__1>(ET.Client.GM_TipsTest4.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_TipsTest5.<Run>d__1>(ET.Client.GM_TipsTest5.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.GM_UploadFile.<Run>d__1>(ET.Client.GM_UploadFile.<Run>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.LSCardSelectPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.LSLobbyPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.LSLobbyPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.LSRoomPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.LSRoomPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.LobbyPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.LobbyPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.LoginPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.LoginPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.MainPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.MainPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__5>(ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__8>(ET.Client.MessageTipsViewComponentSystem.<YIUIOpen>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.PlayViewComponentSystem.<YIUIOpen>d__6>(ET.Client.PlayViewComponentSystem.<YIUIOpen>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.RedDotPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.RedDotPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.ReplayViewComponentSystem.<YIUIOpen>d__6>(ET.Client.ReplayViewComponentSystem.<YIUIOpen>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.SettlePanelComponentSystem.<YIUIOpen>d__5>(ET.Client.SettlePanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.SettleStatViewComponentSystem.<YIUIOpen>d__5>(ET.Client.SettleStatViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.TextTipsViewComponentSystem.<YIUIOpen>d__5>(ET.Client.TextTipsViewComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.TipsPanelComponentSystem.<OpenTips>d__8>(ET.Client.TipsPanelComponentSystem.<OpenTips>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.TipsPanelComponentSystem.<PutTips>d__9>(ET.Client.TipsPanelComponentSystem.<PutTips>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.TipsPanelComponentSystem.<YIUIOpen>d__5>(ET.Client.TipsPanelComponentSystem.<YIUIOpen>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.YIUIMgrComponentSystem.<ClosePanelAsync>d__16<object>>(ET.Client.YIUIMgrComponentSystem.<ClosePanelAsync>d__16<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.MoveComponentSystem.<MoveToAsync>d__5>(ET.MoveComponentSystem.<MoveToAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.MoveHelper.<MoveToAsync>d__0>(ET.Client.MoveHelper.<MoveToAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<long>.Start<ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4>(ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.ClientSenderComponentSystem.<Call>d__6>(ET.Client.ClientSenderComponentSystem.<Call>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.HttpClientHelper.<Get>d__0>(ET.Client.HttpClientHelper.<Get>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.PrefabPoolComponentSystem.<FetchAsync>d__5>(ET.Client.PrefabPoolComponentSystem.<FetchAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__5<object>>(ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__5<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__4<object>>(ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.RouterHelper.<CreateRouterSession>d__0>(ET.Client.RouterHelper.<CreateRouterSession>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.UploadFileAddressComponentSystem.<UploadFileAsync>d__2>(ET.Client.UploadFileAddressComponentSystem.<UploadFileAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIInvokeCoroutineLockHandler.<Handle>d__0>(ET.Client.YIUIInvokeCoroutineLockHandler.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIPanelComponentSystem.<OpenViewAsync>d__11<object>>(ET.Client.YIUIPanelComponentSystem.<OpenViewAsync>d__11<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__1<object>>(ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__1<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__3<object,int>>(ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__3<object,int>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__5<object,object,object,object>>(ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__5<object,object,object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.ObjectWaitSystem.<Wait>d__4<object>>(ET.ObjectWaitSystem.<Wait>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.ObjectWaitSystem.<Wait>d__5<object>>(ET.ObjectWaitSystem.<Wait>d__5<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.RpcInfo.<Wait>d__7>(ET.RpcInfo.<Wait>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.SessionSystem.<Call>d__3>(ET.SessionSystem.<Call>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.SessionSystem.<Call>d__4>(ET.SessionSystem.<Call>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.Start<ET.Client.RouterHelper.<Connect>d__2>(ET.Client.RouterHelper.<Connect>d__2&)
		// object ET.Entity.AddChild<object,object,object,object>(object,object,object,bool)
		// object ET.Entity.AddChildWithId<object,int,byte>(long,int,byte,bool)
		// object ET.Entity.AddChildWithId<object,int,object>(long,int,object,bool)
		// object ET.Entity.AddChildWithId<object,int>(long,int,bool)
		// object ET.Entity.AddChildWithId<object,object,object,object>(long,object,object,object,bool)
		// object ET.Entity.AddChildWithId<object,object,object>(long,object,object,bool)
		// object ET.Entity.AddChildWithId<object,object>(long,object,bool)
		// object ET.Entity.AddChildWithId<object>(long,bool)
		// object ET.Entity.AddComponent<object,UnityEngine.Vector3,float,float>(UnityEngine.Vector3,float,float,bool)
		// object ET.Entity.AddComponent<object,byte>(byte,bool)
		// object ET.Entity.AddComponent<object,int,int>(int,int,bool)
		// object ET.Entity.AddComponent<object,int>(int,bool)
		// object ET.Entity.AddComponent<object,long>(long,bool)
		// object ET.Entity.AddComponent<object,object,int>(object,int,bool)
		// object ET.Entity.AddComponent<object,object,object,byte>(object,object,byte,bool)
		// object ET.Entity.AddComponent<object,object>(object,bool)
		// object ET.Entity.AddComponent<object>(bool)
		// object ET.Entity.AddComponentWithId<object,TrueSync.TSVector,TrueSync.TSQuaternion>(long,TrueSync.TSVector,TrueSync.TSQuaternion,bool)
		// object ET.Entity.AddComponentWithId<object,UnityEngine.Vector3,float,float>(long,UnityEngine.Vector3,float,float,bool)
		// object ET.Entity.AddComponentWithId<object,byte>(long,byte,bool)
		// object ET.Entity.AddComponentWithId<object,int,int>(long,int,int,bool)
		// object ET.Entity.AddComponentWithId<object,int,object,TrueSync.TSVector>(long,int,object,TrueSync.TSVector,bool)
		// object ET.Entity.AddComponentWithId<object,int,object,object>(long,int,object,object,bool)
		// object ET.Entity.AddComponentWithId<object,int>(long,int,bool)
		// object ET.Entity.AddComponentWithId<object,long>(long,long,bool)
		// object ET.Entity.AddComponentWithId<object,object,int>(long,object,int,bool)
		// object ET.Entity.AddComponentWithId<object,object,object,byte>(long,object,object,byte,bool)
		// object ET.Entity.AddComponentWithId<object,object,object,object>(long,object,object,object,bool)
		// object ET.Entity.AddComponentWithId<object,object,object>(long,object,object,bool)
		// object ET.Entity.AddComponentWithId<object,object>(long,object,bool)
		// object ET.Entity.AddComponentWithId<object>(long,bool)
		// object ET.Entity.GetChild<object>(long)
		// object ET.Entity.GetComponent<object>()
		// object ET.Entity.GetParent<object>()
		// System.Void ET.Entity.RemoveComponent<object>()
		// System.Void ET.EntitySystemSingleton.Awake<TrueSync.TSVector,TrueSync.TSQuaternion>(ET.Entity,TrueSync.TSVector,TrueSync.TSQuaternion)
		// System.Void ET.EntitySystemSingleton.Awake<UnityEngine.Vector3,float,float>(ET.Entity,UnityEngine.Vector3,float,float)
		// System.Void ET.EntitySystemSingleton.Awake<byte>(ET.Entity,byte)
		// System.Void ET.EntitySystemSingleton.Awake<int,byte>(ET.Entity,int,byte)
		// System.Void ET.EntitySystemSingleton.Awake<int,int>(ET.Entity,int,int)
		// System.Void ET.EntitySystemSingleton.Awake<int,object,TrueSync.TSVector>(ET.Entity,int,object,TrueSync.TSVector)
		// System.Void ET.EntitySystemSingleton.Awake<int,object,object>(ET.Entity,int,object,object)
		// System.Void ET.EntitySystemSingleton.Awake<int,object>(ET.Entity,int,object)
		// System.Void ET.EntitySystemSingleton.Awake<int>(ET.Entity,int)
		// System.Void ET.EntitySystemSingleton.Awake<long>(ET.Entity,long)
		// System.Void ET.EntitySystemSingleton.Awake<object,int>(ET.Entity,object,int)
		// System.Void ET.EntitySystemSingleton.Awake<object,object,byte>(ET.Entity,object,object,byte)
		// System.Void ET.EntitySystemSingleton.Awake<object,object,object>(ET.Entity,object,object,object)
		// System.Void ET.EntitySystemSingleton.Awake<object,object>(ET.Entity,object,object)
		// System.Void ET.EntitySystemSingleton.Awake<object>(ET.Entity,object)
		// long ET.EnumHelper.FromString<long>(string)
		// System.Void ET.EventSystem.Invoke<ET.NetComponentOnRead>(long,ET.NetComponentOnRead)
		// System.Void ET.EventSystem.Publish<object,ET.ChangePosition>(object,ET.ChangePosition)
		// System.Void ET.EventSystem.Publish<object,ET.ChangeRotation>(object,ET.ChangeRotation)
		// System.Void ET.EventSystem.Publish<object,ET.Client.AfterCreateCurrentScene>(object,ET.Client.AfterCreateCurrentScene)
		// System.Void ET.EventSystem.Publish<object,ET.Client.AfterUnitCreate>(object,ET.Client.AfterUnitCreate)
		// System.Void ET.EventSystem.Publish<object,ET.Client.EnterMapFinish>(object,ET.Client.EnterMapFinish)
		// System.Void ET.EventSystem.Publish<object,ET.Client.LSSceneInitFinish>(object,ET.Client.LSSceneInitFinish)
		// System.Void ET.EventSystem.Publish<object,ET.Client.SceneChangeFinish>(object,ET.Client.SceneChangeFinish)
		// System.Void ET.EventSystem.Publish<object,ET.Client.SceneChangeStart>(object,ET.Client.SceneChangeStart)
		// System.Void ET.EventSystem.Publish<object,ET.LSCardBagAdd>(object,ET.LSCardBagAdd)
		// System.Void ET.EventSystem.Publish<object,ET.LSCardBagRemove>(object,ET.LSCardBagRemove)
		// System.Void ET.EventSystem.Publish<object,ET.LSCardSelectAdd>(object,ET.LSCardSelectAdd)
		// System.Void ET.EventSystem.Publish<object,ET.LSCardSelectDone>(object,ET.LSCardSelectDone)
		// System.Void ET.EventSystem.Publish<object,ET.LSEscape>(object,ET.LSEscape)
		// System.Void ET.EventSystem.Publish<object,ET.LSGridDataReset>(object,ET.LSGridDataReset)
		// System.Void ET.EventSystem.Publish<object,ET.LSPlacementDrag>(object,ET.LSPlacementDrag)
		// System.Void ET.EventSystem.Publish<object,ET.LSPlacementNew>(object,ET.LSPlacementNew)
		// System.Void ET.EventSystem.Publish<object,ET.LSPlacementRotate>(object,ET.LSPlacementRotate)
		// System.Void ET.EventSystem.Publish<object,ET.LSSelectionChanged>(object,ET.LSSelectionChanged)
		// System.Void ET.EventSystem.Publish<object,ET.LSStageGameOver>(object,ET.LSStageGameOver)
		// System.Void ET.EventSystem.Publish<object,ET.LSTouchDrag>(object,ET.LSTouchDrag)
		// System.Void ET.EventSystem.Publish<object,ET.LSTouchDragCancel>(object,ET.LSTouchDragCancel)
		// System.Void ET.EventSystem.Publish<object,ET.LSTouchDragEnd>(object,ET.LSTouchDragEnd)
		// System.Void ET.EventSystem.Publish<object,ET.LSTouchDragStart>(object,ET.LSTouchDragStart)
		// System.Void ET.EventSystem.Publish<object,ET.LSUnitCasting>(object,ET.LSUnitCasting)
		// System.Void ET.EventSystem.Publish<object,ET.LSUnitCreate>(object,ET.LSUnitCreate)
		// System.Void ET.EventSystem.Publish<object,ET.LSUnitFloating>(object,ET.LSUnitFloating)
		// System.Void ET.EventSystem.Publish<object,ET.LSUnitFx>(object,ET.LSUnitFx)
		// System.Void ET.EventSystem.Publish<object,ET.LSUnitMoving>(object,ET.LSUnitMoving)
		// System.Void ET.EventSystem.Publish<object,ET.LSUnitPlaced>(object,ET.LSUnitPlaced)
		// System.Void ET.EventSystem.Publish<object,ET.LSUnitRemove>(object,ET.LSUnitRemove)
		// System.Void ET.EventSystem.Publish<object,ET.MoveStart>(object,ET.MoveStart)
		// System.Void ET.EventSystem.Publish<object,ET.MoveStop>(object,ET.MoveStop)
		// System.Void ET.EventSystem.Publish<object,ET.NumbericChange>(object,ET.NumbericChange)
		// System.Void ET.EventSystem.Publish<object,ET.PropChange>(object,ET.PropChange)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.Client.AppStartInitFinish>(object,ET.Client.AppStartInitFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.Client.LSSceneChangeStart>(object,ET.Client.LSSceneChangeStart)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.Client.LoginFinish>(object,ET.Client.LoginFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.EntryEvent1>(object,ET.EntryEvent1)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.EntryEvent2>(object,ET.EntryEvent2)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.EntryEvent3>(object,ET.EntryEvent3)
		// object ET.MongoHelper.FromJson<object>(string)
		// object ET.ObjectPool.Fetch<object>()
		// System.Void ET.RandomGenerator.BreakRank<object>(System.Collections.Generic.List<object>)
		// object ET.World.AddSingleton<object>()
		// string Luban.StringUtil.CollectionToString<byte>(System.Collections.Generic.IEnumerable<byte>)
		// string Luban.StringUtil.CollectionToString<int,int>(System.Collections.Generic.IDictionary<int,int>)
		// string Luban.StringUtil.CollectionToString<int>(System.Collections.Generic.IEnumerable<int>)
		// string Luban.StringUtil.CollectionToString<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.List<ET.SearchUnitPackable> MemoryPack.Formatters.ListFormatter.DeserializePackable<ET.SearchUnitPackable>(MemoryPack.MemoryPackReader&)
		// System.Collections.Generic.List<object> MemoryPack.Formatters.ListFormatter.DeserializePackable<object>(MemoryPack.MemoryPackReader&)
		// System.Void MemoryPack.Formatters.ListFormatter.DeserializePackable<ET.SearchUnitPackable>(MemoryPack.MemoryPackReader&,System.Collections.Generic.List<ET.SearchUnitPackable>&)
		// System.Void MemoryPack.Formatters.ListFormatter.DeserializePackable<object>(MemoryPack.MemoryPackReader&,System.Collections.Generic.List<object>&)
		// System.Void MemoryPack.Formatters.ListFormatter.SerializePackable<ET.SearchUnitPackable>(MemoryPack.MemoryPackWriter&,System.Collections.Generic.List<ET.SearchUnitPackable>&)
		// System.Void MemoryPack.Formatters.ListFormatter.SerializePackable<object>(MemoryPack.MemoryPackWriter&,System.Collections.Generic.List<object>&)
		// byte[] MemoryPack.Internal.MemoryMarshalEx.AllocateUninitializedArray<byte>(int,bool)
		// byte& MemoryPack.Internal.MemoryMarshalEx.GetArrayDataReference<byte>(byte[])
		// MemoryPack.MemoryPackFormatter<ET.SearchUnitPackable> MemoryPack.MemoryPackFormatterProvider.GetFormatter<ET.SearchUnitPackable>()
		// MemoryPack.MemoryPackFormatter<byte> MemoryPack.MemoryPackFormatterProvider.GetFormatter<byte>()
		// MemoryPack.MemoryPackFormatter<long> MemoryPack.MemoryPackFormatterProvider.GetFormatter<long>()
		// MemoryPack.MemoryPackFormatter<object> MemoryPack.MemoryPackFormatterProvider.GetFormatter<object>()
		// bool MemoryPack.MemoryPackFormatterProvider.IsRegistered<ET.SearchUnitPackable>()
		// bool MemoryPack.MemoryPackFormatterProvider.IsRegistered<int>()
		// bool MemoryPack.MemoryPackFormatterProvider.IsRegistered<object>()
		// System.Void MemoryPack.MemoryPackFormatterProvider.Register<ET.SearchUnitPackable>(MemoryPack.MemoryPackFormatter<ET.SearchUnitPackable>)
		// System.Void MemoryPack.MemoryPackFormatterProvider.Register<int>(MemoryPack.MemoryPackFormatter<int>)
		// System.Void MemoryPack.MemoryPackFormatterProvider.Register<object>(MemoryPack.MemoryPackFormatter<object>)
		// System.Void MemoryPack.MemoryPackReader.DangerousReadUnmanagedArray<byte>(byte[]&)
		// byte[] MemoryPack.MemoryPackReader.DangerousReadUnmanagedArray<byte>()
		// MemoryPack.IMemoryPackFormatter<ET.SearchUnitPackable> MemoryPack.MemoryPackReader.GetFormatter<ET.SearchUnitPackable>()
		// MemoryPack.IMemoryPackFormatter<byte> MemoryPack.MemoryPackReader.GetFormatter<byte>()
		// MemoryPack.IMemoryPackFormatter<long> MemoryPack.MemoryPackReader.GetFormatter<long>()
		// MemoryPack.IMemoryPackFormatter<object> MemoryPack.MemoryPackReader.GetFormatter<object>()
		// System.Void MemoryPack.MemoryPackReader.ReadPackable<object>(object&)
		// object MemoryPack.MemoryPackReader.ReadPackable<object>()
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<ET.ActorId>(ET.ActorId&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<ET.LSCommandData>(ET.LSCommandData&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<ET.SearchUnitPackable>(ET.SearchUnitPackable&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<ST.GridBuilder.FieldV2>(ST.GridBuilder.FieldV2&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<TrueSync.FP,TrueSync.FP>(TrueSync.FP&,TrueSync.FP&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<TrueSync.FP>(TrueSync.FP&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<TrueSync.TSQuaternion>(TrueSync.TSQuaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<TrueSync.TSVector,TrueSync.TSQuaternion>(TrueSync.TSVector&,TrueSync.TSQuaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<TrueSync.TSVector2>(TrueSync.TSVector2&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<TrueSync.TSVector>(TrueSync.TSVector&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.float3>(Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.quaternion,int>(Unity.Mathematics.quaternion&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.quaternion>(Unity.Mathematics.quaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,ET.LSCommandData>(byte&,int&,ET.LSCommandData&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,long>(byte&,int&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int>(byte&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte>(byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,ET.ActorId,byte>(int&,ET.ActorId&,byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,ET.ActorId,long,int>(int&,ET.ActorId&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,Unity.Mathematics.float3>(int&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,byte,ST.GridBuilder.FieldV2>(int&,byte&,ST.GridBuilder.FieldV2&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,int>(int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,long,Unity.Mathematics.float3,Unity.Mathematics.quaternion>(int&,long&,Unity.Mathematics.float3&,Unity.Mathematics.quaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,long,long>(int&,long&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int>(int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,TrueSync.FP>(long&,TrueSync.FP&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,TrueSync.TSVector2>(long&,TrueSync.TSVector2&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,Unity.Mathematics.float3>(long&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,byte,byte,TrueSync.TSVector,TrueSync.TSVector2,TrueSync.TSVector2,TrueSync.TSQuaternion>(long&,byte&,byte&,TrueSync.TSVector&,TrueSync.TSVector2&,TrueSync.TSVector2&,TrueSync.TSQuaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,byte,byte>(long&,byte&,byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,byte>(long&,byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,TrueSync.TSVector,TrueSync.TSQuaternion>(long&,int&,TrueSync.TSVector&,TrueSync.TSQuaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,TrueSync.TSVector,int,int>(long&,int&,TrueSync.TSVector&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,byte,byte,int,int,int>(long&,int&,byte&,byte&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,byte,int,int>(long&,int&,byte&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,int,Unity.Mathematics.float3,Unity.Mathematics.float3>(long&,int&,int&,Unity.Mathematics.float3&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,int,byte>(long&,int&,int&,byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,int,int,int,int>(long&,int&,int&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,int,int>(long&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,int,long,int,int,int>(long&,int&,int&,long&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,int>(long&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,long,TrueSync.FP,TrueSync.TSVector,TrueSync.TSVector,TrueSync.TSVector,TrueSync.FP,TrueSync.FP>(long&,int&,long&,TrueSync.FP&,TrueSync.TSVector&,TrueSync.TSVector&,TrueSync.TSVector&,TrueSync.FP&,TrueSync.FP&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int,long,long,TrueSync.TSVector,int>(long&,int&,long&,long&,TrueSync.TSVector&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int>(long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,long,long,int,long,TrueSync.TSVector,TrueSync.TSVector>(long&,long&,long&,int&,long&,TrueSync.TSVector&,TrueSync.TSVector&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,long>(long&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long>(long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<uint>(uint&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanagedArray<byte>(byte[]&)
		// byte[] MemoryPack.MemoryPackReader.ReadUnmanagedArray<byte>()
		// System.Void MemoryPack.MemoryPackReader.ReadValue<object>(object&)
		// byte MemoryPack.MemoryPackReader.ReadValue<byte>()
		// long MemoryPack.MemoryPackReader.ReadValue<long>()
		// object MemoryPack.MemoryPackReader.ReadValue<object>()
		// int MemoryPack.MemoryPackSerializer.Deserialize<object>(System.ReadOnlySpan<byte>,object&,MemoryPack.MemoryPackSerializerOptions)
		// object MemoryPack.MemoryPackSerializer.Deserialize<object>(System.ReadOnlySpan<byte>,MemoryPack.MemoryPackSerializerOptions)
		// System.Void MemoryPack.MemoryPackWriter.DangerousWriteUnmanagedArray<byte>(byte[])
		// MemoryPack.IMemoryPackFormatter<ET.SearchUnitPackable> MemoryPack.MemoryPackWriter.GetFormatter<ET.SearchUnitPackable>()
		// MemoryPack.IMemoryPackFormatter<byte> MemoryPack.MemoryPackWriter.GetFormatter<byte>()
		// MemoryPack.IMemoryPackFormatter<long> MemoryPack.MemoryPackWriter.GetFormatter<long>()
		// MemoryPack.IMemoryPackFormatter<object> MemoryPack.MemoryPackWriter.GetFormatter<object>()
		// System.Void MemoryPack.MemoryPackWriter.WritePackable<object>(object&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<ET.SearchUnitPackable>(ET.SearchUnitPackable&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<TrueSync.FP,TrueSync.FP>(TrueSync.FP&,TrueSync.FP&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<TrueSync.TSVector,TrueSync.TSQuaternion>(TrueSync.TSVector&,TrueSync.TSQuaternion&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<Unity.Mathematics.quaternion,int>(Unity.Mathematics.quaternion&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<byte>(byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int,byte,ST.GridBuilder.FieldV2>(int&,byte&,ST.GridBuilder.FieldV2&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int,int>(int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int>(int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,TrueSync.FP>(long&,TrueSync.FP&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,TrueSync.TSVector2>(long&,TrueSync.TSVector2&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,byte,byte,TrueSync.TSVector,TrueSync.TSVector2,TrueSync.TSVector2,TrueSync.TSQuaternion>(long&,byte&,byte&,TrueSync.TSVector&,TrueSync.TSVector2&,TrueSync.TSVector2&,TrueSync.TSQuaternion&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,byte,byte>(long&,byte&,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,byte>(long&,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,TrueSync.TSVector,int,int>(long&,int&,TrueSync.TSVector&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,byte,byte,int,int,int>(long&,int&,byte&,byte&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,byte,int,int>(long&,int&,byte&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,int,int,int,int>(long&,int&,int&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,int,int>(long&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,int,long,int,int,int>(long&,int&,int&,long&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,int>(long&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,long,TrueSync.FP,TrueSync.TSVector,TrueSync.TSVector,TrueSync.TSVector,TrueSync.FP,TrueSync.FP>(long&,int&,long&,TrueSync.FP&,TrueSync.TSVector&,TrueSync.TSVector&,TrueSync.TSVector&,TrueSync.FP&,TrueSync.FP&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int,long,long,TrueSync.TSVector,int>(long&,int&,long&,long&,TrueSync.TSVector&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int>(long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,long,long,int,long,TrueSync.TSVector,TrueSync.TSVector>(long&,long&,long&,int&,long&,TrueSync.TSVector&,TrueSync.TSVector&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,long>(long&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long>(long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedArray<byte>(byte[])
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,ET.LSCommandData>(byte,byte&,int&,ET.LSCommandData&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,long>(byte,byte&,int&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int>(byte,byte&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte>(byte,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<int,ET.ActorId,byte>(byte,int&,ET.ActorId&,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<int,ET.ActorId,long,int>(byte,int&,ET.ActorId&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<int,Unity.Mathematics.float3>(byte,int&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<int,int>(byte,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<int,long,Unity.Mathematics.float3,Unity.Mathematics.quaternion>(byte,int&,long&,Unity.Mathematics.float3&,Unity.Mathematics.quaternion&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<int,long,long>(byte,int&,long&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<int>(byte,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<long,Unity.Mathematics.float3>(byte,long&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<long,int,TrueSync.TSVector,TrueSync.TSQuaternion>(byte,long&,int&,TrueSync.TSVector&,TrueSync.TSQuaternion&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<long,int,int,Unity.Mathematics.float3,Unity.Mathematics.float3>(byte,long&,int&,int&,Unity.Mathematics.float3&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<long,int,int,byte>(byte,long&,int&,int&,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<long>(byte,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<uint>(byte,uint&)
		// System.Void MemoryPack.MemoryPackWriter.WriteValue<byte>(byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteValue<long>(long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteValue<object>(object&)
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(MongoDB.Bson.IO.IBsonReader,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(string,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// MongoDB.Bson.Serialization.IBsonSerializer<object> MongoDB.Bson.Serialization.BsonSerializer.LookupSerializer<object>()
		// object MongoDB.Bson.Serialization.IBsonSerializerExtensions.Deserialize<object>(MongoDB.Bson.Serialization.IBsonSerializer<object>,MongoDB.Bson.Serialization.BsonDeserializationContext)
		// object ReferenceCollector.Get<object>(string)
		// object System.Activator.CreateInstance<object>()
		// byte[] System.Array.Empty<byte>()
		// object[] System.Array.Empty<object>()
		// object System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,object>(System.Collections.Generic.IReadOnlyDictionary<int,object>,int)
		// object System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,object>(System.Collections.Generic.IReadOnlyDictionary<int,object>,int,object)
		// bool System.Enum.TryParse<int>(string,bool,int&)
		// bool System.Enum.TryParse<int>(string,int&)
		// int System.HashCode.Combine<int,int,int>(int,int,int)
		// ET.RpcInfo[] System.Linq.Enumerable.ToArray<ET.RpcInfo>(System.Collections.Generic.IEnumerable<ET.RpcInfo>)
		// System.Collections.Generic.HashSet<int> System.Linq.Enumerable.ToHashSet<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.HashSet<int> System.Linq.Enumerable.ToHashSet<int>(System.Collections.Generic.IEnumerable<int>,System.Collections.Generic.IEqualityComparer<int>)
		// System.Collections.Generic.List<int> System.Linq.Enumerable.ToList<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Span<byte> System.MemoryExtensions.AsSpan<byte>(byte[])
		// object System.Reflection.CustomAttributeExtensions.GetCustomAttribute<object>(System.Reflection.MemberInfo)
		// bool System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<object>()
		// byte& System.Runtime.CompilerServices.Unsafe.Add<byte>(byte&,int)
		// byte& System.Runtime.CompilerServices.Unsafe.As<byte,byte>(byte&)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// byte& System.Runtime.CompilerServices.Unsafe.AsRef<byte>(byte&)
		// long& System.Runtime.CompilerServices.Unsafe.AsRef<long>(long&)
		// object& System.Runtime.CompilerServices.Unsafe.AsRef<object>(object&)
		// ET.ActorId System.Runtime.CompilerServices.Unsafe.ReadUnaligned<ET.ActorId>(byte&)
		// ET.LSCommandData System.Runtime.CompilerServices.Unsafe.ReadUnaligned<ET.LSCommandData>(byte&)
		// ET.SearchUnitPackable System.Runtime.CompilerServices.Unsafe.ReadUnaligned<ET.SearchUnitPackable>(byte&)
		// ST.GridBuilder.FieldV2 System.Runtime.CompilerServices.Unsafe.ReadUnaligned<ST.GridBuilder.FieldV2>(byte&)
		// TrueSync.FP System.Runtime.CompilerServices.Unsafe.ReadUnaligned<TrueSync.FP>(byte&)
		// TrueSync.TSQuaternion System.Runtime.CompilerServices.Unsafe.ReadUnaligned<TrueSync.TSQuaternion>(byte&)
		// TrueSync.TSVector System.Runtime.CompilerServices.Unsafe.ReadUnaligned<TrueSync.TSVector>(byte&)
		// TrueSync.TSVector2 System.Runtime.CompilerServices.Unsafe.ReadUnaligned<TrueSync.TSVector2>(byte&)
		// Unity.Mathematics.float3 System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Unity.Mathematics.float3>(byte&)
		// Unity.Mathematics.quaternion System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Unity.Mathematics.quaternion>(byte&)
		// byte System.Runtime.CompilerServices.Unsafe.ReadUnaligned<byte>(byte&)
		// int System.Runtime.CompilerServices.Unsafe.ReadUnaligned<int>(byte&)
		// long System.Runtime.CompilerServices.Unsafe.ReadUnaligned<long>(byte&)
		// object System.Runtime.CompilerServices.Unsafe.ReadUnaligned<object>(byte&)
		// uint System.Runtime.CompilerServices.Unsafe.ReadUnaligned<uint>(byte&)
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<ET.ActorId>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<ET.LSCommandData>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<ET.SearchUnitPackable>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<ST.GridBuilder.FieldV2>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<TrueSync.FP>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<TrueSync.TSQuaternion>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<TrueSync.TSVector2>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<TrueSync.TSVector>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<Unity.Mathematics.float3>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<Unity.Mathematics.quaternion>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<byte>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<int>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<long>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<object>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<uint>()
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<ET.ActorId>(byte&,ET.ActorId)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<ET.LSCommandData>(byte&,ET.LSCommandData)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<ET.SearchUnitPackable>(byte&,ET.SearchUnitPackable)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<ST.GridBuilder.FieldV2>(byte&,ST.GridBuilder.FieldV2)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<TrueSync.FP>(byte&,TrueSync.FP)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<TrueSync.TSQuaternion>(byte&,TrueSync.TSQuaternion)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<TrueSync.TSVector2>(byte&,TrueSync.TSVector2)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<TrueSync.TSVector>(byte&,TrueSync.TSVector)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<Unity.Mathematics.float3>(byte&,Unity.Mathematics.float3)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<Unity.Mathematics.quaternion>(byte&,Unity.Mathematics.quaternion)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<byte>(byte&,byte)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<int>(byte&,int)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<long>(byte&,long)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<uint>(byte&,uint)
		// byte& System.Runtime.InteropServices.MemoryMarshal.GetReference<byte>(System.ReadOnlySpan<byte>)
		// byte& System.Runtime.InteropServices.MemoryMarshal.GetReference<byte>(System.Span<byte>)
		// System.Threading.Tasks.Task<object> System.Threading.Tasks.TaskFactory.StartNew<object>(System.Func<object>,System.Threading.CancellationToken)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// System.Void UnityEngine.GameObject.GetComponentsInChildren<object>(bool,System.Collections.Generic.List<object>)
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<byte>(byte&,int,byte)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<float>(float&,int,float)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<int>(int&,int,int)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<long>(long&,int,long)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<object>(object&,int,object)
		// byte YIUIFramework.ParamVo.Get<byte>(int,byte)
		// float YIUIFramework.ParamVo.Get<float>(int,float)
		// int YIUIFramework.ParamVo.Get<int>(int,int)
		// long YIUIFramework.ParamVo.Get<long>(int,long)
		// object YIUIFramework.ParamVo.Get<object>(int,object)
		// bool YIUIFramework.StrConv.ToNumber<byte>(string,byte&)
		// bool YIUIFramework.StrConv.ToNumber<float>(string,float&)
		// bool YIUIFramework.StrConv.ToNumber<int>(string,int&)
		// bool YIUIFramework.StrConv.ToNumber<long>(string,long&)
		// bool YIUIFramework.StrConv.ToNumber<object>(string,object&)
		// object YIUIFramework.UIBindComponentTable.FindComponent<object>(string)
		// object YIUIFramework.UIBindDataTable.FindDataValue<object>(string)
		// object YIUIFramework.UIBindEventTable.FindEvent<object>(string)
		// YooAsset.AllAssetsHandle YooAsset.ResourcePackage.LoadAllAssetsAsync<object>(string,uint)
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetAsync<object>(string,uint)
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetSync<object>(string)
		// string string.Join<byte>(string,System.Collections.Generic.IEnumerable<byte>)
		// string string.Join<int>(string,System.Collections.Generic.IEnumerable<int>)
		// string string.Join<object>(string,System.Collections.Generic.IEnumerable<object>)
		// string string.JoinCore<byte>(System.Char*,int,System.Collections.Generic.IEnumerable<byte>)
		// string string.JoinCore<int>(System.Char*,int,System.Collections.Generic.IEnumerable<int>)
		// string string.JoinCore<object>(System.Char*,int,System.Collections.Generic.IEnumerable<object>)
	}
}