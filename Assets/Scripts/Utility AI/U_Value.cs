using System.Collections.Generic;
using UnityEngine;
using GOAP;

namespace UtilityAI {
    [CreateAssetMenu(fileName = "Utility Value", menuName = "Scriptable Objects/Utility Value")]
    public class U_Value : ScriptableObject {
        #region Variables

        [SerializeField] AnimationCurve responseCurve;

        [SerializeField] U_ValueSource varSource;
        [SerializeField] float floatVar = 0f;
        [SerializeField] int intVar = 0;
        [SerializeField] G_FloatState floatStateVar;
        [SerializeField] G_IntState intStateVar;
        [SerializeField] U_Value utilityVar;

        [SerializeField] U_ValueSource maxSource;
        [SerializeField] float floatMax = 0f;
        [SerializeField] int intMax = 0;
        [SerializeField] G_FloatState floatStateMax;
        [SerializeField] G_IntState intStateMax;
        [SerializeField] U_Value utilityMax;

        #endregion

        #region Properties

        public U_ValueSource VarSource { get { return varSource; } }
        public U_ValueSource MaxSource { get { return maxSource; } }
        public G_FloatState FloatStateVar { get { return floatStateVar; } }
        public G_FloatState FloatStateMax { get { return floatStateMax; } }
        public G_IntState IntStateVar { get { return intStateVar; } }
        public G_IntState IntStateMax { get { return intStateMax; } }
        public U_Value UtilityVar { get { return utilityVar; } }
        public U_Value UtilityMax { get { return utilityMax; } }

        #endregion
    }
}

public enum U_ValueSource {
    Int,
    Float,
    IntState,
    FloatState,
    Utility
}
