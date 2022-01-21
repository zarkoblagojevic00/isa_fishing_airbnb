<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
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
    mounted() {
        this.$router.push(this.baseUrlInstructor + "services");
    },
};
</script>

<style scoped></style>
