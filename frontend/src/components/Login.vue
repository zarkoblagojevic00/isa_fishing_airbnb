<template>
  <div class="welcome">Welcome!</div>
  <div class="wrapper-input">
    <input
      type="text"
      class="input-field input-username"
      placeholder="Email"
      v-model="email"
      :class="[ValidateUser() == '' ? 'error-input' : '']"
    />
    <input
      type="password"
      class="input-field input-password"
      placeholder="Password"
      v-model="password"
      :class="[ValidatePassword() == '' ? 'error-input' : '']"
    />
  </div>

  <span class="error">{{ error != "" ? "Error: " + error : "" }}</span>

  <div class="login-forgot">
    <a href="#">Forgot password?</a>
    <button class="login-btn">Login</button>
  </div>

  <hr class="hr" />

  <div class="card-footer">
    <span class="footer-text">
      Don't have an account? <br />
      No worries! Register <a href="#" @click="changeMode()">here</a>
    </span>
  </div>
</template>

<script>
export default {
  name: "Login",
  components: {},
  props: {
    changeMode: Function,
  },
  data() {
    return {
      email: "",
      password: "",
      error: "",
    };
  },
  methods: {
    ValidateUser() {
      if (this.email.indexOf("@") == -1) return false;
      return true;
    },
    ValidatePassword() {
      if (this.password.length < 8) return false;

      return true;
    },
  },
};
</script>

<style scoped>
.welcome {
  font-family: "Brush Script MT", cursive;
  font-size: 50px;
  margin-bottom: 60px;
}

.input-field {
  height: 50px;
  margin-bottom: 20px;
  outline: none;
  border-radius: 10px;
  border: 1px solid #cecece;
  background-repeat: no-repeat;
  background-position: center left 8px;

  padding-left: 50px;
  max-width: 350px;
  width: 90%;
  display: flex;
}

.wrapper-input {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.login-forgot {
  justify-content: space-between;
  display: flex;
  max-width: 350px;
  width: 100%;
  align-items: center;
}

.login-btn {
  height: 50px;
  width: 100px;
  border: none;
  border-radius: 25px;
  color: white;
  background-color: #345fed;
  cursor: pointer;
  font-size: 18px;
  transition: background-color 300ms linear;
}

.login-btn:hover {
  background-color: #54cc39;
}

.hr {
  width: 75%;
  border: 1px solid #c3c3c3;
  border-bottom: none;
  margin-top: 60px;
  margin-bottom: 40px;
}

.input-username {
  background-image: url("../assets/user.png");
}
.input-password {
  background-image: url("../assets/key.png");
}

.error {
  color: red;
  height: 50px;
}

@media (max-width: 374px) {
  .login-forgot {
    flex-direction: column-reverse;
  }

  .login-btn {
    margin-bottom: 20px;
  }
}
.error-input {
  border: 1px solid red;
}
</style>
