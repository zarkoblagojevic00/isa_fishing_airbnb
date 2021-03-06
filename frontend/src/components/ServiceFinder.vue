<template>
    <div class="wrapper shadow-item">
        <div class="search-title">{{ title }}</div>
        <form class="form-wrapper">
            <div v-if="reservation" class="step step-container">
                1. Enter reservation details
                <span class="step-explanation">required</span>
            </div>
            <div class="control-wrapper">
                <span class="input-label">Reservation time</span>
                <div v-if="adventure" class="adventure-time-picker">
                    <DateTimespanPicker v-model="fromToDate" />
                </div>

                <div v-else class="regular-date-picker">
                    <Datepicker
                        class="date-picker"
                        v-model="fromToDate"
                        range
                        twoCalendars
                        placeholder="Select a date range"
                        :partialRange="false"
                        :clearable="false"
                        :enableTimePicker="false"
                        :minDate="minDate"
                        :inputClassName="
                            isValidDatePicker ? '' : 'control-invalid'
                        "
                    />
                    <small
                        v-if="!isValidDatePicker"
                        class="control-invalid-hint"
                        >both dates must be picked</small
                    >
                </div>
            </div>

            <div class="control-wrapper capacity">
                <span class="input-label">Number of people</span>
                <NumInputRange
                    v-model="searchParams.capacity"
                    :min="1"
                    :max="100"
                    placeholder="Enter capacity"
                    required
                />
            </div>

            <div v-if="reservation" class="step">2. Narrow your search</div>

            <div class="control-wrapper">
                <span class="input-label step-container"
                    >Price range
                    <span class="step-explanation">$/day</span>
                </span>
                <div class="slider">
                    <Slider
                        v-model="fromToPrice"
                        :max="300"
                        :format="{ prefix: '$', decimals: 0 }"
                    />
                </div>
            </div>

            <div class="control-wrapper">
                <span class="input-label">Mark</span>
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

            <div class="control-wrapper">
                <span class="input-label step-container"
                    >Name
                    <span class="step-explanation">
                        you can leave this unspecified
                    </span>
                </span>

                <input
                    class="control transition-ease"
                    v-model="searchParams.name"
                    type="text"
                    placeholder="Enter a service name"
                />
            </div>

            <div class="control-wrapper">
                <span class="input-label step-container"
                    >Location
                    <span class="step-explanation">
                        you can leave this unspecified
                    </span>
                </span>

                <input
                    class="control transition-ease"
                    v-model="searchParams.location"
                    type="text"
                    placeholder="Enter a location"
                />
            </div>

            <div v-if="reservation" class="step step-container">
                3. Let's find you a deal
                <span class="step-explanation">press search</span>
            </div>

            <button
                class="search clickable primary transition-ease"
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
import moment from "moment";

import Slider from "@vueform/slider";
import NumInputRange from "../components/NumInputRange.vue";
import DateTimespanPicker from "../components/DateTimespanPicker.vue";

export default {
    name: "ServiceFinder",
    components: {
        Datepicker,
        Slider,
        NumInputRange,
        DateTimespanPicker,
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
        reservation: {
            type: Boolean,
            default: false,
        },
        adventure: {
            type: Boolean,
            default: false,
        },
    },
    data() {
        const fromDateString = localStorage.getItem("fromDate");
        const toDateString = localStorage.getItem("toDate");

        const fDate = fromDateString
            ? new Date(Date.parse(fromDateString))
            : moment().add(20, "days").toDate();
        const tDate = toDateString
            ? new Date(Date.parse(toDateString))
            : moment().add(25, "days").toDate();
        return {
            searchParams: {
                name: "",
                location: "",
                fromPrice: null,
                toPrice: null,
                givenMark: 0,
                capacity: 4,
            },
            fromToPrice: [10, 60],
            minDate: moment().add(1, "days").startOf("day").toDate(),
            fromToDate: [fDate, tDate],
        };
    },

    mounted() {
        this.onSearch();
    },

    methods: {
        async onSearch() {
            localStorage.setItem("fromDate", this.fromToDate[0]);
            localStorage.setItem("toDate", this.fromToDate[1]);
            if (!this.isValidDatePicker) {
                alert("Please insert all the required data in valid format!");
                return;
            }
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

    computed: {
        isValidDatePicker() {
            return this.fromToDate && this.fromToDate[1];
        },
    },
};
</script>

<style src="@vueform/slider/themes/default.css"></style>
<style scoped>
.wrapper {
    width: 100%;
    border-radius: 5px;
    padding: 0.8em;
    background: #fdfdfd;
}

.search-title {
    font-size: 1.4rem;
    text-align: left;
    margin-bottom: 1.3em;
    padding-bottom: 0.2em;
    border-bottom: 1px solid var(--control-border-color);
}

.form-wrapper {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: space-between;
}

.control-wrapper {
    margin-bottom: 0.9em;
}

.step-container {
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    box-sizing: border-box;
}

.step {
    font-size: 0.9rem;
    font-weight: 550;
    text-align: left;
    padding-bottom: 0.3em;
    border-bottom: 2px solid var(--primary);
    margin-bottom: 0.8em;
    margin-top: 0.5em;
}

.step-explanation {
    font-size: 0.75rem;
    color: #b1b1b1;
    margin-bottom: 1px;
}

.date-picker {
    width: 100%;
}

.time-picker {
    margin-top: 0.5em;
}

.slider {
    margin-top: 2.5em;
    min-width: 100%;
    --slider-connect-bg: var(--primary);
    --slider-tooltip-bg: var(--primary);
}

.search {
    margin-top: 20px;
}
</style>
