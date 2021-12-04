<template>
  <div class="welcome">Register here!</div>
  <div class="wrapper-input">
    <div class="user-choices">
      <div
        class="user-type-card"
        :class="[usertype == 'regular-user' ? 'chosen-type' : '']"
        @click="ChangeUserType('regular-user')"
      >
        <div>Regular user</div>
        <div class="card-background user"></div>
      </div>
      <div
        class="user-type-card"
        :class="[usertype == 'villa-owner' ? 'chosen-type' : '']"
        @click="ChangeUserType('villa-owner')"
      >
        <div>Villa owner</div>
        <div class="card-background villa"></div>
      </div>
      <div
        class="user-type-card"
        :class="[usertype == 'boat-owner' ? 'chosen-type' : '']"
        @click="ChangeUserType('boat-owner')"
      >
        <div>Boat owner</div>
        <div class="card-background boat"></div>
      </div>
      <div
        class="user-type-card"
        :class="[usertype == 'instructor' ? 'chosen-type' : '']"
        @click="ChangeUserType('instructor')"
      >
        <div>instructor</div>
        <div class="card-background instructor"></div>
      </div>
    </div>
    <input
      type="text"
      class="input-field input-username"
      placeholder="Email"
      v-model="email"
      :class="[ValidateEmail() == '' ? 'error-input' : '']"
    />
    <input
      type="password"
      class="input-field input-password"
      placeholder="Password"
      v-model="password"
      :class="[ValidatePassword() == '' ? 'error-input' : '']"
    />
    <input
      type="password"
      class="input-field input-password"
      placeholder="Confirm Password"
      v-model="confirmPassword"
      :class="[ValidateConfirmPassword() == '' ? 'error-input' : '']"
    />
    <div class="two-fields">
      <input
        type="text"
        placeholder="First name"
        class="small-input"
        v-model="firstname"
        :class="[firstname == '' ? 'error-input' : '']"
      />
      <input
        type="text"
        placeholder="Last name"
        class="small-input"
        v-model="lastname"
        :class="[lastname == '' ? 'error-input' : '']"
      />
    </div>
    <input
      type="text"
      class="input-field input-address"
      placeholder="Address"
      v-model="address"
      :class="[address == '' ? 'error-input' : '']"
    />
    <div class="two-fields">
      <input
        type="text"
        placeholder="City"
        class="small-input"
        list="cities"
        v-model="city"
        :class="[ValidateCity() == '' ? 'error-input' : '']"
      />
      <input
        type="text"
        placeholder="Phone number"
        class="small-input"
        v-model="phone"
        :class="[ValidatePhone() == '' ? 'error-input' : '']"
      />

      <datalist id="cities">
        <option v-for="city in cities" :key="city.id" :value="city.name" />
      </datalist>
    </div>

    <textarea
      placeholder="Registration reason"
      v-show="usertype != 'regular-user'"
      v-model="regreason"
      :class="[ValidateReason() == '' ? 'error-input' : '']"
    ></textarea>
  </div>

  <span class="error">{{ error != "" ? "Error: " + error : "" }}</span>

  <div class="login-forgot">
    <button class="login-btn" @click="SubmitRegistration()">Register</button>
  </div>

  <hr class="hr" />

  <div class="card-footer">
    <span class="footer-text">
      Already have an account? <br />
      Amazing! Sign in <a href="#" @click="changeMode()">here</a>
    </span>
  </div>
</template>

