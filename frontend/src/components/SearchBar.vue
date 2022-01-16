<template>
    <div class="search-container">
        <form>
            <input
                type="text"
                :placeholder="searchPlaceholder"
                v-model="searchParam"
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
        searchPlaceholder: String,
    },
    data() {
        return {
            searchParam: "",
        };
    },
    methods: {
        onSearch(e) {
            e.preventDefault();
            axios
                .get("/api/Adventure/FilterOwnedAdventures", {
                    params: {
                        name: this.searchParam,
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
