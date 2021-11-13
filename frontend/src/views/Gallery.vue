<template>
  <div>
    <Header
      headerText="Sunrise adventure"
      subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
    />
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="images">
      <h1>Images from previous adventures</h1>
    </div>

    <div class="images" v-viewer>
      <img
        v-for="img in images"
        :key="img"
        v-bind:src="require('../assets/' + img)"
      />
    </div>
  </div>
</template>
<script>
import Header from "@/components/Header.vue";
import Navbar from "@/components/Navbar.vue";

export default {
  name: "Gallery",
  components: {
    Header,
    Navbar,
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
      images: [
        "gallery-item-1.jpg",
        "gallery-item-2.jpg",
        "gallery-item-3.jpg",
        "gallery-item-1.jpg",
        "gallery-item-2.jpg",
        "gallery-item-3.jpg",
      ],
    };
  },
};
</script>

<style scoped>
.images {
  display: flex;
  flex-wrap: wrap;
  justify-content: flex-start;
  padding: 50px;
}

img {
  margin: 20px;
  height: 350px;
  width: auto;
}

p {
  font-size: 26px;
  font-weight: bolder;
}
</style>
