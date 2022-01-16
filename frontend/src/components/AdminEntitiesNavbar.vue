<template>
    <ul>
        <li class="custom-select">
            <select @change="onChangeUsers">
                <option value="0">Clients</option>

                <option value="1">Villa owners</option>
                <option value="2">Boat owners</option>
                <option value="3">Instructors</option>
                <option value="4">Admins</option>
            </select>
        </li>
        <li class="custom-select">
            <select @change="onChangeEntities">
                <option value="0">Villas</option>
                <option value="1">Boats</option>
                <option value="2">Adventures</option>
            </select>
        </li>
        <li class="custom-select">
            <select @change="onChangeRequests">
                <option value="0">Registration requests</option>
                <option value="1">Mark requests</option>
                <option value="2">Account deletion requests</option>
            </select>
        </li>
        <li>
            <router-link class="link" :to="formatUrl('Issues')">
                Issues
            </router-link>
        </li>
        <li>
            <router-link class="link" :to="formatUrl('Analytics')">
                Analytics
            </router-link>
        </li>
        <li>
            <router-link class="link" :to="formatUrl('My profile')">
                My profile
            </router-link>
        </li>
    </ul>
</template>

<script>
export default {
    props: {
        baseUrl: String,
    },
    methods: {
        formatUrl(input) {
            return this.baseUrl + input.replace(/\s+/g, "-").toLowerCase();
        },
        onChangeUsers(e) {
            let data = e.target.value;
            this.$router.push({
                name: "AdminHomepage", //use name for router push
                params: { data },
            });
            this.$emit("userTypeChanged", e.target.value);
        },
        onChangeEntities(e) {
            let data = e.target.value;
            this.$router.push({
                name: "AdminEntities", //use name for router push
                params: { data },
            });
            this.$emit("serviceTypeChanged", e.target.value);
        },
        onChangeRequests(e) {
            let data = e.target.value;
            this.$router.push({
                name: "AdminRequests", //use name for router push
                params: { data },
            });
            this.$emit("requestTypeChanged", e.target.value);
        },
    },
};
</script>

<style scoped>
ul {
    display: flex;
    justify-content: space-evenly;
    list-style-type: none;
    margin: 0;
    padding: 0;
    overflow: hidden;
    background-color: rgb(0, 0, 0);
}

li .link {
    display: block;
    color: white;
    background-color: black;
    text-align: center;
    padding: 14px 16px;
    text-decoration: none;
}

select {
    display: block;
    background-color: rgb(0, 0, 0);
    color: white;
    text-align: center;
    padding: 14px 16px;
    text-decoration: none;
    border: none;
}

select option {
    text-align: left;
}

/* Change the link color to #111 (black) on hover */
li .link:hover {
    background-color: #111;
}
</style>
