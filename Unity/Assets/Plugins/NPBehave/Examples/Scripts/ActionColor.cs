using UnityEngine;

namespace NPBehave.Examples
{
    public class ActionColor : Action
    {
        private readonly Transform transform;
        private readonly Color color;
        
        public ActionColor(Transform transform, Color color)
        {
            this.transform = transform;
            this.color = color;
        }
        
        protected override bool OnAction()
        {
            transform.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
            return true;
        }
    }
}