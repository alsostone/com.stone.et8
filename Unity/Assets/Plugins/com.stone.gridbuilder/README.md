# GridBuilder & FlowField
该模块实现了放置系统和基于流场的路径规划算法。

## 如何使用
1. **网格管理** 使用 `GridMap` 类来创建和管理网格。可以在Inspector中调整网格的大小和单格大小。
2. **默认障碍** 
   1. 为障碍设置层级
   2. 添加碰撞体（射线检测占用时使用）
   3. 在`GridMap`的Inspector中配置默认障碍层级
3. **地块/建筑预制**
   1. 为预制添加`Placement`脚本
   2. 设置预览材质
   3. 设置类型以及目标层级
   4. 编辑逻辑形状
   5. 添加碰撞体（拖拽时使用）
   6. 设置层级（没用）
4. **放置地块**
   1. 在场景中创建一个 `GridBuilder` 对象并添加脚本 `GridBuilder`
   2. 在Inspector中设置主摄像机、网格
   3. 调用 `SetPlacementObject` 方法来设置放置物体
5. **流场寻路**
   1. 调用 `GridData` 的 `SetDestination` 方法设置流场终点
   2. 在地块、障碍、终点变化时使用 `ResetFlowField` 方法重置流场
   3. 使用 `GetFieldVector` 方法推力向量