using UnityEngine;

namespace GOAP {
    public class G_ConditionBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        
        G_State state;
        G_StateComparison comparison = G_StateComparison.EqualTo;
        object expectedValue;
        Object expectedReference;
        bool met = false;
        bool useExpectedReference = false;


        public G_ConditionBuilder() {

        }
        #endregion

        #region With Functions
        public G_ConditionBuilder State(G_State state) {
            this.state = state;
            return this;
        }

        public G_ConditionBuilder IsEqualTo(object expectedValue) {
            this.comparison = G_StateComparison.EqualTo;
            this.expectedValue = expectedValue;
            return this;
        }

        public G_ConditionBuilder IsEqualTo(Object expectedReference) {
            this.comparison = G_StateComparison.EqualTo;
            this.expectedReference = expectedReference;
            useExpectedReference = true;
            return this;
        }

        public G_ConditionBuilder IsNotEqualTo(object expectedValue) {
            this.comparison = G_StateComparison.NotEqualTo;
            this.expectedValue = expectedValue;
            return this;
        }

        public G_ConditionBuilder IsGreaterThan(object expectedValue) {
            this.comparison = G_StateComparison.GreaterThan;
            this.expectedValue = expectedValue;
            return this;
        }


        public G_ConditionBuilder IsGreaterThanOrEqualTo(object expectedValue) {
            this.comparison = G_StateComparison.GreaterThanOrEqualTo;
            this.expectedValue = expectedValue;
            return this;
        }
        public G_ConditionBuilder IsLessThan(object expectedValue) {
            this.comparison = G_StateComparison.LessThan;
            this.expectedValue = expectedValue;
            return this;
        }

        public G_ConditionBuilder IsLessThanOrEqualTo(object expectedValue) {
            this.comparison = G_StateComparison.GreaterThanOrEqualTo;
            this.expectedValue = expectedValue;
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
            useExpectedReference = true;
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
