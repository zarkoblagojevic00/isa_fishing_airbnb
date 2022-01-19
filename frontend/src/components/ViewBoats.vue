<template>
    <div class="wrapperdiv">
        <div class="no-villas" v-if="receivedBoats.length == 0">
            <h1>You dont have any boats right now!</h1>
            <h2>To add one checkout Add boat from the Boats submenu!</h2>
        </div>

        <div class="search-box" v-if="receivedBoats.length != 0">
            <input
                type="text"
                class="input-field"
                placeholder="Search by name"
                v-model="searchField"
            />
        </div>

        <div class="with-villas" v-if="receivedBoats.length != 0">
            <div
                class="my-card"
                v-for="boat in boats"
                :key="boat.boatId"
                @click="ClickedOnBoat(boat.boatId)"
            >
                <div class="cover-img">
                    <div class="shading"></div>
                    <img
                        src="../assets/missing-img.png"
                        class="image"
                        v-if="
                            boat.imageIds == undefined ||
                            boat.imageIds.length == 0
                        "
                    />
                    <img
                        v-bind:src="
                            '/api/Image/GetImage?id=' + boat.imageIds[0]
                        "
                        class="image"
                        v-if="
                            boat.imageIds != undefined &&
                            boat.imageIds.length > 0
                        "
                    />
                </div>
                <div class="card-content">
                    <span class="header-text">{{ boat.name }}</span>
                    <div class="normal-text">
                        Address:
                        <span class="normal-content-text">{{
                            boat.address
                        }}</span>
                    </div>
                    <div class="normal-text">
                        Price per day:
                        <span class="normal-content-text"
                            >{{ boat.pricePerDay }}&euro;</span
                        >
                    </div>
                    <div class="normal-text">
                        Capacity:
                        <span class="normal-content-text">{{
                            boat.capacity
                        }}</span>
                    </div>
                    <div class="normal-text">
                        Latitude:
                        <span class="normal-content-text">{{
                            boat.latitude
                        }}</span>
                    </div>
                    <div class="normal-text">
                        Longitude:
                        <span class="normal-content-text">{{
                            boat.longitude
                        }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script scoped>
import swalCommons from "../mixins/swal-commons.js";
export default {
    name: "ViewBoats",
    mixins: [swalCommons],
    props: {
        changeMode: Function,
        changeChosenBoat: Function,
    },
    mounted() {
        let role = window.localStorage.getItem("role");
        if (role != "BoatOwner") {
            this.$router.push("/");
            return;
        }
        this.GetVillas();
    },
    data() {
        return {
            receivedBoats: [],
            boats: [],
            searchField: "",
        };
    },
    methods: {
        GetVillas() {
            let vue = this;
            fetch("/api/BoatManagement/GetOwnedBoats", {
                method: "GET",
                redirect: "follow",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
            })
                .then((response) => {
                    if (response.status != 200) {
                        vue.toast.fire({
                            icon: "error",
                            title:
                                "Something went wrong!\nStatus code: " +
                                response.status,
                        });
                        return;
                    }
                    return response.json();
                })
                .then((data) => {
                    vue.receivedBoats = data;
                    vue.boats = vue.receivedBoats;
                });
        },
        ClickedOnBoat(boatId) {
            this.$props.changeChosenBoat(boatId);
            this.$props.changeMode("UpdateBoat");
        },
    },
    watch: {
        searchField: function () {
            this.boats = this.receivedBoats.filter(
                (boat) =>
                    boat.name
                        .toLowerCase()
                        .indexOf(this.searchField.toLowerCase()) != -1
            );
        },
    },
};
</script>

<style scoped>
.wrapperdiv {
    width: 100%;
    box-sizing: border-box;
    padding: 25px;
}

.no-villas {
    display: flex;
    width: 100%;
    align-items: center;
    flex-direction: column;
}

.with-villas {
    display: flex;
    flex-flow: row wrap;
    justify-content: space-around;
}

.my-card {
    max-width: 400px;
    min-width: 200px;
    min-height: 400px;
    width: 100%;
    background-color: white;
    box-shadow: 5px 5px 15px 5px #000000;
    margin-left: 15px;
    margin-right: 15px;
    margin-bottom: 20px;
}

.my-card:hover {
    background-color: #c3c3c3;
    cursor: pointer;
}

.cover-img {
    height: 200px;
    position: relative;
}

.image {
    width: 100%;
    height: 100%;
}

.shading {
    background-color: rgba(0, 0, 0, 0.35);
    height: 100%;
    width: 100%;
    position: absolute;
    z-index: 1;
}

.card-content {
    display: flex;
    padding-left: 20px;
    padding-right: 10px;
    box-sizing: border-box;
    padding-top: 10px;
    flex-direction: column;
    align-items: flex-start;
    position: relative;
    height: calc(100% - 200px);
}

.header-text {
    color: black;
    font-weight: 550;
    font-size: 20px;
    margin-left: 5px;
    margin-bottom: 15px;
}

.normal-content-text {
    font-style: italic;
    text-transform: capitalize;
}

.normal-text {
    margin-bottom: 5px;
}

.lonlat {
    flex-flow: row wrap;
}

.search-box {
    width: 100%;
    height: auto;
    display: flex;
    justify-content: center;
    margin-bottom: 15px;
}

.input-field {
    height: 50px;
    border-radius: 15px;
    outline: none;
    border: 1px solid #c3c3c3;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    padding-left: 15px;
    padding-right: 15px;
    box-sizing: border-box;
    font-size: 15px;
}
</style>
