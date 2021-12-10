<template v-if="villas.length">
    <div class="container">
        <h1>Villas</h1>
        <VillaExpoItem
            v-for="idx in 5"
            :key="idx"
            :villa="villas[0]"
        ></VillaExpoItem>
    </div>
</template>

<script>
import villaService from "../services/villa-service.js";
import VillaExpoItem from "../components/VillaExpoItem.vue";
export default {
    name: "VillasExpo",
    components: {
        VillaExpoItem,
    },
    data() {
        return {
            villas: [],
        };
    },
    async created() {
        this.villas = await villaService.getAllVillas();
        this.villas.forEach((v) => console.log(v.capacity));
    },
};
</script>

<style scoped>
.container {
    max-width: 80%;
    margin: 30px auto;
}
</style>
