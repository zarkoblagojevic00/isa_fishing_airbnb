<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="heading">
        <div class="flex-heading">
            <h1>Instructor profile</h1>
            <font-awesome-icon
                icon="edit"
                class="fa-2x icon"
                @click="onEditClick"
            />
        </div>
        <div
            class="delete-account"
            v-if="editMode && !currentDeletionRequest"
            @click="onDeleteAccount"
        >
            Delete account?
        </div>
        <div class="delete-account" v-if="editMode && currentDeletionRequest">
            Deletion request sent
        </div>
    </div>
    <div class="flexbox-container">
        <div class="flexbox-row">
            <div class="item bold">Name:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.name.$model"
                :readonly="readMode"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.name.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>

        <div class="flexbox-row">
            <div class="item bold">Surname:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.surname.$model"
                :readonly="readMode"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.surname.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Address:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.address.$model"
                :readonly="readMode"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.address.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Phone number:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.phone.$model"
                :readonly="readMode"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.phone.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Email:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.email.$model"
                :readonly="readMode"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.email.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">City:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.city.$model"
                :readonly="readMode"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.city.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Country:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.country.$model"
                :readonly="readMode"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.country.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
    </div>
    <div class="button-row" v-if="editMode">
        <button class="button-decline" @click="onCancelEdit">Cancel</button>
        <button class="button-accept" @click="onSaveEdit">Update</button>
        <button class="button-password" @click="onChangePassword">
            Change password
        </button>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import axios from "../api/api.js";

import { useVuelidate } from "@vuelidate/core";
import { required, email } from "@vuelidate/validators";

export default {
    name: "InstructorProfile",
    components: {
        Navbar,
    },
    mounted() {
        axios.get("/api/GeneralUser/GetBasicInfo").then((res) => {
            this.userInfo = res.data;
            this.userInfoBackup = JSON.parse(JSON.stringify(res.data));
            console.log(res.data);
        });
        this.getUsersDeletionRequest();
    },
    methods: {
        onEditClick() {
            this.editMode = true;
            this.readMode = false;

            const Toast = this.$swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
            });

            Toast.fire({
                icon: "success",
                title: "Edit mode enabled",
            });
        },
        onCancelEdit() {
            this.readMode = true;
            this.editMode = false;
            this.userInfo = JSON.parse(JSON.stringify(this.userInfoBackup));
            const Toast = this.$swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
            });

            Toast.fire({
                icon: "success",
                title: "Read mode enabled",
            });
        },
        onSaveEdit() {
            const Toast = this.$swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
            });
            axios
                .post("/api/GeneralUser/UpdateBasicInfo", this.userInfo)
                .then(() => {
                    Toast.fire({
                        icon: "success",
                        title: "User information successfully updated.",
                    });
                    this.readMode = true;
                    this.editMode = false;
                })
                .catch((err) => console.log(err));
        },
        getUsersDeletionRequest() {
            axios
                .get("/api/GeneralUser/GetUsersRequestForDeletion")
                .then((res) => {
                    this.currentDeletionRequest = res.data;
                });
        },
        async onDeleteAccount() {
            const Toast = this.$swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
            });

            await this.$swal.fire({
                title: "Reason for account deletion",
                html: '<textarea id="deletionReason" class="swal2-input" placeholder="Reason" type="text">',
                focusConfirm: false,
                preConfirm: () => {
                    axios
                        .post("/api/GeneralUser/RequestAccountDeletion", {
                            reason: document.getElementById("deletionReason")
                                .value,
                        })
                        .then(() => {
                            Toast.fire({
                                icon: "success",
                                title: "Deletion request successfully sent.",
                            });
                            this.readMode = true;
                            this.editMode = false;
                            this.getUsersDeletionRequest();
                        })
                        .catch((error) => {
                            if (error.response) {
                                this.$swal.fire(error.response.data);
                            }
                        });
                },
            });
        },
        async onChangePassword() {
            const Toast = this.$swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
            });
            const { value: formValues } = await this.$swal.fire({
                title: "Password change",
                html:
                    '<input id="swal-input1" class="swal2-input" placeholder="Old password" type="password">' +
                    '<input id="swal-input2" class="swal2-input" placeholder="New password" type="password">' +
                    '<input id="swal-input3" class="swal2-input" placeholder="Confirm new password" type="password">',
                focusConfirm: false,
                preConfirm: () => {
                    return {
                        oldPassword:
                            document.getElementById("swal-input1").value,
                        newPassword:
                            document.getElementById("swal-input2").value,
                        newPasswordConfirmed:
                            document.getElementById("swal-input3").value,
                    };
                },
            });

            if (formValues) {
                console.log(formValues);
                axios
                    .put("/api/GeneralUser/ChangePassword", formValues)
                    .then(() => {
                        Toast.fire({
                            icon: "success",
                            title: "User information successfully updated.",
                        });
                        this.readMode = true;
                        this.editMode = false;
                    });
            }
        },
    },
    setup() {
        return { v$: useVuelidate() };
    },
    data() {
        return {
            navbarItems: [
                "Services",
                "Reservations",
                "Availability",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/instructor/",
            userInfo: {
                name: "",
                surname: "",
                address: "",
                phone: "",
                email: "",
                city: "",
                country: "",
            },
            userInfoBackup: {},
            readMode: true,
            editMode: false,
            currentDeletionRequest: undefined,
        };
    },
    validations() {
        return {
            userInfo: {
                name: { required },
                surname: { required },
                address: { required },
                phone: { required },
                email: { email },
                city: { required },
                country: { required },
            },
        };
    },
};
</script>

<style scoped>
.flexbox-container {
    padding: 50px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: flex-start;
}

.flex-heading {
    display: flex;
    justify-content: flex-start;
}

.delete-account {
    cursor: pointer;
    color: #af2828;
}

.flexbox-row {
    display: flex;
    justify-content: flex-start;
}

.item {
    width: 15rem;
    display: flex;
    justify-content: flex-start;
    margin-right: 10px;
}

.bold {
    font-weight: bolder;
}

.heading {
    display: flex;
    justify-content: space-between;
    padding: 50px;
}

input {
    font-size: 14px;
}

.icon:hover {
    cursor: pointer;
}

.button-accept {
    background-color: #59a552;
    color: whitesmoke;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    border: 0;
    font-size: 12px;
    width: 120px;
}

.button-decline {
    background-color: #a34b4b;
    color: whitesmoke;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    border: 0;
    font-size: 12px;
    margin-right: 50px;
    width: 120px;
}

.button-password {
    background-color: #000000;
    border-radius: 12px;
    color: rgb(255, 255, 255);
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    border: 0;
    font-size: 12px;
    width: 120px;
    margin-left: 50px;
}

button:hover {
    background-color: #2843d8;
}

.button-row {
    display: flex;
    justify-content: flex-start;
    padding: 50px;
}

.input-errors {
    color: rgb(158, 62, 62);
    font-size: 10px;
}
</style>
