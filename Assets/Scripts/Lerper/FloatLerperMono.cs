using UnityEngine;

namespace Numetry.Tools.Lerper
{
    public class FloatLerperMono : AbstractLerperMono<float>
    {
        #region PRIVATE_FIELDS
        private FloatLerper lerper = null;
        #endregion

        #region PROPERTIES
        private FloatLerper Lerper
        {
            get
            {
                if (lerper == null)
                {
                    lerper = new FloatLerper(lerpTime, smoothType);
                }

                return lerper;
            }
        }
        #endregion

        #region UNITY_CALLS
        protected override void Update()
        {
            Lerper.Update();
        }
        #endregion

        #region OVERRIDE
        public override float GetValue()
        {
            return Lerper.CurrentValue;
        }

        public override bool HasReached()
        {
            return Lerper.Reached;
        }

        public override bool IsOn()
        {
            return Lerper.On;
        }

        public override void SetValues(float start, float end, float time, bool on = false)
        {
            Lerper.SetValues(start,end,time,on);
        }
        #endregion

    }
}
