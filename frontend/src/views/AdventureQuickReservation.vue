<template>
  <Header
    headerText="Sunrise adventure"
    subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
  />
  <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
  <div class="flexbox-container">
    <h1>Quick reservation</h1>
  </div>
  <div class="flexbox-container">
    <QuickReservation />
  </div>
</template>

<script>
import Header from "@/components/Header.vue";
import Navbar from "../components/Navbar.vue";
import QuickReservation from "../components/QuickReservation.vue";

export default {
  name: "AdventureQuickReservation",
  components: {
    Header,
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
      baseUrlInstructor: "/adventure/",
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
