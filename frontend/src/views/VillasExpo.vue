<template>
    <div class="container">
        <h1>Villas</h1>
        <ServiceFinder
            class="sidebar"
            title="Search villas by"
            :search="onSearch"
            @filtered="onFiltered"
        ></ServiceFinder>
        <div class="service-content">
            <div v-if="villas.length">
                <VillaExpoItem
                    v-for="(villa, idx) in villas"
                    :key="idx"
                    :villa="villa"
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
export default {
    name: "VillasExpo",
    components: {
        VillaExpoItem,
        ServiceFinder,
        SearchNoResults,
    },
    data() {
        return {
            villas: [],
        };
    },
    async created() {
        this.villas = await villaService.getAllVillas();
    },
    methods: {
        onSearch: villaService.searchVillas,
        onFiltered(value) {
            this.villas = value;
        },
    },
};
</script>

<style src="../styles/form.css"></style>
<style scoped>
.container {
    max-width: 80%;
    margin: 30px auto;
}
</style>
