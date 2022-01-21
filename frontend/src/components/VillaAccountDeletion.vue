<template>
    <h3>Request account deletion</h3>
    <hr />
    <span class="info"
        >We are really sorry to hear that you are considering deleting your
        account. In order to improve and maybe have you back we will need you to
        write a reason for such decision. Once your account is deleted you will
        not be able to access any information you had previously. We wish you
        well.</span
    >
    <div class="wrapper-div">
        <div class="input-wrapper">
            <span class="label">Reason:</span>
            <textarea
                class="input-textarea"
                type="text"
                v-model="reason"
                :class="[ValidateString(reason) ? '' : 'error-outline']"
            ></textarea>
            <button class="submit-btn" @click="Submit">Request deletion</button>
        </div>
    </div>
    <div class="input-wrapper push-down" v-if="errors.length > 0">
        <span class="label red">Errors:</span>
        <ul>
            <li class="red" v-for="error in errors" :key="error">
                *{{ error }}
            </li>
        </ul>
    </div>
</template>

<script>
import swalCommons from "../mixins/swal-commons.js";
export default {
    name: "VillaAccountDeletion",
    mixins: [swalCommons],
    data() {
        return {
            reason: "",
            errors: [],
        };
    },
    methods: {
        Submit() {
            this.errors = new Array();
            if (!this.ValidateString(this.reason)) {
                this.errors.push("Reason is mandatory");
            }

            if (this.errors.length > 0) {
                return;
            }

            let dto = {
                reason: this.reason,
            };

            let vue = this;

            fetch("/api/GeneralUser/RequestAccountDeletion", {
                method: "POST",
                redirect: "follow",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
                body: JSON.stringify(dto),
            })
                .then((response) => {
                    if (response.ok) {
                        vue.toast.fire({
                            icon: "info",
                            title: "Account deletion request noted!",
                        });
                        vue.reason = "";
                        return "";
                    } else {
                        return response.text();
                    }
                })
                .then((data) => {
                    if (data === "") {
                        return;
                    }

                    let message = "";
                    try {
                        message = JSON.parse(data);
                    } catch {
                        message = data;
                    }

                    if (message.constructor == "".constructor) {
                        vue.errors.push(message);
                        vue.toast.fire({
                            icon: "error",
                            title: message,
                        });
                    } else {
                        for (let key in message) {
                            for (let err of message[key]) {
                                vue.errors.push(err);
                            }
                        }
                    }
                });
        },
        ValidateString(string) {
            if (string.length == 0) return false;
            return true;
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
    height: 350px;
    max-width: 400px;
    min-width: 400px;
    width: 100%;
    border: 1px solid #c3c3c3;
    border-radius: 15px;
    padding: 15px;
    box-sizing: border-box;
    resize: none;
    font-size: 15px;
    outline: none;
}
.error-outline {
    border: 1px solid red !important;
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
    align-self: center;
    margin-top: 10px;
}

.submit-btn:hover {
    background-color: #54cc39;
}

.wrapper-div {
    width: auto;
    margin-right: 50px;
    display: flex;
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
</style>
