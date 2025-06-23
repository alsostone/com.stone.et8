using UnityEngine;
using Random = UnityEngine.Random;

namespace ET
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
            if (Input.GetKeyDown(KeyCode.N))
            {
                GameObject go = Instantiate(buildingPrefab[Random.Range(0, buildingPrefab.Length)]);
                Building building = go.GetComponent<Building>();
                gridBuilder.SetPlacementBuilding(building);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gridBuilder.RotationPlacementBuilding();
            }
        }
    }
}
