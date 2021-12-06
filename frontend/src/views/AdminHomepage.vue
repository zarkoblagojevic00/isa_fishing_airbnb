<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <h1>Admin</h1>
</template>

<script>
import Navbar from "@/components/Navbar.vue";

export default {
    name: "AdminHomepage",
    components: {
        Navbar,
    },
    data() {
        return {
            navbarItems: [
                "Requests",
                "Users",
                "Entities",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/admin/",
        };
    },
};
</script>

<style scoped></style>
