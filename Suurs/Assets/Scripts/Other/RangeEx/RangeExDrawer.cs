using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(RangeExAttribute))]
internal sealed class RangeExDrawer : PropertyDrawer
{
		private float value;

		//
		// Methods
		//
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
				var rangeAttribute = (RangeExAttribute)base.attribute;

				if (property.propertyType == SerializedPropertyType.Float)
				{
						value = EditorGUI.Slider(position, label, value, rangeAttribute.min, rangeAttribute.max);

						value = (float)Math.Round((decimal)value, rangeAttribute.decimals, MidpointRounding.AwayFromZero);
						property.floatValue = value;
				}
				else
				{
						EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
				}
		}
}