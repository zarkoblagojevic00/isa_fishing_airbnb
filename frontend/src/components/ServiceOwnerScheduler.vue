<template>
    <div class="flexbox">
        <VueCal
            class="instructor-calendar"
            :disable-views="['days', 'day', 'years']"
            :snap-to-time="30"
            :dblclickToNavigate="false"
            :editableEvents="{
                title: false,
                drag: false,
                resize: false,
                delete: false,
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
        await this.loadReservations();
    },
    methods: {
        async loadReservations() {
            let response = await axios.get(
                "/api/GeneralService/GetReservationsWithBasicUserInfo"
            );
            this.reservations = response.data;
            this.mapReservationsToEvents();
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
    },
    data() {
        return {
            events: [],
            reservations: "",
        };
    },
};
</script>
<style>
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

.instructor-calendar {
    margin-top: 10px;
}
</style>
