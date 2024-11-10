using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gilzoide.LottiePlayer
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class CustomImageLottiePlayer : MaskableGraphic
    {
        [FormerlySerializedAs("_animationAsset")]
        [Header("Animation Options")] 
        [SerializeField] protected string _animationPath;
        [SerializeField] protected AutoPlayEvent _autoPlay = AutoPlayEvent.OnStart;
        [SerializeField] protected bool _loop = true;

        [Header("Texture Options")]
        [SerializeField, Min(2)] protected int _width = 128;
        [SerializeField, Min(2)] protected int _height = 128;
        [SerializeField] protected bool _keepAspect = true;

        protected Texture2D _texture;
        protected LottieAnimation _animation;
        protected float _time = 0;
        protected uint _currentFrame = 0;
        protected uint _lastRenderedFrame = 0;
        protected JobHandle _renderJobHandle;
        protected Coroutine _playCoroutine;

        public override Texture mainTexture => _texture;

        public bool IsPlaying => _playCoroutine != null;

        protected override void OnEnable()
        {
            base.OnEnable();
            RecreateAnimationIfNeeded();
            if (_autoPlay == AutoPlayEvent.OnEnable && Application.isPlaying)
            {
                Play();
            }
        }

        protected override void Start()
        {
            base.Start();
            if (_autoPlay == AutoPlayEvent.OnStart && Application.isPlaying)
            {
                Play();
            }
        }

        protected override void OnDisable()
        {
            Pause();
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            DestroyImmediate(_texture);
            // Disposeすると落ちる
            if (_animation.IsValid())
            {
                _animation.Dispose();
            }
            base.OnDestroy();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (!_animation.IsValid())
            {
                return;
            }

            Rect pixelAdjustedRect = GetPixelAdjustedRect();
            if (_keepAspect)
            {
                pixelAdjustedRect = pixelAdjustedRect.AspectFit(_animation.GetSize().GetAspect());
            }
            Color32 color = this.color;
            vh.AddVert(new Vector3(pixelAdjustedRect.xMin, pixelAdjustedRect.yMin), color, new Vector2(0f, 1f));
            vh.AddVert(new Vector3(pixelAdjustedRect.xMin, pixelAdjustedRect.yMax), color, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(pixelAdjustedRect.xMax, pixelAdjustedRect.yMax), color, new Vector2(1f, 0f));
            vh.AddVert(new Vector3(pixelAdjustedRect.xMax, pixelAdjustedRect.yMin), color, new Vector2(1f, 1f));
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        [ContextMenu("Play")]
        public void Play(float startTime = 0)
        {
            Pause();
            _time = startTime;
            Unpause();
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            if (_playCoroutine != null)
            {
                StopCoroutine(_playCoroutine);
                _playCoroutine = null;
            }
        }

        [ContextMenu("Unpause")]
        public void Unpause()
        {
            if (!IsPlaying && _animation.IsValid())
            {
                _playCoroutine = StartCoroutine(PlayRoutine());
            }
        }

        protected IEnumerator PlayRoutine()
        {
            // force render first frame
            _lastRenderedFrame = uint.MaxValue;

            float duration = (float) _animation.GetDuration();
            while (_loop || _time < duration)
            {
                _currentFrame = _animation.GetFrameAtTime(_time, _loop);
                if (_currentFrame != _lastRenderedFrame)
                {
                    ScheduleRenderJob(_currentFrame);
                }
                yield return null;
                _time += Time.deltaTime;
                if (_currentFrame != _lastRenderedFrame)
                {
                    CompleteRenderJob();
                }
            }
            CompleteRenderJob();
            _playCoroutine = null;
        }

        protected void RecreateAnimationIfNeeded()
        {
            Debug.Log($"[Debug][ImageLottiePlayer] Anim valid?:{_animation.IsValid()}");
            if (_animationPath == null) return;
            
            if (_animation.IsValid())
            {
                _animation.Dispose();
            }

            if (!_animation.IsValid())
            {
                _animation = new LottieAnimation(_animationPath);
            }
            if (_texture == null
                || _width != _texture.width
                || _height != _texture.height)
            {
                DestroyImmediate(_texture);
                _texture = _animation.CreateTexture(_width, _height, true);
            }

            if (!Application.isPlaying)
            {
                RenderNow();
            }
        }

        protected void RenderNow()
        {
            _animation.Render(_currentFrame, _texture, keepAspectRatio: false);
            _texture.Apply(true);
        }

        protected void ScheduleRenderJob(uint frame)
        {
            _renderJobHandle = _animation.NativeHandle.CreateRenderJob(frame, _texture, keepAspectRatio: false).Schedule();
        }

        protected void CompleteRenderJob()
        {
            _lastRenderedFrame = _currentFrame;
            _renderJobHandle.Complete();
            _texture.Apply(true);
        }

        private NativeLottieAnimation CreateNativeAnimation(LottieAnimationAsset asset)
        {
            return new NativeLottieAnimation(asset.Json, asset.CacheKey, asset.ResourcePath);
        }

        public bool UpdateMetadata(NativeLottieAnimation animation)
        {
            if (animation.IsCreated)
            {
                // _size = animation.GetSize();
                // _frameCount = animation.GetTotalFrame();
                // _frameRate = animation.GetFrameRate();
                // _duration = animation.GetDuration();
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (IsActive())
            {
                RecreateAnimationIfNeeded();
            }
        }
#endif
        public void SetPath(string path)
        {
            _animationPath = path;
            RecreateAnimationIfNeeded();
        }
    }

}
