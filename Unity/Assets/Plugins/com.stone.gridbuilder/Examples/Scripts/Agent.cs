using UnityEngine;
using Random = UnityEngine.Random;

namespace ST.GridBuilder
{
    public class Agent : MonoBehaviour
    {
        private GridMap gridMap;

        private void Start()
        {
            gridMap = FindObjectOfType<GridMap>();
        }

        // Update is called once per frame
        void Update()
        {
            var position = transform.position;
            position += gridMap.GetFieldVector(position) * Time.deltaTime * 2;
            transform.position = position;
        }
    }
}
