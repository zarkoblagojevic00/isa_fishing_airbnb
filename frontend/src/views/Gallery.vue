<template>
  <div>
    <Header
      headerText="Sunrise adventure"
      subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
    />
    <h1>Images from previous adventures</h1>
    <div class="images" v-viewer>
      <img
        v-for="img in images"
        :key="img"
        v-bind:src="require('../assets/' + img)"
      />
    </div>
    <router-link to="/adventure"><p>Go back...</p></router-link>
  </div>
</template>
<script>
import Header from "@/components/Header.vue";

export default {
  name: "Gallery",
  components: {
    Header,
  },
  data() {
    return {
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
