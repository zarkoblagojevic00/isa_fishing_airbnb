<template>
    <div class="container">
        <h1>Adventures</h1>
        <div class="service-content">
            <div v-if="adventures.length">
                <Sorters
                    :items="adventures"
                    :sortBy="sortBy"
                    @sorted="onSorted"
                >
                </Sorters>

                <AdventurePromoExpoItem
                    v-for="(adventurePromo, idx) in adventures"
                    :key="idx"
                    :adventure="adventurePromo.adventure"
                    :promo="adventurePromo.promo"
                ></AdventurePromoExpoItem>
            </div>
        </div>
    </div>
</template>

<script>
import adventureService from "../services/adventure-service.js";
import Sorters from "../components/Sorters.vue";
import AdventurePromoExpoItem from "../components/AdventurePromoExpoItem.vue";
import roleValidatorMixin from "../mixins/role-validator.js";

export default {
    components: {
        AdventurePromoExpoItem,
        Sorters,
    },
    mixins: [roleValidatorMixin],

    data() {
        return {
            adventures: [],
            sortBy: {
                "adventure.cityName": "Location",
                "adventure.name": "Service name",
                "adventure.averageMark": "Mark",
                "promo.pricePerDay": "Price",
            },
        };
    },

    async created() {
        this.adventures = await adventureService.getQuickActions();
    },

    methods: {
        onSorted(value) {
            this.adventures = value;
        },
    },
};
</script>
