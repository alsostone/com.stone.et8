using System.Collections.Generic;

namespace ST.GridBuilder
{
    public class MultiFlowField
    {
        private readonly GridData gridData;
        private readonly List<FlowFieldNode[]> flowFields;
        private readonly Stack<int> freeFlowField;

        public MultiFlowField(GridData gridData)
        {
            this.gridData = gridData;
            flowFields = new List<FlowFieldNode[]>();
            freeFlowField = new Stack<int>();
        }
        
        public FlowFieldNode[] GetFlowField(int index)
        {
            if (index < 0 || index >= flowFields.Count)
                return null;
            return flowFields[index];
        }

        public void ReleaseFlowField(int index)
        {
            if (index < 0 || index >= flowFields.Count)
                return;
            freeFlowField.Push(index);
        }

        public int GenerateFlowField(FieldV2 destination)
        {
            FlowFieldNode[] flowField;
            if (freeFlowField.TryPop(out int index)) {
                flowField = flowFields[index];
            } else {
                flowField = new FlowFieldNode[gridData.xLength * gridData.zLength];
                flowFields.Add(flowField);
                index = flowFields.Count - 1;
            }
            
            gridData.ResetDijkstraData(flowField, destination);
            gridData.GenerateDijkstraData(flowField);
            return index;
        }

        public int GenerateFlowField(List<FieldV2> destinations)
        {
            FlowFieldNode[] flowField;
            if (freeFlowField.TryPop(out int index)) {
                flowField = flowFields[index];
            } else {
                flowField = new FlowFieldNode[gridData.xLength * gridData.zLength];
                flowFields.Add(flowField);
                index = flowFields.Count - 1;
            }
            
            gridData.ResetDijkstraData(flowField, destinations);
            gridData.GenerateDijkstraData(flowField);
            return index;
        }

    }
}