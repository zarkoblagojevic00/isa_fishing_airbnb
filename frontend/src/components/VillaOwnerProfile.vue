<template>
    <div class="wholediv">
        <div class="wrapper-div">
            <div class="input-wrapper">
                <span class="label">Email:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="email"
                    disabled
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Name:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="name"
                    :class="[ValidateString(name) ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Surname:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="surname"
                    :class="[ValidateString(surname) ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Phone:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="phone"
                    :class="[ValidatePhone(phone) ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Address:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="address"
                    :class="[ValidateString(address) ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">City:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="city"
                    list="cities"
                    :class="[ValidateCity(city) ? '' : 'error-outline']"
                />
                <datalist id="cities">
                    <option
                        v-for="city in cities"
                        :key="city.id"
                        :value="city.name"
                    />
                </datalist>
            </div>
            <div class="input-wrapper push-down" v-if="errors.length > 0">
                <span class="label red">Errors:</span>
                <ul>
                    <li class="red" v-for="error in errors" :key="error">
                        *{{ error }}
                    </li>
                </ul>
            </div>
            <div class="submit-div">
                <button class="submit-btn" @click="Submit">Submit</button>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "VillaOwnerProfile",
    data() {
        return {
            email: "",
            name: "",
            surname: "",
            phone: "",
            address: "",
            city: "",
            errors: [],
            cities: [],
        };
    },
    mounted() {
        this.GetCities();
        this.GetInfo();
    },
    methods: {
        isNumber(evt) {
            evt = evt ? evt : window.event;
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (
                charCode > 31 &&
                (charCode < 48 || charCode > 57) &&
                charCode !== 46
            ) {
                evt.preventDefault();
            } else {
                return true;
            }
        },
        ValidateString(str) {
            if (str == 0) return false;
            return true;
        },
        ValidateNumber(num) {
            if (num < 1 || !Number.isInteger(num)) return false;

            return true;
        },
        ValidateCity() {
            for (const c of this.cities)
                if (c.name.toLowerCase() == this.city.toLowerCase())
                    return true;
            return false;
        },
        async GetCities() {
            fetch("/api/City/GetCities")
                .then((response) => response.json())
                .then((data) => (this.cities = data));
        },
        async GetInfo() {
            let vue = this;
            fetch("/api/GeneralUser/GetBasicInfo")
                .then((response) => response.json())
                .then((data) => {
                    vue.email = data.email;
                    vue.name = data.name;
                    vue.surname = data.surname;
                    vue.phone = data.phone;
                    vue.address = data.address;
                    vue.city = data.city;
                });
        },
        ValidatePhone() {
            const regex = new RegExp("^[0-9]{9,10}$");

            if (!regex.test(this.phone)) return false;

            return true;
        },
        async Submit() {
            this.errors = new Array();
            if (!this.ValidateString(this.name)) {
                this.errors.push("Name is not in the correct format");
            }
            if (!this.ValidateString(this.surname)) {
                this.errors.push("Surname is not in the correct format");
            }
            if (!this.ValidatePhone(this.phone)) {
                this.errors.push("Phone is not in the correct format");
            }
            if (!this.ValidateString(this.address)) {
                this.errors.push("Address is not in the correct format");
            }
            if (!this.ValidateCity(this.city)) {
                this.errors.push("City is not one of the known cities");
            }

            if (this.errors.length > 0) {
                return;
            }

            let dto = {
                name: this.name,
                surname: this.surname,
                address: this.address,
                city: this.city,
                phone: this.phone,
            };

            let response = await fetch("/api/GeneralUser/UpdateBasicInfo", {
                method: "POST",
                redirect: "follow",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
                body: JSON.stringify(dto),
            });

            if (response.ok) {
                alert("Successfully updated!");
                this.GetInfo();
                return;
            }

            let message = await response.text();
            let error = "";
            try {
                error = JSON.parse(message);
            } catch {
                error = message;
            }

            if (error.constructor == "".constructor) {
                alert("Something went wrong!\nError message: " + error);
            } else {
                let errors = error.errors;
                for (let err of errors) {
                    this.errors.push(err);
                }
            }
        },
    },
};
</script>

<style scoped>
.input-wrapper {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 15px;
}

.label {
    font-size: 14px;
    color: black;
    margin-bottom: 5px;
    padding-left: 10px;
}

.input-field {
    height: 50px;
    border-radius: 15px;
    outline: none;
    border: 1px solid #c3c3c3;
    max-width: 400px;
    min-width: 400px;
    width: 100%;
    padding-left: 15px;
    padding-right: 15px;
    box-sizing: border-box;
    font-size: 15px;
}

.input-textarea {
    height: 150px;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    border: 1px solid #c3c3c3;
    border-radius: 15px;
    padding: 15px;
    box-sizing: border-box;
    resize: none;
    font-size: 15px;
    outline: none;
}

.horizontal-wrapper {
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    display: flex;
    margin-bottom: 10px;
    padding-left: 10px;
}

.input-checkbox {
    width: 15px;
    height: 15px;
    position: relative;
    top: -2px;
}

.submit-btn {
    height: 50px;
    border-radius: 15px;
    border: none;
    font-size: 20px;
    color: white;
    background-color: #345fed;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 300ms linear;
}

.submit-btn:hover {
    background-color: #54cc39;
    color: black;
}

.red {
    color: red !important;
}

.error-outline {
    border: 1px solid red !important;
}

.wholediv {
    display: flex;
    flex-direction: row;
    width: 100%;
    height: auto;
    position: relative;
}

.wrapper-div {
    width: auto;
    margin-right: 50px;
}

li {
    display: flex;
    color: red;
    font-size: 13px;
}

.label {
    font-size: 14px;
    color: black;
    margin-bottom: 5px;
    padding-left: 10px;
}

.red {
    color: red !important;
}

.submit-div {
    display: flex;
    justify-content: center;
}

.push-down {
    margin-top: 25px;
}
</style>
