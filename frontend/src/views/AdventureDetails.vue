<template>
  <Header
    headerText="Sunrise adventure"
    subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
  />
  <div class="flexbox-container">
    <QuickReservation />
    <div class="card">
      <PriceList />
      <AdditionalFishingEquipment />
    </div>
    <AdventureRules />
  </div>
</template>

<script>
import QuickReservation from "../components/QuickReservation.vue";
import PriceList from "../components/PriceList.vue";
import AdventureRules from "../components/AdventureRules.vue";
import Header from "../components/Header.vue";
import AdditionalFishingEquipment from "../components/AdditionalFishingEquipment.vue";

export default {
  name: "AdventureDetails",
  components: {
    QuickReservation,
    Header,
    PriceList,
    AdventureRules,
    AdditionalFishingEquipment,
  },
};
</script>

<style scoped>
.flexbox-container {
  display: flex;
  justify-content: space-between;
  padding: 10px;
}

.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  min-width: 500px;
  max-width: 500px;
  cursor: pointer;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}
</style>
