using UnityEngine;
using Random = UnityEngine.Random;

namespace ST.GridBuilder
{
    public class BuildingSpawner : MonoBehaviour
    {
        public GameObject[] buildingPrefab;
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
                GameObject go = Instantiate(buildingPrefab[Random.Range(0, buildingPrefab.Length)]);
                Placement placement = go.GetComponent<Placement>();
                gridBuilder.SetPlacementObject(placement);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gridBuilder.RotationPlacementBuilding();
            }
        }
    }
}
