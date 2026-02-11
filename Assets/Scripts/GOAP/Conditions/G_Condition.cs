using UnityEngine;

namespace GOAP {
    public class G_Condition
    {
        #region Variables
        // the state being tested by the condition
        G_State _state;
        public G_State State {
            get { return _state; }
        }

        // the actual comparison for the condition
        G_StateComparison _comparison;
        public G_StateComparison Comparison {
            get { return _comparison; }
        }

        // the value we will be comparing to the current value in the state
        object _expectedValue;
        public object ExpectedValue {
            get { return _expectedValue; }
        }

        // has the condition been meet during planning?
        bool met = false;
        public bool Met {
            get { return met; }
        }
        #endregion
    }
}
