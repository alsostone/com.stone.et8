using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUDIndicator {

	[AddComponentMenu("HUD Indicator/Indicator Renderer")]
	public class IndicatorRenderer : MonoBehaviour {
        public bool visible = true;
        public float margin = 32f;
        public Color canvasColor = new Color(0, 207f/255f, 1f, 27f/255f);
        public new Camera camera;

        private RectTransform rectTransform;

        void Awake() {
            rectTransform = GetComponent<RectTransform>();

            if (camera == null) {
                camera = Camera.main;
			}
        }

		public Rect GetRect() {
            return SetMarginToRect(rectTransform.rect, margin);
		}

        public RectTransform GetRectTransform() {
            return rectTransform;
		}

        private Rect SetMarginToRect(Rect rect, float margin) {
            rect.x += margin;
            rect.y += margin;
            rect.width -= margin * 2f;
            rect.height -= margin * 2f;
            return rect;
        }

        private void OnDrawGizmosSelected() {
            rectTransform = GetComponent<RectTransform>();
            Rect rect = SetMarginToRect(rectTransform.rect, margin);

            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, canvasColor);
            tex.Apply();

            Vector3 pos = rectTransform.TransformPoint(new Vector3(rect.x, rect.y, 0));
            Graphics.DrawTexture(new Rect(pos.x, pos.y, rect.width * rectTransform.lossyScale.x, rect.height * rectTransform.lossyScale.y), tex);

            DestroyImmediate(tex);
        }
    }
}
