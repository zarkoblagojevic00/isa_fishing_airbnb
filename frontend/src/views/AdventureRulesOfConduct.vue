<template>
  <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
  <div class="flexbox-container">
    <h1>Rules of conduct</h1>
    <h1>Additional fishing equipment</h1>
  </div>
  <div class="flexbox-container">
    <AdventureRules />
    <AdditionalFishingEquipment />
  </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import AdventureRules from "../components/AdventureRules.vue";
import AdditionalFishingEquipment from "../components/AdditionalFishingEquipment.vue";

export default {
  name: "AdventureRulesOfConduct",
  components: {
    Navbar,
    AdventureRules,
    AdditionalFishingEquipment,
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
  justify-content: space-between;
  padding: 50px;
}
</style>
