<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div>
        <div class="container">
            <h1>Price list</h1>
        </div>
        <div class="container">
            <table class="price-table">
                <thead>
                    <tr>
                        <th>Service</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody v-for="service in services" :key="service.id">
                    <tr>
                        <td class="left">{{ service.name }}</td>
                        <td class="right">{{ service.price }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";

export default {
    name: "PriceList",
    components: {
        Navbar,
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
            services: [
                {
                    id: 1,
                    name: "Fishing",
                    price: 450,
                },
                {
                    id: 2,
                    name: "Sailing and fishing",
                    price: 700,
                },
                {
                    id: 3,
                    name: "Sailing and fishing in the sunrise",
                    price: 1000,
                },
            ],
        };
    },
};
</script>

<style scoped>
.container {
    padding: 50px;
    display: flex;
    justify-content: flex-start;
}

.left {
    text-align: left;
    width: 250px;
}

.right {
    text-align: left;
    width: 250px;
}

.price-table {
    border: solid 1px #ddeeee;
    border-collapse: collapse;
    border-spacing: 0;
    font: normal 13px Arial, sans-serif;
}
.price-table thead th {
    background-color: #ddefef;
    border: solid 1px #ddeeee;
    color: #336b6b;
    padding: 10px;
    text-align: left;
    text-shadow: 1px 1px 1px #fff;
}
.price-table tbody td {
    border: solid 1px #ddeeee;
    color: #333;
    padding: 10px;
    text-shadow: 1px 1px 1px #fff;
}
</style>
