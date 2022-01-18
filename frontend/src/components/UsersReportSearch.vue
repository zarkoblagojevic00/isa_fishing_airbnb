<template>
    <div class="search-container">
        <form>
            <input
                type="text"
                :placeholder="placeholderName"
                v-model="searchParamName"
            />
            <input
                type="text"
                :placeholder="placeholderSurname"
                v-model="searchParamSurname"
            />
            <button type="submit" @click="onSearch">
                <font-awesome-icon icon="search"></font-awesome-icon>
            </button>
        </form>
    </div>
</template>

<script>
import axios from "../api/api.js";

export default {
    props: {
        placeholderName: String,
        placeholderSurname: String,
    },
    data() {
        return {
            searchParamName: "",
            searchParamSurname: "",
        };
    },
    methods: {
        onSearch(e) {
            e.preventDefault();
            axios
                .get("/api/Instructor/FilterReservationsByUser", {
                    params: {
                        name: this.searchParamName,
                        surname: this.searchParamSurname,
                    },
                })
                .then((res) => {
                    console.log(this.searchParam);
                    console.log(res.data);
                    this.$emit("filtered", res.data);
                });
        },
    },
};
</script>

<style scoped>
.search-container {
    display: flex;
    justify-content: flex-end;
}
input[type="text"] {
    padding: 6px;
    margin-top: 8px;
    font-size: 17px;
    width: 300px;
    margin-right: 10px;
}

.search-container button {
    padding: 8px;
    margin-top: 8px;
    background: #ddd;
    font-size: 17px;
    border: none;
    cursor: pointer;
}

.search-container button:hover {
    background: #ccc;
}
</style>
