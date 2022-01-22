<template>
    <div class="flexbox-container">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Address</th>
                    <th>City</th>
                    <th>Country</th>
                    <th>Phone number</th>
                    <th>Email</th>
                    <th>Verified</th>
                    <th></th>
                </tr>
            </thead>
            <tbody v-for="u in users" :key="u.userId">
                <tr>
                    <td class="left">{{ u.name }}</td>
                    <td class="left">{{ u.surname }}</td>
                    <td class="left">{{ u.address }}</td>
                    <td class="left">{{ u.city }}</td>
                    <td class="left">{{ u.country }}</td>
                    <td class="left">{{ u.phone }}</td>
                    <td class="left">{{ u.email }}</td>
                    <td class="left">
                        {{ u.isAccountVerified ? "YES" : "NO" }}
                    </td>
                    <td class="left" v-if="u.userType != 4">
                        <button class="button-decline" @click="onRemoveUser(u)">
                            Remove
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import axios from "../api/api.js";
export default {
    name: "UsersTable",
    methods: {
        onRemoveUser(u) {
            this.$swal
                .fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes, delete it!",
                })
                .then((result) => {
                    if (result.isConfirmed) {
                        axios
                            .delete("/api/Admin/DeleteUser?userId=" + u.userId)
                            .then(() => {
                                this.$emit("deleted", u.userType);
                                this.$swal.fire(
                                    "Deleted!",
                                    "Your user has been deleted.",
                                    "success"
                                );
                            })
                            .catch(() => {
                                this.$swal.fire(
                                    "User cannot be deleted right now, as he has unresolved reservations."
                                );
                            });
                    }
                });
        },
    },
    data() {
        return {};
    },
    props: {
        users: Array,
    },
};
</script>

<style scoped>
.heading {
    display: flex;
    justify-content: space-between;
    padding: 50px;
}

.flexbox-container {
    padding: 50px;
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap;
}
.flexbox-container-reservations {
    padding: 50px;
    padding-bottom: 1px;
    display: flex;
    justify-content: space-evenly;
    align-items: flex-start;
    flex-direction: column;
}
.reservations-table {
    border: solid 1px #ddeeee;
    border-collapse: collapse;
    border-spacing: 0;
    font: normal 13px Arial, sans-serif;
}
.reservations-table thead th {
    background-color: #ddefef;
    border: solid 1px #ddeeee;
    color: #336b6b;
    padding: 10px;
    text-align: left;
    text-shadow: 1px 1px 1px #fff;
}
.reservations-table tbody td {
    border: solid 1px #ddeeee;
    color: #333;
    padding: 10px;
    text-shadow: 1px 1px 1px #fff;
}
.left {
    text-align: left;
    width: 250px;
}

.button-new-admin {
    background-color: #000000;
    border-radius: 12px;
    color: rgb(255, 255, 255);
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    text-align: center;
    width: 120px;
    height: 50px;
    box-sizing: border-box;
    border: 0;
    font-size: 12px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
    text-decoration: none;
    align-items: center;
    font-size: 14px;
}

.button-accept {
    background-color: #15ff00;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    text-align: center;
    transition: 200ms;
    width: 100%;
    box-sizing: border-box;
    border: 0;
    font-size: 12px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
}

.button-decline {
    background-color: #eb1414;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    text-align: center;
    transition: 200ms;
    width: 100%;
    box-sizing: border-box;
    border: 0;
    font-size: 12px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
}

button:hover {
    background-color: #2843d8;
}
</style>
