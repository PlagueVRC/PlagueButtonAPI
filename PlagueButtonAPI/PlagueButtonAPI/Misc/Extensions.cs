using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;

namespace PlagueButtonAPI.Misc
{
    public static class Extensions
	{
        private static MethodInfo ourShowAlertMethod;

        private static MethodInfo ourShowOKDialogMethod;

        private static MethodInfo ourShowConfirmDialogMethod;

        private static MethodInfo ourAskConfirmOpenURLMethod;

        public static MethodInfo ShowOKDialogMethod
        {
            get
            {
                if (ourShowOKDialogMethod != null)
                {
                    return ourShowOKDialogMethod;
                }

                ourShowOKDialogMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods().First(it => it.GetParameters().Length == 3 && it.Name.Contains("_PDM") && it.GetParameters().First().ParameterType == typeof(string) && it.GetParameters().Last().ParameterType == typeof(Il2CppSystem.Action) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "ConfirmDialog"));

                return ourShowOKDialogMethod;
            }
        }

        public static MethodInfo ShowConfirmDialogMethod
        {
            get
            {
                if (ourShowConfirmDialogMethod != null)
                {
                    return ourShowConfirmDialogMethod;
                }

                ourShowConfirmDialogMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods().First(it => it.GetParameters().Length == 4 && it.Name.Contains("_PDM") && it.GetParameters()[0].ParameterType == typeof(string) && it.GetParameters()[1].ParameterType == typeof(string) && it.GetParameters()[2].ParameterType == typeof(Il2CppSystem.Action) && it.GetParameters()[3].ParameterType == typeof(Il2CppSystem.Action) && XrefScanner.UsedBy(it).ToList().Count > 30);

                return ourShowConfirmDialogMethod;
            }
        }

        public static MethodInfo AskConfirmOpenURLMethod
        {
            get
            {
                if (ourAskConfirmOpenURLMethod != null)
                {
                    return ourAskConfirmOpenURLMethod;
                }

                ourAskConfirmOpenURLMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods().First(it => it.GetParameters().Length == 1 && it.Name.Contains("_Virtual_Final_New") && it.GetParameters().First().ParameterType == typeof(string) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject() != null && jt.ReadAsObject().ToString().Contains("This link will open in your web browser.")));

                return ourAskConfirmOpenURLMethod;
            }
        }

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
			foreach (var transform in array)
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
			var text = "/" + gameObject.name;
			while (gameObject.transform.parent != null)
			{
				gameObject = gameObject.transform.parent.gameObject;
				text = "/" + gameObject.name + text;
			}
			return text;
		}

		public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
		{
			for (var num = transform.childCount - 1; num >= 0; num--)
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
			var rect = new Rect(0f, 0f, tex.width, tex.height);
			var pivot = new Vector2(0.5f, 0.5f);
			var border = Vector4.zero;
			var sprite = Sprite.CreateSprite_Injected(tex, ref rect, ref pivot, 50f, 0u, SpriteMeshType.FullRect, ref border, false);
			sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			return sprite;
		}

		public static string ReplaceFirst(this string text, string search, string replace)
		{
			var num = text.IndexOf(search);
			if (num < 0)
			{
				return text;
			}
			return text.Substring(0, num) + replace + text.Substring(num + search.Length);
		}

		public static ColorBlock SetColor(this ColorBlock block, Color color)
		{
			var result = default(ColorBlock);
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
			var invocationList = @delegate.GetInvocationList();
			for (var i = 0; i < invocationList.Length; i++)
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
            if (ourShowAlertMethod == null)
            {
                var methods = typeof(ModalAlert).GetMethods();

                foreach (var methodInfo in methods)
                {
                    if (methodInfo.Name.Contains("Method_Private_Void_") && !methodInfo.Name.Contains("PDM") && methodInfo.GetParameters().Length == 0)
                    {
                        qm.field_Private_ModalAlert_0.field_Private_String_0 = message;

                        methodInfo.Invoke(qm.field_Private_ModalAlert_0, null);

                        if (qm.transform.Find("Container/Window/QMParent/Modal_Alert/Background_Alert").gameObject.activeSelf)
                        {
                            ourShowAlertMethod = methodInfo;
                            break;
                        }
                    }
                }
            }
            else
            {
                qm.field_Private_ModalAlert_0.field_Private_String_0 = message;
                ourShowAlertMethod.Invoke(qm.field_Private_ModalAlert_0, null);
            }
        }

		public static void ShowOKDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action okButton = null)
		{
            ShowOKDialogMethod.Invoke(qm, new object[]
            {
                title,
                message,
                DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(okButton)
            });
        }

		public static void ShowConfirmDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action yesButton = null, Action noButton = null)
		{
            ShowConfirmDialogMethod.Invoke(qm, new object[]
            {
                title,
                message,
                DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(yesButton),
                DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(noButton)
            });
        }

		public static void AskConfirmOpenURL(this VRC.UI.Elements.QuickMenu qm, string url)
		{
            AskConfirmOpenURLMethod.Invoke(qm, new object[] { url });
        }
	}
}
