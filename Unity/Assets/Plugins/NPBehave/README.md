# 双端可序列化的行为树
## 前言
之前的俩个项目使用Tencent开源项目Behaviac，集成简单，支持全平台。
后续有序列化需求，开始时考虑对Behaviac进行拓展，但涉及到编辑器和运行时，包袱太重。

## 为什么基于NPBehave
- 轻量，快速，简洁，用代码构建行为树。
- 保持当前状态，事件驱动后才继续遍历，性能高。
- 与引擎藕合低，剥离成本低。

## 要解决的痛点

- 由于要用到帧同步中，有纯逻辑需求`引擎无关性`，需移除运行时对Unity的依赖。
- 需使用MemoryPack做序列化/反序列化，主要调整如下。
  - 使用回调的节点全部改为使用继承，同时继承方式对后续做编辑器也是有益的。
  - 原使用object作为黑板值类型，改为确定类型，只支持bool、int、float。
  - 为节点和黑板添加GUID，事件广播通过GUID反查对象的接口，不再使用注册回调的方式。
- 可视化编辑器`项目初期行为树由程序构建，往后放`
- 调试`自带的Debugger能查看行为树的运行状态，够用`

### 原项目连接
原始项目的Readme能帮助理解NPBehave的设计理念和用法。  
[https://github.com/meniku/NPBehave](https://github.com/meniku/NPBehave)
### 本项目链接
[https://github.com/alsostone/NPBehave](https://github.com/alsostone/NPBehave)

## 正文
下文将对主要调整进行阐述。`Github提交记录可作为参考`。  
为保证其通用性，不对该项目进行帧同步相关的调整，如有需求可在[游戏项目](https://github.com/alsostone/com.stone.et8)中获取定点数版本。

### 安装
1. 安装MemoryPack
2. 将NPBehave文件夹放入项目中 `Scripts子文件夹是核心库`

### 例子：一个事件驱动的行为树
这有一个使用黑板的事件驱动的行为树例子
```csharp
public class ActionLog : Action
{
    private readonly string text;
    public ActionLog(string text)
    {
        this.text = text;
    }
    protected override bool OnAction()
    {
        Debug.Log(text);
        return true;
    }
}

private class UpdateService : Service
{
    public UpdateService(float interval, Node decoratee) : base(interval, decoratee)
    {
    }
    protected override void OnService()
    {
        Blackboard.SetBool("foo", !Blackboard.GetBool("foo"));
    }
}

/// ...
behaviorTree = new Root(UnityContext.GetBehaveWorld(),
    new UpdateService(0.5f,
        new Selector(
            new BlackboardBool("foo", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,
                new Sequence(
                    new ActionLog("foo"),
                    new WaitUntilStopped()
                )
            ),

            new Sequence(
                new ActionLog("bar"),
                new WaitUntilStopped()
            )
        )
    )
);
behaviorTree.Start();
//...
```
这个示例将在每500毫秒交替打印“foo”和“bar”。我们使用一个`服务`装饰器节点在黑板上切换foo boolean值。我们使用BlackboardBool装饰器节点根据这个boolean值来决定是否执行分支。BlackboardBool还会根据这个值监视黑板的变化，`Stops.IMMEDIATE_RESTART`作用是如果条件不再为真，则当前执行的分支将停止，如果条件再次为真，则立即重新启动。

## 新增和调整

### 世界 BehaveWorld
时钟和黑板不可new()，只能通过调用世界的接口创建，接口将对黑板和时钟进行缓存确保能正确序列化。序列化世界即可把属于世界的时钟和黑板同步序列化。
一个世界包含一个时钟，例子UnityContext.cs中每一帧都更新一次时钟。在某些情况下，你可能想要拥有更多的控制权。查看 Clock Throttling.cs。
黑板不限制个数，常规情况下1个单位会拥有1个黑板，敌我双方会各自共享1个黑板，全局再共享1个，共计3层。

### 根节点 Root
- Root(BehaveWorld world, Node decoratee):Root是一个特殊的装饰节点，无休止的运行被装饰的节点
- Root(BehaveWorld world, Blackboard blackboard, Node decoratee):使用给定的黑板，而不是实例化一个

### 序列化
```csharp
var world = new BehaveWorld();
var root1 = new Root(world, new Sequence(new ActionLog("root1 1"), new ActionLog("root1 2")));
var root2 = new Root(world, new Sequence(new ActionLog("root2 1"), new ActionLog("root2 2")));

var worldBytes = MemoryPackSerializer.Serialize(world);
var root1Bytes = MemoryPackSerializer.Serialize(root1);
var root2Bytes = MemoryPackSerializer.Serialize(root2);

world = MemoryPackSerializer.Deserialize<BehaveWorld>(worldBytes);

// 反序列化节点1并重建其上下文
root1 = MemoryPackSerializer.Deserialize<Root>(root1Bytes);
root1.SetWorld(world);

// 反序列化节点2并重建其上下文
root2 = MemoryPackSerializer.Deserialize<Root>(root2Bytes);
root2.SetWorld(world);
```
以上的例子为反序列化1个世界和2个根节点。根节点单独序列化原因：实战中1个世界中总会包含多个AI，AI跟随具体单位序列化更符合直觉。

### 事件分发
1. 需要监听时把当前节点通过GUID的方式注册到黑板中
```csharp
Blackboard.AddObserver(blackboardKey, Guid);
Blackboard.RemoveObserver(blackboardKey, Guid);
```
2. 黑板数值被改变时广播
```csharp
BehaveWorld.GuidReceiverMapping[Guid].OnObservingChanged(notification.type);
```

