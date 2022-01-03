<template>
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
        @event-drag-create="onEventDragCreated"
        @event-delete="onEventDeleted"
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
    },
    methods: {
        onEventDragCreated(event) {
            if (this.isEventOverlappingWithAny(event)) {
                this.loadAvailabilityPeriods();
                //alert overlapping period
            } else {
                this.addAvailabilityPeriod(event);
                this.loadAvailabilityPeriods();
            }
        },
        onEventDeleted(event) {
            this.deleteAvailabilityPeriod(event);
        },
        datesOverlap(first, second) {
            if (first.start < second.end && first.end > second.start) {
                return true;
            }
            return false;
        },
        isEventOverlappingWithAny(event) {
            for (const period of this.events) {
                if (this.datesOverlap(event, period)) {
                    console.log("true");
                    return true;
                } else {
                    console.log("false");
                }
            }
            return false;
        },
        loadAvailabilityPeriods() {
            axios
                .get("/api/Instructor/GetAvailabilityPeriods")
                .then(({ data }) => {
                    this.periods = data;
                    this.mapPeriodsToEvents();
                });
        },
        addAvailabilityPeriod(event) {
            const period = {
                periodStart: new Date(event.start).toISOString(),
                periodEnd: new Date(event.end).toISOString(),
                status: true,
            };
            axios
                .post("/api/Instructor/AddInstructorAvailabilityPeriod", period)
                .then(() => {
                    console.log(period);
                });
        },
        deleteAvailabilityPeriod(event) {
            const period = {
                periodStart: new Date(event.start),
                periodEnd: new Date(event.end),
                status: undefined,
                userId: undefined,
            };
            axios
                .delete("/api/Instructor/DeleteInstructorAvailabilityPeriod", {
                    data: period,
                })
                .then(() => {
                    this.loadAvailabilityPeriods();
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
