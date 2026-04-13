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

        #region Calculate Utility
        public float GetUtility() {
            float value = 0f;
            float variable = ReturnSourceValue(varSource, true);
            float maximum = ReturnSourceValue(maxSource, false);
            float percentage = 0f;

            if (variable == 0f && maximum == 0f) {
                percentage = 0f;
            }
            else {
                percentage = variable / maximum;
            }

            float result = responseCurve.Evaluate(percentage);

            return result;
        }

        float ReturnSourceValue(U_ValueSource sourceType, bool isVar) {
            float value = 0f;

            switch (sourceType) {
                case U_ValueSource.Int:
                    value = isVar ? intVar : intMax;
                    break;
                case U_ValueSource.Float:
                    value = isVar ? floatVar : floatMax;
                    break;
                case U_ValueSource.IntState:
                    value = isVar ? (float)intStateVar.GetValue() : (float)intStateMax.GetValue();
                    break;
                case U_ValueSource.FloatState:
                    value = isVar ? (float)floatStateVar.GetValue() : (float)floatStateMax.GetValue();
                    break;
                case U_ValueSource.Utility:
                    value = isVar ? utilityVar.GetUtility() : utilityMax.GetUtility();
                    break;
            }

            return value;
        }

        #endregion

        #region Set Functions

        public void SetValue(bool isVar, float value) {
            if (isVar) {
                floatVar = value;
            }
            else {
                floatMax = value;
            }
        }

        public void SetValue(bool isVar, int value) {
            if (isVar) {
                intVar = value;
            }
            else {
                intMax = value;
            }
        }

        public void AssignReference(bool isVar, G_IntState value) {
            if (isVar) {
                intStateVar = value;
            }
            else {
                intStateMax = value;
            }
        }

        public void AssignReference(bool isVar, G_FloatState value) {
            if (isVar) {
                floatStateVar = value;
            }
            else {
                floatStateMax = value;
            }
        }

        public void AssignReference(bool isVar, U_Value value) {
            if (isVar) {
                utilityVar = value;
            }
            else {
                utilityMax = value;
            }
        }

        #endregion

        public float Get(bool isVar) {
            U_ValueSource source = isVar ? varSource : maxSource;
            return ReturnSourceValue(source, isVar);
        }

        #region Local State Assignment

        public void AssignLocalValues(List<U_Value> localU_Values, List<G_State> localStates) {
            switch (varSource) {
                case U_ValueSource.IntState:
                    FindAndSetLocalValue(localStates, intStateVar, true);
                    break;
                case U_ValueSource.FloatState:
                    FindAndSetLocalValue(localStates, floatStateVar, true);
                    break;
                case U_ValueSource.Utility:
                    FindAndSetLocalValue(localU_Values, utilityVar, true);
                    break;
            }

            switch (maxSource) {
                case U_ValueSource.IntState:
                    FindAndSetLocalValue(localStates, intStateMax, false);
                    break;
                case U_ValueSource.FloatState:
                    FindAndSetLocalValue(localStates, floatStateMax, false);
                    break;
                case U_ValueSource.Utility:
                    FindAndSetLocalValue(localU_Values, utilityMax, false);
                    break;
            }
        }

        void FindAndSetLocalValue(List<G_State> localStates, G_State refState, bool isVar) {
            G_State foundState = localStates.Find((state) => state != null && state.name == refState.name);

            if (foundState != null) {
                if (refState is G_IntState) {
                    AssignReference(isVar, foundState as G_IntState);
                }
                else if (refState is G_FloatState) {
                    AssignReference(isVar, foundState as G_FloatState);
                }
            }
        }

        void FindAndSetLocalValue(List<U_Value> localU_Values, U_Value refValue, bool isVar) {
            U_Value foundValue = localU_Values.Find((value) => value != null && value.name == refValue.name);

            if (foundValue != null) {
                AssignReference(isVar, foundValue);
            }
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

