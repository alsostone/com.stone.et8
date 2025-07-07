using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ST.GridBuilder
{
    public class PlacementSpawner : MonoBehaviour
    {
        public GameObject[] gameObjects;
        private GridBuilder gridBuilder;

        private void Start()
        {
            gridBuilder = FindObjectOfType<GridBuilder>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameObject go = Instantiate(gameObjects[Random.Range(0, gameObjects.Length)]);
                Placement placement = go.GetComponent<Placement>();
                gridBuilder.SetPlacementObject(placement);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gridBuilder.RotatePlacementObject();
            }
        }
    }
}
