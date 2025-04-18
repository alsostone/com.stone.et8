using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUDIndicator {

    [System.Serializable]
    public class IndicatorIconStyle {
        public Texture texture = null;
        public Color color = Color.white;
        public float width = 32f;
        public float height = 32f;
    }
}