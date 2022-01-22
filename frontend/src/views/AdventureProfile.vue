<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="header">
        <div class="heading-edit">
            <h1>{{ adventure.name }}</h1>
            <font-awesome-icon
                icon="edit"
                class="fa-2x icon"
                @click="$router.push(baseUrlInstructor + 'edit')"
                v-if="currentRole == 'Instructor'"
            />
        </div>

        <div class="card-price">${{ adventure.pricePerDay }} per hour</div>
    </div>
    <div class="sub-header">
        <p>{{ adventure.promoDescription }}</p>
    </div>
    <div class="flexbox-container">
        <InstructorInfo />
        <Map
            v-if="addressLoaded"
            :lon="addressInfo.longitude"
            :lat="addressInfo.latitude"
            class="map"
        />
    </div>
    <div class="flex-column">
        <div class="subheading">Additional offers</div>
        <div class="flexbox-container card">
            {{ adventure.additionalOffers }}
        </div>
        <div class="subheading">Additional equipment we offer you</div>
        <div class="flexbox-container card">
            <table class="reservations-table">
                <thead>
                    <tr>
                        <th>Additional</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody v-for="eq in additionalEquipmentArray" :key="eq.name">
                    <tr>
                        <td class="left">{{ eq.name }}</td>
                        <td class="left">
                            {{ eq.price }}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="subheading">Terms of use</div>
        <div class="flexbox-container card">
            {{ adventure.termsOfUse }}
        </div>
        <div class="subheading">Maximum people</div>
        <div class="flexbox-container card">
            {{ adventure.capacity }}
        </div>
        <div class="subheading">Conditions of cancellation</div>
        <div class="flexbox-container card">
            {{
                adventure.isPercentageTakenFromCanceledReservations
                    ? adventure.percentageToTake +
                      "% is taken upon cancellation"
                    : "Free cancellation"
            }}
        </div>
    </div>
    <button
        v-if="isRegistered"
        class="clickable primary transition-ease book-service-details"
        @click.stop="openBookServiceDialog"
    >
        Book
    </button>
</template>

<script>
import InstructorInfo from "@/components/InstructorInfo.vue";
import Map from "@/components/DisplayMap.vue";
import Navbar from "../components/Navbar.vue";
import axios from "../api/api.js";

import fetchImageBackground from "../mixins/fetch-image-bg.js";
import roleValidator from "../mixins/role-validator.js";
import vnodeInSwal from "../mixins/vnode-in-swal.js";
import swalCommons from "../mixins/swal-commons.js";
import BookAdventure from "../components/BookAdventure.ce.vue";
import generalService from "../services/general-service.js";
import { getId } from "../utils/local-storage-util.js";
import equipmentPicker from "../mixins/equipment-picker.js";
import stateDateLoader from "../mixins/state-date-loader.js";

