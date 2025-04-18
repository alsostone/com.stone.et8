using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace HUDIndicator {

    public abstract class Indicator : MonoBehaviour {

        public bool visible = true;
        [SerializeField] private List<IndicatorRenderer> renderers = new List<IndicatorRenderer>();
        
        protected Dictionary<IndicatorRenderer, IndicatorCanvas> indicatorsCanvas = new Dictionary<IndicatorRenderer, IndicatorCanvas>();

		private void Start() {
            if (renderers.Count == 0) {
                IndicatorRenderer[] renderersInScene = GameObject.FindObjectsOfType<IndicatorRenderer>(true);

                if (renderersInScene.Length > 0) {
                    renderers = renderersInScene.ToList();
				}
                else {
                    Debug.LogError("No IndicatorRenderer found in scene");
                }
			}
            
            foreach(IndicatorRenderer renderer in renderers) {
                CreateIndicatorCanvas(renderer);
            }
        }

        public List<IndicatorRenderer> GetRenderers() {
            return renderers;
        }

        public void SetRenderer(IndicatorRenderer renderer) {
			renderers = new List<IndicatorRenderer> {
				renderer
			};
		}

        public void SetRenderer(List<IndicatorRenderer> renderers) {
            this.renderers = renderers;
        }

		private void Update() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                element.Value.Update();
            }
        }

        private void OnEnable() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                element.Value.OnEnable();
            }
        }

        private void OnDisable() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                element.Value.OnDisable();
            }
        }

		private void OnDestroy() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                DestroyIndicatorCanvas(element.Key);
            }
		}

        protected abstract void CreateIndicatorCanvas(IndicatorRenderer renderer);

        private void DestroyIndicatorCanvas(IndicatorRenderer renderer) {
            if(indicatorsCanvas.ContainsKey(renderer)) {
                indicatorsCanvas[renderer].Destroy();
			}
		}
    }
}
