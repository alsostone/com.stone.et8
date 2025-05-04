using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace ST.HUD
{
    [RequireComponent(typeof(Text))]
    public class FontRender2Texture : MonoBehaviour
    {
        internal int DrawingCount => _count - _freeIndices.Count;
        
        public Camera uiCamera;
        public Texture2DArray TextureArray { get; private set; }

        private Text _text;
        private RenderTexture _renderTexture;
        private Vector2Int _textureSize;
        
        private int _count = 0;
        private Dictionary<string, int> _nameIndexMapping;
        private Dictionary<int, int> _indexRefMapping;
        private Stack<int> _freeIndices;

        private void Awake()
        {
            _nameIndexMapping = new Dictionary<string, int>(HudConst.MaxTextureCount);
            _indexRefMapping = new Dictionary<int, int>(HudConst.MaxTextureCount);
            _freeIndices = new Stack<int>(HudConst.MaxTextureCount);
            
            var size = ((RectTransform)transform).rect.size;
            _textureSize = new Vector2Int((int)size.x, (int)size.y);

            _text = GetComponent<Text>();
            TextureArray = new Texture2DArray(_textureSize.x, _textureSize.y, HudConst.MaxTextureCount, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
            TextureArray.name = "FontTextureArray";

            _renderTexture = new RenderTexture(_textureSize.x, _textureSize.y, 0, GraphicsFormat.R8G8B8A8_UNorm);
            _renderTexture.name = "FontRenderTexture";
            _renderTexture.useMipMap = false;

            uiCamera.enabled = false;
            uiCamera.targetTexture = _renderTexture;
        }

        internal int Draw(string txt)
        {
            if (!_nameIndexMapping.TryGetValue(txt, out var index))
            {
                if (!_freeIndices.TryPop(out index))
                {
                    if (_count >= HudConst.MaxTextureCount)
                    {
                        Debug.LogError("FontRender2Texture max texture count reached");
                        return 0;
                    }
                    index = _count;
                    _count++;
                }
                
                _text.text = txt;
                uiCamera.enabled = true;
                RenderTexture.active = _renderTexture;
                uiCamera.Render();
                RenderTexture.active = null;
                uiCamera.enabled = false;
                Graphics.CopyTexture(_renderTexture, 0, 0, 0, 0, _textureSize.x, _textureSize.y, TextureArray, index, 0, 0, 0);
                _nameIndexMapping.Add(txt, index);
            }
            
            if (_indexRefMapping.TryGetValue(index, out var refCount))
            {
                _indexRefMapping[index] = refCount + 1;
            } else {
                _indexRefMapping.Add(index, 1);
            }
            return index;
        }

        internal void Remove(int index)
        {
            if (_indexRefMapping.TryGetValue(index, out var refCount))
            {
                --refCount;
                if (refCount == 0)
                {
                    _freeIndices.Push(index);
                    _indexRefMapping.Remove(index);
                }
                else
                {
                    _indexRefMapping[index] = refCount;
                }
            }
        }
        
    }
}