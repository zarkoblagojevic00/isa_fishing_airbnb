<template>
    <div class="card" @click="instructorProfile">
        <img src="../assets/avatar.jpg" alt="Avatar" style="width: 100%" />
        <div class="container">
            <h4>
                <b>Instructor {{ instructor.name }} {{ instructor.surname }}</b>
            </h4>
            <p>{{ adventure.shortInstructorBiography }}</p>
        </div>
    </div>
</template>

<script>
import axios from "../api/api.js";
export default {
    name: "InstructorInfo",
    props: {
        shortInstructorBiography: String,
    },
    data() {
        return {
            instructor: {},
            addressInfo: {},
            adventure: {},
        };
    },
    mounted() {
        axios
            .get(
                "/api/Adventure/GetBasicInfo?adventureId=" +
                    this.$route.params.id
            )
            .then((res) => {
                this.instructor = res.data;
                console.log(res.data);
            });
        this.loadAdventure();
    },
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
        instructorProfile() {
            this.$router.push("/instructor/services");
        },
    },
};
</script>

<style scoped>
.card {
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    max-width: 500px;
    cursor: pointer;
}
.card:hover {
    box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}
.container {
    padding: 2px 16px;
}
</style>
