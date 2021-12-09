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

        <div class="card-price">${{ adventure.pricePerDay }} per day</div>
    </div>
    <div class="sub-header">
        <p>{{ adventure.promoDescription }}</p>
    </div>
    <div class="flexbox-container">
        <InstructorInfo />
        <Map />
    </div>
    <div class="flex-column">
        <div class="subheading">Additional offers</div>
        <div class="flexbox-container card">
            {{ adventure.additionalOffers }}
        </div>
        <div class="subheading">Additional equipment we offer you</div>
        <div class="flexbox-container card">
            {{ adventure.additionalEquipment }}
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
</template>

<script>
import InstructorInfo from "@/components/InstructorInfo.vue";
import Map from "@/components/Map.vue";
import Navbar from "../components/Navbar.vue";
import axios from "../api/api.js";

export default {
    name: "AdventureProfile",
    components: {
        InstructorInfo,
        Map,
        Navbar,
    },
    mounted() {
        this.loadAdventure();
        this.currentRole = localStorage.getItem("role");
    },
    data() {
        return {
            navbarItems: [
                "Quick reservation",
                "Gallery",
                "Price list",
                "Rules of conduct",
            ],
            baseUrlInstructor: "/adventure/" + this.$route.params.id + "/",
            adventure: {},
            currentRole: "",
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
                    console.log(res.data);
                    this.range.start = res.data.availableFrom;
                    this.range.end = res.data.availableTo;
                })
                .catch((err) => console.log(err));
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
</style>
