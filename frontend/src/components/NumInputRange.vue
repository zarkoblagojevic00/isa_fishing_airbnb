<template>
    <input
        class="control transition-ease"
        type="number"
        v-model.number.lazy="realNumber"
        :min="min"
        :max="max"
        :placeholder="placeholder"
    />
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
    },

    data() {
        return {
            realNumber: this.modelValue,
        };
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
