using UnityEngine;

namespace Numetry.Tools.Lerper
{
    public class Vector3LerperMono : AbstractLerperMono<Vector3>
    {
        #region PRIVATE_FIELDS
        private Vector3Lerper lerper = null;
        #endregion

        #region PROPERTIES
        public Vector3Lerper Lerper
        {
            get
            {
                if (lerper == null)
                {
                    lerper = new Vector3Lerper(lerpTime, smoothType);
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
        public override Vector3 GetValue()
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

        public override void SetValues(Vector3 start, Vector3 end, float time, bool on = false)
        {
            Lerper.SetValues(start, end,time, on);
        }
        #endregion
    }
}