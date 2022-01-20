<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container-space-between">
        <h1>Services</h1>
        <SearchBar
            searchPlaceholder="Search services..."
            @filtered="onFiltered"
        />
        <button @click="$router.push(baseUrlInstructor + 'new-adventure')">
            Add new service
        </button>
    </div>

    <div class="flexbox-container">
        <InstructorServiceCard
            v-for="service in services"
            :key="service.adventureId"
            :service="service"
            @click="onAdventureClick(service)"
        />
    </div>
</template>

<script>
import InstructorServiceCard from "../components/InstructorServiceCard.vue";
import Navbar from "@/components/Navbar.vue";
import SearchBar from "@/components/SearchBar.vue";
import axios from "../api/api.js";

export default {
    name: "InstructorServices",
    components: {
        InstructorServiceCard,
        Navbar,
        SearchBar,
    },

    mounted() {
        axios.get("/api/Adventure/GetAllOwnedAdventures").then((res) => {
            this.services = res.data;
            console.log(res.data);
        });
    },
    data() {
        return {
            services: [],
            navbarItems: [
                "Services",
                "Reservations",
                "Availability",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/instructor/",
        };
    },
    methods: {
        onFiltered(value) {
            this.services = value;
        },
        onAdventureClick(service) {
            this.$router.push({
                path: "/adventure/" + service.adventureId + "/home",
                params: {
                    serviceName: service.name,
                },
            });
        },
    },
};
</script>

<style scoped>
.flexbox-container {
    padding: 50px;
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    grid-column-gap: 10px;
    grid-row-gap: 10px;
}

.flexbox-container-space-between {
    padding: 50px;
    display: flex;
    justify-content: space-between;
    align-items: center;
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
}

button:hover {
    background-color: #ccbf05;
}
</style>
