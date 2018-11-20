using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class RangeExAttribute : PropertyAttribute
{
		public readonly float min;
		public readonly float max;
		public readonly int decimals;

		public RangeExAttribute(float min, float max,int decimals)
		{
				this.min = min;
				this.max = max;
				this.decimals = decimals;
		}
}