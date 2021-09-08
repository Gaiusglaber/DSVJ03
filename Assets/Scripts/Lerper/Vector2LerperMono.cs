using UnityEngine;

namespace Numetry.Tools.Lerper
{
    public class Vector2LerperMono : AbstractLerperMono<Vector2>
    {
        #region PRIVATE_FIELDS
        private Vector2Lerper lerper = null;
        #endregion

        #region PROPERTIES
        private Vector2Lerper Lerper
        {
            get
            {
                if (lerper == null)
                {
                    lerper = new Vector2Lerper(lerpTime, smoothType);
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
        public override Vector2 GetValue()
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

        public override void SetValues(Vector2 start, Vector2 end, float time, bool on = false)
        {
            Lerper.SetValues(start, end,time, on);
        }
        #endregion
    }
}