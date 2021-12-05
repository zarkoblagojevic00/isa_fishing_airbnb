<template>
    <div class="card">
        <img src="../assets/map.png" style="width: 100%" />
        <div class="container">
            <h4><b>Address placeholder</b></h4>
            <p>StreetName 65</p>
        </div>
    </div>
</template>

<script>
export default {
    name: "Map",
};
</script>

<style scoped>
.card {
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    width: 500px;
}

.card:hover {
    box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
    padding: 2px 16px;
}
</style>
