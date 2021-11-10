<template>
  <div>
    <router-link to="/adventure/details">
      <div class="card">
        <img class="img" src="../assets/fishing.jpg" style="width: 100%" />
        <div class="container">
          <p>
            <b
              >For quick reservation, price list and rules of conduct, follow
              here!</b
            >
          </p>
        </div>
      </div>
    </router-link>
  </div>
</template>

<script>
export default {
  name: "DetailsCard",
  components: {},
};
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  min-width: 350px;
  max-width: 350px;
  cursor: pointer;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
  padding: 2px 16px;
}

.img {
  width: 100%;
}

p {
  margin-top: 50px;
}
</style>
