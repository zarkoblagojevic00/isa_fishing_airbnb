<template>
  <div class="card">
    <img src="../assets/gallery-item-1.jpg" alt="Avatar" style="width: 100%" />
    <div class="container">
      <h4>
        <b>{{ service.name }}</b>
      </h4>
      <p>{{ service.promoDescription }}</p>
      <p>{{ service.address }}</p>
    </div>
  </div>
</template>

<script>
export default {
  name: "InstructorServiceCard",
  props: {
    service: Object,
  },
};
</script>

<style scoped>
.card {
  position: relative;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  min-width: 350px;
  max-width: 350px;
  cursor: pointer;
  margin-top: 20px;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
  padding: 2px 16px;
}
</style>
