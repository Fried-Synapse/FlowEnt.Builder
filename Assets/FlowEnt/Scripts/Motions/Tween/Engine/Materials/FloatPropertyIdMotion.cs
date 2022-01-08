using FriedSynapse.FlowEnt.Motions.Tween.Abstract;
using UnityEngine;

namespace FriedSynapse.FlowEnt.Motions.Tween.Materials
{
    /// <summary>
    /// Lerps the value for the specified shader property.
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    public class FloatPropertyIdMotion<TMaterial> : AbstractFloatMotion<TMaterial>
        where TMaterial : Material
    {
        public FloatPropertyIdMotion(TMaterial item, int propertyId, float value) : base(item, value)
        {
            this.propertyId = propertyId;
        }

        public FloatPropertyIdMotion(TMaterial item, int propertyId, float? from, float to) : base(item, from, to)
        {
            this.propertyId = propertyId;
        }

        private readonly int propertyId;
        protected override float GetFrom() => item.GetFloat(propertyId);
        protected override void SetValue(float value) => item.SetFloat(propertyId, value);
    }
}