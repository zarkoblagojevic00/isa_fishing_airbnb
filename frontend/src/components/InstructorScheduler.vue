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
        events-on-month-view="short"
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
    async mounted() {
        await this.loadQuickActions();
        this.loadAvailabilityPeriods();
    },
    methods: {
        async onEventDragCreated(event) {
            if (this.isEventOverlappingWithAny(event)) {
                this.loadAvailabilityPeriods();
                //alert overlapping period
            } else {
                const eventType = await this.chooseEventType();
                console.log(eventType);
                this.addAvailabilityPeriod(event, eventType);
            }
        },
        async chooseEventType() {
            const { value: eventType } = await this.$swal.fire({
                title: "Select event type",
                input: "select",
                inputOptions: {
                    available: "Available",
                    unavailable: "Unavailable",
                },
                inputPlaceholder: "Select type",
                showCancelButton: true,
            });
            return eventType;
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
        async loadQuickActions() {
            await axios
                .get("/api/QuickAction/GetUsersQuickActions")
                .then(({ data }) => {
                    this.quickActions = data;
                    this.mapQuickActionsToEvents();
                });
        },
        addAvailabilityPeriod(event, eventType) {
            const period = {
                periodStart: new Date(event.start).toISOString(),
                periodEnd: new Date(event.end).toISOString(),
                status: eventType == "available" ? true : false,
            };
            axios
                .post("/api/Instructor/AddInstructorAvailabilityPeriod", period)
                .then(() => {
                    this.loadAvailabilityPeriods();
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
                    title: period.status ? "Available" : "Unavailable",
                    class: period.status ? "free-slot" : "unavailable-slot",
                };
            });
            this.events = this.events.concat(this.quickActions);
            console.log(this.quickActions);
        },
        mapQuickActionsToEvents() {
            this.quickActions = this.quickActions.map(function (action) {
                return {
                    start: new Date(action.startDateTime),
                    end: new Date(action.endDateTime),
                    title: "Quick action",
                    class: "quick-action",
                };
            });
        },
    },
    data() {
        return {
            periods: [],
            events: [],
            quickActions: [],
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

.unavailable-slot {
    background-color: rgba(207, 58, 58, 0.35);
}

.quick-action {
    background-color: rgba(68, 58, 207, 0.35);
}
</style>
