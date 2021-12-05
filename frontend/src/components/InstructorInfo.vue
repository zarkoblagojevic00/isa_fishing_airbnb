<template>
  <div class="card">
    <img src="../assets/avatar.png" alt="Avatar" style="width: 100%" />
    <div class="container">
      <h4>
        <b>Instructor {{ instructor.name }} {{ instructor.surname }}</b>
      </h4>
      <p>{{ instructor.email }}</p>
    </div>
  </div>
</template>

<script>
import axios from "../api/api.js";
export default {
  name: "InstructorInfo",
  data() {
    return {
      instructor: {},
      addressInfo: {},
    };
  },
  mounted() {
    axios
      .get("/api/Adventure/GetBasicInfo?adventureId=" + this.$route.params.id)
      .then((res) => {
        this.instructor = res.data;
        console.log(res.data);
      });
  },
};
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  max-width: 500px;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
  padding: 2px 16px;
}
</style>
