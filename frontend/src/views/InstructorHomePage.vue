<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <h1>Instructor</h1>
</template>

<script>
import Navbar from "@/components/Navbar.vue";

export default {
    name: "InstructorHomePage",
    components: {
        Navbar,
    },
    data() {
        return {
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

<style scoped></style>
