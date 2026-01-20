
namespace ET
{
    [LSEntitySystemOf(typeof(TargetableComponent))]
    [EntitySystemOf(typeof(TargetableComponent))]
    [FriendOf(typeof(TargetableComponent))]
    public static partial class TargetableComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TargetableComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(175, self.LSParent().Id);
            LSUnit lsOwner = self.LSOwner();
            LSTargetsComponent targetsComponent = self.LSWorld().GetComponent<LSTargetsComponent>();
            TeamComponent teamComponent = lsOwner.GetComponent<TeamComponent>();
            TransformComponent transformComponent = lsOwner.GetComponent<TransformComponent>();
            
            var targets = targetsComponent.GetTargetsTree(teamComponent.Type);
            self.ProxyId = targets.CreateProxy(transformComponent.GetAABB(), lsOwner.Id);
            self.PreviousPosition = transformComponent.Position;
        }
        
        [EntitySystem]
        private static void Destroy(this TargetableComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(174, self.LSParent().Id);
            LSTargetsComponent targetsComponent = self.LSWorld().GetComponent<LSTargetsComponent>();
            TeamComponent teamComponent = self.LSOwner().GetComponent<TeamComponent>();
            var targets = targetsComponent.GetTargetsTree(teamComponent.Type);
            targets.DestroyProxy(self.ProxyId);
        }

        [LSEntitySystem]
        private static void LSUpdate(this TargetableComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(173, self.LSParent().Id);
            if (self.IsDirty)
            {
                LSUnit lsOwner = self.LSOwner();
                LSTargetsComponent targetsComponent = self.LSWorld().GetComponent<LSTargetsComponent>();
                TeamComponent teamComponent = lsOwner.GetComponent<TeamComponent>();
                TransformComponent transformComponent = lsOwner.GetComponent<TransformComponent>();
                
                var targets = targetsComponent.GetTargetsTree(teamComponent.Type);
                targets.MoveProxy(self.ProxyId, transformComponent.GetAABB(), transformComponent.Position - self.PreviousPosition);
                self.PreviousPosition = transformComponent.Position;
            }
        }
        
    }
}