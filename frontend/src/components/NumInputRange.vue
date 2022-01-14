<template>
    <input
        class="control transition-ease"
        :class="{ 'control-invalid': isValid }"
        type="number"
        v-model.number.lazy="realNumber"
        :min="min"
        :max="max"
        :placeholder="placeholder"
    />
    <small v-if="isValid" class="control-invalid-hint"
        >number must be between {{ min }} and {{ max }}</small
    >
</template>

<script>
export default {
    props: {
        modelValue: {
            type: Number,
            required: true,
        },
        min: {
            type: Number,
            default: Number.NEGATIVE_INFINITY,
        },
        max: {
            type: Number,
            default: Number.POSITIVE_INFINITY,
        },
        placeholder: {
            type: String,
            default: "",
        },
        required: {
            type: Boolean,
            default: false,
        },
    },

    data() {
        return {
            realNumber: this.modelValue,
        };
    },

    computed: {
        isValid() {
            return this.required && !this.modelValue;
        },
    },

    watch: {
        realNumber: function () {
            this.realNumber =
                this.realNumber < this.min
                    ? this.min
                    : this.realNumber > this.max
                    ? this.max
                    : this.realNumber;

            this.$emit("update:modelValue", this.realNumber);
        },
    },
};
</script>

<style src="../styles/form.css"></style>
