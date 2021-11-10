<template>
  <div class="hero-image">
    <div class="hero-text wood-text">
      <h1>{{ headerText }}</h1>
    </div>
    <div class="subtitle-text">
      {{ subtitleText }}
    </div>
  </div>
</template>

<script>
export default {
  name: "Header",
  data() {
    return {};
  },
  props: {
    headerText: String,
    subtitleText: String,
  },
};
</script>

<style scoped>
.hero-image {
  background-image: linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)),
    url("../assets/fishing-header.jpg");
  height: 70vh;
  background-repeat: no-repeat;
  background-size: cover;
  position: relative;
}
.hero-text {
  text-align: center;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  color: white;
}

.wood-text {
  background: url("../assets/wood.jpg");
  background-size: cover;
  font-size: 2rem;
  text-transform: uppercase;
  font-family: "bebas neue";
  background-clip: text;
  -webkit-background-clip: text;
  color: transparent;
}

.subtitle-text {
  background: url("../assets/wood.jpg");
  background-size: cover;
  font-size: 1.5rem;
  font-family: "bebas neue";
  background-clip: text;
  -webkit-background-clip: text;
  color: transparent;
  text-align: center;
  position: absolute;
  top: 70%;
  left: 50%;
  transform: translate(-50%, -50%);
}
</style>
