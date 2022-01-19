<template>
    <div class="flexbox">
        <h3>Set periods when you are unavailable to teach!</h3>
        <div class="row">
            <div class="from">From</div>
            <v-date-picker
                v-model="unavailabilityStart"
                mode="dateTime"
                is24hr
                :min-date="new Date()"
            >
                <template v-slot="{ inputValue, inputEvents }">
                    <input
                        class="
                            px-2
                            py-1
                            border
                            rounded
                            focus:outline-none focus:border-blue-300
                        "
                        :value="inputValue"
                        v-on="inputEvents"
                    />
                </template>
            </v-date-picker>
            <div class="to">To</div>
            <v-date-picker
                v-model="unavailabilityEnd"
                mode="dateTime"
                is24hr
                :min-date="
                    unavailabilityStart ? unavailabilityStart : new Date()
                "
            >
                <template v-slot="{ inputValue, inputEvents }">
                    <input
                        class="
                            px-2
                            py-1
                            border
                            rounded
                            focus:outline-none focus:border-blue-300
                        "
                        :value="inputValue"
                        v-on="inputEvents"
                    />
                </template>
            </v-date-picker>
            <div class="button-submit" @click="submitUnavailability">
                Submit
            </div>
        </div>

        <VueCal
            class="instructor-calendar"
            :disable-views="['days', 'day', 'years']"
            :snap-to-time="30"
            :dblclickToNavigate="false"
            :editableEvents="{
                title: false,
                drag: false,
                resize: false,
                delete: true,
                create: false,
            }"
            :timeFrom="0"
            :timeTo="60 * 24"
            :timeStep="30"
            :timeCellHeight="80"
            :events="events"
            :min-date="new Date()"
            events-on-month-view="short"
            today-button
            @event-delete="onEventDeleted"
            style="height: 100%; width: 100%"
        />
    </div>
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
        await this.loadAvailabilityPeriods();
        await this.loadReservations();
        console.log(this.events);
    },
    methods: {
        onEventDeleted(event) {
            this.deleteAvailabilityPeriod(event);
        },
        async loadAvailabilityPeriods() {
            let response = await axios.get(
                "/api/Instructor/GetAvailabilityPeriods"
            );
            this.periods = response.data;
            this.mapPeriodsToEvents();
        },
        async loadReservations() {
            let response = await axios.get(
                "/api/Instructor/GetReservationsWithBasicUserInfo"
            );
            this.reservations = response.data;
            this.mapReservationsToEvents();
        },
        async loadQuickActions() {
            let response = await axios.get(
                "/api/QuickAction/GetUsersQuickActions"
            );
            this.quickActions = response.data;
            this.mapQuickActionsToEvents();
            // .then(({ data }) => {
            //     this.quickActions = data;
            //     this.mapQuickActionsToEvents();
            // });
        },
        addAvailabilityPeriod() {
            const period = {
                periodStart: new Date(this.unavailabilityStart).toISOString(),
                periodEnd: new Date(this.unavailabilityEnd).toISOString(),
                status: false,
            };
            axios
                .post("/api/Instructor/AddInstructorAvailabilityPeriod", period)
                .then(async () => {
                    await this.loadQuickActions();
                    await this.loadAvailabilityPeriods();
                    await this.loadReservations();
                })
                .catch(async (error) => {
                    if (error.response) {
                        this.$swal.fire(error.response.data);
                        // Request made and server responded
                        await this.loadQuickActions();
                        await this.loadAvailabilityPeriods();
                        await this.loadReservations();
                    }
                });
        },
        async deleteAvailabilityPeriod(event) {
            if (event.class != "unavailable-slots") {
                this.$swal.fire("Cannot delete reservation or quick action.");
                this.events = [];
                await this.loadQuickActions();
                await this.loadAvailabilityPeriods();
                await this.loadReservations();
                return;
            }
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
                    title: "Unavailable",
                    class: "unavailable-slots",
                };
            });
            this.events = this.events.concat(this.quickActions);
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
        mapReservationsToEvents() {
            this.reservations = this.reservations.map(function (reservation) {
                return {
                    start: new Date(reservation.serviceFrom),
                    end: new Date(reservation.serviceTo),
                    title: "Reservation",
                    class: "reservation",
                    content:
                        reservation.usersName +
                        " " +
                        reservation.usersSurname +
                        "\n" +
                        reservation.usersEmail,
                };
            });
            this.events = this.events.concat(this.reservations);
        },
        submitUnavailability() {
            if (this.unavailabilityStart && this.unavailabilityEnd)
                this.addAvailabilityPeriod();
            else {
                this.$swal.fire("Please fill in the required fields.");
            }
        },
    },
    data() {
        return {
            periods: [],
            events: [],
            quickActions: [],
            unavailabilityStart: "",
            unavailabilityEnd: "",
            reservations: "",
        };
    },
};
</script>
<style>
.vuecal__event {
    background-color: rgba(207, 58, 58, 0.35);
}

.unavailable-slots {
    background-color: rgba(207, 58, 58, 0.35);
}

.quick-action {
    background-color: rgba(68, 58, 207, 0.35);
}

.reservation {
    background-color: rgba(255, 243, 139, 0.35);
}

.flexbox {
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: flex-start;
    width: 100%;
}

.row {
    display: flex;
    justify-content: flex-start;
}

.from {
    padding-right: 30px;
}

.to {
    padding-right: 30px;
    padding-left: 50px;
}

.instructor-calendar {
    margin-top: 10px;
}

.button-submit {
    background-color: #95e1ff;
    border-radius: 5px;
    color: rgb(0, 0, 0);
    cursor: pointer;
    font-weight: bold;
    border: 0;
    font-size: 12px;
    width: 70px;
    margin-left: 50px;
    padding: 5px;
}
</style>
