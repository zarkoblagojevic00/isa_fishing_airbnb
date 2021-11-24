<template>
  <div class="whole-page">
    <div class="card">
      <div class="card-content">
        <Register v-if="mode == 'register'" :changeMode="ChangeToLogin" />
        <Login v-if="mode == 'login'" :changeMode="ChangeToRegister" />
      </div>
    </div>
  </div>
</template>

<script>
import Login from "../components/Login.vue";
import Register from "../components/Register.vue";

export default {
  name: "LoginRegForgotPass",
  components: {
    Login,
    Register,
  },
  data() {
    return {
      mode: "login",
    };
  },
  methods: {
    ChangeToRegister() {
      this.mode = "register";
    },
    ChangeToLogin() {
      this.mode = "login";
    },
  },
};
</script>

<style scoped>
.whole-page {
  min-width: 100%;
  min-height: 100vh;
  background-image: url("../assets/gallery.jpeg");
  background-repeat: no-repeat;
  background-size: cover;
  position: relative;
  overflow: hidden;
  justify-content: center;
  align-items: center;
  display: flex;
  font-size: 13px;
}

.backdrop {
  background-color: rgba(0, 0, 0, 0.5);
  height: 100vh;
  position: relative;
}

.content {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  padding-left: 10px;
  padding-right: 10px;
}

.card {
  background-color: white;
  min-height: 600px;
  border-radius: 25px;
  padding: 20px;
  margin: 10px;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  max-width: 500px;
  box-shadow: rgba(50, 50, 93, 0.25) 0px 50px 100px -20px,
    rgba(0, 0, 0, 0.3) 0px 30px 60px -30px,
    rgba(10, 37, 64, 0.35) 0px -2px 6px 0px inset;
}

.card-content {
  display: flex;
  flex-direction: column;
  width: 100%;
  position: relative;
  align-items: center;
}

.layout {
  max-height: 100%;
  height: 100%;
  min-height: 100%;
  overflow-y: auto;
}
</style>
