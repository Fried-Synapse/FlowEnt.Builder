using FriedSynapse.FlowEnt.Motions.Tween.Abstract;
using UnityEngine;

namespace FriedSynapse.FlowEnt.Motions.Tween.Lights
{
    /// <summary>
    /// Lerps the <see cref="Light.range" /> value.
    /// </summary>
    public class RangeMotion : AbstractFloatMotion<Light>
    {
        public RangeMotion(Light item, float value) : base(item, value)
        {
        }

        public RangeMotion(Light item, float? from, float to) : base(item, from, to)
        {
        }

        protected override float GetFrom() => item.range;
        protected override void SetValue(float value) => item.range = value;
    }
}