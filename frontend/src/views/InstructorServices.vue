<template>
    <Header
        headerText="Sunrise adventure"
        subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
    />
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container-space-between">
        <h1>Services</h1>
        <SearchBar searchPlaceholder="Search services..." />
        <button>Add new service</button>
    </div>

    <div class="flexbox-container">
        <InstructorServiceCard
            v-for="service in services"
            :key="service.id"
            :service="service"
        />
    </div>
</template>

<script>
import InstructorServiceCard from "../components/InstructorServiceCard.vue";
import Header from "@/components/Header.vue";
import Navbar from "@/components/Navbar.vue";
import SearchBar from "@/components/SearchBar.vue";

export default {
    name: "InstructorServices",
    components: {
        InstructorServiceCard,
        Header,
        Navbar,
        SearchBar,
    },
    computed: {},
    data() {
        return {
            services: [
                {
                    id: 1,
                    name: "Sunrise adventure",
                    description: "Promo description",
                    address: "Bulevard of broken dreams 0",
                },
                {
                    id: 2,
                    name: "Sunset adventure",
                    description: "Promo description",
                    address: "Bulevard of broken dreams 1",
                },
                {
                    id: 3,
                    name: "Sunset adventure",
                    description: "Promo description",
                    address: "Bulevard of broken dreams 2",
                },
            ],
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
};
</script>

<style scoped>
.flexbox-container {
    padding: 50px;
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap;
}

.flexbox-container-space-between {
    padding: 50px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

button {
    background-color: #fff000;
    border-radius: 12px;
    color: #000;
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
