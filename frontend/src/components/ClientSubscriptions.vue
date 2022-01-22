<template>
    <div class="container">
        <h1>Reservations history</h1>
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
                    history
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
        this.reservations = await generalService.getSubscriptions();
    },

    methods: {
        onSorted(value) {
            this.reservations = value;
        },
    },
};
</script>

<style scoped>
h1 {
    text-align: left;
}
</style>
