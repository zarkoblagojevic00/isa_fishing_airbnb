<template>
    <div v-if="adventures.length" class="container">
        <h1>Adventures</h1>
        <AdventureExpoItem
            v-for="idx in 5"
            :key="idx"
            :adventure="adventures[0]"
        ></AdventureExpoItem>
    </div>
</template>

<script>
import adventureService from "../services/adventure-service.js";
import AdventureExpoItem from "../components/AdventureExpoItem.vue";
export default {
    name: "AdventuresExpo",
    components: {
        AdventureExpoItem,
    },
    data() {
        return {
            adventures: [],
        };
    },
    async created() {
        this.adventures = await adventureService.getAllAdventures();
        console.log(this.adventures);
    },
};
</script>

<style scoped>
.container {
    max-width: 80%;
    margin: 30px auto;
}
</style>
