<template>
    <div class="sorters-back-fall">
        <div class="sorters-wrapper">
            <div class="sort-by">Sort by</div>
            <button
                class="sorter underline-container transition-ease"
                :class="{ 'active-sorter': isActive(index) }"
                v-for="(value, key, index) in sortBy"
                :key="key"
                @click="activate(index)"
            >
                {{ value }}
                <div v-if="isActive(index)" class="sorter-icon">
                    <font-awesome-icon
                        v-if="sorters[index].order === -1"
                        icon="sort-up"
                    />
                    <font-awesome-icon v-else icon="sort-down" />
                </div>
                <div v-else class="ghost-padding"></div>
            </button>
        </div>
        <div class="sorter-right-end">
            <slot></slot>
        </div>
    </div>
</template>

<script>
import { parseProperty } from "../utils/property-parser.js";

export default {
    emits: ["sorted"],
    props: {
        items: {
            type: Array,
            required: true,
        },
        // object {fieldName: "displayName"} e.g {firstName: "First name"}
        sortBy: {
            type: Object,
            required: true,
        },
    },

    data() {
        return {
            sorters: Object.keys(this.sortBy).map((field) => ({
                id: field,
                order: -1,
                getSortFunction() {
                    return (a, b) =>
                        parseProperty(a, field) < parseProperty(b, field)
                            ? this.order
                            : -this.order;
                },
            })),
            activeSorterIndex: -1,
        };
    },

    methods: {
        activate(index) {
            if (this.activeSorterIndex !== index) {
                this.activeSorterIndex = index;
            } else {
                this.sorters[index].order *= -1;
            }
            this.apply();
        },

        apply() {
            const activeSorter = this.sorters[this.activeSorterIndex];
            const copyItems = [...this.items];
            const result = !activeSorter
                ? copyItems
                : copyItems.sort(activeSorter.getSortFunction());
            this.$emit("sorted", result);
        },

        isActive(index) {
            return this.activeSorterIndex === index;
        },
    },
};
</script>

<style scoped>
.sorters-back-fall {
    height: 4.7em;
    position: sticky;
    top: 0;
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    margin-bottom: 2em;
    background: #fff;
    border-bottom: 1px solid var(--control-border-color);
}

.sorters-wrapper {
    box-sizing: border-box;
    height: 2.85rem;
    width: 100%;
    display: flex;
    justify-content: flex-begin;
}

.sort-by {
    align-self: center;
    font-size: 1.4em;
    margin-right: 2em;
    margin-left: 1em;
}

.sorter {
    min-width: 10%;
    align-self: stretch;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border: none;
    font-size: 1rem;
    padding: 0.3em 0.3em 0.3em 1em;
    background: transparent;
    border-right: 1px solid var(--control-border-color);
    cursor: pointer;
}

.underline-container {
    background-image: -webkit-linear-gradient(var(--primary), var(--primary)),
        -webkit-linear-gradient(transparent, transparent);
    background-image: -moz-linear-gradient(var(--primary), var(--primary)),
        -moz-linear-gradient(transparent, transparent);
    background-image: -ms-linear-gradient(var(--primary), var(--primary)),
        -ms-linear-gradient(transparent, transparent);
    background-image: -o-linear-gradient(var(--primary), var(--primary)),
        -o-linear-gradient(transparent, transparent);
    background-image: linear-gradient(var(--primary), var(--primary)),
        linear-gradient(transparent, transparent);

    background-size: 0 5px, auto;
    background-repeat: no-repeat;
    background-position: center bottom;
}

.underline-container:hover {
    background-size: 100% 5px, auto;
}

.sorter-icon {
    min-width: 1em;
    margin-left: 1em;
}

.active-sorter {
    background-size: 100% 5px, auto;
    background-color: rgba(var(--primary-transparency), 0.3);
}

.ghost-padding {
    min-width: 1em;
    margin-left: 1em;
}

.sorter-right-end {
    margin-right: 0.5em;
    margin-bottom: 0.5em;
}
</style>
