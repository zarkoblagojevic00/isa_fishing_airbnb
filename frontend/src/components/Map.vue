<template>
  <div class="card">
    <img src="../assets/map.png" style="width: 100%" />
    <div class="container">
      <h4>
        <b> {{ addressInfo.address }} </b>
      </h4>
      <p>({{ addressInfo.longitude }}, {{ addressInfo.latitude }})</p>
    </div>
  </div>
</template>

<script>
import axios from "../api/api.js";
export default {
  name: "Map",
  data() {
    return {
      addressInfo: {},
    };
  },
  mounted() {
    axios
      .get(
        "/api/Adventure/GetAddressInfoByAdventureId?adventureId=" +
          this.$route.params.id
      )
      .then((res) => {
        this.addressInfo = res.data;
        console.log(res.data);
      });
  },
};
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  width: 500px;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
  padding: 2px 16px;
}
</style>
