<template>
    <h3>Change password</h3>
    <hr />
    <span class="info"
        >If you want to you can change your password quickly here! Just fill out
        this quick form!</span
    >
    <div class="whole-div">
        <div class="wrapper-div">
            <div class="input-wrapper">
                <span class="label">Old password:</span>
                <input
                    class="input-field"
                    type="password"
                    v-model="oldPassword"
                    :class="[
                        ValidateString(oldPassword) ? '' : 'error-outline',
                    ]"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">New password:</span>
                <input
                    class="input-field"
                    type="password"
                    v-model="newPassword"
                    :class="[
                        ValidatePassword(newPassword) ? '' : 'error-outline',
                    ]"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Confirm new password:</span>
                <input
                    class="input-field"
                    type="password"
                    v-model="confirmNewPassword"
                    :class="[
                        ValidateConfirmPass(confirmNewPassword)
                            ? ''
                            : 'error-outline',
                    ]"
                />
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
    name: "VillaOwnerPassChange",
    data() {
        return {
            oldPassword: "",
            newPassword: "",
            confirmNewPassword: "",
            errors: [],
        };
    },
    methods: {
        ValidateString(str) {
            if (str == 0) return false;
            return true;
        },
        ValidatePassword(pass) {
            if (!this.ValidateString(pass)) return false;
            if (pass.length < 8) return false;
            return true;
        },
        ValidateConfirmPass(pass) {
            if (!this.ValidatePassword(pass)) return false;

            if (this.newPassword !== this.confirmNewPassword) return false;
            return true;
        },
        async Submit() {
            this.errors = new Array();
            let vue = this;
            if (!this.ValidateString(this.oldPassword)) {
                this.errors.push("Old password is mandatory");
            }
            if (!this.ValidatePassword(this.newPassword)) {
                this.errors.push(
                    "New password is mandatory and has to be atleast 8 chars"
                );
            }
            if (!this.ValidateConfirmPass(this.confirmNewPassword)) {
                this.errors.push("You haven't confirmed the new password");
            }

            if (this.errors.length > 0) {
                return;
            }

            let dto = {
                oldPassword: this.oldPassword,
                newPassword: this.newPassword,
                newPasswordConfirmed: this.confirmNewPassword,
            };

            fetch("/api/GeneralUser/ChangePassword", {
                method: "PUT",
                redirect: "follow",
                mode: "cors",
                credentials: "same-origin",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
                body: JSON.stringify(dto),
            })
                .then((resp) => {
                    if (resp.ok) {
                        vue.oldPassword = "";
                        vue.newPassword = "";
                        vue.confirmNewPassword = "";
                        alert("Password successfully updated!");
                        return "";
                    } else {
                        return resp.text();
                    }
                })
                .then((err) => {
                    if (err == "") {
                        return;
                    }
                    let error = "";
                    try {
                        error = JSON.parse(err);
                    } catch {
                        error = err;
                    }

                    if (error.constructor == "".constructor) {
                        alert("Something went wrong!\nError message: " + error);
                    } else {
                        for (let err in error) {
                            for (let innerErr of error[err]) {
                                vue.errors.push(innerErr);
                            }
                        }
                    }
                });
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
    width: fit-content;
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

.whole-div {
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

h3 {
    display: flex;
    padding-left: 25px;
}

.info {
    display: flex;
    text-align: left;
    margin-bottom: 15px;
    padding: 0px 15px;
}
</style>