export default {
    name: "AdventureProfile",
    components: {
        InstructorInfo,
        Map,
        Navbar,
    },
    mixins: [
        fetchImageBackground,
        roleValidator,
        vnodeInSwal,
        swalCommons,
        equipmentPicker,
        stateDateLoader,
    ],
    mounted() {
        this.loadAddressInfo();
        this.loadAdventure();
        this.bookAdventure = this.loadAdventure();
        this.currentRole = localStorage.getItem("role");
    },
    data() {
        return {
            navbarItems: ["Home", "Quick reservation", "Gallery"],
            baseUrlInstructor: "/adventure/" + this.$route.params.id + "/",
            adventure: {},
            bookAdventure: JSON.parse(localStorage.getItem("adventure")),
            currentRole: "",
            addressInfo: { longitude: 19.8227, latitude: 45.2396 },
            addressLoaded: false,
            additionalEquipmentArray: [],
        };
    },
    computed: {},
    methods: {
        loadAdventure() {
            axios
                .get(
                    "/api/Adventure/GetAdventureInfoById?adventureId=" +
                        this.$route.params.id
                )
                .then((res) => {
                    this.adventure = res.data;
                    this.ParseAdditionalEquipment(
                        this.adventure.additionalEquipment
                    );
                })
                .catch((err) => console.log(err));
        },
        loadAddressInfo() {
            axios
                .get(
                    "/api/Adventure/GetAddressInfoByAdventureId?adventureId=" +
                        this.$route.params.id
                )
                .then((res) => {
                    this.addressInfo = res.data;
                    this.addressLoaded = true;
                    console.log(res.data);
                });
        },

        ParseAdditionalEquipment(additionalEquipment) {
            this.additionalEquipmentArray = new Array();
            if (
                additionalEquipment == null ||
                additionalEquipment == undefined
            ) {
                return;
            }
            let receivedEq = additionalEquipment.split(";");
            for (let eq of receivedEq) {
                let name = eq.split(":")[0];
                let price = eq.split(":")[1];
                if (name === "" || price === "") {
                    continue;
                }
                this.additionalEquipmentArray.push({
                    name: name,
                    price: price,
                });
            }
        },
        openBookServiceDialog() {
            this.showComponent(
                BookAdventure,
                {
                    adventure: this.adventure,
                    fromDate: this.fromDate,
                    toDate: this.toDate,
                    userId: getId(),
                },
                this.bookSetupObject,
                (componentRes, sawlRes) => {
                    if (!sawlRes.isConfirmed) return;
                    this.handleReservationResult(componentRes);
                }
            );
        },
        async handleReservationResult(componentRes) {
            this.$router.push({ name: "ClientHomePage" });
            try {
                await generalService.makeReservation(componentRes);
                this.toast.fire({
                    icon: "success",
                    title: "You reservation was successful. Check your email for reservation details!",
                });
            } catch (error) {
                this.toast.fire({
                    icon: "error",
                    title: "Service was made unavailable or was reserved before you finished reservation",
                });
            }
        },
    },
};
</script>

<style scoped>
.flexbox-container {
    display: flex;
    justify-content: space-between;
    padding: 50px;
}

.icon {
    cursor: pointer;
}

.heading-edit {
    display: flex;
    justify-content: flex-start;
}

.header {
    display: flex;
    justify-content: space-between;
    padding-left: 50px;
    padding-right: 50px;
    padding-top: 50px;
    align-items: center;
}

.sub-header {
    display: flex;
    justify-content: space-between;
    padding-left: 50px;
    padding-right: 50px;
    padding-bottom: 50px;
    background-color: rgb(240, 248, 255);
}

button {
    background-color: #000;
    border-radius: 12px;
    color: #fff;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 15px;
    text-align: center;
    transition: 200ms;
    height: 50px;
    box-sizing: border-box;
    border: 0;
    font-size: 16px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
    margin-top: 7px;
    float: left;
    margin-left: 50px;
    margin-bottom: 50px;
}

.flex-column {
    display: flex;
    flex-direction: column;
    padding: 50px;
}

.card {
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    cursor: pointer;
    margin-bottom: 50px;
    text-align: left;
}
.card:hover {
    box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.subheading {
    font-weight: bolder;
    display: flex;
    justify-content: flex-start;
}

.card-price {
    display: inline-block;

    width: auto;
    height: 38px;

    background-color: #6ab070;
    -webkit-border-radius: 3px 4px 4px 3px;
    -moz-border-radius: 3px 4px 4px 3px;
    border-radius: 3px 4px 4px 3px;

    border-left: 1px solid #6ab070;

    /* This makes room for the triangle */
    margin-left: 19px;

    position: relative;

    color: white;
    font-weight: 300;
    font-size: 22px;
    line-height: 38px;

    padding: 0 10px 0 10px;
}

/* Makes the triangle */
.card-price:before {
    content: "";
    position: absolute;
    display: block;
    left: -19px;
    width: 0;
    height: 0;
    border-top: 19px solid transparent;
    border-bottom: 19px solid transparent;
    border-right: 19px solid #6ab070;
}

/* Makes the circle */
.card-price:after {
    content: "";
    background-color: white;
    border-radius: 50%;
    width: 4px;
    height: 4px;
    display: block;
    position: absolute;
    left: -9px;
    top: 17px;
}

.map {
    width: 500px;
    height: 500px;
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

button.book-service-details {
    width: 8%;
    position: fixed;
    font-size: 1.5rem;
    bottom: 35%;
    right: 10px;
    background: #ada;
}
</style>
