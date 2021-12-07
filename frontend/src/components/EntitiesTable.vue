<template>
    <div class="flexbox-container">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Owner name</th>
                    <th>Owner surname</th>
                    <th>Address</th>
                    <th>Promo description</th>
                    <th>Price per day</th>
                    <th>Capacity</th>
                    <th>Terms of use</th>
                    <th>Cancellation fee (%)</th>
                    <th></th>
                </tr>
            </thead>
            <tbody v-for="s in services" :key="s.serviceId">
                <tr>
                    <td class="left">{{ s.name }}</td>
                    <td class="left">{{ s.ownerName }}</td>
                    <td class="left">{{ s.ownerSurname }}</td>
                    <td class="left">{{ s.address }}</td>
                    <td class="left">{{ s.promoDescription }}</td>
                    <td class="left">{{ s.pricePerDay }}</td>
                    <td class="left">{{ s.capacity }}</td>
                    <td class="left">{{ s.termsOfUse }}</td>
                    <td class="left">
                        {{
                            s.isPercentageTakenFromCanceledReservations
                                ? s.percentageToTake
                                : "FREE CANCELLATION"
                        }}
                    </td>

                    <td class="left">
                        <button
                            class="button-decline"
                            @click="onRemoveService(s)"
                        >
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
        onRemoveService(s) {
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
                            .delete(
                                "/api/Admin/DeleteService?serviceId=" +
                                    s.serviceId
                            )
                            .then(() => {
                                this.$emit("deleted", s.serviceType);
                                this.$swal.fire(
                                    "Deleted!",
                                    "Your service has been deleted.",
                                    "success"
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
        services: Array,
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
