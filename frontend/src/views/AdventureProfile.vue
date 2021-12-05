<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <InstructorInfo />
        <Map />
    </div>
</template>

<script>
import InstructorInfo from "@/components/InstructorInfo.vue";
import Map from "@/components/Map.vue";
import Navbar from "../components/Navbar.vue";

export default {
    name: "AdventureProfile",
    components: {
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
            baseUrlInstructor: "/adventure/" + this.$route.params.id + "/",
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
