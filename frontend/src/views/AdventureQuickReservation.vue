<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <h1>Quick reservation</h1>
        <button
            @click="$router.push(this.baseUrlInstructor + 'new-quick-action')"
            v-if="currentRole == 'Instructor'"
        >
            New quick action
        </button>
    </div>
    <div class="flexbox-container">
        <QuickReservation />
    </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import QuickReservation from "../components/QuickReservation.vue";

export default {
    name: "AdventureQuickReservation",
    components: {
        Navbar,
        QuickReservation,
    },
    mounted() {
        this.currentRole = localStorage.getItem("role");
    },
    data() {
        return {
            navbarItems: ["Home", "Quick reservation", "Gallery"],
            baseUrlInstructor: "/adventure/" + this.$route.params.id + "/",
            currentRole: "",
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

button:hover {
    background-color: rgb(35, 46, 97);
}
</style>
