<template>
  <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
  <InstructorServices v-if="isInstructorServices" />
  <InstructorAvailability v-if="isInstructorAvailability" />
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import InstructorServices from "@/views/InstructorServices.vue";
import InstructorAvailability from "@/views/InstructorAvailability.vue";

export default {
  name: "InstructorHomePage",
  components: {
    Navbar,
    InstructorServices,
    InstructorAvailability,
  },
  computed: {
    isInstructorServices() {
      return this.$route.name === "InstructorServices";
    },
    isInstructorAvailability() {
      return this.$route.name === "InstructorAvailability";
    },
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
