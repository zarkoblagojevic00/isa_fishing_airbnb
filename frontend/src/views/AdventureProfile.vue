<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <InstructorInfo />
        <Map />
        <button @click="$router.push(baseUrlInstructor + 'edit')">
            Review adventure information
        </button>
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
    justify-content: space-evenly;
    padding: 50px;
}

button {
    background-color: #000;
    border-radius: 12px;
    color: #fff;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 15px;
    text-align: center;
    transition: 200ms;
    height: 50px;
    box-sizing: border-box;
    border: 0;
    font-size: 16px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
    margin-top: 7px;
    float: left;
    margin-left: 50px;
    margin-bottom: 50px;
}
</style>
