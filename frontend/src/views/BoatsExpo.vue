<template>
    <div class="container">
        <h1>Boats</h1>
        <ServiceFinder
            class="sidebar vertical-scroll-no-bar fix-margin-top"
            :title="isRegistered ? 'Book boat' : 'Search boats by'"
            :search="onSearch"
            @filtered="onFiltered"
            :reservation="isRegistered"
        ></ServiceFinder>

        <div class="service-content">
            <div v-if="boats.length">
                <Sorters :items="boats" :sortBy="sortBy" @sorted="onSorted">
                </Sorters>

                <BoatExpoItem
                    v-for="(boat, idx) in boats"
                    :key="idx"
                    :boat="boat"
                ></BoatExpoItem>
            </div>
            <SearchNoResults v-else></SearchNoResults>
        </div>

        <button
            @click="viewPromos"
            class="clickable danger transition-ease promo"
        >
            Promo
        </button>
    </div>
</template>

<script>
import boatService from "../services/boat-service.js";
import BoatExpoItem from "../components/BoatExpoItem.vue";
import ServiceFinder from "../components/ServiceFinder.vue";
import SearchNoResults from "../components/SearchNoResults.vue";
import Sorters from "../components/Sorters.vue";
import roleValidatorMixin from "../mixins/role-validator.js";

export default {
    name: "BoatsExpo",
    components: {
        BoatExpoItem,
        ServiceFinder,
        SearchNoResults,
        Sorters,
    },
    mixins: [roleValidatorMixin],

    data() {
        return {
            boats: [],
            sortBy: {
                cityName: "Location",
                name: "Service name",
                averageMark: "Mark",
                pricePerDay: "Price",
            },
        };
    },

    methods: {
        onSearch: boatService.searchBoats,
        onFiltered(value) {
            this.boats = value;
        },
        onSorted(value) {
            this.boats = value;
        },
        viewPromos() {
            this.$router.push({ name: "BoatsPromoExpo" });
        },
    },
};
</script>

<style scoped>
.container {
    max-width: 80%;
    margin: 30px auto;
}

.fix-margin-top {
    margin-top: 30px;
}

.promo {
    width: 8%;
    position: fixed;
    font-size: 1.5rem;
    bottom: 35%;
    right: 2%;
}
</style>
