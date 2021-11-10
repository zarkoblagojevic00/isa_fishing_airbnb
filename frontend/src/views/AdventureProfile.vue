<template>
  <Header
    headerText="Sunrise adventure"
    subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
  />
  <div class="flexbox-container">
    <GalleryCard />
    <DetailsCard />
    <InstructorInfo />
    <Map />
  </div>
</template>

<script>
import Header from "@/components/Header.vue";
import InstructorInfo from "@/components/InstructorInfo.vue";
import DetailsCard from "@/components/DetailsCard.vue";
import Map from "@/components/Map.vue";
import GalleryCard from "../components/GalleryCard.vue";

export default {
  name: "AdventureProfile",
  components: {
    Header,
    InstructorInfo,
    DetailsCard,
    Map,
    GalleryCard,
  },
  computed: {},
};
</script>

<style scoped>
.flexbox-container {
  display: flex;
  justify-content: space-evenly;
  padding: 15px;
}
</style>
