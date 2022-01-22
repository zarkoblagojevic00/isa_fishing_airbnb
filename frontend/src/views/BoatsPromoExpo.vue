<template>
    <div class="container">
        <h1>Boats</h1>
        <div class="service-content">
            <div v-if="boats.length">
                <Sorters :items="boats" :sortBy="sortBy" @sorted="onSorted">
                </Sorters>

                <BoatPromoExpoItem
                    v-for="(boatPromo, idx) in boats"
                    :key="idx"
                    :boat="boatPromo.boat"
                    :promo="boatPromo.promo"
                ></BoatPromoExpoItem>
            </div>
        </div>
    </div>
</template>

<script>
import boatService from "../services/boat-service.js";
import Sorters from "../components/Sorters.vue";
import BoatPromoExpoItem from "../components/BoatPromoExpoItem.vue";
import roleValidatorMixin from "../mixins/role-validator.js";

export default {
    components: {
        BoatPromoExpoItem,
        Sorters,
    },
    mixins: [roleValidatorMixin],

    data() {
        return {
            boats: [],
            sortBy: {
                "boat.cityName": "Location",
                "boat.name": "Service name",
                "boat.averageMark": "Mark",
                "promo.pricePerDay": "Price",
            },
        };
    },

    async created() {
        this.boats = await boatService.getQuickActions();
    },

    methods: {
        onSorted(value) {
            this.boats = value;
        },
    },
};
</script>

<style></style>
