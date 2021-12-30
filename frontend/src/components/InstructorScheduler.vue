<template>
    <div>Scheduler</div>
    <VueCal
        :disable-views="['years', 'year']"
        :snap-to-time="30"
        :dblclickToNavigate="false"
        :editableEvents="{
            title: true,
            drag: true,
            resize: true,
            delete: true,
            create: true,
        }"
        :timeFrom="480"
        :timeTo="960"
        :timeStep="30"
        :timeCellHeight="80"
        :events="events"
        v-model:events="periods"
        today-button
        @cell-click="cellClicked"
        @event-drag-create="eventDragCreated"
        @event-title-change="eventTitleChanged"
    />
</template>
<script>
import VueCal from "vue-cal";
import "vue-cal/dist/vuecal.css";
import axios from "../api/api.js";

export default {
    components: {
        VueCal,
    },
    mounted() {
        this.loadAvailabilityPeriods();
        console.log("init", this.eventsInitial);
    },
    methods: {
        cellClicked() {
            console.log(this.periods);
        },
        eventDragCreated(e) {
            this.addAvailabilityPeriod(e);
            console.log("drag", e);
        },
        eventTitleChanged(event, oldTitle) {
            console.log(event);
            console.log(oldTitle);
        },
        loadAvailabilityPeriods() {
            axios
                .get("/api/Instructor/GetAvailabilityPeriods")
                .then(({ data }) => {
                    this.periods = data;
                    this.mapPeriodsToEvents();
                    console.log(this.events);
                });
        },
        addAvailabilityPeriod(event) {
            const period = {
                periodStart: new Date(event.start),
                periodEnd: new Date(event.end),
                status: true,
            };
            axios
                .post("/api/Instructor/AddInstructorAvailabilityPeriod", period)
                .then(() => {
                    console.log(period);
                })
                .catch(function (error) {
                    if (error.response) {
                        // Request made and server responded
                        console.log(error.response.data);
                        console.log(error.response.status);
                        console.log(error.response.headers);
                    } else if (error.request) {
                        // The request was made but no response was received
                        console.log(error.request);
                    } else {
                        // Something happened in setting up the request that triggered an Error
                        console.log("Error", error.message);
                    }
                });
        },
        mapPeriodsToEvents() {
            this.events = this.periods.map(function (period) {
                return {
                    start: new Date(period.periodStart),
                    end: new Date(period.periodEnd),
                    title: "Available",
                    class: "free-slot",
                };
            });
        },
    },
    data() {
        return {
            periods: [],
            events: [],
        };
    },
};
</script>
<style>
.vuecal__event {
    background-color: rgba(58, 207, 78, 0.35);
}

.free-slot {
    background-color: rgba(58, 207, 78, 0.35);
}
</style>
