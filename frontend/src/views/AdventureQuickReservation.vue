<template>
  <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
  <div class="flexbox-container">
    <h1>Quick reservation</h1>
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
  justify-content: flex-start;
  padding: 50px;
}
</style>
