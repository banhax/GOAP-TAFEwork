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

        #region Construction Functions

        public void Construct(
            string name,
            U_ValueSource varSource,
            U_ValueSource maxSource,
            AnimationCurve responseCurve,
            int intVar = 0,
            float floatVar = 0f,
            G_IntState intStateVar = null,
            G_FloatState floatStateVar = null,
            U_Value utilityVar = null,
            int intMax = 0,
            float floatMax = 0f,
            G_IntState intStateMax = null,
            G_FloatState floatStateMax = null,
            U_Value utilityMax = null
            ) {

            this.name = name;
            this.responseCurve = responseCurve;
            this.varSource = varSource;

            this.intVar = intVar;
            this.floatVar = floatVar;
            this.intStateVar = intStateVar;
            this.floatStateVar = floatStateVar;
            this.utilityVar = utilityVar;

            this.maxSource = maxSource;
            this.intMax = intMax;
            this.floatMax = floatMax;
            this.intStateMax = intStateMax;
            this.floatStateMax = floatStateMax;
            this.utilityMax = utilityMax;
        }

        public U_Value Clone() {
            U_Value clone = CreateInstance<U_Value>();
            
            // can use semi colons when using big constructors to pick specific arguments
            // (as long as the others are set to some value in the constructor originally!)

            //clone.Construct(
            //    name,
            //    varSource,
            //    maxSource,
            //    responseCurve,
            //    floatVar: this.floatVar,
            //    floatMax: this.floatMax,
            //    utilityVar: this.utilityVar);

            clone.Construct(
                name,
                varSource,
                maxSource,
                responseCurve,
                intVar,
                floatVar,
                intStateVar,
                floatStateVar,
                utilityVar,
                intMax,
                floatMax,
                intStateMax,
                floatStateMax,
                utilityMax);

            return clone;
        }

        #endregion
    }

    public enum U_ValueSource {
        Int,
        Float,
        IntState,
        FloatState,
        Utility
    }
}

