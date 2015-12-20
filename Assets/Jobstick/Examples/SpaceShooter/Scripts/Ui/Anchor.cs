using System.Collections;
using UnityEngine;

namespace Assets.CommonB.UI {
	[ExecuteInEditMode]
	public class Anchor : MonoBehaviour {

		public enum EdgeVert {
			None, Top, Center, Bottom
		}

		public enum EdgeHor {
			None, Left, Center, Right
		}

		public FlexibleCamera flexibleCamera;
		public EdgeHor hor = EdgeHor.None;
		public EdgeVert vert = EdgeVert.None;
		public Vector2 indent;
		public bool ignoreScale = true;

		void Awake() {
			this.FindAndSetInNotNull(ref flexibleCamera);
			if (flexibleCamera) {
				flexibleCamera.OnChangeSize += UpdatePosition;
			}
		}

		void OnEnable()
		{
			UpdatePosition();
//			StartCoroutine(LateCoroutine());
		}

		void Start() {
//			UpdatePosition();
//			StartCoroutine(LateCoroutine());
		}
		
		public void SetFlexibleCamera(FlexibleCamera flexibleCamera) {
			if (this.flexibleCamera) {
				flexibleCamera.OnChangeSize -= UpdatePosition;
			}
			if (flexibleCamera) {
				flexibleCamera.OnChangeSize += UpdatePosition;
			}
			this.flexibleCamera = flexibleCamera;
			UpdatePosition();
		}

		private IEnumerator LateCoroutine() {
			yield return new WaitForEndOfFrame();
			UpdatePosition();
		}

		void OnDestroy() {
			if (flexibleCamera) {
				flexibleCamera.OnChangeSize -= UpdatePosition;
			}
		}

		public void UpdatePosition() {
			Vector3 localPosition = transform.localPosition;
			float x;
			switch (hor) {
				case EdgeHor.Left:
					x = -flexibleCamera.width/2f;
					break;
				case EdgeHor.Right:
					x = flexibleCamera.width / 2f;
					break;
				case EdgeHor.Center:
					x = 0;
					break;
				default:
					x = localPosition.x;
					break;
			}
			float y;
			switch (vert) {
				case EdgeVert.Top:
					y = flexibleCamera.height / 2f;
					break;
				case EdgeVert.Bottom:
					y = -flexibleCamera.height / 2f;
					break;
				case EdgeVert.Center:
					y = 0;
					break;
				default:
					y = localPosition.y;
					break;
			}
			localPosition.x = x + indent.x;
			localPosition.y = y + indent.y;
			var parent = transform.parent;
			if (parent && ignoreScale) {
				var lossyScale = parent.lossyScale;
				if (lossyScale.x != 0 && lossyScale.y != 0) {
					localPosition.x /= lossyScale.x;
					localPosition.y /= lossyScale.y;
				}
			}
			transform.localPosition = localPosition;
		}

	}
}
