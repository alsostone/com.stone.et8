using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUDIndicator {

    public abstract class IndicatorCanvas {
        protected Indicator indicator { private set; get; }
        protected IndicatorRenderer renderer { private set; get; }
        protected GameObject gameObject;
        protected bool active;

        public virtual void Create(Indicator indicator, IndicatorRenderer renderer) {
            this.indicator = indicator;
            this.renderer = renderer;

            active = true;
		}

        public abstract void Update();

        public virtual void OnEnable() {
            if(gameObject != null) {
                gameObject.SetActive(true);
			}
            active = true;
        }
        public virtual void OnDisable(){
            if(gameObject != null) {
                gameObject.SetActive(false);
            }
            active = false;
        }

        public virtual void Destroy() {}

        public bool IsVisible() {
            return indicator.visible && renderer.visible;
        }
    }
}