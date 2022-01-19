<template>
    <div class="container">
        <h1>Villas</h1>
        <ServiceFinder
            class="sidebar vertical-scroll-no-bar"
            :title="isRegistered ? 'Book villa' : 'Search villas by'"
            :search="onSearch"
            @filtered="onFiltered"
            v-model:fromDate="reservationFromDate"
            v-model:toDate="reservationToDate"
            :reservation="isRegistered"
        ></ServiceFinder>

        <div class="service-content">
            <div v-if="villas.length">
                <Sorters
                    :items="villas"
                    :sortBy="sortBy"
                    @sorted="onSorted"
                ></Sorters>

                <VillaExpoItem
                    v-for="(villa, idx) in villas"
                    :key="idx"
                    :villa="villa"
                    :fromDate="reservationFromDate"
                    :toDate="reservationToDate"
                ></VillaExpoItem>
            </div>
            <SearchNoResults v-else></SearchNoResults>
        </div>
    </div>
</template>

<script>
import villaService from "../services/villa-service.js";
import VillaExpoItem from "../components/VillaExpoItem.vue";
import ServiceFinder from "../components/ServiceFinder.vue";
import SearchNoResults from "../components/SearchNoResults.vue";
import Sorters from "../components/Sorters.vue";

import roleValidatorMixin from "../mixins/role-validator.js";
export default {
    name: "VillasExpo",
    components: {
        VillaExpoItem,
        ServiceFinder,
        SearchNoResults,
        Sorters,
    },
    mixins: [roleValidatorMixin],

    data() {
        return {
            villas: [],
            reservationFromDate: null,
            reservationToDate: null,
            sortBy: {
                cityName: "Location",
                name: "Service name",
                averageMark: "Mark",
                pricePerDay: "Price",
            },
        };
    },

    methods: {
        onSearch: villaService.searchVillas,
        onFiltered(value) {
            this.villas = value;
        },
        onSorted(value) {
            this.villas = value;
        },
    },
};
</script>

<style scoped>
.container {
    max-width: 80%;
    margin: 30px auto;
}
</style>
