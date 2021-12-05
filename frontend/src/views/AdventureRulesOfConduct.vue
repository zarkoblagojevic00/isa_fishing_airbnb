<template>
    <Header
        headerText="Sunrise adventure"
        subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
    />
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <h1>Rules of conduct</h1>
        <h1>Additional fishing equipment</h1>
    </div>
    <div class="flexbox-container">
        <AdventureRules />
        <AdditionalFishingEquipment />
    </div>
</template>

<script>
import Header from "@/components/Header.vue";
import Navbar from "../components/Navbar.vue";
import AdventureRules from "../components/AdventureRules.vue";
import AdditionalFishingEquipment from "../components/AdditionalFishingEquipment.vue";

export default {
    name: "AdventureRulesOfConduct",
    components: {
        Header,
        Navbar,
        AdventureRules,
        AdditionalFishingEquipment,
    },
    data() {
        return {
            navbarItems: [
                "Quick reservation",
                "Gallery",
                "Price list",
                "Rules of conduct",
            ],
            baseUrlInstructor: "/adventure/",
        };
    },
    computed: {},
};
</script>

<style scoped>
.flexbox-container {
    display: flex;
    justify-content: space-between;
    padding: 50px;
}
</style>
