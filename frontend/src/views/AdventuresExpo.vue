<template>
    <div class="container">
        <h1>Adventures</h1>
        <ServiceFinder
            class="sidebar vertical-scroll-no-bar fix-margin-top"
            :title="isRegistered ? 'Book adventure' : 'Search adventures by'"
            :search="onSearch"
            @filtered="onFiltered"
            :reservation="isRegistered"
            adventure
        ></ServiceFinder>

        <div class="service-content">
            <div v-if="adventures.length">
                <Sorters
                    :items="adventures"
                    :sortBy="sortBy"
                    @sorted="onSorted"
                >
                </Sorters>

                <AdventureExpoItem
                    v-for="(adventure, idx) in adventures"
                    :key="idx"
                    :adventure="adventure"
                ></AdventureExpoItem>
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
import adventureService from "../services/adventure-service.js";
import AdventureExpoItem from "../components/AdventureExpoItem.vue";
import ServiceFinder from "../components/ServiceFinder.vue";
import SearchNoResults from "../components/SearchNoResults.vue";
import Sorters from "../components/Sorters.vue";
import roleValidatorMixin from "../mixins/role-validator.js";
export default {
    name: "AdventuresExpo",
    components: {
        AdventureExpoItem,
        ServiceFinder,
        SearchNoResults,
        Sorters,
    },
    mixins: [roleValidatorMixin],
    data() {
        return {
            adventures: [],
            sortBy: {
                cityName: "Location",
                name: "Service name",
                averageMark: "Mark",
                pricePerDay: "Price",
            },
        };
    },

    methods: {
        onSearch: adventureService.searchAdventures,
        onFiltered(value) {
            this.adventures = value;
        },
        onSorted(value) {
            this.adventures = value;
        },
        viewPromos() {
            this.$router.push({ name: "AdventuresPromoExpo" });
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
