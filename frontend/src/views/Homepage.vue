<template>
    <div>
        <h1>TODO: Homepage</h1>
        <div>
            <router-link to="adventure">
                Adventure profile (temporary)
            </router-link>
        </div>
        <div>
            <router-link to="instructor">
                instructor homepage (temporary)
            </router-link>
        </div>
        <div>
            <router-link to="login"> Login </router-link>
        </div>
        <div>
            <router-link to="TestPage"> TestPage </router-link>
        </div>
        <div>
            <router-link to="admin"> Admin </router-link>
        </div>
    </div>
    <!--<v-date-picker v-model="date" isRange="true" mode="dateTime" />-->
</template>

<script>
export default {
    name: "Homepage",
};
</script>

<style scoped></style>
