using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.CommonB {
	public static class Extensions {

		public static bool UnityAny<T>(this IEnumerable<T> array, Func<T, bool> predicate) {
#if UNITY_IPHONE
			foreach (var element in array) {
				if (predicate(element)) return true;
			}
			return false;
#else
			return array.Any(predicate);
#endif
		}

		public static bool UnityAny<T>(this IEnumerable<T> array) {
#if UNITY_IPHONE
			return array.GetEnumerator().MoveNext();
#else
			return array.Any();
#endif
		}

		public static int UnitySum(this IEnumerable<int> array) {
#if UNITY_IPHONE
			int summ = 0;
			foreach (var i in array) {
				summ += i;
			}
			return summ;
#else
			return array.Sum();
#endif
		}

		public static int UnityMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) {
#if UNITY_IPHONE
			int max = 0;
			bool setted = false;
			foreach (var el in source) {
				int i = selector.Invoke(el);
				if (!setted || i > max) {
					max = i;
					setted = true;
				}
			}
			return max;
#else
			return source.Max(selector);
#endif
		}

		public static void FindAndSetInNotNull<T>(ref T component) where T: Behaviour{
			if (!component) {
				component = Object.FindObjectOfType<T>();
			}
		}

		public static void DestroyChildren(this Transform transform) {
			var children = transform.Cast<Transform>().ToArray();
			foreach (var child in children) {
				Object.Destroy(child.gameObject);
			}
		}

		public static void DestroyChildrenImmediate(this Transform transform) {
			var children = transform.Cast<Transform>().ToArray();
			foreach (var child in children) {
				Object.DestroyImmediate(child.gameObject);
			}
		}

		public static void FindAndSetInNotNull<T>(this MonoBehaviour monoBehaviour, ref T component) where T : Behaviour {
			if (component) return;
			component = monoBehaviour.GetComponent<T>();
			if (component) return;
			component = monoBehaviour.FindInParents<T>();
			if (component) return;
			component = monoBehaviour.GetComponentInChildren<T>();
			if (component) return;
			component = Object.FindObjectOfType<T>();
		}

		public static IEnumerable<T> GetComponentsOfTypeInChildren<T>(this Transform transform) where T: Component {
			IEnumerable<Transform> children = transform.Cast<Transform>();
			foreach (var child in children) {
				var component = child.GetComponent<T>();
				if (component) yield return component;
				foreach (var inn in child.GetComponentsOfTypeInChildren<T>().ToArray()) {
					yield return inn;
				}
			}
		}

        public static void DestroyObject(this MonoBehaviour monoBehaviour)
        {
            GameObject gameObject = monoBehaviour.gameObject;
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

		public static T FindInParents<T>(this MonoBehaviour monoBehaviour) where T: Behaviour {
			Transform transform = monoBehaviour.transform.parent;
			T find = null;
			while (transform) {
				find = transform.GetComponent<T>();
				if (find) break;
				transform = transform.parent;
			}
			return find;
		}

		public static void ForAll<T>(this IEnumerable<T> array, Action<T> action) {
			foreach (var element in array) {
				action(element);
			}
		}

		public static void ForAll<T>(this IEnumerable<T> array, Action<T, int> action) {
			int i = 0;
			foreach (var element in array) {
				action(element, i);
				i++;
			}
		}
	}
}