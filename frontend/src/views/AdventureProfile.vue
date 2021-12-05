<template>
    <Header
        headerText="Sunrise adventure"
        subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
    />
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <InstructorInfo />
        <Map />
    </div>
</template>

<script>
import Header from "@/components/Header.vue";
import InstructorInfo from "@/components/InstructorInfo.vue";
import Map from "@/components/Map.vue";
import Navbar from "../components/Navbar.vue";

export default {
    name: "AdventureProfile",
    components: {
        Header,
        InstructorInfo,
        Map,
        Navbar,
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
