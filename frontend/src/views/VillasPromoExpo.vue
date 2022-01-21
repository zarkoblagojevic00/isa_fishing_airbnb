<template>
    <div class="container">
        <h1>Villas</h1>
        <div class="service-content">
            <div v-if="villas.length">
                <Sorters :items="villas" :sortBy="sortBy" @sorted="onSorted">
                </Sorters>

                <VillaPromoExpoItem
                    v-for="(villaPromo, idx) in villas"
                    :key="idx"
                    :villa="villaPromo.villa"
                    :promo="villaPromo.promo"
                ></VillaPromoExpoItem>
            </div>
        </div>
    </div>
</template>

<script>
import villaService from "../services/villa-service.js";
import Sorters from "../components/Sorters.vue";
import VillaPromoExpoItem from "../components/VillaPromoExpoItem.vue";
import roleValidatorMixin from "../mixins/role-validator.js";

export default {
    components: {
        VillaPromoExpoItem,
        Sorters,
    },
    mixins: [roleValidatorMixin],

    data() {
        return {
            villas: [],
            sortBy: {
                "villa.cityName": "Location",
                "villa.name": "Service name",
                "villa.averageMark": "Mark",
                "promo.pricePerDay": "Price",
            },
        };
    },

    async created() {
        this.villas = await villaService.getQuickActions();
    },

    methods: {
        onSorted(value) {
            this.villas = value;
        },
    },
};
</script>

<style></style>
