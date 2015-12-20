using System;
using System.IO;
using UnityEngine;

namespace Assets.CommonB.UI {
	[ExecuteInEditMode]
	public class FlexibleCamera : MonoBehaviour {
		public enum FixedBy {
			Width,
			Height,
			Auto
		}

		public FixedBy fixedBy;
		public int fixedWidth;
		public int fixedHeight;
		public bool fictive = false;

		private int _width;
		private int _height;
		private int _fixedWidth;
		private int _fixedHeight;

		private int lastWidth, lastHeight;
		private DeviceOrientation lastOrientation;
		private FixedBy _lastFixedBy;

		public event Action OnChangeSize;

		private void Awake() {
			if (!fictive && GetComponent<Camera>()) {
				GetComponent<Camera>().orthographic = true;
			}
			Check();
		}

		public int width {
			get {
				Check();
				return _width;
			}
		}

		public int height {
			get {
				Check();
				return _height;
			}
		}

		private void FixedUpdate() {
			Check();
		}

		private void Check() {
			bool needUpdate = lastWidth != Screen.width || lastHeight != Screen.height || _lastFixedBy != fixedBy ||
							lastOrientation != Input.deviceOrientation || _fixedWidth != fixedWidth || _fixedHeight != fixedHeight;
			if (needUpdate) {
				UpdateDim();
			}
		}

		private void UpdateDim() {
			lastWidth = Screen.width;
			lastHeight = Screen.height;
			_fixedWidth = fixedWidth;
			_fixedHeight = fixedHeight;
			lastOrientation = Input.deviceOrientation;
			_lastFixedBy = fixedBy;
			FixedDimBy(fixedBy);
			UpdateSize();
			FireEventSize();
		}

		public Vector2 CalculateDimension(FixedBy fixedBy, int fixedWidth, int fixedHeight) {
			if (Screen.width == 0 || Screen.height == 0) return Vector2.zero;
			int w = fixedWidth;
			int h = fixedHeight;
			var finalFixedBy = fixedBy;
			if (fixedBy == FixedBy.Auto) {
				float fd = w/(float) h;
				float sd = Screen.width/(float) Screen.height;
				finalFixedBy = sd > fd ? FixedBy.Width : FixedBy.Height;
			}
			var result = new Vector2();
			switch (finalFixedBy) {
				case FixedBy.Width:
					result.Set(w, Screen.height*w/Screen.width);
					break;
				case FixedBy.Height:
					result.Set(Screen.width*h/Screen.height, h);
					break;
			}
			return result;
		}

		private void FixedDimBy(FixedBy fixedBy) {
			var dimension = CalculateDimension(fixedBy, fixedWidth, fixedHeight);
			_width = (int) dimension.x;
			_height = (int) dimension.y;
		}

		private void UpdateSize() {
			if (!fictive && GetComponent<Camera>()) {
				GetComponent<Camera>().orthographicSize = height/2;
			}
		}

		private void FireEventSize() {
			if (OnChangeSize != null) {
				OnChangeSize();
			}
		}

		public Vector2 ToAppCoord(Vector2 cursorPosition) {
			float x = cursorPosition.x*width/Screen.width;
			float y = cursorPosition.y*height/Screen.height;
			return new Vector2(x, y);
		}
	}
}