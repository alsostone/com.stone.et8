using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUDIndicator {
	public class IndicatorCanvasOffScreen : IndicatorCanvas {

        private IndicatorOffScreen indicatorOffScreen;

        // Icon variables
        private RawImage rawImage;
        private RectTransform rectTransform;
        private IndicatorIconStyle style;

        // Arrow variables
        private GameObject arrowGameObject;
        private RawImage arrowRawImage;
        private RectTransform arrowRectTransform;
        private IndicatorArrowStyle arrowStyle;

        public override void Create(Indicator indicator, IndicatorRenderer renderer) {
            base.Create(indicator, renderer);

            indicatorOffScreen = indicator as IndicatorOffScreen;

            // Get indicator & arrow style
            style = indicatorOffScreen.style;
            arrowStyle = indicatorOffScreen.arrowStyle;

            // Create game object
            gameObject = new GameObject($"IndicatorOffScreen:{indicator.gameObject.name}");
            gameObject.transform.SetParent(renderer.transform);

            // Setup rect transform
            rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Create raw image
            rawImage = gameObject.AddComponent<RawImage>();

            // Create arrow game object
            arrowGameObject = new GameObject($"IndicatorOffScreen:Arrow:{indicator.gameObject.name}");
            arrowGameObject.transform.SetParent(gameObject.transform);

            // Create arrow raw image
            arrowRawImage = arrowGameObject.AddComponent<RawImage>();

            // Setup arrow rect transform
            arrowRectTransform = arrowGameObject.GetComponent<RectTransform>();
            arrowRectTransform.localScale = Vector3.one;


            // Update icon and arrow style
            UpdateStyle();
            UpdateArrowStyle();
        }

        public override void Update() {
            if(!active) return;

            if(IsVisible()) {
                // Update icon style
                UpdateStyle();

                // Update arrow style and show/hide arrow
                if(indicatorOffScreen.showArrow) {
                    arrowGameObject.SetActive(true);
                    UpdateArrowStyle();
				}
				else {
                    arrowGameObject.SetActive(false);
                }

                // Update icon position
                UpdatePosition();
            }
            else {
                gameObject.SetActive(false);
            }
        }

        private void UpdateStyle() {
            rectTransform.sizeDelta = new Vector2(style.width, style.height);
            rawImage.texture = style.texture;
            rawImage.color = style.color;
        }

        private void UpdateArrowStyle() {
            arrowRectTransform.sizeDelta = new Vector2(arrowStyle.width, arrowStyle.height);
            arrowRectTransform.pivot = new Vector2(0.5f, 1 + ((style.height + style.width) / 4f + arrowStyle.margin) / arrowStyle.height);

            arrowRawImage.texture = arrowStyle.texture;
            arrowRawImage.color = arrowStyle.color;
        }

        private void UpdatePosition() {
            Rect rendererRect = renderer.GetRect();
            Vector3 pos = renderer.GetRectTransform().InverseTransformPoint(renderer.camera.WorldToScreenPoint(indicator.gameObject.transform.position));

            rendererRect.x += style.width / 2f;
            rendererRect.y += style.height / 2f;
            rendererRect.width -= style.width;
            rendererRect.height -= style.height;

            // On-screen (Hide)
            if(pos.z >= 0 && pos.x >= rendererRect.x && pos.x <= rendererRect.x + rendererRect.width && pos.y >= rendererRect.y && pos.y <= rendererRect.y + rendererRect.height) {
                gameObject.SetActive(false);
            }
            // Off-screen (Show)
            else {
                gameObject.SetActive(true);

                if(pos.z < 0) {
                    pos *= -1;
                }

                float a = pos.x / rendererRect.width;
                float b = pos.y / rendererRect.height;

                // The indicator lies on left or right corner
                if(Mathf.Abs(a) > Mathf.Abs(b)) {

                    // Right corner
                    if(a > 0) {
                        float y = rendererRect.width / 2f * pos.y / pos.x;
                        pos = new Vector2(rendererRect.width / 2f, y);
                    }
                    // Left corner
                    else {
                        float y = -rendererRect.width / 2f * pos.y / pos.x;
                        pos = new Vector2(-rendererRect.width / 2f, y);
                    }
                }
                // The indicator lies on top or bottom corner
                else {
                    // Top corner
                    if(b > 0) {
                        float x = rendererRect.height / 2f * pos.x / pos.y;
                        pos = new Vector2(x, rendererRect.height / 2f);
                    }
                    // Bottom corner
                    else {
                        float x = -rendererRect.height / 2f * pos.x / pos.y;
                        pos = new Vector2(x, -rendererRect.height / 2f);
                    }
                }

                // Update arrow rotation
                Vector2 dir = new Vector2(pos.x, pos.y);
                arrowRectTransform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, dir));

                rectTransform.position = renderer.GetRectTransform().TransformPoint(new Vector3(pos.x, pos.y, 0));
            }

        }

		public override void Destroy() {
			GameObject.Destroy(gameObject);
		}
	}
}
