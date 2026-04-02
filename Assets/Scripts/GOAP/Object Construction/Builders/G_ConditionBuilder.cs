using UnityEngine;

namespace GOAP {
    public class G_ConditionBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        
        G_State state;
        G_StateComparison comparison = G_StateComparison.equal;
        object expectedValue;
        Object expectedReference;
        bool met = false;
        bool useExpectedReference = false;


        public G_ConditionBuilder() {

        }
        #endregion

        #region With Functions
        public G_ConditionBuilder WithState(G_State state) {
            this.state = state;
            return this;
        }
        public G_ConditionBuilder WithComparison(G_StateComparison comparison) {
            this.comparison = comparison;
            return this;
        }
        public G_ConditionBuilder WithExpectedValue(object expectedValue) {
            this.expectedValue = expectedValue;
            return this;
        }
        public G_ConditionBuilder WithExpectedReference(Object expectedReference) {
            this.expectedReference = expectedReference;
            this.useExpectedReference = true;
            return this;
        }

        public G_ConditionBuilder WithExpectedReference(Object expectedReference, bool useExpectedReference) {
            this.expectedReference = expectedReference;
            this.useExpectedReference = useExpectedReference;
            return this;
        }

        public G_ConditionBuilder IsMet(bool met) {
            this.met = met;
            return this;
        }
        #endregion

        #region Object Creation

        public G_Condition Build() {
            G_Condition condition = new G_Condition(state,
                expectedValue,
                expectedReference,
                useExpectedReference,
                comparison,
                met);
            return condition;
        }

        public static implicit operator G_Condition(G_ConditionBuilder builder) {
            return builder.Build();
        }

        #endregion
    }   
}
