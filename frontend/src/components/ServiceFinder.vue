<template>
    <div class="wrapper shadow-item">
        <div class="title">{{ title }}</div>
        <form class="form-wrapper">
            <div class="input-wrapper">
                <span class="label">Date range</span>
                <Datepicker
                    class="date-range"
                    v-model="fromToDate"
                    range
                    twoCalendars
                    placeholder="Select a date range"
                    :enableTimePicker="false"
                    :minDate="minDate"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Price range (per day)</span>
                <div class="slider">
                    <Slider
                        v-model="fromToPrice"
                        :max="300"
                        :format="{ prefix: '$', decimals: 0 }"
                        tooltipPosition="bottom"
                    />
                </div>
            </div>
            <div class="input-wrapper">
                <span class="label mark">Mark</span>
                <select
                    class="control transition-ease"
                    v-model="searchParams.givenMark"
                    name="mark"
                    id="mark"
                >
                    <option :value="0">All</option>
                    <option v-for="i in 5" :value="i" :key="i">{{ i }}</option>
                </select>
            </div>
            <div class="input-wrapper">
                <span class="label">Name</span>
                <input
                    class="control transition-ease"
                    v-model="searchParams.name"
                    type="text"
                    placeholder="Enter a service name"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Location</span>
                <input
                    class="control transition-ease"
                    v-model="searchParams.location"
                    type="text"
                    placeholder="Enter a location"
                />
            </div>
            <div class="input-wrapper capacity">
                <span class="label">Capacity</span>
                <NumInputRange
                    v-model="searchParams.capacity"
                    :min="0"
                    :max="100"
                    placeholder="Enter capacity"
                />
            </div>
            <button
                class="search clickable primary-outline transition-ease"
                type="submit"
                @click.prevent="onSearch"
            >
                <span class="icon-title-centered">
                    <font-awesome-icon
                        class="icon"
                        icon="search"
                    ></font-awesome-icon>
                    <span>Search</span>
                </span>
            </button>
        </form>
    </div>
</template>

<script>
import Datepicker from "vue3-date-time-picker";
import "vue3-date-time-picker/dist/main.css";
import Slider from "@vueform/slider";
import NumInputRange from "../components/NumInputRange.vue";
import moment from "moment";

export default {
    name: "ServiceFinder",
    components: {
        Datepicker,
        Slider,
        NumInputRange,
    },
    props: {
        title: {
            type: String,
            required: true,
        },
        search: {
            type: Function,
            required: true,
        },
    },
    data() {
        return {
            searchParams: {
                name: "",
                location: "",
                fromDate: null,
                toDate: null,
                fromPrice: null,
                toPrice: null,
                givenMark: 0,
                capacity: null,
            },
            fromToPrice: [10, 30],
            minDate: moment().add(1, "days").toDate(),
            fromToDate: [
                moment().add(1, "days").toDate(),
                moment().add(8, "days").toDate(),
            ],
        };
    },
    methods: {
        async onSearch() {
            this.searchParams = {
                ...this.searchParams,
                ...{
                    fromDate: this.fromToDate[0].toISOString(),
                    toDate: this.fromToDate[1].toISOString(),
                    fromPrice: this.fromToPrice[0],
                    toPrice: this.fromToPrice[1],
                    capacity: this.searchParams.capacity ?? 0,
                },
            };
            const result = await this.search(this.searchParams);
            this.$emit("filtered", result);
        },
    },
};
</script>

<style src="@vueform/slider/themes/default.css"></style>
<style src="../styles/form.css"></style>
<style scoped>
.wrapper {
    width: 100%;
    border-radius: 5px;
    padding: 0.8em;
    background: #fdfdfd;
}

.title {
    font-size: 1.2rem;
    text-align: left;
    margin-bottom: 0.2em;
    padding-bottom: 0.2em;
    border-bottom: 1px solid var(--control-border-color);
}

.form-wrapper {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: space-between;
}

.mark {
    margin-top: 0.2em;
}

.slider {
    margin-top: 0.8em;
    min-height: 50px;
    min-width: 200px;
    --slider-connect-bg: var(--primary);
    --slider-tooltip-bg: var(--primary);
}

.search {
    margin-top: 25px;
}
</style>