<script>
import registrationService from "../services/registration-service.js";
export default {
  name: "Register",
  props: {
    changeMode: Function,
  },
  data() {
    return {
      email: "",
      password: "",
      confirmPassword: "",
      error: "",
      usertype: "regular-user",
      cities: [],
      firstname: "",
      lastname: "",
      address: "",
      city: "",
      phone: "",
      regreason: "",
    };
  },
  mounted() {
    this.GetCities();
  },
  methods: {
    ChangeUserType(newType) {
      this.usertype = newType;
    },
    ValidateEmail() {
      if (this.email == "") return false;

      if (this.email.indexOf("@") == -1) return false;

      return true;
    },
    ValidatePassword() {
      if (this.password.length < 8) return false;

      return true;
    },
    ValidateConfirmPassword() {
      if (
        this.confirmPassword.length < 8 ||
        this.password != this.confirmPassword
      )
        return false;

      return true;
    },
    ValidateCity() {
      for (const c of this.cities)
        if (c.name.toLowerCase() == this.city.toLowerCase()) return true;
      return false;
    },
    ValidatePhone() {
      const regex = new RegExp("^[0-9]{9,10}$");

      if (!regex.test(this.phone)) return false;

      return true;
    },
    ValidateReason() {
      if (this.regreason == "" && this.usertype != "regular-user") return false;
      return true;
    },
    //City getter
    GetCities() {
      fetch(process.env.VUE_APP_BASE_API_URL + "api/City/GetCities")
        .then((response) => response.json())
        .then((data) => (this.cities = data));
    },
    SubmitRegistration() {
      if (
        !this.ValidateEmail() ||
        !this.ValidatePassword() ||
        !this.ValidateCity() ||
        !this.ValidatePhone() ||
        !this.ValidateReason()
      ) {
        alert("Error in validation!");
        return false;
      }

      if (this.usertype != "regular-user") {
        let userType = 1;
        if (userType == "boat-owner") userType = 2;
        else if (userType == "instructor") userType = 3;

        let cityId = this.GetCityId();

        let dto = {
          UserType: userType,
          Name: this.firstname,
          Surname: this.lastname,
          Password: this.password,
          ConfirmPassword: this.confirmPassword,
          Address: this.address,
          CityId: cityId,
          PhoneNumber: this.phone,
          Email: this.email,
          Reason: this.regreason,
        };

        console.log(dto);

        fetch(
          process.env.VUE_APP_BASE_API_URL +
            "api/Registration/RegisterServiceOwner",
          {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify(dto),
            headers: {
              "Content-Type": "application/json",
            },
          }
        )
          .then((response) => {
            alert(
              "Success! You should receive the confirmation link in the mailbox!"
            );
            return response.json();
          })
          .catch((data) => {
            console.log(data);
            if (data.errors != undefined && data.errors.length > 0) {
              let message = "";
              for (let key of data.errors) {
                message += key + ": " + data.errors[key];
              }
              alert(message);
            }
          });
      } else {
        this.RegisterServiceUser();
      }
    },

    GetCityId() {
      for (let city of this.cities) {
        if (city.name == this.city) {
          return city.cityId;
        }
      }
    },

    async RegisterServiceUser() {
      const cityId = this.GetCityId();
      let data = {
        Name: this.firstname,
        Surname: this.lastname,
        Password: this.password,
        ConfirmPassword: this.confirmPassword,
        Address: this.address,
        CityId: cityId,
        PhoneNumber: this.phone,
        Email: this.email,
      };
      try {
        await registrationService.registerServiceUser(data);
        alert(
          "Your registration has been sub successfully. You should receive the confirmation link in the mailbox!"
        );
      } catch (e) {
        alert(e.response.data);
      }
    },
  },
};
</script>

<style scoped>
.welcome {
  font-family: "Brush Script MT", cursive;
  font-size: 50px;
  margin-bottom: 50px;
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
  min-width: 250px;
}

.wrapper-input {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.login-forgot {
  justify-content: center;
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
.input-address {
  background-image: url("../assets/address.png");
}
.error {
  color: red;
  height: 50px;
}

@media (max-width: 353px) {
  .login-forgot {
    flex-direction: column-reverse;
  }

  .login-btn {
    margin-bottom: 20px;
  }
}

.user-choices {
  min-height: 150px;
  width: 100%;
  max-width: 350px;
  position: relative;
  margin-bottom: 15px;
  display: flex;
  justify-content: space-around;
  flex-flow: row wrap;
}

.user-type-card {
  width: 150px;
  height: 100px;
  margin-bottom: 10px;
  border-radius: 10px;
  border: 2px solid #c3c3c3;
  font-size: 14px;
  cursor: pointer;
  display: flex;
  justify-content: space-around;
  flex-direction: column;
}

.card-background {
  width: 100%;
  height: 50px;
  background-position: center;
  background-repeat: no-repeat;
  background-size: 50px;
}

.user {
  background-image: url("../assets/reg-user.png");
}

.villa {
  background-image: url("../assets/villa.png");
}

.boat {
  background-image: url("../assets/boat.png");
}

.instructor {
  background-image: url("../assets/instructor.png");
}

.chosen-type {
  border: 2px solid #4d4dff !important;
}

.two-fields {
  display: flex;
  flex-flow: row wrap;
  justify-content: space-between;
  max-width: 300px;
  width: 100%;
  position: relative;
  display: flex;
}

.small-input {
  height: 50px;
  margin-bottom: 20px;
  outline: none;
  border-radius: 10px;
  border: 1px solid #cecece;
  max-width: 100px;
  min-width: 100px;
  padding-left: 20px;
  padding-right: 20px;
}

input[list]::-webkit-calendar-picker-indicator {
  display: none;
  -webkit-appearance: none;
  opacity: 0;
}

textarea {
  resize: none;
  width: 270px;
  height: 80px;
  outline: none;
  border: 1px solid #c3c3c3;
  padding: 10px;
  border-radius: 15px;
}

.error-input {
  border: 1px solid red;
}
</style>
