<template>
    <div class="container">
        <h1>Booked reservations</h1>
        <div>
            <div v-if="reservations.length">
                <Sorters
                    :items="reservations"
                    :sortBy="sortBy"
                    @sorted="onSorted"
                >
                </Sorters>

                <ReservationExpoItem
                    v-for="(reservationService, idx) in reservations"
                    :key="idx"
                    :reservation="reservationService.reservation"
                    :service="reservationService.service"
                    booked
                ></ReservationExpoItem>
            </div>
        </div>
    </div>
</template>

<script>
import generalService from "../services/general-service.js";
import Sorters from "./Sorters.vue";
import ReservationExpoItem from "../components/ReservationExpoItem.vue";
import roleValidatorMixin from "../mixins/role-validator.js";
import { getId } from "../utils/local-storage-util.js";

export default {
    components: {
        ReservationExpoItem,
        Sorters,
    },
    mixins: [roleValidatorMixin],

    data() {
        return {
            reservations: [],
            sortBy: {
                "service.cityName": "Location",
                "service.name": "Service name",
                "service.averageMark": "Mark",
                "reservation.price": "Price",
                // "reservation.StartDateTime": "Date",
            },
        };
    },

    async created() {
        this.reservations = await generalService.getBookedClientReservations(
            getId()
        );
    },

    methods: {
        onSorted(value) {
            this.reservations = value;
        },
    },
};
</script>

<style>
h1 {
    text-align: left;
}
</style>
