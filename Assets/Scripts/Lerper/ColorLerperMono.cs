using UnityEngine;

namespace Numetry.Tools.Lerper
{
    public class ColorLerperMono : AbstractLerperMono<Color>
    {
        #region PRIVATE_FIELDS
        private ColorLerper lerper = null;
        #endregion

        #region PROPERTIES
        private ColorLerper Lerper
        {
            get
            {
                if (lerper == null)
                {
                    lerper = new ColorLerper(lerpTime, smoothType);
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
        public override void SetValues(Color start, Color end, float time, bool on = false)
        {
            Lerper.SetValues(start, end, time, on);
        }

        public override Color GetValue()
        {
            return Lerper.CurrentValue;
        }

        public override bool IsOn()
        {
            return Lerper.On;
        }

        public override bool HasReached()
        {
            return Lerper.Reached;
        }
        #endregion
    }
}
