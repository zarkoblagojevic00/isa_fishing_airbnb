<template>
  <Header
    headerText="Sunrise adventure"
    subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
  />
  <div class="flexbox-container">
    <QuickReservation />
  </div>
</template>

<script>
import QuickReservation from "../components/QuickReservation.vue";
import Header from "../components/Header.vue";

export default {
  name: "AdventureDetails",
  components: {
    QuickReservation,
    Header,
  },
};
</script>

<style scoped>
.flexbox-container {
  display: flex;
  justify-content: space-evenly;
  padding: 15px;
}
</style>
