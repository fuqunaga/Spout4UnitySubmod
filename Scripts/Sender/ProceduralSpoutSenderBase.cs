﻿using UnityEngine;
using System.Collections;

namespace Spout {
	
	public abstract class ProceduralSpoutSenderBase : MonoBehaviour {
		
		public string sharingName = "UnitySender";
		public bool debugConsole = false;

		public SpoutSenderImpl.TextureFormat textureFormat = SpoutSenderImpl.TextureFormat.DXGI_FORMAT_R8G8B8A8_UNORM;
        public int depth = 24;
        public RenderTextureFormat format = RenderTextureFormat.ARGB32;
        public RenderTextureReadWrite gamma = RenderTextureReadWrite.Linear;
		public int width = 1920;
		public int height = 1080;

		RenderTexture _tex;
		SpoutSenderImpl _impl;

        public RenderTexture texture => _tex;

        protected abstract void NotifyOnUpdateTexture(RenderTexture tex);

		protected virtual void Awake() {
			Spout.instance.OnEnabled-= _OnSpoutEnabled;
			Spout.instance.OnEnabled+= _OnSpoutEnabled;			
		}
		protected virtual void OnEnable() {
			if(debugConsole)
				Spout.instance.initDebugConsole();
		}
		protected virtual void OnDisable(){
			if (_impl != null) {
				_impl.Dispose ();
				_impl = null;
			}
			if (_tex != null) {
				Destroy (_tex);
				_tex = null;
                NotifyOnUpdateTexture (_tex);
			}
		}
		protected virtual void Update(){
			if (_tex == null || _tex.width != width || _tex.height != height)
                Rebuild ();
			if (_impl != null)
				_impl.Update ();
		}

        public virtual void Rebuild () {
            Destroy (_tex);
            _tex = new RenderTexture (width, height, depth, format, gamma);
            _tex.Create ();
            if (_impl != null)
                _impl.Dispose ();
            _impl = new SpoutSenderImpl (sharingName, textureFormat, _tex);
            NotifyOnUpdateTexture (_tex);
        }

		protected virtual void _OnSpoutEnabled(){
			if(enabled){
				//force a reconnection
				enabled = !enabled;
				enabled = !enabled;
			}
		}
	}
}
