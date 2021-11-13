using System;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace PlagueButtonAPI.Misc
{
    public static class Extensions
	{
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Behaviour
        {
            var comp = obj.GetComponent<T>();

            if (comp == null)
            {
                comp = obj.AddComponent<T>();
            }

            return comp;
        }

        public static T GetOrAddComponent<T>(this Transform obj) where T : Behaviour
        {
            var comp = obj.GetComponent<T>();

            if (comp == null)
            {
                comp = obj.gameObject.AddComponent<T>();
            }

            return comp;
        }

        public static GameObject FindObject(this GameObject parent, string name)
		{
			Transform[] array = parent.GetComponentsInChildren<Transform>(true);
			foreach (Transform transform in array)
			{
				if (transform.name == name)
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		public static string GetPath(this GameObject gameObject)
		{
			string text = "/" + gameObject.name;
			while (gameObject.transform.parent != null)
			{
				gameObject = gameObject.transform.parent.gameObject;
				text = "/" + gameObject.name + text;
			}
			return text;
		}

		public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
		{
			for (int num = transform.childCount - 1; num >= 0; num--)
			{
				if (exclude == null || exclude(transform.GetChild(num)))
				{
					UnityEngine.Object.DestroyImmediate(transform.GetChild(num).gameObject);
				}
			}
		}

		public static void DestroyChildren(this Transform transform)
		{
			transform.DestroyChildren(null);
		}

		public static Vector3 SetX(this Vector3 vector, float x)
		{
			return new Vector3(x, vector.y, vector.z);
		}

		public static Vector3 SetY(this Vector3 vector, float y)
		{
			return new Vector3(vector.x, y, vector.z);
		}

		public static Vector3 SetZ(this Vector3 vector, float z)
		{
			return new Vector3(vector.x, vector.y, z);
		}

		public static float RoundAmount(this float i, float nearestFactor)
		{
			return (float)Math.Round(i / nearestFactor) * nearestFactor;
		}

		public static Vector3 RoundAmount(this Vector3 i, float nearestFactor)
		{
			return new Vector3(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor), i.z.RoundAmount(nearestFactor));
		}

		public static Vector2 RoundAmount(this Vector2 i, float nearestFactor)
		{
			return new Vector2(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor));
		}

		public static Sprite ToSprite(this Texture2D tex)
		{
			Rect rect = new Rect(0f, 0f, tex.width, tex.height);
			Vector2 pivot = new Vector2(0.5f, 0.5f);
			Vector4 border = Vector4.zero;
			Sprite sprite = Sprite.CreateSprite_Injected(tex, ref rect, ref pivot, 50f, 0u, SpriteMeshType.FullRect, ref border, false);
			sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			return sprite;
		}

		public static string ReplaceFirst(this string text, string search, string replace)
		{
			int num = text.IndexOf(search);
			if (num < 0)
			{
				return text;
			}
			return text.Substring(0, num) + replace + text.Substring(num + search.Length);
		}

		public static ColorBlock SetColor(this ColorBlock block, Color color)
		{
			ColorBlock result = default(ColorBlock);
			result.colorMultiplier = block.colorMultiplier;
			result.disabledColor = Color.grey;
			result.highlightedColor = color;
			result.normalColor = color / 1.5f;
			result.pressedColor = Color.white;
			result.selectedColor = color / 1.5f;
			return result;
		}

		public static void DelegateSafeInvoke(this Delegate @delegate, params object[] args)
		{
			Delegate[] invocationList = @delegate.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				try
				{
					invocationList[i].DynamicInvoke(args);
				}
				catch (Exception ex)
				{
					MelonLogger.Error("Error while executing delegate:\n" + ex.ToString());
				}
			}
		}

		public static string ToEasyString(this TimeSpan timeSpan)
		{
			if (Mathf.FloorToInt(timeSpan.Ticks / 864000000000L) > 0)
			{
				return $"{timeSpan:%d} days";
			}
			if (Mathf.FloorToInt(timeSpan.Ticks / 36000000000L) > 0)
			{
				return $"{timeSpan:%h} hours";
			}
			if (Mathf.FloorToInt(timeSpan.Ticks / 600000000) > 0)
			{
				return $"{timeSpan:%m} minutes";
			}
			return $"{timeSpan:%s} seconds";
		}

		public static void ShowAlert(this VRC.UI.Elements.QuickMenu qm, string message)
		{
			qm.Method_Public_Virtual_Final_New_Void_String_4(message);
		}

		public static void ShowOKDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action okButton = null)
		{
			qm.Method_Public_Void_String_String_Action_PDM_0(title, message, okButton);
		}

		public static void ShowConfirmDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action yesButton = null, Action noButton = null)
		{
			qm.Method_Public_Void_String_String_Action_Action_PDM_0(title, message, yesButton, noButton);
		}

		public static void AskConfirmOpenURL(this VRC.UI.Elements.QuickMenu qm, string url)
		{
			qm.Method_Public_Virtual_Final_New_Void_String_3(url);
		}
	}
}
